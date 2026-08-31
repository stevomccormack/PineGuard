#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Validation;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustValidatableInfoResolverTestData
{
    public static class TryGetValidatableTypeInfo
    {
        public static TheoryData<Case> Cases =>
        [
            new("a-type-with-a-validator-is-claimed", typeof(CreateOrder), new ClaimExpected(true)),
            new("a-type-without-a-validator-is-claimed-too", typeof(Customer), new ClaimExpected(true)),
            new("a-primitive-is-claimed-and-costs-nothing", typeof(string), new ClaimExpected(true))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-type", () => new MustValidatableInfoResolver().TryGetValidatableTypeInfo(null!, out _), new ExpectedException(typeof(ArgumentNullException), "type"))
        ];

        /// <summary>
        /// Whether the resolver claimed what it was asked about.
        /// </summary>
        public sealed record ClaimExpected(bool IsValid) : ReturnExpected(IsValid);

        public sealed record Case(string Name, Type Value, ClaimExpected Expected)
            : ReturnCase<Type, ClaimExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class TryGetValidatableParameterInfo
    {
        public static TheoryData<Case> Cases =>
        [
            new("a-parameter-is-declined-because-its-type-is-already-claimed", Parameter(nameof(SampleEndpoints.WithValidatedParameter)), new TryGetValidatableTypeInfo.ClaimExpected(false)),
            new("an-unvalidated-parameter-is-declined-as-well", Parameter(nameof(SampleEndpoints.WithoutValidatedParameter)), new TryGetValidatableTypeInfo.ClaimExpected(false))
        ];

        private static ParameterInfo Parameter(string handler) => SampleEndpoints.Handler(handler).GetParameters()[0];

        public sealed record Case(string Name, ParameterInfo Value, TryGetValidatableTypeInfo.ClaimExpected Expected)
            : ReturnCase<ParameterInfo, TryGetValidatableTypeInfo.ClaimExpected>(Name, Value, Expected);
    }

    public static class ValidateAsync
    {
        public static TheoryData<Case> Cases =>
        [
            new("a-valid-value-records-nothing", (CreateOrder.Valid, typeof(CreateOrder), Validators, "", PineGuardOnly), new ValidationErrorsExpected(true)),
            new("a-type-without-a-validator-records-nothing", (new Customer(), typeof(Customer), Validators, "", PineGuardOnly), new ValidationErrorsExpected(true)),
            new("a-null-value-records-nothing", (null, typeof(CreateOrder), Validators, "", PineGuardOnly), new ValidationErrorsExpected(true)),
            new("an-invalid-value-records-every-failure", (CreateOrder.Invalid, typeof(CreateOrder), Validators, "", PineGuardOnly), new ValidationErrorsExpected(false, ["Email", "Lines[1].Sku"], ["Email must be a valid email address.", "Lines[1].Sku must not be null or whitespace."])),
            new("a-nested-path-prefixes-every-key", (CreateOrder.Invalid, typeof(CreateOrder), Validators, "Order", PineGuardOnly), new ValidationErrorsExpected(false, ["Order.Email", "Order.Lines[1].Sku"])),
            new("a-root-failure-is-published-under-the-current-path", (CreateOrder.Invalid, typeof(CreateOrder), ValidatorsWithConsistency, "Order", PineGuardOnly), new ValidationErrorsExpected(false, ["Order.Email", "Order.Lines[1].Sku", "Order"])),
            new("a-root-failure-at-the-root-path-keys-on-the-empty-string", (CreateOrder.Invalid, typeof(CreateOrder), ValidatorsWithConsistency, "", PineGuardOnly), new ValidationErrorsExpected(false, ["Email", "Lines[1].Sku", ""])),
            new("several-validators-append-to-one-key", (CreateOrder.Invalid, typeof(CreateOrder), TwiceRegisteredValidator, "", PineGuardOnly), new ValidationErrorsExpected(false, ["Email", "Lines[1].Sku"], ["Email must be a valid email address.", "Email must be a valid email address.", "Lines[1].Sku must not be null or whitespace.", "Lines[1].Sku must not be null or whitespace."])),
            new("the-next-resolver-still-runs", (CreateOrder.Invalid, typeof(CreateOrder), Validators, "", WithNextResolver), new ValidationErrorsExpected(false, ["Email", "Lines[1].Sku", NextResolverKey])),
            new("a-declining-resolver-is-passed-over", (CreateOrder.Valid, typeof(CreateOrder), Validators, "", WithDecliningResolverFirst), new ValidationErrorsExpected(false, [NextResolverKey])),
            new("without-options-nothing-is-delegated", (CreateOrder.Valid, typeof(CreateOrder), Validators, "", NoOptions), new ValidationErrorsExpected(true))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new FuncThrowsCase("null-context", () => Info().ValidateAsync(CreateOrder.Valid, null!, CancellationToken.None), new ExpectedException(typeof(ArgumentNullException), "context"))
        ];

        /// <summary>
        /// The key <see cref="SampleValidatableInfoResolver"/> records under, proving the chain continued
        /// past PineGuard's own resolver.
        /// </summary>
        private const string NextResolverKey = "next-resolver";

        private static IValidatableInfo Info()
        {
            new MustValidatableInfoResolver().TryGetValidatableTypeInfo(typeof(CreateOrder), out var info);

            return info!;
        }

        private static void Validators(IServiceCollection services) => services.AddMustValidator<CreateOrderValidator>();

        private static void ValidatorsWithConsistency(IServiceCollection services)
        {
            services.AddMustValidator<CreateOrderValidator>();
            services.AddMustValidator<CreateOrderConsistencyValidator>();
        }

        private static void TwiceRegisteredValidator(IServiceCollection services)
        {
            services.AddMustValidator<CreateOrderValidator>();
            services.AddMustValidator<CreateOrderValidator>();
        }

        /// <summary>
        /// An application that registered nothing but PineGuard's resolver: the chain is walked, finds only
        /// resolvers to skip, and ends.
        /// </summary>
        private static ValidationOptions? PineGuardOnly() => new ValidationOptions().AddMustValidatorResolver();

        private static ValidationOptions? WithNextResolver()
        {
            var options = new ValidationOptions().AddMustValidatorResolver();
            options.Resolvers.Add(new SampleValidatableInfoResolver(NextResolverKey));

            return options;
        }

        /// <summary>
        /// A resolver that declines every type sits ahead of one that claims them, so the walk has to pass
        /// over the first to reach the second.
        /// </summary>
        private static ValidationOptions? WithDecliningResolverFirst()
        {
            var options = new ValidationOptions().AddMustValidatorResolver();
            options.Resolvers.Add(new SampleValidatableInfoResolver(errorKey: null));
            options.Resolvers.Add(new SampleValidatableInfoResolver(NextResolverKey));

            return options;
        }

        private static ValidationOptions? NoOptions() => null;

        public sealed record ValidationErrorsExpected(bool IsValid, string[]? Keys = null, string[]? Messages = null) : ReturnExpected(IsValid);

        public sealed record Case(string Name, (object? value, Type validatedType, Action<IServiceCollection> configureServices, string currentValidationPath, Func<ValidationOptions?> validationOptions) Value, ValidationErrorsExpected Expected)
            : ReturnCase<(object? value, Type validatedType, Action<IServiceCollection> configureServices, string currentValidationPath, Func<ValidationOptions?> validationOptions), ValidationErrorsExpected>(Name, Value, Expected);

        private sealed record FuncThrowsCase(string Name, Func<Task> Value, ExpectedException ExpectedException)
            : ThrowsCase<Func<Task>>(Name, Value, ExpectedException);
    }
}
#pragma warning restore ASP0029
#endif
