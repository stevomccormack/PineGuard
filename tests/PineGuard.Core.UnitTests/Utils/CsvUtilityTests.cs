using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class CsvUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvLine.ValidCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvLine))]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvLine.EdgeCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvLine))]
    public void TryParseCsvLine_ReturnsExpected(CsvUtilityTestData.TryParseCsvLine.ValidCase testCase)
    {
        // Act
        var ok = CsvUtility.TryParseCsvLine(testCase.Value, out var fields);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (testCase.ExpectedOutValue is null)
        {
            Assert.Null(fields);
            return;
        }

        Assert.NotNull(fields);
        Assert.Equal(testCase.ExpectedOutValue, fields);
    }

    [Theory]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvHeaderLine.ValidCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvHeaderLine))]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvHeaderLine.EdgeCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvHeaderLine))]
    public void TryParseCsvHeaderLine_ReturnsExpected(CsvUtilityTestData.TryParseCsvHeaderLine.ValidCase testCase)
    {
        // Act
        var ok = CsvUtility.TryParseCsvHeaderLine(testCase.Value, out var header);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (testCase.ExpectedOutValue is null)
        {
            Assert.Null(header);
            return;
        }

        Assert.NotNull(header);
        Assert.Equal(testCase.ExpectedOutValue, header);
    }

    [Theory]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvHeaderLineExpected.ValidCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvHeaderLineExpected))]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvHeaderLineExpected.EdgeCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvHeaderLineExpected))]
    public void TryParseCsvHeaderLineExpected_ReturnsExpected(CsvUtilityTestData.TryParseCsvHeaderLineExpected.ValidCase testCase)
    {
        // Act
        var ok = CsvUtility.TryParseCsvHeaderLine(testCase.Value.Line, testCase.Value.ExpectedHeader, out var parsed, ',', testCase.Value.Comparison);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (!ok)
        {
            Assert.Null(parsed);
        }
    }

    [Theory]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvRowLineExpectedCount.ValidCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvRowLineExpectedCount))]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvRowLineExpectedCount.EdgeCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvRowLineExpectedCount))]
    public void TryParseCsvRowLineExpectedCount_ReturnsExpected(CsvUtilityTestData.TryParseCsvRowLineExpectedCount.ValidCase testCase)
    {
        // Act
        var ok = CsvUtility.TryParseCsvRowLine(testCase.Value.Line, testCase.Value.ExpectedCount, out var fields);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (!ok)
        {
            Assert.Null(fields);
        }
    }

    [Theory]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvRowLineSchema.ValidCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvRowLineSchema))]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvRowLineSchema.EdgeCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvRowLineSchema))]
    public void TryParseCsvRowLineSchema_ReturnsExpected(CsvUtilityTestData.TryParseCsvRowLineSchema.ValidCase testCase)
    {
        // Act
        var ok = CsvUtility.TryParseCsvRowLine(testCase.Value.Line, testCase.Value.Schema, out var fields);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (!ok)
        {
            Assert.Null(fields);
        }
    }

    [Theory]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvRowLineHeaderTypes.ValidCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvRowLineHeaderTypes))]
    [MemberData(nameof(CsvUtilityTestData.TryParseCsvRowLineHeaderTypes.EdgeCases), MemberType = typeof(CsvUtilityTestData.TryParseCsvRowLineHeaderTypes))]
    public void TryParseCsvRowLineHeaderTypes_ReturnsExpected(CsvUtilityTestData.TryParseCsvRowLineHeaderTypes.ValidCase testCase)
    {
        // Act
        var ok = CsvUtility.TryParseCsvRowLine(testCase.Value.Line, testCase.Value.Header, testCase.Value.Types, out var fields, ',', testCase.Value.Comparison);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (!ok)
        {
            Assert.Null(fields);
        }
    }
}
