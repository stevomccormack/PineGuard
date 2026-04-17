using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentCsvExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class CsvLineValidator : AbstractValidator<Model>
    {
        public CsvLineValidator() => RuleFor(x => x.Value).CsvLine();
    }

    private sealed class CsvHeaderLineValidator : AbstractValidator<Model>
    {
        public CsvHeaderLineValidator(IReadOnlyList<string>? expectedHeader) => RuleFor(x => x.Value).CsvHeaderLine(expectedHeader);
    }

    private sealed class CsvRowLineSchemaValidator : AbstractValidator<Model>
    {
        public CsvRowLineSchemaValidator(IReadOnlyList<CsvColumnSchema>? schema) => RuleFor(x => x.Value).CsvRowLine(schema);
    }

    private sealed class CsvRowLineHeaderValidator : AbstractValidator<Model>
    {
        public CsvRowLineHeaderValidator(IReadOnlyList<string>? header, IReadOnlyDictionary<string, CsvColumnType>? types) => RuleFor(x => x.Value).CsvRowLine(header, types);
    }

    [Theory]
    [MemberData(nameof(FluentCsvExtensionsTestData.CsvLine.Cases), MemberType = typeof(FluentCsvExtensionsTestData.CsvLine))]
    public void CsvLine_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new CsvLineValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentCsvExtensionsTestData.CsvHeaderLine.Cases), MemberType = typeof(FluentCsvExtensionsTestData.CsvHeaderLine))]
    public void CsvHeaderLine_BehavesAsExpected(FluentCase<(string? line, IReadOnlyList<string>? expectedHeader)> tc)
    {
        var result = new CsvHeaderLineValidator(tc.Value.expectedHeader).Validate(new Model { Value = tc.Value.line });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentCsvExtensionsTestData.CsvRowLineWithSchema.Cases), MemberType = typeof(FluentCsvExtensionsTestData.CsvRowLineWithSchema))]
    public void CsvRowLineWithSchema_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new CsvRowLineSchemaValidator(FluentCsvExtensionsTestData.CsvRowLineWithSchema.Schema).Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentCsvExtensionsTestData.CsvRowLineWithHeader.Cases), MemberType = typeof(FluentCsvExtensionsTestData.CsvRowLineWithHeader))]
    public void CsvRowLineWithHeader_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new CsvRowLineHeaderValidator(FluentCsvExtensionsTestData.CsvRowLineWithHeader.Header, FluentCsvExtensionsTestData.CsvRowLineWithHeader.Types).Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
