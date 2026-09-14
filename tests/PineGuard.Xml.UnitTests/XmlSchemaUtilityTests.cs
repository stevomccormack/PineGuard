using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Xml.UnitTests.Schemas;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class XmlSchemaUtilityTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(XmlSchemaUtilityTestData.TryValidate.Cases), MemberType = typeof(XmlSchemaUtilityTestData.TryValidate))]
    public void TryValidate_BehavesAsExpected(XmlSchemaUtilityTestData.TryValidate.Case tc)
    {
        // Arrange
        var (value, schemas, options) = tc.Value;

        // Act
        var ok = XmlSchemaUtility.TryValidate(value, schemas, options, out var violations);

        // Assert
        Assert.Equal(tc.Expected.Ok, ok);
        Assert.Equal(tc.Expected.Violations.Length, violations.Count);

        for (var i = 0; i < tc.Expected.Violations.Length; i++)
        {
            var (expectedKind, expectedPath) = tc.Expected.Violations[i];
            var actual = violations[i];

            Assert.Equal(expectedKind, actual.Kind);
            Assert.Equal(expectedPath, actual.Path);

            if (expectedKind == XmlSchemaViolationKind.UnknownNamespace)
                Assert.Equal(0, actual.LineNumber);
            else
                Assert.True(actual.LineNumber > 0);
        }
    }

    [Theory]
    [MemberData(nameof(XmlSchemaUtilityTestData.TryValidateNullSchemas.InvalidCases), MemberType = typeof(XmlSchemaUtilityTestData.TryValidateNullSchemas))]
    public void TryValidateNullSchemas_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (XmlSchemaUtilityTestData.TryValidateNullSchemas.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => XmlSchemaUtility.TryValidate(TestSchemas.ValidMessage, t.Value!, null, out _));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
