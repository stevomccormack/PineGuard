using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardCsvClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvLine.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvLine))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvLine.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvLine))]
    public void NotCsvLine_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotCsvLine(value));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvLine(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvHeaderLine.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvHeaderLine))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvHeaderLine.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvHeaderLine))]
    public void NotCsvHeaderLine_BehavesAsExpected(GuardCase<(string? line, IReadOnlyList<string>? expectedHeader)> tc)
    {
        var value = tc.Value.line;
        var result = AssertResult(tc, () => Guard.Against.NotCsvHeaderLine(value, tc.Value.expectedHeader));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvHeaderLine(value, tc.Value.expectedHeader, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineSchema.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineSchema))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineSchema.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineSchema))]
    public void NotCsvRowLineSchema_BehavesAsExpected(GuardCase<(string? line, IReadOnlyList<CsvColumnSchema>? schema)> tc)
    {
        var value = tc.Value.line;
        var result = AssertResult(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.schema));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.schema, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes))]
    public void NotCsvRowLineHeaderTypes_BehavesAsExpected(GuardCase<(string? line, IReadOnlyList<string>? header, IReadOnlyDictionary<string, CsvColumnType>? types)> tc)
    {
        var value = tc.Value.line;
        var result = AssertResult(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.header, tc.Value.types));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.header, tc.Value.types, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
