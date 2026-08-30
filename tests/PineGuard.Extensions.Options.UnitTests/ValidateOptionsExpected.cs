using PineGuard.Testing.Common;

namespace PineGuard.Extensions.Options.UnitTests;

public sealed record ValidateOptionsExpected(bool IsValid, string? Message = null, bool Skipped = false, IReadOnlyList<string>? Failures = null)
    : ReturnExpected(IsValid, Message);
