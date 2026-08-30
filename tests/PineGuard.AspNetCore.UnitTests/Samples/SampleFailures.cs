using PineGuard.MustClauses;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The failures Plan 03's story-2 body is built from, plus the ones the edge cases need.
/// </summary>
public static class SampleFailures
{
    public const string SecretValue = "correct-horse-battery-staple";

    public static MustFailure Email => new("Email", "email.address.invalid", "Email must be a valid email address.", "not-an-email");

    public static MustFailure LineSku => new("Lines[1].Sku", "text.content.blank", "Lines[1].Sku must not be null or whitespace.", null);

    public static MustFailure EmailTooLong => new("Email", "text.length.above-maximum", "Email must be at most 256 characters.", "not-an-email");

    public static MustFailure Root => new("", "value.state.invalid", "The order is not consistent.", null);

    public static MustFailure Password => new("Password", "text.content.blank", "Password must not be null or whitespace.", SecretValue);
}
