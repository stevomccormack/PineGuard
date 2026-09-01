using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class EndpointConventionBuilderExtensionTestData
{
    public static class CreateFilter
    {
        public static TheoryData<Case> Cases =>
        [
            new("a-validated-parameter-attaches-the-filter", (SampleEndpoints.Handler(nameof(SampleEndpoints.WithValidatedParameter)), Validators), new FilterFactoryExpected(true)),
            new("a-later-validated-parameter-attaches-the-filter", (SampleEndpoints.Handler(nameof(SampleEndpoints.WithSecondParameterValidated)), Validators), new FilterFactoryExpected(true)),
            new("no-validated-parameter-hands-the-pipeline-back-untouched", (SampleEndpoints.Handler(nameof(SampleEndpoints.WithoutValidatedParameter)), Validators), new FilterFactoryExpected(false)),
            new("no-parameters-at-all-hands-the-pipeline-back-untouched", (SampleEndpoints.Handler(nameof(SampleEndpoints.WithoutParameters)), Validators), new FilterFactoryExpected(false)),
            new("no-validators-registered-hands-the-pipeline-back-untouched", (SampleEndpoints.Handler(nameof(SampleEndpoints.WithValidatedParameter)), NoValidators), new FilterFactoryExpected(false))
        ];

        private static void NoValidators(IServiceCollection services) => _ = services;

        /// <summary>
        /// Registers a validator for every parameter type the sample handlers declare as validated —
        /// <see cref="CreateOrder"/> for the first-parameter case and <see cref="SearchQuery"/> for the
        /// later-parameter one.
        /// </summary>
        private static void Validators(IServiceCollection services)
        {
            services.AddMustValidator<CreateOrderValidator>();
            services.AddMustValidator<SearchQueryValidator>();
        }

        public sealed record FilterFactoryExpected(bool IsValid) : ReturnExpected(IsValid);

        public sealed record Case(string Name, (MethodInfo methodInfo, Action<IServiceCollection> configureServices) Value, FilterFactoryExpected Expected)
            : ReturnCase<(MethodInfo methodInfo, Action<IServiceCollection> configureServices), FilterFactoryExpected>(Name, Value, Expected);
    }

    public static class CreateFilterWithoutServiceProbe
    {
        public static TheoryData<Case> Cases =>
        [
            new("a-container-that-cannot-answer-attaches-the-filter", SampleEndpoints.Handler(nameof(SampleEndpoints.WithoutValidatedParameter)), new CreateFilter.FilterFactoryExpected(true))
        ];

        public sealed record Case(string Name, MethodInfo Value, CreateFilter.FilterFactoryExpected Expected)
            : ReturnCase<MethodInfo, CreateFilter.FilterFactoryExpected>(Name, Value, Expected);
    }

    public static class AddMustValidation
    {
        public static TheoryData<Case> Cases =>
        [
            new("one-call-adds-one-filter-factory", 1, new FilterCountExpected(true, 1)),
            new("a-second-call-adds-a-second-filter-factory", 2, new FilterCountExpected(true, 2))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-builder", () => EndpointConventionBuilderExtension.AddMustValidation<SampleEndpointConventionBuilder>(null!), new ExpectedException(typeof(ArgumentNullException), "builder"))
        ];

        public sealed record FilterCountExpected(bool IsValid, int FilterFactoryCount) : ReturnExpected(IsValid);

        public sealed record Case(string Name, int Value, FilterCountExpected Expected)
            : ReturnCase<int, FilterCountExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
