using FluentValidation;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentMiscExtensionsTests : BaseUnitTest
{
    private sealed record DateModel { public DateOnly Value { get; init; } }
    private sealed record TaskModel { public Task? Value { get; init; } }
    private sealed record GenericModel<T> { public T? Value { get; init; } }

    public static class InSqlDateRange
    {
        private sealed class Validator : AbstractValidator<DateModel>
        {
            public Validator() => RuleFor(x => x.Value).InSqlDateRange();
        }

        [Theory]
        [MemberData(nameof(FluentMiscExtensionsTestData.InSqlDateRange.ValidCases), MemberType = typeof(FluentMiscExtensionsTestData.InSqlDateRange))]
        [MemberData(nameof(FluentMiscExtensionsTestData.InSqlDateRange.EdgeCases), MemberType = typeof(FluentMiscExtensionsTestData.InSqlDateRange))]
        public static void BehavesAsExpected(FluentMiscExtensionsTestData.InSqlDateRange.ValidCase testCase)
        {
            var result = new Validator().Validate(new DateModel { Value = testCase.Value });
            Assert.Equal(testCase.Expected, result.IsValid);
            if (testCase is { Expected: false, ExpectedMessage: not null })
                Assert.EndsWith(testCase.ExpectedMessage, result.Errors[0].ErrorMessage);
        }
    }

    public static class Completed
    {
        private sealed class Validator : AbstractValidator<TaskModel>
        {
            public Validator() => RuleFor(x => x.Value).Completed();
        }

        [Theory]
        [MemberData(nameof(FluentMiscExtensionsTestData.Completed.ValidCases), MemberType = typeof(FluentMiscExtensionsTestData.Completed))]
        [MemberData(nameof(FluentMiscExtensionsTestData.Completed.EdgeCases), MemberType = typeof(FluentMiscExtensionsTestData.Completed))]
        public static void BehavesAsExpected(FluentMiscExtensionsTestData.Completed.ValidCase testCase)
        {
            var result = new Validator().Validate(new TaskModel { Value = testCase.Value });
            Assert.Equal(testCase.Expected, result.IsValid);
            if (testCase is { Expected: false, ExpectedMessage: not null })
                Assert.EndsWith(testCase.ExpectedMessage, result.Errors[0].ErrorMessage);
        }
    }

    public static class Default
    {
        private sealed class Validator : AbstractValidator<GenericModel<int?>>
        {
            public Validator() => RuleFor(x => x.Value).Default();
        }

        [Theory]
        [MemberData(nameof(FluentMiscExtensionsTestData.Default.ValidCases), MemberType = typeof(FluentMiscExtensionsTestData.Default))]
        [MemberData(nameof(FluentMiscExtensionsTestData.Default.EdgeCases), MemberType = typeof(FluentMiscExtensionsTestData.Default))]
        public static void BehavesAsExpected(FluentMiscExtensionsTestData.Default.ValidCase testCase)
        {
            var result = new Validator().Validate(new GenericModel<int?> { Value = testCase.Value });
            Assert.Equal(testCase.Expected, result.IsValid);
            if (testCase is { Expected: false, ExpectedMessage: not null })
                Assert.EndsWith(testCase.ExpectedMessage, result.Errors[0].ErrorMessage);
        }
    }
}
