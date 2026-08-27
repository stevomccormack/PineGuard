using PineGuard.Codes;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardValidatorClausesTestData
{
    public sealed record Widget(string Name, int Count);

    public static InlineMustValidator<Widget> NewValidator()
    {
        var validator = new InlineMustValidator<Widget>();
        validator.RuleFor(x => x.Name, name => Must.Be.NotEmpty(name));
        validator.RuleFor(x => x.Count, count => Must.Be.Positive(count));
        return validator;
    }

    public static class Valid
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class InvalidValue
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class NullValue
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class MultipleFailures
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class MapActive
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class NullValidator
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class ExpectedCodes
    {
        public const string NameEmpty = MustCodes.Text.Content.Empty;
        public const string CountNotPositive = MustCodes.Number.Sign.NotPositive;
    }
}
