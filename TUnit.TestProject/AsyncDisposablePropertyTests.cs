﻿namespace TUnit.TestProject;

public class AsyncDisposablePropertyTests
{
#pragma warning disable TUnit0023
    public TextWriter? TextWriter { get; private set; }
#pragma warning restore TUnit0023

    [Before(Test)]
    public void Setup()
    {
        TextWriter = new StringWriter();
    }

    [After(Test)]
    public void Blah()
    {
        TextWriter!.Dispose();
    }

    [Test]
    public void Test1()
    {
        // Dummy method
    }
}