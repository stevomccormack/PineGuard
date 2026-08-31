using PineGuard.Testing.Common;

namespace PineGuard.Analyzers.UnitTests;

/// <summary>
/// What a C# snippet is expected to produce: nothing at all (<paramref name="IsValid"/>), or one
/// diagnostic with the given id, message and location — and, for a fix group, the source the code
/// fix must produce.
/// </summary>
public sealed record AnalyzerExpected(
    bool IsValid,
    string? Message = null,
    string? DiagnosticId = null,
    int? Line = null,
    int? Column = null,
    string? FixedSource = null)
    : ReturnExpected(IsValid, Message);
