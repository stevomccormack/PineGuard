using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardValidatorClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardValidatorClausesTestData.Valid.Cases), MemberType = typeof(GuardValidatorClausesTestData.Valid))]
    public void Invalid_ValidValue_ReturnsValue(bool _)
    {
        // Arrange
        var widget = new GuardValidatorClausesTestData.Widget("widget", 1);
        var validator = GuardValidatorClausesTestData.NewValidator();

        // Act
        var result = Guard.Against.Invalid(widget, validator);

        // Assert
        Assert.Same(widget, result);
    }

    [Theory]
    [MemberData(nameof(GuardValidatorClausesTestData.InvalidValue.Cases), MemberType = typeof(GuardValidatorClausesTestData.InvalidValue))]
    public void Invalid_InvalidValue_ThrowsArgumentExceptionWithDataStampedFromFirstFailure(bool _)
    {
        // Arrange
        var widget = new GuardValidatorClausesTestData.Widget(string.Empty, 1);
        var validator = GuardValidatorClausesTestData.NewValidator();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => Guard.Against.Invalid(widget, validator));

        // Assert
        Assert.Equal("Name", ex.ParamName);
        Assert.True(ex.HasMustCode(GuardValidatorClausesTestData.ExpectedCodes.NameEmpty));
        Assert.Equal("Name", ex.GetMustPropertyPath());
    }

    [Theory]
    [MemberData(nameof(GuardValidatorClausesTestData.NullValue.Cases), MemberType = typeof(GuardValidatorClausesTestData.NullValue))]
    public void Invalid_NullValue_ThrowsArgumentNullExceptionWithGuardedParamName(bool _)
    {
        // Arrange
        GuardValidatorClausesTestData.Widget widget = null!;
        var validator = GuardValidatorClausesTestData.NewValidator();

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => Guard.Against.Invalid(widget, validator));

        // Assert
        Assert.Equal("widget", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(GuardValidatorClausesTestData.MultipleFailures.Cases), MemberType = typeof(GuardValidatorClausesTestData.MultipleFailures))]
    public void Invalid_MultipleFailures_UsesOnlyTheFirstFailure(bool _)
    {
        // Arrange
        var widget = new GuardValidatorClausesTestData.Widget(string.Empty, -1);
        var validator = GuardValidatorClausesTestData.NewValidator();

        // Act
        var ex = Assert.Throws<ArgumentException>(() => Guard.Against.Invalid(widget, validator));

        // Assert
        Assert.True(ex.HasMustCode(GuardValidatorClausesTestData.ExpectedCodes.NameEmpty));
        Assert.False(ex.HasMustCode(GuardValidatorClausesTestData.ExpectedCodes.CountNotPositive));
    }

    [Theory]
    [MemberData(nameof(GuardValidatorClausesTestData.MapActive.Cases), MemberType = typeof(GuardValidatorClausesTestData.MapActive))]
    public void Invalid_MapActive_ReceivesFailureBuiltFromFirstFailure(bool _)
    {
        // Arrange
        var widget = new GuardValidatorClausesTestData.Widget(string.Empty, 1);
        var validator = GuardValidatorClausesTestData.NewValidator();
        GuardFailure? captured = null;

        // BeginScope (AsyncLocal) rather than Map (a plain static field) — this test class runs
        // concurrently with every other GuardClauses.UnitTests class, all of which are sensitive to
        // the effective map, so a global Map()/Clear() pair here would be observable as flaky failures
        // in unrelated guard clause tests running on other threads at the same time.
        using (GuardExceptionPolicy.BeginScope(failure =>
               {
                   captured = failure;
                   return new NotSupportedException("mapped: " + failure.Message);
               }))
        {
            // Act
            var ex = Assert.Throws<NotSupportedException>(() => Guard.Against.Invalid(widget, validator));

            // Assert
            Assert.NotNull(captured);
            Assert.Equal(GuardValidatorClausesTestData.ExpectedCodes.NameEmpty, captured.Code);
            Assert.Equal("Name", captured.ParamName);
            Assert.StartsWith("mapped: ", ex.Message, StringComparison.Ordinal);
        }
    }

    [Theory]
    [MemberData(nameof(GuardValidatorClausesTestData.NullValidator.Cases), MemberType = typeof(GuardValidatorClausesTestData.NullValidator))]
    public void Invalid_NullValidator_ThrowsArgumentNullException(bool _)
    {
        // Arrange
        var widget = new GuardValidatorClausesTestData.Widget("widget", 1);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => Guard.Against.Invalid(widget, (IMustValidator<GuardValidatorClausesTestData.Widget>)null!));
        Assert.Equal("validator", ex.ParamName);
    }
}
