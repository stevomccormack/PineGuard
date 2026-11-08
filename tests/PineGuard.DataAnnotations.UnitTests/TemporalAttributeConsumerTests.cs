using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TemporalAttributeConsumerTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    private sealed class Model
    {
        [InRangeDateOnly(min: "2024-01-01", max: "2024-12-31")]
        public DateOnly Date { get; init; }

        [OutOfRangeTimeOnly(min: "09:00", max: "17:00")]
        public TimeOnly Clock { get; init; }

        [GreaterThanTimeSpan(other: "00:00:00")]
        public TimeSpan Duration { get; init; }

        [LessThanTimeSpanString(other: "02:00:00")]
        public string DurationText { get; init; } = string.Empty;

        [InRangeDateOnlyString("2024-01-01", "2024-12-31")]
        public string DateText { get; init; } = string.Empty;

        [OutOfRangeDateOnlyString("2024-01-01", "2024-12-31")]
        public string OutsideDateText { get; init; } = string.Empty;

        [OutOfRangeDateTimeOffsetString("2024-01-01T00:00:00Z", "2024-12-31T00:00:00Z")]
        public string OutsideInstantText { get; init; } = string.Empty;
    }

    [Theory]
    [MemberData(nameof(TemporalAttributeConsumerTestData.AttributeSyntax.Cases), MemberType = typeof(TemporalAttributeConsumerTestData.AttributeSyntax))]
    public void AttributeSyntax_CompilesAndValidates(TemporalAttributeConsumerTestData.Case tc)
    {
        // Arrange
        var model = new Model
        {
            Date = tc.Value ? new DateOnly(2024, 1, 1) : new DateOnly(2025, 1, 1),
            Clock = tc.Value ? new TimeOnly(8, 0) : new TimeOnly(12, 0),
            Duration = tc.Value ? TimeSpan.FromHours(1) : TimeSpan.Zero,
            DurationText = tc.Value ? "01:00:00" : "03:00:00",
            DateText = tc.Value ? "2024-06-01" : "2025-06-01",
            OutsideDateText = tc.Value ? "2025-06-01" : "2024-06-01",
            OutsideInstantText = tc.Value ? "2025-06-01T00:00:00Z" : "2024-06-01T00:00:00Z"
        };
        var failures = new List<ValidationResult>();

        // Act
        var valid = Validator.TryValidateObject(model, new ValidationContext(model), failures, validateAllProperties: true);

        // Assert
        Assert.Equal(tc.Expected, valid);
        Assert.Equal(tc.Expected ? 0 : 7, failures.Count);
    }
}
