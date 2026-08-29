using Microsoft.Extensions.Options;
using PineGuard.Extensions.Options.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Extensions.Options.UnitTests;

public sealed class MustRulesValidateOptionsTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustRulesValidateOptionsTestData.Constructor.InvalidCases), MemberType = typeof(MustRulesValidateOptionsTestData.Constructor))]
    public void Constructor_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustRulesValidateOptionsTestData.Validate.Cases), MemberType = typeof(MustRulesValidateOptionsTestData.Validate))]
    public void Validate_BehavesAsExpected(ValidateOptionsCase<(string? registeredName, string? name, SmtpOptions options)> tc)
    {
        // Arrange
        var (registeredName, name, options) = tc.Value;
        var sut = new MustRulesValidateOptions<SmtpOptions>(registeredName, new SmtpOptionsValidator());

        // Act
        var result = sut.Validate(name, options);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustRulesValidateOptionsTestData.Validate.InvalidCases), MemberType = typeof(MustRulesValidateOptionsTestData.Validate))]
    public void Validate_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustRulesValidateOptionsTestData.FormatFailure.Cases), MemberType = typeof(MustRulesValidateOptionsTestData.FormatFailure))]
    public void FormatFailure_BehavesAsExpected(MustRulesValidateOptionsTestData.FormatFailure.Case tc)
    {
        // Act
        var result = MustRulesValidateOptions<SmtpOptions>.FormatFailure(tc.Value);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    private static void AssertResult<T>(ValidateOptionsCase<T> tc, ValidateOptionsResult result)
    {
        var expected = tc.Expected;

        Assert.Equal(expected.IsValid, result.Succeeded);
        Assert.Equal(expected.Skipped, result.Skipped);
        Assert.Equal(!expected.IsValid && !expected.Skipped, result.Failed);

        if (expected.Failures is not null)
            Assert.Equal(expected.Failures, result.Failures);

        if (expected.Message is not null)
            Assert.Equal(expected.Message, result.FailureMessage);
    }
}
