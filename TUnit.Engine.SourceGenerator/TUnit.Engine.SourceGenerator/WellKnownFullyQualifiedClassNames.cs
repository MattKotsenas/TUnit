﻿namespace TUnit.Engine.SourceGenerator;

public static class WellKnownFullyQualifiedClassNames
{
    // Test Definition Attributes
    public const string BaseTestAttribute = "global::TUnit.Core.BaseTestAttribute";

    public const string TestAttribute = "global::TUnit.Core.TestAttribute";
    public const string DataDrivenTestAttribute = "global::TUnit.Core.DataDrivenTestAttribute";
    public const string DataSourceDrivenTestAttribute = "global::TUnit.Core.DataSourceDrivenTestAttribute";
    public const string CombinativeTestAttribute = "global::TUnit.Core.CombinativeTestAttribute";
    
    // Test Data Attributes
    public const string ArgumentsAttribute = "global::TUnit.Core.ArgumentsAttribute";
    public const string MethodDataAttribute = "global::TUnit.Core.MethodDataAttribute";
    public const string ClassDataAttribute = "global::TUnit.Core.ClassDataAttribute";
    public const string InjectAttribute = "global::TUnit.Core.InjectAttribute";
    public const string CombinativeValuesAttribute = "global::TUnit.Core.CombinativeValuesAttribute";
    
    // Test Metadata Attributes
    public const string RepeatAttribute = "global::TUnit.Core.RepeatAttribute";
    public const string RetryAttribute = "global::TUnit.Core.RetryAttribute";
    public const string TimeoutAttribute = "global::TUnit.Core.TimeoutAttribute";
    
    // Test Hooks Attributes
    public const string AssemblySetUpAttribute = "global::TUnit.Core.AssemblySetUpAttribute";
    public const string AssemblyCleanUpAttribute = "global::TUnit.Core.AssemblyCleanUpAttribute";
    public const string BeforeAllTestsInClassAttribute = "global::TUnit.Core.BeforeAllTestsInClassAttribute";
    public const string AfterAllTestsInClassAttribute = "global::TUnit.Core.AfterAllTestsInClassAttribute";

    // Interfaces
    public const string IApplicableTestAttribute = "global::TUnit.Core.Interfaces.IApplicableTestAttribute";
}