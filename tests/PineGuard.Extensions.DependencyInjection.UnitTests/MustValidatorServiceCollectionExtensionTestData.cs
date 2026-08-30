using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Extensions.DependencyInjection.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public static class MustValidatorServiceCollectionExtensionTestData
{
    private static readonly Assembly SampleAssembly = typeof(OrderValidator).Assembly;

    private static readonly Type[] OrderValidatorServiceTypes = [typeof(OrderValidator), typeof(IMustValidator<Order>), typeof(IMustValidator)];

    private static readonly Type[] ContactValidatorServiceTypes = [typeof(ContactValidator), typeof(IMustValidator<Customer>), typeof(IMustValidator<Supplier>), typeof(IMustValidator)];

    private static readonly Type[] SecondOrderValidatorServiceTypes = [typeof(SecondOrderValidator), typeof(IMustValidator<Order>), typeof(IMustValidator)];

    private static readonly Type[] EverySampleValidatorServiceTypes = [.. OrderValidatorServiceTypes, .. SecondOrderValidatorServiceTypes, .. ContactValidatorServiceTypes];

    public static class AddMustValidator
    {
        public static TheoryData<MustValidatorRegistrationCase<Action<IServiceCollection>>> Cases =>
        [
            new("single-type-validator-registers-concrete-closed-interface-and-non-generic", services => services.AddMustValidator<OrderValidator>(), new MustValidatorRegistrationExpected(true, OrderValidatorServiceTypes, ServiceLifetime.Singleton)),
            new("multi-type-validator-registers-every-closed-interface-once", services => services.AddMustValidator<ContactValidator>(), new MustValidatorRegistrationExpected(true, ContactValidatorServiceTypes, ServiceLifetime.Singleton)),
            new("scoped-lifetime-is-honoured-on-every-descriptor", services => services.AddMustValidator<OrderValidator>(ServiceLifetime.Scoped), new MustValidatorRegistrationExpected(true, OrderValidatorServiceTypes, ServiceLifetime.Scoped)),
            new("transient-lifetime-is-honoured-on-every-descriptor", services => services.AddMustValidator<OrderValidator>(ServiceLifetime.Transient), new MustValidatorRegistrationExpected(true, OrderValidatorServiceTypes, ServiceLifetime.Transient)),
            new("registering-the-same-validator-twice-adds-two-sets", services => services.AddMustValidator<OrderValidator>().AddMustValidator<OrderValidator>(), new MustValidatorRegistrationExpected(true, [.. OrderValidatorServiceTypes, .. OrderValidatorServiceTypes], ServiceLifetime.Singleton))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null-services", () => MustValidatorServiceCollectionExtension.AddMustValidator<OrderValidator>(null!), new ExpectedException(typeof(ArgumentNullException), "services"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class AddMustValidatorsFromAssembly
    {
        public static TheoryData<MustValidatorRegistrationCase<(Assembly assembly, ServiceLifetime lifetime, Func<Type, bool>? filter)>> Cases =>
        [
            new("unfiltered-scan-registers-every-concrete-validator", (SampleAssembly, ServiceLifetime.Singleton, null), new MustValidatorRegistrationExpected(true, EverySampleValidatorServiceTypes, ServiceLifetime.Singleton)),
            new("filter-narrows-the-scan-to-one-validator", (SampleAssembly, ServiceLifetime.Singleton, type => type == typeof(ContactValidator)), new MustValidatorRegistrationExpected(true, ContactValidatorServiceTypes, ServiceLifetime.Singleton)),
            new("filter-rejecting-everything-registers-nothing", (SampleAssembly, ServiceLifetime.Singleton, _ => false), new MustValidatorRegistrationExpected(true, [], ServiceLifetime.Singleton)),
            new("scoped-lifetime-is-honoured-on-every-descriptor", (SampleAssembly, ServiceLifetime.Scoped, null), new MustValidatorRegistrationExpected(true, EverySampleValidatorServiceTypes, ServiceLifetime.Scoped))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null-services", () => MustValidatorServiceCollectionExtension.AddMustValidatorsFromAssembly(null!, SampleAssembly), new ExpectedException(typeof(ArgumentNullException), "services")),
            new InvalidCase("null-assembly", () => new ServiceCollection().AddMustValidatorsFromAssembly(null!), new ExpectedException(typeof(ArgumentNullException), "assembly"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class AddMustValidatorsFromAssemblies
    {
        public static TheoryData<MustValidatorRegistrationCase<(IEnumerable<Assembly> assemblies, ServiceLifetime lifetime, Func<Type, bool>? filter)>> Cases =>
        [
            new("single-assembly-matches-the-single-assembly-overload", (new[] { SampleAssembly }, ServiceLifetime.Singleton, null), new MustValidatorRegistrationExpected(true, EverySampleValidatorServiceTypes, ServiceLifetime.Singleton)),
            new("same-assembly-listed-twice-registers-twice", (new[] { SampleAssembly, SampleAssembly }, ServiceLifetime.Singleton, null), new MustValidatorRegistrationExpected(true, [.. EverySampleValidatorServiceTypes, .. EverySampleValidatorServiceTypes], ServiceLifetime.Singleton)),
            new("empty-sequence-registers-nothing", (Array.Empty<Assembly>(), ServiceLifetime.Singleton, null), new MustValidatorRegistrationExpected(true, [], ServiceLifetime.Singleton)),
            new("filter-and-lifetime-reach-every-assembly", (new[] { SampleAssembly }, ServiceLifetime.Transient, type => type == typeof(OrderValidator)), new MustValidatorRegistrationExpected(true, OrderValidatorServiceTypes, ServiceLifetime.Transient))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null-services", () => MustValidatorServiceCollectionExtension.AddMustValidatorsFromAssemblies(null!, [SampleAssembly]), new ExpectedException(typeof(ArgumentNullException), "services")),
            new InvalidCase("null-assemblies", () => new ServiceCollection().AddMustValidatorsFromAssemblies(null!), new ExpectedException(typeof(ArgumentNullException), "assemblies"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class AddMustValidatorsFromAssemblyContaining
    {
        public static TheoryData<MustValidatorRegistrationCase<Action<IServiceCollection>>> Cases =>
        [
            new("marker-type-resolves-its-own-assembly", services => services.AddMustValidatorsFromAssemblyContaining<OrderValidator>(), new MustValidatorRegistrationExpected(true, EverySampleValidatorServiceTypes, ServiceLifetime.Singleton)),
            new("lifetime-and-filter-are-forwarded", services => services.AddMustValidatorsFromAssemblyContaining<Order>(ServiceLifetime.Scoped, type => type == typeof(ContactValidator)), new MustValidatorRegistrationExpected(true, ContactValidatorServiceTypes, ServiceLifetime.Scoped))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null-services", () => MustValidatorServiceCollectionExtension.AddMustValidatorsFromAssemblyContaining<OrderValidator>(null!), new ExpectedException(typeof(ArgumentNullException), "services"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class IsMustValidatorType
    {
        public static TheoryData<Case> Cases =>
        [
            new("single-type-validator", typeof(OrderValidator), true),
            new("multi-type-validator", typeof(ContactValidator), true),
            new("closed-generic-validator", typeof(OpenGenericValidator<Order>), true),
            new("interface-is-not-a-class", typeof(IMustValidator), false),
            new("value-type-is-not-a-class", typeof(int), false),
            new("abstract-validator", typeof(AbstractOrderValidator), false),
            new("open-generic-validator", typeof(OpenGenericValidator<>), false),
            new("class-that-is-not-a-validator", typeof(NotAValidator), false)
        ];

        public sealed record Case(string Name, Type Value, bool Expected)
            : ReturnCase<Type, bool>(Name, Value, Expected);
    }

    public static class IsMustValidatorInterface
    {
        public static TheoryData<Case> Cases =>
        [
            new("closed-must-validator-interface", typeof(IMustValidator<Order>), true),
            new("non-generic-must-validator-interface", typeof(IMustValidator), false),
            new("unrelated-generic-interface", typeof(IEnumerable<Order>), false)
        ];

        public sealed record Case(string Name, Type Value, bool Expected)
            : ReturnCase<Type, bool>(Name, Value, Expected);
    }
}
