using FluentValidation;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Address = PineGuard.FluentValidation.UnitTests.RuleBuilderExtensionTestData.Address;

namespace PineGuard.FluentValidation.UnitTests;

public static class MustChildValidatorTestData
{
    private const string ValidCity = "Wellington";
    private const string BlankCity = "   ";

    internal static MustChildValidator<Address> NewValidator() =>
        new(new RuleBuilderExtensionTestData.AddressMustValidator());

    internal static MustChildValidator<Address> NewAsyncValidator() =>
        new(new RuleBuilderExtensionTestData.AsyncAddressMustValidator());

    public static class Validate
    {
        public static TheoryData<ValidationBridgeCase<Address?>> Cases =>
        [
            new("a null instance is valid — presence is a separate rule", null, new ValidationBridgeExpected(true, [])),
            new("an address passing every rule is valid", new Address(ValidCity), new ValidationBridgeExpected(true, [])),
            new("a failing address reports the failure unrooted", new Address(BlankCity), new ValidationBridgeExpected(false, [("City", RuleBuilderExtensionTestData.CityCode, RuleBuilderExtensionTestData.CityMessage)]))
        ];
    }

    public static class ValidateAsync
    {
        public static TheoryData<ValidationBridgeCase<Address?>> Cases =>
        [
            new("a null instance is valid on the async path too", null, new ValidationBridgeExpected(true, [])),
            new("an address passing every async rule is valid", new Address(ValidCity), new ValidationBridgeExpected(true, [])),
            new("a failing address reports the async failure unrooted", new Address(BlankCity), new ValidationBridgeExpected(false, [("City", RuleBuilderExtensionTestData.CityCode, RuleBuilderExtensionTestData.CityMessage)]))
        ];
    }

    public static class ValidateContext
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-context", () => NewValidator().Validate((IValidationContext)null!), new ExpectedException(typeof(ArgumentNullException), "context")),
            new ActionThrowsCase("null-context-async", () => _ = NewValidator().ValidateAsync((IValidationContext)null!, CancellationToken.None), new ExpectedException(typeof(ArgumentNullException), "context"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class CanValidateInstancesOfType
    {
        public static TheoryData<TypeCase> Cases =>
        [
            new("the validated type itself", typeof(Address), true),
            new("an unrelated type", typeof(string), false)
        ];

        public sealed record TypeCase(string Name, Type Value, bool Expected)
            : ReturnCase<Type, bool>(Name, Value, Expected);
    }

    public static class CreateDescriptor
    {
        public static TheoryData<bool> Cases => [true];
    }
}
