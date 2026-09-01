using Microsoft.CodeAnalysis.CSharp;
using PineGuard.Analyzers.CodeFixes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using CreateGuardData = PineGuard.Analyzers.UnitTests.GuardSyntaxFactoryTestData.CreateGuard;

namespace PineGuard.Analyzers.UnitTests;

public sealed class GuardSyntaxFactoryTests
{
    [Theory]
    [MemberData(nameof(CreateGuardData.InvalidCases), MemberType = typeof(CreateGuardData))]
    public void CreateGuard_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (CreateGuardData.InvalidCase)tc;
        var node = SyntaxFactory.ParseStatement(t.Value);

        // Act & Assert
        var ex = Assert.Throws(
            tc.ExpectedException.Type,
            () => GuardSyntaxFactory.CreateGuard(node, CreateGuardData.NullGuardDiagnostic));

        ThrowsCaseAssert.Expected(ex, tc);
    }
}
