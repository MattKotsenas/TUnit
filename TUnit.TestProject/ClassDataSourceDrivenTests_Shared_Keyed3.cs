﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;
using TUnit.TestProject.Dummy;

namespace TUnit.TestProject;

[ClassDataSource<SomeAsyncDisposableClass>(Shared = SharedType.Keyed, Key = "🌲")]
[SuppressMessage("Usage", "TUnit0018:Test methods should not assign instance data")]
public class ClassDataSourceDrivenTests_Shared_Keyed3
{
    private static readonly List<SomeAsyncDisposableClass> MethodLevels = [];
    private static readonly List<SomeAsyncDisposableClass> ClassLevels = [];

    public ClassDataSourceDrivenTests_Shared_Keyed3(SomeAsyncDisposableClass someClass)
    {
        ClassLevels.Add(someClass);
    }
    
    [DataSourceDrivenTest]
    [ClassDataSource<SomeAsyncDisposableClass>(Shared = SharedType.Keyed, Key = "🔑")]
    public void DataSource_Class(SomeAsyncDisposableClass value)
    {
        MethodLevels.Add(value);
    }

    [DataSourceDrivenTest]
    [ClassDataSource<SomeAsyncDisposableClass>(Shared = SharedType.Keyed, Key = "🔑")]
    public void DataSource_Class_Generic(SomeAsyncDisposableClass value)
    {
        MethodLevels.Add(value);
    }

    [AfterAllTestsInClass]
    public static async Task AssertAfter()
    {
        await Assert.That(ClassLevels).Is.Not.Empty();
        await Assert.That(MethodLevels).Is.Not.Empty();

        foreach (var classLevel in ClassLevels)
        {
            await Assert.That(classLevel.IsDisposed).Is.True();
        }
        
        foreach (var methodLevel in MethodLevels)
        {
            await Assert.That(methodLevel.IsDisposed).Is.True();
        }
    }
}