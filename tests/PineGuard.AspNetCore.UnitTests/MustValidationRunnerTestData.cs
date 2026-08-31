using Microsoft.Extensions.DependencyInjection;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests.MustClauses;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustValidationRunnerTestData
{
    public static class ValidateAsync
    {
        public static TheoryData<MustValidationCase<(object?[] arguments, Action<IServiceCollection> configureServices, MustValidationMode mode)>> Cases =>
        [
            new("no-arguments-succeed", ([], AllValidators, MustValidationMode.Aggregate), new MustValidationExpected(true, FailureCount: 0)),
            new("a-null-argument-has-no-runtime-type-and-is-skipped", ([null], AllValidators, MustValidationMode.Aggregate), new MustValidationExpected(true, FailureCount: 0)),
            new("an-argument-with-no-validator-is-left-alone", ([new Customer()], AllValidators, MustValidationMode.Aggregate), new MustValidationExpected(true, FailureCount: 0)),
            new("no-validators-registered-leaves-every-argument-alone", ([CreateOrder.Invalid], NoValidators, MustValidationMode.Aggregate), new MustValidationExpected(true, FailureCount: 0)),
            new("a-valid-argument-succeeds", ([CreateOrder.Valid], AllValidators, MustValidationMode.Aggregate), new MustValidationExpected(true, FailureCount: 0)),
            new("an-invalid-argument-reports-every-failure", ([CreateOrder.Invalid], AllValidators, MustValidationMode.Aggregate), new MustValidationExpected(false, FailureCount: 2, PropertyPath: "Email", Code: "email.address.invalid")),
            new("two-invalid-arguments-merge-in-argument-order", ([CreateOrder.Invalid, SearchQuery.Invalid], AllValidators, MustValidationMode.Aggregate), new MustValidationExpected(false, FailureCount: 3, PropertyPath: "Email", Code: "email.address.invalid")),
            new("stop-on-first-failure-stops-at-the-first-failing-validator", ([CreateOrder.Invalid, SearchQuery.Invalid], AllValidators, MustValidationMode.StopOnFirstFailure), new MustValidationExpected(false, FailureCount: 2, PropertyPath: "Email", Code: "email.address.invalid")),
            new("stop-on-first-failure-still-reaches-a-later-failing-argument", ([CreateOrder.Valid, SearchQuery.Invalid], AllValidators, MustValidationMode.StopOnFirstFailure), new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Term", Code: "text.content.blank")),
            new("every-validator-registered-for-one-type-runs", ([CreateOrder.Invalid], TwoOrderValidators, MustValidationMode.Aggregate), new MustValidationExpected(false, FailureCount: 4, PropertyPath: "Email", Code: "email.address.invalid")),
            new("the-request-token-reaches-the-validator", ([new TokenProbe()], AllValidators, MustValidationMode.Aggregate), new MustValidationExpected(true, FailureCount: 0))
        ];

        private static void NoValidators(IServiceCollection services) => _ = services;

        private static void AllValidators(IServiceCollection services)
        {
            services.AddMustValidator<CreateOrderValidator>();
            services.AddMustValidator<SearchQueryValidator>();
            services.AddMustValidator<TokenProbeValidator>();
        }

        private static void TwoOrderValidators(IServiceCollection services)
        {
            services.AddMustValidator<CreateOrderValidator>();
            services.AddMustValidator<CreateOrderValidator>();
        }
    }
}
