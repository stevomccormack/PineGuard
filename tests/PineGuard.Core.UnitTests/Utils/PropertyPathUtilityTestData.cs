using System.Linq.Expressions;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class PropertyPathUtilityTestData
{
    internal sealed record PathSample(string? Name, PathSample? Nested, int Age, int[] Values);

    public static class Combine
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("null parent returns property unchanged", (null, "Name"), "Name"),
            new("empty parent returns property unchanged", ("", "Name"), "Name"),
            new("non-empty parent combines with separator", ("Address", "City"), "Address.City")
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null property throws", ("Address", null), new ExpectedException(typeof(ArgumentNullException), "property"))
        ];

        public sealed record ValidCase(string Name, (string? parent, string property) Value, string Expected)
            : ReturnCase<(string? parent, string property), string>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (string? parent, string? property) Value, ExpectedException ExpectedException)
            : ThrowsCase<(string? parent, string? property)>(Name, Value, ExpectedException);
    }

    public static class Index
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("null parent wraps index only", (null, 2), "[2]"),
            new("empty parent wraps index only", ("", 0), "[0]"),
            new("non-empty parent appends bracketed index", ("Lines", 3), "Lines[3]")
        ];

        public sealed record ValidCase(string Name, (string? parent, int index) Value, string Expected)
            : ReturnCase<(string? parent, int index), string>(Name, Value, Expected);
    }

    public static class Key
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("null parent wraps key only", (null, "Sku"), "[Sku]"),
            new("empty parent wraps key only", ("", "Sku"), "[Sku]"),
            new("non-empty parent appends bracketed key without quotes", ("Order", "Sku"), "Order[Sku]")
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null key throws", ("Order", null), new ExpectedException(typeof(ArgumentNullException), "key"))
        ];

        public sealed record ValidCase(string Name, (string? parent, string key) Value, string Expected)
            : ReturnCase<(string? parent, string key), string>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (string? parent, string? key) Value, ExpectedException ExpectedException)
            : ThrowsCase<(string? parent, string? key)>(Name, Value, ExpectedException);
    }

    public static class Transform
    {
        private static readonly Func<string, string> ToUpperTransform = static s => s.ToUpperInvariant();
        private static readonly Func<string, string> ThrowingTransform = static _ => throw new InvalidOperationException("segmentTransform must not be invoked when path is null or empty.");

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("null path short circuits without invoking transform", (null, ThrowingTransform), ""),
            new("empty path short circuits without invoking transform", ("", ThrowingTransform), ""),
            new("path with no bracketed segments transforms every segment", ("Address.City", ToUpperTransform), "ADDRESS.CITY"),
            new("path where every segment has a bracket suffix", ("Lines[0].Items[1]", ToUpperTransform), "LINES[0].ITEMS[1]"),
            new("path with a mix of bracketed and non-bracketed segments", ("Lines[2].Sku", ToUpperTransform), "LINES[2].SKU")
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null segmentTransform throws", ("Address.City", null), new ExpectedException(typeof(ArgumentNullException), "segmentTransform"))
        ];

        public sealed record ValidCase(string Name, (string? path, Func<string, string> segmentTransform) Value, string Expected)
            : ReturnCase<(string? path, Func<string, string> segmentTransform), string>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (string? path, Func<string, string>? segmentTransform) Value, ExpectedException ExpectedException)
            : ThrowsCase<(string? path, Func<string, string>? segmentTransform)>(Name, Value, ExpectedException);
    }

    public static class FromExpression
    {
        private static readonly Expression<Func<PathSample, PathSample>> RootParameter = x => x;
        private static readonly Expression<Func<PathSample, object>> RootParameterBoxed = x => x;
        private static readonly Expression<Func<PathSample, string?>> SingleMember = x => x.Name;
        private static readonly Expression<Func<PathSample, string?>> MultiMember = x => x.Nested!.Name;
        private static readonly Expression<Func<PathSample, object>> BoxedValueMember = x => x.Age;
        private static readonly Expression<Func<PathSample, string?>> MethodCallBody = x => x.ToString();
        private static readonly Expression<Func<PathSample, int>> IndexerBody = x => x.Values[2];
        private static readonly Expression<Func<PathSample, DateTime>> StaticMemberBody = _ => DateTime.MinValue;

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("root parameter body resolves to the root path", RootParameter, ""),
            new("boxed root parameter body resolves to the root path", RootParameterBoxed, ""),
            new("single member chain resolves the member name", SingleMember, "Name"),
            new("multi member chain resolves the dotted path", MultiMember, "Nested.Name"),
            new("boxed value type member resolves the member name", BoxedValueMember, "Age")
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null expression throws", null, new ExpectedException(typeof(ArgumentNullException), "expression")),
            new("method call body throws", MethodCallBody, new ExpectedException(typeof(ArgumentException), "expression")),
            new("indexer body throws", IndexerBody, new ExpectedException(typeof(ArgumentException), "expression")),
            new("static member body (chain never reaches the parameter) throws", StaticMemberBody, new ExpectedException(typeof(ArgumentException), "expression"))
        ];

        public sealed record ValidCase(string Name, LambdaExpression Value, string Expected)
            : ReturnCase<LambdaExpression, string>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, LambdaExpression? Value, ExpectedException ExpectedException)
            : ThrowsCase<LambdaExpression?>(Name, Value, ExpectedException);
    }
}
