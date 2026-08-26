using PineGuard.Core.UnitTests.MustClauses.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustPropertyRuleTestData
{
    public static class PropertyPath
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class FluentBuilders
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class Overrides
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class NullArgumentGuards
    {
        public static TheoryData<Case> Cases =>
        [
            new("When null condition", NullWhen, new ExpectedException(typeof(ArgumentNullException), "condition")),
            new("Unless null condition", NullUnless, new ExpectedException(typeof(ArgumentNullException), "condition")),
            new("WithCode null code", NullCode, new ExpectedException(typeof(ArgumentNullException), "code")),
            new("WithCode empty code", EmptyCode, new ExpectedException(typeof(ArgumentException), "code")),
            new("WithMessage null template", NullMessage, new ExpectedException(typeof(ArgumentNullException), "messageTemplate")),
            new("WithPropertyPath null path", NullPropertyPath, new ExpectedException(typeof(ArgumentNullException), "propertyPath"))
        ];

        private static MustPropertyRule<OrderLine, string?> NewRule() =>
            new InlineMustValidator<OrderLine>().RuleFor(x => x.Sku, sku => MustResult<string>.Ok(sku));

        private static void NullWhen() => NewRule().When(null!);
        private static void NullUnless() => NewRule().Unless(null!);
        private static void NullCode() => NewRule().WithCode(null!);
        private static void EmptyCode() => NewRule().WithCode(string.Empty);
        private static void NullMessage() => NewRule().WithMessage(null!);
        private static void NullPropertyPath() => NewRule().WithPropertyPath(null!);

        public sealed record Case(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
