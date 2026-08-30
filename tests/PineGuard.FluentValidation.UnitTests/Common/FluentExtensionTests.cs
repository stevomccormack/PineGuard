using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;
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

    public static class MustBeAsync
    {
        [Theory]
        [MemberData(nameof(FluentExtensionTestData.MustBeAsync.ValidCases), MemberType = typeof(FluentExtensionTestData.MustBeAsync))]
        [MemberData(nameof(FluentExtensionTestData.MustBeAsync.EdgeCases), MemberType = typeof(FluentExtensionTestData.MustBeAsync))]
        public static async Task BehavesAsExpected(FluentExtensionTestData.MustBeAsync.ValidCase testCase)
        {
            // Arrange
            var validator = new InlineValidator<Model>();
            var rule = validator.RuleFor(x => x.Value).MustBeAsync((_, _) => new ValueTask<MustResult<bool>>(testCase.Result), testCase.Message);
            if (testCase.PropertyNameOverride is not null)
                rule.OverridePropertyName(testCase.PropertyNameOverride);

            // Act
            var result = await validator.ValidateAsync(new Model { Value = testCase.Value });

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
        [MemberData(nameof(FluentExtensionTestData.MustBeAsync.InvalidCases), MemberType = typeof(FluentExtensionTestData.MustBeAsync))]
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

    public static class ErrorCode
    {
        [Theory]
        [MemberData(nameof(FluentExtensionTestData.ErrorCode.Cases), MemberType = typeof(FluentExtensionTestData.ErrorCode))]
        public static void CodeNull_LeavesFluentValidationDefaultErrorCode(bool _)
        {
            // Arrange
            var undecorated = new InlineValidator<Model>();
            undecorated.RuleFor(x => x.Value).Must(_ => false);

            var decorated = new InlineValidator<Model>();
            decorated.RuleFor(x => x.Value).MustBe(_ => MustResult<bool>.Fail("test.code", "bad", null, "x"), null);

            // Act
            var undecoratedResult = undecorated.Validate(new Model());
            var decoratedResult = decorated.Validate(new Model());

            // Assert
            Assert.Equal(undecoratedResult.Errors[0].ErrorCode, decoratedResult.Errors[0].ErrorCode);
        }

        [Theory]
        [MemberData(nameof(FluentExtensionTestData.ErrorCode.Cases), MemberType = typeof(FluentExtensionTestData.ErrorCode))]
        public static void CodeSet_BecomesErrorCode_OnAllThreeOverloads(bool _)
        {
            // Arrange
            var single = new InlineValidator<Model>();
            single.RuleFor(x => x.Value).MustBe(_ => MustResult<bool>.Fail("test.code", "bad", null, "x"), null, "sample.code");

            var model = new InlineValidator<Model>();
            model.RuleFor(x => x.Value).MustBe((_, _) => MustResult<bool>.Fail("test.code", "bad", null, "x"), null, "sample.code");

            var structModel = new InlineValidator<ModelStub>();
            structModel.RuleFor(x => x.Id).MustBe(_ => MustResult<int?>.Fail("test.code", "bad", null, null), null, "sample.code");

            // Act
            var singleResult = single.Validate(new Model());
            var modelResult = model.Validate(new Model());
            var structResult = structModel.Validate(new ModelStub());

            // Assert
            Assert.Equal("sample.code", singleResult.Errors[0].ErrorCode);
            Assert.Equal("sample.code", modelResult.Errors[0].ErrorCode);
            Assert.Equal("sample.code", structResult.Errors[0].ErrorCode);
        }

        [Theory]
        [MemberData(nameof(FluentExtensionTestData.ErrorCode.Cases), MemberType = typeof(FluentExtensionTestData.ErrorCode))]
        public static async Task CodeSet_BecomesErrorCode_OnTheAsyncOverload(bool _)
        {
            // Arrange
            var validator = new InlineValidator<Model>();
            validator.RuleFor(x => x.Value)
                .MustBeAsync((_, _) => new ValueTask<MustResult<bool>>(MustResult<bool>.Fail("test.code", "bad", null, "x")), null, "sample.code");

            // Act
            var result = await validator.ValidateAsync(new Model());

            // Assert
            Assert.Equal("sample.code", result.Errors[0].ErrorCode);
        }

        [Theory]
        [MemberData(nameof(FluentExtensionTestData.ErrorCode.Cases), MemberType = typeof(FluentExtensionTestData.ErrorCode))]
        public static void ConsumerWithMessage_AfterExtension_StillWins(bool _)
        {
            // Arrange
            var validator = new InlineValidator<Model>();
            validator.RuleFor(x => x.Value)
                .MustBe(_ => MustResult<bool>.Fail("test.code", "bad", null, "x"), null, "sample.code")
                .WithMessage("consumer message");

            // Act
            var result = validator.Validate(new Model());

            // Assert
            Assert.Equal("consumer message", result.Errors[0].ErrorMessage);
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
