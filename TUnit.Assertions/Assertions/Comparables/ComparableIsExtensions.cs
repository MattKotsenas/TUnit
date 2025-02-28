﻿#nullable disable

using System.Runtime.CompilerServices;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Comparable;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;
using TUnit.Assertions.AssertionBuilders.Wrappers;

namespace TUnit.Assertions.Extensions;

public static class ComparableIsExtensions
{
    public static InvokableValueAssertionBuilder<TActual> IsGreaterThan<TActual>(this IValueSource<TActual> valueSource, TActual expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue = null)
        where TActual : IComparable<TActual>
    {
        return valueSource.RegisterAssertion(new FuncValueAssertCondition<TActual, TActual>(default, (value, _, _) =>
            {
                return value.CompareTo(expected) > 0;
            },
            (value, _, _) => $"it was {value}",
            $"to be greater than {expected}")
            , [doNotPopulateThisValue]); }
    
    public static InvokableValueAssertionBuilder<TActual> IsGreaterThanOrEqualTo<TActual>(this IValueSource<TActual> valueSource, TActual expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue = null) 
        where TActual : IComparable<TActual>
    {
        return valueSource.RegisterAssertion(new FuncValueAssertCondition<TActual, TActual>(default, (value, _, _) =>
            {
                return value.CompareTo(expected) >= 0;
            },
            (value, _, _) => $"it was {value}",
            $"be greater than or equal to {expected}")
            , [doNotPopulateThisValue]); }
    
    public static InvokableValueAssertionBuilder<TActual> IsLessThan<TActual>(this IValueSource<TActual> valueSource, TActual expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue = null) 
        where TActual : IComparable<TActual>
    {
        return valueSource.RegisterAssertion(new FuncValueAssertCondition<TActual, TActual>(default, (value, _, _) =>
            {
                return value.CompareTo(expected) < 0;
            },
            (value, _, _) => $"it was {value}",
            $"be less than {expected}")
            , [doNotPopulateThisValue]); }
    
    public static InvokableValueAssertionBuilder<TActual> IsLessThanOrEqualTo<TActual>(this IValueSource<TActual> valueSource, TActual expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue = null) 
        where TActual : IComparable<TActual>
    {
        return valueSource.RegisterAssertion(new FuncValueAssertCondition<TActual, TActual>(default, (value, _, _) =>
            {
                return value.CompareTo(expected) <= 0;
            },
            (value, _, _) => $"it was {value}",
            $"be less than or equal to {expected}")
            , [doNotPopulateThisValue]); }
    
    public static BetweenAssertionBuilderWrapper<TActual> IsBetween<TActual>(this IValueSource<TActual> valueSource, TActual lowerBound, TActual upperBound, [CallerArgumentExpression(nameof(lowerBound))] string doNotPopulateThisValue1 = null, [CallerArgumentExpression(nameof(upperBound))] string doNotPopulateThisValue2 = null)
        where TActual : IComparable<TActual>
    {
        var assertionBuilder = valueSource.RegisterAssertion(new BetweenAssertCondition<TActual>(lowerBound, upperBound)
            , [doNotPopulateThisValue1, doNotPopulateThisValue2]);
        
        return new BetweenAssertionBuilderWrapper<TActual>(assertionBuilder);
    }
}