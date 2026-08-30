using FluentValidation;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public static class RuleBuilderExtensionTestData
{
    public const string CityCode = "text.content.blank";
    public const string CityMessage = "City must not be null or whitespace.";

    public const string AddressCode = "address.postal-code.mismatch";
    public const string AddressMessage = "Address must carry a postal code when a city is given.";

    private const string ValidCity = "Wellington";
    private const string BlankCity = "   ";

    public sealed record Address(string? City, string? PostalCode = "6011");

    public sealed record Order(Address? ShipTo, IReadOnlyList<Address> Lines);

    /// <summary>Fails on a nested property, so the failure arrives at <c>ShipTo.City</c>.</summary>
    public sealed class AddressMustValidator : MustValidator<Address>
    {
        public AddressMustValidator() =>
            RuleFor(x => x.City, city => string.IsNullOrWhiteSpace(city)
                ? MustResult<string>.Fail(CityCode, CityMessage, nameof(city), city)
                : MustResult<string>.Ok(city, city, nameof(city)));
    }

    /// <summary>Fails on the object itself, so the failure arrives at <c>ShipTo</c>.</summary>
    public sealed class AddressRootMustValidator : MustValidator<Address>
    {
        public AddressRootMustValidator() =>
            RuleFor(x => x, address => string.IsNullOrWhiteSpace(address.PostalCode)
                ? MustResult<Address>.Fail(AddressCode, AddressMessage, nameof(address), address)
                : MustResult<Address>.Ok(address, address, nameof(address)));
    }

    /// <summary>The same nested rule, evaluated asynchronously.</summary>
    public sealed class AsyncAddressMustValidator : MustValidator<Address>
    {
        public AsyncAddressMustValidator() =>
            RuleForAsync(x => x.City, (city, _) => new ValueTask<MustResult<string>>(string.IsNullOrWhiteSpace(city)
                ? MustResult<string>.Fail(CityCode, CityMessage, nameof(city), city)
                : MustResult<string>.Ok(city, city, nameof(city))));
    }

    public sealed class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.ShipTo).SetMustValidator(new AddressMustValidator());
            RuleForEach(x => x.Lines).SetMustValidator(new AddressMustValidator());
        }
    }

    public sealed class RootOrderValidator : AbstractValidator<Order>
    {
        public RootOrderValidator() => RuleFor(x => x.ShipTo).SetMustValidator(new AddressRootMustValidator());
    }

    public sealed class AsyncOrderValidator : AbstractValidator<Order>
    {
        public AsyncOrderValidator() => RuleFor(x => x.ShipTo).SetMustValidator(new AsyncAddressMustValidator());
    }

    private static Order OrderWith(Address? shipTo, params Address[] lines) => new(shipTo, lines);

    public static class SetMustValidator
    {
        public static TheoryData<Case> Cases =>
        [
            new("an address passing every nested rule succeeds", (new OrderValidator(), OrderWith(new Address(ValidCity))), new ValidationBridgeExpected(true, [])),
            new("a nested failure is re-rooted under the property path", (new OrderValidator(), OrderWith(new Address(BlankCity))), new ValidationBridgeExpected(false, [("ShipTo.City", CityCode, CityMessage)])),
            new("a null property is skipped, as SetValidator skips one", (new OrderValidator(), OrderWith(null)), new ValidationBridgeExpected(true, [])),
            new("a failure the nested validator reported at its own root lands on the property", (new RootOrderValidator(), OrderWith(new Address(ValidCity, null))), new ValidationBridgeExpected(false, [("ShipTo", AddressCode, AddressMessage)])),
            new("every collection element carries its index", (new OrderValidator(), OrderWith(new Address(ValidCity), new Address(BlankCity), new Address(ValidCity), new Address(null))), new ValidationBridgeExpected(false, [("Lines[0].City", CityCode, CityMessage), ("Lines[2].City", CityCode, CityMessage)])),
            new("a nested failure and a collection failure are aggregated in registration order", (new OrderValidator(), OrderWith(new Address(BlankCity), new Address(BlankCity))), new ValidationBridgeExpected(false, [("ShipTo.City", CityCode, CityMessage), ("Lines[0].City", CityCode, CityMessage)]))
        ];

        public static TheoryData<Case> AsyncCases =>
        [
            new("an address passing every async nested rule succeeds", (new AsyncOrderValidator(), OrderWith(new Address(ValidCity))), new ValidationBridgeExpected(true, [])),
            new("an async nested failure is re-rooted under the property path", (new AsyncOrderValidator(), OrderWith(new Address(BlankCity))), new ValidationBridgeExpected(false, [("ShipTo.City", CityCode, CityMessage)])),
            new("a null property is skipped on the async path too", (new AsyncOrderValidator(), OrderWith(null)), new ValidationBridgeExpected(true, [])),
            new("the synchronous rules run unchanged when the parent is validated asynchronously", (new OrderValidator(), OrderWith(new Address(BlankCity))), new ValidationBridgeExpected(false, [("ShipTo.City", CityCode, CityMessage)]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-rule-builder", () => RuleBuilderExtension.SetMustValidator<Order, Address>(null!, new AddressMustValidator()), new ExpectedException(typeof(ArgumentNullException), "ruleBuilder")),
            new ActionThrowsCase("null-validator", () => new InlineOrderValidator().Attach(null!), new ExpectedException(typeof(ArgumentNullException), "validator")),
            new ActionThrowsCase("a validator with async rules cannot answer synchronously", () => new AsyncOrderValidator().Validate(OrderWith(new Address(ValidCity))), new ExpectedException(typeof(InvalidOperationException)))
        ];

        public sealed record Case(string Name, (IValidator<Order> validator, Order order) Value, ValidationBridgeExpected Expected)
            : ReturnCase<(IValidator<Order> validator, Order order), ValidationBridgeExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        /// <summary>Exposes a live rule builder so the null-validator guard can be reached the way a consumer would.</summary>
        private sealed class InlineOrderValidator : AbstractValidator<Order>
        {
            public void Attach(IMustValidator<Address> validator) => RuleFor(x => x.ShipTo).SetMustValidator(validator);
        }
    }
}
