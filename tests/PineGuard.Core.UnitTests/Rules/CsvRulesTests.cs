using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class CsvRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CsvRulesTestData.IsCsvLine.Cases), MemberType = typeof(CsvRulesTestData.IsCsvLine))]
    public void IsCsvLine_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = CsvRules.IsCsvLine(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CsvRulesTestData.IsCsvHeaderLine.Cases), MemberType = typeof(CsvRulesTestData.IsCsvHeaderLine))]
    public void IsCsvHeaderLine_BehavesAsExpected(RuleCase<(string? line, IReadOnlyList<string>? expectedHeader)> tc)
    {
        // Arrange
        var (line, expectedHeader) = tc.Value;

        // Act
        var result = CsvRules.IsCsvHeaderLine(line, expectedHeader);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CsvRulesTestData.IsCsvRowLineSchema.Cases), MemberType = typeof(CsvRulesTestData.IsCsvRowLineSchema))]
    public void IsCsvRowLineSchema_BehavesAsExpected(RuleCase<(string? line, IReadOnlyList<CsvColumnSchema>? schema)> tc)
    {
        // Arrange
        var (line, schema) = tc.Value;

        // Act
        var result = CsvRules.IsCsvRowLine(line, schema);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CsvRulesTestData.IsCsvRowLineHeaderTypes.Cases), MemberType = typeof(CsvRulesTestData.IsCsvRowLineHeaderTypes))]
    public void IsCsvRowLineHeaderTypes_BehavesAsExpected(RuleCase<(string? line, IReadOnlyList<string>? header, IReadOnlyDictionary<string, CsvColumnType> types)> tc)
    {
        // Arrange
        var (line, header, types) = tc.Value;

        // Act
        var result = CsvRules.IsCsvRowLine(line, header, types);

        // Assert
        AssertResult(tc, result);
    }
}
