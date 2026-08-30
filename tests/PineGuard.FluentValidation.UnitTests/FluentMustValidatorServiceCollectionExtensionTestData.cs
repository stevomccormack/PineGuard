using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Customer = PineGuard.FluentValidation.UnitTests.FluentMustValidatorTestData.Customer;
using CustomerValidator = PineGuard.FluentValidation.UnitTests.FluentMustValidatorTestData.CustomerValidator;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentMustValidatorServiceCollectionExtensionTestData
{
    public static readonly Customer InvalidCustomer = new("not-an-email", "   ");

    private static ServiceDescriptor SingletonValidator => ServiceDescriptor.Singleton<IValidator<Customer>, CustomerValidator>();
    private static ServiceDescriptor ScopedValidator => ServiceDescriptor.Scoped<IValidator<Customer>, CustomerValidator>();
    private static ServiceDescriptor TransientValidator => ServiceDescriptor.Transient<IValidator<Customer>, CustomerValidator>();
    private static ServiceDescriptor TextValidatorDescriptor => ServiceDescriptor.Singleton<IValidator<string>, TextValidator>();
    private static ServiceDescriptor NonGenericService => ServiceDescriptor.Singleton(typeof(string), "a non-validator registration");
    private static ServiceDescriptor OtherGenericService => ServiceDescriptor.Singleton<IComparer<int>>(Comparer<int>.Default);
    private static ServiceDescriptor OpenGenericValidator => ServiceDescriptor.Transient(typeof(IValidator<>), typeof(PassThroughValidator<>));
    private static ServiceDescriptor HandWrittenMustValidatorDescriptor => ServiceDescriptor.Singleton<IMustValidator<Customer>, HandWrittenMustValidator>();

    public static class AddMustValidatorsFromFluentValidators
    {
        public static TheoryData<Case> Cases =>
        [
            new("an empty collection stays empty", ([], 1), new RegistrationExpected(false, 0, 0)),
            new("a registered validator becomes a Must validator", ([SingletonValidator], 1), new RegistrationExpected(true, 1, 1, ServiceLifetime.Singleton)),
            new("a scoped validator produces a scoped adapter", ([ScopedValidator], 1), new RegistrationExpected(true, 1, 1, ServiceLifetime.Scoped)),
            new("a transient validator produces a transient adapter", ([TransientValidator], 1), new RegistrationExpected(true, 1, 1, ServiceLifetime.Transient)),
            new("every validated type gets its own adapter", ([SingletonValidator, TextValidatorDescriptor], 1), new RegistrationExpected(true, 2, 1, ServiceLifetime.Singleton)),
            new("a non-generic registration is ignored", ([NonGenericService], 1), new RegistrationExpected(false, 0, 0)),
            new("a generic registration that is not a validator is ignored", ([OtherGenericService], 1), new RegistrationExpected(false, 0, 0)),
            new("an open generic validator registration is ignored", ([OpenGenericValidator], 1), new RegistrationExpected(false, 0, 0)),
            new("a second call adds nothing", ([SingletonValidator], 2), new RegistrationExpected(true, 1, 1, ServiceLifetime.Singleton)),
            new("a hand-written Must validator is left in place", ([SingletonValidator, HandWrittenMustValidatorDescriptor], 1), new RegistrationExpected(true, 1, 2, ServiceLifetime.Singleton))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null services", () => _ = ((IServiceCollection)null!).AddMustValidatorsFromFluentValidators(), new ExpectedException(typeof(ArgumentNullException), "services"))
        ];

        public sealed record Case(string Name, (ServiceDescriptor[] descriptors, int calls) Value, RegistrationExpected Expected)
            : ReturnCase<(ServiceDescriptor[] descriptors, int calls), RegistrationExpected>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public sealed record RegistrationExpected(bool IsValid, int AdapterCount, int CustomerValidatorCount, ServiceLifetime? Lifetime = null)
            : ReturnExpected(IsValid);
    }

    public sealed class TextValidator : AbstractValidator<string>;

    public sealed class PassThroughValidator<T> : AbstractValidator<T>;

    public sealed class HandWrittenMustValidator : IMustValidator<Customer>
    {
        public MustValidationResult Validate(Customer value) => MustValidationResult.Ok();

        public ValueTask<MustValidationResult> ValidateAsync(Customer value, CancellationToken cancellationToken = default) =>
            new(MustValidationResult.Ok());
    }
}
