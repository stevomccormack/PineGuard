using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

/// <summary>
/// Builds the <see cref="ValidationContext"/> instances the clock-reading attribute tests validate
/// against.
/// </summary>
/// <remarks>
/// A <see cref="ValidationAttribute"/> cannot take a <see cref="TimeProvider"/> as a constructor argument
/// — attribute arguments must be compile-time constants — so
/// <c>ValidationAttributeBase.ResolveTimeProvider</c> reads the clock off the validation context's service
/// provider instead. These tests are the only place that seam is exercised, so they build the context the
/// same way a host with a registered <see cref="TimeProvider"/> would.
/// </remarks>
internal static class ValidationContextFactory
{
    /// <summary>
    /// Creates a validation context whose service provider resolves <see cref="TimeProvider"/> to
    /// <paramref name="timeProvider"/> and every other service type to <see langword="null"/>.
    /// </summary>
    /// <param name="timeProvider">The clock the attribute under test should read.</param>
    /// <returns>A validation context for a member named <c>Value</c>.</returns>
    internal static ValidationContext WithTimeProvider(TimeProvider timeProvider)
    {
        var context = new ValidationContext(new object()) { MemberName = "Value" };
        context.InitializeServiceProvider(serviceType => serviceType == typeof(TimeProvider) ? timeProvider : null);
        return context;
    }
}
