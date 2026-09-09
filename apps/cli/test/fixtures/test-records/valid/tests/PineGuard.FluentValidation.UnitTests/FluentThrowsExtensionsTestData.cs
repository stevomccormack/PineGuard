using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.FluentValidation.UnitTests;

/// <summary>
/// test-records VIBE valid/ case (d): a <c>ThrowsCase&lt;&gt;</c>-derived
/// record in a layer whose addendum otherwise forbids locally-declared case
/// records. FluentValidation's prohibition is scoped to replacements for its
/// own case type — "Never define <c>ValidCase</c>, <c>NullCase</c>,
/// <c>Args</c>, or any other local record extending
/// <c>ReturnCase&lt;T, bool&gt;</c>" — and a throws row carries an
/// <see cref="ExpectedException"/>, not a <c>FluentExpected</c>, so it has no
/// shared layer equivalent to replace. It is the hand-built shape the root
/// spec keeps for throwing members (unit-test.md §4.2 "Standard Definitions"
/// patterns 2 and 3, §8.2's <c>FooRulesTestData.Parse</c>) and that the
/// DataAnnotations addendum sanctions by name in its "Pattern E". The repo
/// declares exactly this in four FluentValidation adapter-infrastructure
/// TestData files. Zero findings expected.
/// </summary>
public static class FluentThrowsExtensionsTestData
{
    public static class ValidateContext
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase(
                "null-context",
                () => new MustChildValidator<object>(null!).Validate(null!),
                new ExpectedException(typeof(ArgumentNullException), "context")),
        ];
    }
}
