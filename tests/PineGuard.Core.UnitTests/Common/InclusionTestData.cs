using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Common;

public static class InclusionTestData
{
    public static class DefinedValues
    {
        private static ValidCase V(string name, Inclusion inclusion, int expectedIntValue)
            => new(Name: name, Inclusion: inclusion, ExpectedIntValue: expectedIntValue);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("inc", Inclusion.Inclusive, 0) },
            { V("exc", Inclusion.Exclusive, 1) },
        };

        #region Cases

        public record Case(string Name, Inclusion Inclusion);

        public sealed record ValidCase(string Name, Inclusion Inclusion, int ExpectedIntValue)
            : Case(Name, Inclusion);

        #endregion
    }

    public static class UndefinedValues
    {
        private static ValidCase V(string name, Inclusion inclusion, int expectedIntValue)
            => new(Name: name, Inclusion: inclusion, ExpectedIntValue: expectedIntValue);

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("-1", (Inclusion)(-1), -1) },
            { V("2", (Inclusion)2, 2) },
            { V("42", (Inclusion)42, 42) },
            { V("min", (Inclusion)int.MinValue, int.MinValue) },
            { V("max", (Inclusion)int.MaxValue, int.MaxValue) },
        };

        #region Cases

        public record Case(string Name, Inclusion Inclusion);

        public sealed record ValidCase(string Name, Inclusion Inclusion, int ExpectedIntValue)
            : Case(Name, Inclusion);

        #endregion
    }
}
