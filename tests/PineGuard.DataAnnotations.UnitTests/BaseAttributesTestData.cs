using System.Reflection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class BaseAttributesTestData
{
    public static class ObjectAttributeBaseInvokeGenericMust
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("null with member", (MethodName: "Default", Value: null, MemberName: "Value"), true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null without member", (MethodName: "Default", Value: null, MemberName: null), true),
            new("null with inferred type", (MethodName: "Default", Value: null, MemberName: "ToString"), true)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("method not found", (MethodName: "NonExistentMethod", Value: "value", MemberName: null), new ExpectedException(typeof(InvalidOperationException), null, "Method NonExistentMethod not found"))
        ];

        public sealed record ValidCase(string Name, (string MethodName, object? Value, string? MemberName) Value, bool Expected)
            : ReturnCase<(string MethodName, object? Value, string? MemberName), bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (string MethodName, object? Value, string? MemberName) Value, ExpectedException ExpectedException)
            : ThrowsCase<(string MethodName, object? Value, string? MemberName)>(Name, Value, ExpectedException);
    }

    public static class ValidationAttributeBaseBuildInvokeArgs
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("two or more parameters", typeof(string).GetMethod(nameof(string.Substring), [typeof(int), typeof(int)])!, 2)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("no parameters", typeof(object).GetMethod(nameof(ToString), Type.EmptyTypes)!, 0),
            new("single parameter", typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!, 1)
        ];

        public sealed record ValidCase(string Name, MethodInfo Value, int Expected)
            : ReturnCase<MethodInfo, int>(Name, Value, Expected);
    }

    public static class NumberAttributeBaseInvokeAndMap
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("method not found", (MethodName: "NonExistentMethod", Value: 123), new ExpectedException(typeof(InvalidOperationException))),
            new InvalidCase( "type not supported", (MethodName: "Positive", Value: "not a number"), new ExpectedException(typeof(InvalidOperationException), null, "compatible with type String not found"))
        ];

        public sealed record InvalidCase(string Name, (string MethodName, object? Value) Value, ExpectedException ExpectedException)
            : ThrowsCase<(string MethodName, object? Value)>(Name, Value, ExpectedException);
    }
}
