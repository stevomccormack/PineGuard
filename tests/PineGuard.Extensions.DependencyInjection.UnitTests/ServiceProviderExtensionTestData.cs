using Microsoft.Extensions.DependencyInjection;
using PineGuard.Extensions.DependencyInjection.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public static class ServiceProviderExtensionTestData
{
    public static class TryGetMustValidator
    {
        public static TheoryData<MustValidatorResolutionCase> Cases =>
        [
            new("registered-validator-is-found", (services => services.AddMustValidator<OrderValidator>(), typeof(Order)), new MustValidatorResolutionExpected(true, 1, typeof(OrderValidator))),
            new("multi-type-validator-is-found-for-its-first-type", (services => services.AddMustValidator<ContactValidator>(), typeof(Customer)), new MustValidatorResolutionExpected(true, 1, typeof(ContactValidator))),
            new("multi-type-validator-is-found-for-its-second-type", (services => services.AddMustValidator<ContactValidator>(), typeof(Supplier)), new MustValidatorResolutionExpected(true, 1, typeof(ContactValidator))),
            new("last-registration-wins-when-two-validators-share-a-type", (services => services.AddMustValidator<OrderValidator>().AddMustValidator<SecondOrderValidator>(), typeof(Order)), new MustValidatorResolutionExpected(true, 1, typeof(SecondOrderValidator))),
            new("type-without-a-validator-is-not-found", (services => services.AddMustValidator<OrderValidator>(), typeof(Customer)), new MustValidatorResolutionExpected(false, 0)),
            new("empty-container-finds-nothing", (_ => { }, typeof(Order)), new MustValidatorResolutionExpected(false, 0))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null-provider", () => ServiceProviderExtension.TryGetMustValidator(null!, typeof(Order), out _), new ExpectedException(typeof(ArgumentNullException), "provider")),
            new InvalidCase("null-validated-type", () => new ServiceCollection().BuildServiceProvider().TryGetMustValidator(null!, out _), new ExpectedException(typeof(ArgumentNullException), "validatedType"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class GetMustValidators
    {
        public static TheoryData<MustValidatorResolutionCase> Cases =>
        [
            new("single-registration-returns-one", (services => services.AddMustValidator<OrderValidator>(), typeof(Order)), new MustValidatorResolutionExpected(true, 1, typeof(OrderValidator))),
            new("two-registrations-return-both-in-registration-order", (services => services.AddMustValidator<OrderValidator>().AddMustValidator<SecondOrderValidator>(), typeof(Order)), new MustValidatorResolutionExpected(true, 2, typeof(OrderValidator))),
            new("multi-type-validator-is-returned-for-each-type-it-serves", (services => services.AddMustValidator<ContactValidator>(), typeof(Supplier)), new MustValidatorResolutionExpected(true, 1, typeof(ContactValidator))),
            new("type-without-a-validator-returns-empty", (services => services.AddMustValidator<OrderValidator>(), typeof(Customer)), new MustValidatorResolutionExpected(false, 0)),
            new("empty-container-returns-empty", (_ => { }, typeof(Order)), new MustValidatorResolutionExpected(false, 0))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null-provider", () => ServiceProviderExtension.GetMustValidators(null!, typeof(Order)), new ExpectedException(typeof(ArgumentNullException), "provider")),
            new InvalidCase("null-validated-type", () => new ServiceCollection().BuildServiceProvider().GetMustValidators(null!), new ExpectedException(typeof(ArgumentNullException), "validatedType"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
