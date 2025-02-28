﻿using TUnit.Core.Interfaces;

namespace TUnit.Core;

internal record DiscoveredTest<
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis
        .DynamicallyAccessedMemberTypes.All)]
    TTestClass
>(ResettableLazy<TTestClass> resettableLazyTestClassFactory) : DiscoveredTest where TTestClass : class
{
    public TTestClass TestClass => resettableLazyTestClassFactory.Value;
    
    public required Func<TTestClass, CancellationToken, Task> TestBody { get; init; }

    public override async Task ExecuteTest(CancellationToken cancellationToken)
    {
        TestContext.CancellationToken = cancellationToken;
        await TestExecutor.ExecuteTest(TestContext, () => TestBody.Invoke(TestClass, cancellationToken));
    }
    
    public override async Task ResetTestInstance()
    {
        await resettableLazyTestClassFactory.ResetLazy();
    }

    public override IClassConstructor? ClassConstructor => resettableLazyTestClassFactory.ClassConstructor;
}

internal abstract record DiscoveredTest : IComparable<DiscoveredTest>, IComparable
{
    public required TestContext TestContext { get; init; }

    public abstract Task ExecuteTest(CancellationToken cancellationToken);

    public abstract Task ResetTestInstance();
    
    public TestDetails TestDetails => TestContext.TestDetails;
    
    public ITestExecutor TestExecutor { get; internal set; } = DefaultExecutor.Instance;
    
    public abstract IClassConstructor? ClassConstructor { get; }
    
    public IHookExecutor? HookExecutor { get; internal set; }

    internal Dependency[] Dependencies { get; set; } = [];

    public virtual bool Equals(DiscoveredTest? other)
    {
        return other?.TestDetails.TestId == TestDetails.TestId;
    }

    public override int GetHashCode()
    {
        return TestDetails.TestId.GetHashCode();
    }

    public int CompareTo(object? obj)
    {
        return CompareTo(obj as DiscoveredTest);
    }

    public int CompareTo(DiscoveredTest? other)
    {
        return string.Compare(other?.TestDetails.TestId, TestDetails.TestId, StringComparison.Ordinal);
    }
}