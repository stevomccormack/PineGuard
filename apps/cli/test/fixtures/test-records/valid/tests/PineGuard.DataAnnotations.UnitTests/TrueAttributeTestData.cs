using PineGuard.Testing.UnitTests.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

/// <summary>
/// test-records VIBE valid/ case (c): DataAnnotations' one documented
/// exception ("Pattern E — TypeMismatch Throws",
/// docs/ai/specs/data-annotations/unit-test.md) — a private
/// <c>ActionThrowsCase : ThrowsCase&lt;Action&gt;</c> declared inside a
/// <c>*TypeMismatch</c> Op Group is explicitly sanctioned even though
/// DataAnnotations otherwise forbids every other locally-declared case
/// record. Zero findings expected.
/// </summary>
public static class TrueAttributeTestData
{
    public static class TrueAttr
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("true", true, new DataAnnotationExpected(true)),
            new("false", false, new DataAnnotationExpected(false, "Value must be true.")),
        ];
    }

    public static class TrueAttrTypeMismatch
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase(
                "string-value",
                () => new TrueAttribute().GetValidationResult("not a bool", new ValidationContext(new object())),
                new ExpectedException(typeof(InvalidOperationException))),
        ];
    }
}
