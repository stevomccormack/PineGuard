using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustValidationServiceCollectionExtensionTestData
{
    public static class AddMustValidation
    {
        /// <summary>
        /// The assembly the sample validators live in — the one an application would hand its own
        /// <c>typeof(Program).Assembly</c> for.
        /// </summary>
        private static Assembly ValidatorAssembly => typeof(CreateOrderValidator).Assembly;

        public static TheoryData<Case> Cases =>
        [
            new("the-assembly-overload-registers-the-validators-with-default-options", static services => services.AddMustValidation(ValidatorAssembly), new RegistrationExpected(true, IncludeCodes: true, HasOrderValidator: true, typeof(DefaultMustFailureMessageResolver))),
            new("the-configure-overload-applies-the-options", static services => services.AddMustValidation(static options => options.IncludeCodes = false, ValidatorAssembly), new RegistrationExpected(true, IncludeCodes: false, HasOrderValidator: true, typeof(DefaultMustFailureMessageResolver))),
            new("no-assemblies-registers-no-validators", static services => services.AddMustValidation(), new RegistrationExpected(true, IncludeCodes: true, HasOrderValidator: false, typeof(DefaultMustFailureMessageResolver))),
            new("a-resolver-registered-first-is-kept", static services => services.AddSingleton<IMustFailureMessageResolver, StringLocalizerMustFailureMessageResolver>().AddMustValidation(), new RegistrationExpected(true, IncludeCodes: true, HasOrderValidator: false, typeof(StringLocalizerMustFailureMessageResolver)))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-services-on-the-assembly-overload", static () => MustValidationServiceCollectionExtension.AddMustValidation(null!, ValidatorAssembly), new ExpectedException(typeof(ArgumentNullException), "services")),
            new ActionThrowsCase("null-services-on-the-configure-overload", static () => MustValidationServiceCollectionExtension.AddMustValidation(null!, static _ => { }, ValidatorAssembly), new ExpectedException(typeof(ArgumentNullException), "services")),
            new ActionThrowsCase("null-configure", static () => new ServiceCollection().AddMustValidation((Action<MustValidationOptions>)null!, ValidatorAssembly), new ExpectedException(typeof(ArgumentNullException), "configure")),
            new ActionThrowsCase("null-assemblies-on-the-assembly-overload", static () => new ServiceCollection().AddMustValidation((Assembly[])null!), new ExpectedException(typeof(ArgumentNullException), "validatorAssemblies")),
            new ActionThrowsCase("null-assemblies-on-the-configure-overload", static () => new ServiceCollection().AddMustValidation(static _ => { }, null!), new ExpectedException(typeof(ArgumentNullException), "validatorAssemblies"))
        ];

        public sealed record RegistrationExpected(bool IsValid, bool IncludeCodes, bool HasOrderValidator, Type ResolverType) : ReturnExpected(IsValid);

        public sealed record Case(string Name, Func<IServiceCollection, IServiceCollection> Value, RegistrationExpected Expected)
            : ReturnCase<Func<IServiceCollection, IServiceCollection>, RegistrationExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
