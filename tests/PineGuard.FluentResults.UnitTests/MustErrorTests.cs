using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.FluentResults.UnitTests;

public sealed class MustErrorTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustErrorTestData.Constructor.Cases), MemberType = typeof(MustErrorTestData.Constructor))]
    public void Constructor_BehavesAsExpected(FluentResultsCase<(string code, string propertyPath, string message)> tc)
    {
        // Arrange
        var (code, propertyPath, message) = tc.Value;

        // Act
        var error = new MustError(code, propertyPath, message);

        // Assert
        MustErrorAssert.Expected(tc.Expected.Errors![0], error);
    }

    [Theory]
    [MemberData(nameof(MustErrorTestData.Constructor.InvalidCases), MemberType = typeof(MustErrorTestData.Constructor))]
    public void Constructor_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustErrorTestData.FromResult.Cases), MemberType = typeof(MustErrorTestData.FromResult))]
    public void FromResult_BehavesAsExpected(FluentResultsCase<IMustResult> tc)
    {
        // Act
        var error = MustError.From(tc.Value);

        // Assert
        MustErrorAssert.Expected(tc.Expected.Errors![0], error);
    }

    [Theory]
    [MemberData(nameof(MustErrorTestData.FromResult.InvalidCases), MemberType = typeof(MustErrorTestData.FromResult))]
    public void FromResult_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustErrorTestData.FromFailure.Cases), MemberType = typeof(MustErrorTestData.FromFailure))]
    public void FromFailure_BehavesAsExpected(FluentResultsCase<MustFailure> tc)
    {
        // Act
        var error = MustError.From(tc.Value);

        // Assert
        MustErrorAssert.Expected(tc.Expected.Errors![0], error);
    }

    [Theory]
    [MemberData(nameof(MustErrorTestData.FromFailure.InvalidCases), MemberType = typeof(MustErrorTestData.FromFailure))]
    public void FromFailure_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
