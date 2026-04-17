using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.DefaultEqualityRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDefaultEqualityClausesTestData
{
    public static class DefaultInt32
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsDefaultInt32.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<int>> InvalidCases => F.IsDefaultInt32.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be the default value."));
    }

    public static class DefaultString
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsDefaultString.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsDefaultString.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be the default value."));
    }

    public static class NotDefaultInt32
    {
        public static TheoryData<MustCase<int>> ValidCases => F.IsDefaultInt32.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<int>> InvalidCases => F.IsDefaultInt32.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be the default value."));
    }

    public static class NotDefaultString
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsDefaultString.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsDefaultString.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be the default value."));
    }

    public static class NullOrDefaultNullableInt32
    {
        public static TheoryData<MustCase<int?>> ValidCases => F.IsNullOrDefaultNullableInt32.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<int?>> InvalidCases => F.IsNullOrDefaultNullableInt32.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be null or the default value."));
    }

    public static class NotNullOrDefaultNullableInt32
    {
        public static TheoryData<MustCase<int?>> ValidCases => F.IsNullOrDefaultNullableInt32.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<int?>> InvalidCases => F.IsNullOrDefaultNullableInt32.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be null or the default value."));
    }
}
