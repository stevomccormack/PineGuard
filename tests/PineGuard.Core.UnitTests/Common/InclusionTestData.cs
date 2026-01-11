using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class InclusionTestData
{
    public static class DefinedValues
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("inc", Inclusion.Inclusive, 0),
            new("exc", Inclusion.Exclusive, 1),
        ];

        public static TheoryData<Case> EdgeCases => [];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, Inclusion Inclusion, int ExpectedIntValue)
            : ReturnCase<Inclusion, int>(Name, Inclusion, ExpectedIntValue);

        #endregion
    }

    public static class UndefinedValues
    {
        public static TheoryData<Case> ValidCases => [];

        public static TheoryData<Case> EdgeCases =>
        [
            new("-1", (Inclusion)(-1), -1),
            new("2", (Inclusion)2, 2),
            new("42", (Inclusion)42, 42),
            new("min", (Inclusion)int.MinValue, int.MinValue),
            new("max", (Inclusion)int.MaxValue, int.MaxValue),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, Inclusion Inclusion, int ExpectedIntValue)
            : ReturnCase<Inclusion, int>(Name, Inclusion, ExpectedIntValue);

        #endregion
    }
}
