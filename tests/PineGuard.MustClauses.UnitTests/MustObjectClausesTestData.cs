using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.ObjectRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustObjectClausesTestData
{
    public static class EqualTo
    {
        public static TheoryData<MustCase<(object value, object other)>> ValidCases =>
        [
            new(nameof(F.IsEqualTo.EqualStrings), (F.IsEqualTo.EqualStrings.value!, F.IsEqualTo.EqualStrings.other!), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object value, object other)>> InvalidCases =>
        [
            new(nameof(F.IsEqualTo.NotEqualStrings), (F.IsEqualTo.NotEqualStrings.value!, F.IsEqualTo.NotEqualStrings.other!), new MustExpected(false, "value must be equal to the expected value."))
        ];
    }

    public static class NotEqualTo
    {
        public static TheoryData<MustCase<(object value, object other)>> ValidCases =>
        [
            new(nameof(F.IsEqualTo.NotEqualStrings), (F.IsEqualTo.NotEqualStrings.value!, F.IsEqualTo.NotEqualStrings.other!), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object value, object other)>> InvalidCases =>
        [
            new(nameof(F.IsEqualTo.EqualStrings), (F.IsEqualTo.EqualStrings.value!, F.IsEqualTo.EqualStrings.other!), new MustExpected(false, "value must not be equal to the expected value."))
        ];
    }

    public static class OfType
    {
        public static TheoryData<MustCase<(object value, Type type)>> ValidCases =>
        [
            new(nameof(F.IsOfType.StringValue), (F.IsOfType.StringValue!, typeof(string)), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object value, Type type)>> InvalidCases =>
        [
            new("not-type", (F.IsOfType.StringValue!, typeof(int)), new MustExpected(false))
        ];
    }

    public static class NotOfType
    {
        public static TheoryData<MustCase<(object value, Type type)>> ValidCases =>
        [
            new("not-type", (F.IsOfType.StringValue!, typeof(int)), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object value, Type type)>> InvalidCases =>
        [
            new(nameof(F.IsOfType.StringValue), (F.IsOfType.StringValue!, typeof(string)), new MustExpected(false))
        ];
    }

    public static class AssignableToType
    {
        public static TheoryData<MustCase<(object value, Type type)>> ValidCases =>
        [
            new(nameof(F.IsAssignableToType.StringValue), (F.IsAssignableToType.StringValue!, typeof(object)), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object value, Type type)>> InvalidCases =>
        [
            new("not-assignable", (F.IsAssignableToType.StringValue!, typeof(int)), new MustExpected(false))
        ];
    }

    public static class NotAssignableToType
    {
        public static TheoryData<MustCase<(object value, Type type)>> ValidCases =>
        [
            new("not-assignable", (F.IsAssignableToType.StringValue!, typeof(int)), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object value, Type type)>> InvalidCases =>
        [
            new(nameof(F.IsAssignableToType.StringValue), (F.IsAssignableToType.StringValue!, typeof(object)), new MustExpected(false))
        ];
    }

    public static class SameReferenceAs
    {
        public static TheoryData<MustCase<(object a, object b)>> ValidCases =>
        [
            new(nameof(F.IsSameReferenceAs.SameReference), (F.IsSameReferenceAs.SameReference.a!, F.IsSameReferenceAs.SameReference.b!), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object a, object b)>> InvalidCases =>
        [
            new(nameof(F.IsSameReferenceAs.DifferentReference), (F.IsSameReferenceAs.DifferentReference.a!, F.IsSameReferenceAs.DifferentReference.b!), new MustExpected(false, "a must reference the same instance."))
        ];
    }

    public static class NotSameReferenceAs
    {
        public static TheoryData<MustCase<(object a, object b)>> ValidCases =>
        [
            new(nameof(F.IsSameReferenceAs.DifferentReference), (F.IsSameReferenceAs.DifferentReference.a!, F.IsSameReferenceAs.DifferentReference.b!), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(object a, object b)>> InvalidCases =>
        [
            new(nameof(F.IsSameReferenceAs.SameReference), (F.IsSameReferenceAs.SameReference.a!, F.IsSameReferenceAs.SameReference.b!), new MustExpected(false, "a must not reference the same instance."))
        ];
    }
}
