using PineGuard.Testing.UnitTests;

namespace PineGuard.Analyzers.UnitTests;

/// <summary>
/// One C# snippet under analysis. The positional value is the source itself.
/// </summary>
public sealed record AnalyzerCase(string Name, string Source, AnalyzerExpected Expected)
    : ReturnCase<string, AnalyzerExpected>(Name, Source, Expected);
