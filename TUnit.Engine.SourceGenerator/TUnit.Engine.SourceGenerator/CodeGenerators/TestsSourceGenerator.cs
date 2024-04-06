using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using TUnit.Engine.SourceGenerator.Models;

namespace TUnit.Engine.SourceGenerator.CodeGenerators;

/// <summary>
/// A sample source generator that creates C# classes based on the text file (in this case, Domain Driven Design ubiquitous language registry).
/// When using a simple text file as a baseline, we can create a non-incremental source generator.
/// </summary>
[Generator]
public class TestsSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var testMethods = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s), 
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)
            .Collect();
            
        context.RegisterSourceOutput(testMethods, Execute);
    }
    
    static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    {
        return node is MethodDeclarationSyntax { AttributeLists.Count: > 0 } methodDeclarationSyntax;
    }

    static Method? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return null;
        }
        
        var symbol = context.SemanticModel.GetDeclaredSymbol(context.Node);
        
        if (symbol is not IMethodSymbol methodSymbol)
        {
            return null;
        }

        var attributes = methodSymbol.GetAttributes();

        if (!attributes.Any(x =>
                x.AttributeClass?.BaseType?.ToDisplayString(DisplayFormats.FullyQualifiedGenericWithGlobalPrefix)
                == WellKnownFullyQualifiedClassNames.BaseTestAttribute))
        {
            return null;
        }

        return new Method(methodDeclarationSyntax, methodSymbol);
    }
    
    private static void Execute(SourceProductionContext context, ImmutableArray<Method?> methods)
    {
        foreach (var method in methods.OfType<Method>())
        {
            var classSource = ProcessTests(method);
                
            if (string.IsNullOrEmpty(classSource))
            {
                continue;
            }

            var className = $"{method.MethodSymbol.Name}_{Guid.NewGuid():N}";
            context.AddSource($"{className}.g.cs", SourceText.From(WrapInClass(className, classSource), Encoding.UTF8));
        }
    }

    private static string WrapInClass(string className, string methodCode)
    {
        return $$"""
               // <auto-generated/>
               using System.Linq;
               using System.Runtime.CompilerServices;

               namespace TUnit.Engine;

               file class {{className}}
               {
                   [ModuleInitializer]
                   public static void Initialise()
                   {
                        {{methodCode}}
                   }
               } 
               """;
    }

    private static string ProcessTests(Method method)
    {
        var methodSymbol = method.MethodSymbol;
        
        if (methodSymbol.ContainingType.IsAbstract)
        {
            return string.Empty;
        }

        var sourceBuilder = new StringBuilder();
        
        foreach (var testInvocationCode in GetTestInvocationCode(methodSymbol))
        {
            sourceBuilder.AppendLine(testInvocationCode);
        }
        
        return sourceBuilder.ToString();
    }

    private static IEnumerable<string> GetTestInvocationCode(IMethodSymbol methodSymbol)
    {
        var writeableTests = WriteableTestsRetriever.GetWriteableTests(methodSymbol);
        
        foreach (var writeableTest in writeableTests)
        {
            yield return GenericTestInvocationGenerator.GenerateTestInvocationCode(writeableTest);
        }
    }
}