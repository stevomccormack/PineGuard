using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests.Common;

public sealed class FluentExtensionTests : BaseUnitTest
{
    public static class MustBe
    {
        [Theory]
        [MemberData(nameof(FluentExtensionTestData.MustBe.ValidCases), MemberType = typeof(FluentExtensionTestData.MustBe))]
        [MemberData(nameof(FluentExtensionTestData.MustBe.EdgeCases), MemberType = typeof(FluentExtensionTestData.MustBe))]
        public static void BehavesAsExpected(FluentExtensionTestData.MustBe.ValidCase testCase)
        {
            // Arrange
            var validator = new InlineValidator<Model>();
            var rule = validator.RuleFor(x => x.Value).MustBe(_ => testCase.Result, testCase.Message);
            if (testCase.PropertyNameOverride is not null)
                rule.OverridePropertyName(testCase.PropertyNameOverride);

            // Act
            var result = validator.Validate(new Model { Value = testCase.Value });

            // Assert
            Assert.Equal(testCase.Expected, result.IsValid);
            if (testCase.Expected)
            {
                Assert.Empty(result.Errors);
                return;
            }

            var error = Assert.Single(result.Errors);
            Assert.Equal(testCase.ExpectedErrorMessage, error.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(FluentExtensionTestData.MustBe.InvalidCases), MemberType = typeof(FluentExtensionTestData.MustBe))]
        public static void ThrowsExpected(IThrowsCase testCase)
        {
            // Arrange
            var actionCase = Assert.IsType<ThrowsCase<Action>>(testCase, exactMatch: false);

            // Act
            var ex = Assert.Throws(testCase.ExpectedException.Type, () => actionCase.Value.Invoke());

            // Assert
            ThrowsCaseAssert.Expected(ex, testCase);
        }
    }

    public static class MustBeModel
    {
        [Theory]
        [MemberData(nameof(FluentExtensionTestData.MustBeModel.ValidCases), MemberType = typeof(FluentExtensionTestData.MustBeModel))]
        [MemberData(nameof(FluentExtensionTestData.MustBeModel.EdgeCases), MemberType = typeof(FluentExtensionTestData.MustBeModel))]
        public static void BehavesAsExpected(FluentExtensionTestData.MustBeModel.ValidCase testCase)
        {
            // Arrange
            var validator = new InlineValidator<Model>();
            var rule = validator.RuleFor(x => x.Value).MustBe((_, _) => testCase.Result, testCase.Message);
            if (testCase.PropertyNameOverride is not null)
                rule.OverridePropertyName(testCase.PropertyNameOverride);

            // Act
            var result = validator.Validate(new Model { Value = testCase.Value });

            // Assert
            Assert.Equal(testCase.Expected, result.IsValid);
            if (testCase.Expected)
            {
                Assert.Empty(result.Errors);
                return;
            }

            var error = Assert.Single(result.Errors);
            Assert.Equal(testCase.ExpectedErrorMessage, error.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(FluentExtensionTestData.MustBeModel.InvalidCases), MemberType = typeof(FluentExtensionTestData.MustBeModel))]
        public static void ThrowsExpected(IThrowsCase testCase)
        {
            // Arrange
            var actionCase = Assert.IsType<ThrowsCase<Action>>(testCase, exactMatch: false);

            // Act
            var ex = Assert.Throws(testCase.ExpectedException.Type, () => actionCase.Value.Invoke());

            // Assert
            ThrowsCaseAssert.Expected(ex, testCase);
        }
    }

    public static class MustBeStruct
    {
        [Theory]
        [MemberData(nameof(FluentExtensionTestData.MustBeStruct.ValidCases), MemberType = typeof(FluentExtensionTestData.MustBeStruct))]
        [MemberData(nameof(FluentExtensionTestData.MustBeStruct.EdgeCases), MemberType = typeof(FluentExtensionTestData.MustBeStruct))]
        public static void BehavesAsExpected(FluentExtensionTestData.MustBeStruct.ValidCase testCase)
        {
            // Arrange
            var validator = new InlineValidator<ModelStub>();
            var rule = validator.RuleFor(x => x.Id).MustBe(_ => testCase.Result, testCase.Message);
            if (testCase.PropertyNameOverride is not null)
                rule.OverridePropertyName(testCase.PropertyNameOverride);

            // Act
            var result = validator.Validate(new ModelStub { Id = testCase.Value });

            // Assert
            Assert.Equal(testCase.Expected, result.IsValid);
            if (testCase.Expected)
            {
                Assert.Empty(result.Errors);
                return;
            }

            var error = Assert.Single(result.Errors);
            Assert.Equal(testCase.ExpectedErrorMessage, error.ErrorMessage);
        }
    }

    private sealed record Model
    {
        public string? Value { get; init; }
    }

    private sealed record ModelStub
    {
        public int? Id { get; init; }
    }
}
