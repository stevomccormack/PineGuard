using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.FluentValidation.UnitTests;

/// <summary>
/// test-records VIBE invalid/ case: Fluent's addendum
/// (docs/ai/specs/fluent-validation/unit-test.md, "Explicit Prohibitions")
/// forbids every locally-declared case record outright — only
/// <see cref="FluentCase{TValue}"/> is allowed. This <c>ValidCase</c> has a
/// textbook-correct base (<c>ReturnCase&lt;DateOnly, bool&gt;</c>), which is
/// exactly the shape the old regex-based Rule52 would have silently passed
/// (it only ever checked "does it have some real base"). This rule must
/// still flag it, because Fluent forbids the record regardless of its base.
/// </summary>
public static class FluentBadExtensionsTestData
{
    public static class After
    {
        public static TheoryData<ValidCase> Cases =>
        [
            new("in-range", new DateOnly(2020, 1, 2), true, null),
            new("out-of-range", new DateOnly(2019, 12, 31), false, "Value must be after the cutoff."),
        ];

        public sealed record ValidCase(string Name, DateOnly Value, bool Expected, string? ExpectedMessage)
            : ReturnCase<DateOnly, bool>(Name, Value, Expected);
    }
}
