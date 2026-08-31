using Microsoft.CodeAnalysis;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Analyzers.UnitTests;

public static class GuardSyntaxFactoryTestData
{
    private const string NullClause = "Null";
    private const string NameArgument = "name";
    private const string ShapeMessage = "if statement";

    public static class CreateGuard
    {
        /// <summary>
        /// The property bag a <c>PG1001</c> report carries, so the factory fails on the node it was
        /// handed rather than on a missing clause name.
        /// </summary>
        public static Diagnostic NullGuardDiagnostic => Diagnostic.Create(
            DiagnosticDescriptors.UseGuardAgainstNull,
            Location.None,
            DiagnosticProperties.ForGuard(NullClause, NameArgument),
            NameArgument);

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("a-return-statement-is-not-a-reported-shape", "return;", new ExpectedException(typeof(ArgumentOutOfRangeException), "node", ShapeMessage)),
            new InvalidCase("an-assignment-statement-is-not-a-reported-shape", "name = null;", new ExpectedException(typeof(ArgumentOutOfRangeException), "node", ShapeMessage)),
            new InvalidCase("a-throw-statement-is-not-a-reported-shape", "throw new ArgumentNullException(nameof(name));", new ExpectedException(typeof(ArgumentOutOfRangeException), "node", ShapeMessage))
        ];

        public sealed record InvalidCase(string Name, string Value, ExpectedException ExpectedException)
            : ThrowsCase<string>(Name, Value, ExpectedException);
    }
}
