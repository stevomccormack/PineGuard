using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.PhoneRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class PhoneAttributesTestData
{
    public static class PhoneNumberTypeMismatch
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("int-value", () => new PhoneNumberAttribute().GetValidationResult(123, new ValidationContext(new object()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException)))
        ];
    }

    // DefaultPhoneNumber — uses IsPhoneNumber fixture; null is skipped by DA layer
    public static class DefaultPhoneNumber
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsPhoneNumber.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsPhoneNumber.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, Code: MustCodes.Phone.Number.Invalid)
        });
    }

    public static class CustomPhoneNumber
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("min", "123", new DataAnnotationExpected(true)),
            new("max", "12345", new DataAnnotationExpected(true)),
            new("null", null, new DataAnnotationExpected(true)),
            new("short", "12", new DataAnnotationExpected(false)),
            new("long", "123456", new DataAnnotationExpected(false))
        ];
    }
}
