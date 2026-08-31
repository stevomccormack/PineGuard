namespace PineGuard.Analyzers.UnitTests;

public static class PreferGuardAnalyzerTestData
{
    private const string UseGuardAgainstNull = "PG1001";
    private const string NameMessage = "Replace this null check with Guard.Against.Null(name)";

    private const string IsNullThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string IsNullThrowFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.Null(name);
            }
        }
        """;

    private const string EqualsNullThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string NullEqualsThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (null == name)
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string BlockThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
            }
        }
        """;

    private const string CoalesceThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            private readonly string _name;

            public Order(string name)
            {
                _name = name ?? throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string CoalesceThrowFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            private readonly string _name;

            public Order(string name)
            {
                _name = Guard.Against.Null(name);
            }
        }
        """;

    private const string ThrowIfNull = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                ArgumentNullException.ThrowIfNull(name);
            }
        }
        """;

    private const string ThrowIfNullFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.Null(name);
            }
        }
        """;

    private const string ThrowIfNullWithParamName = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                ArgumentNullException.ThrowIfNull(name, nameof(name));
            }
        }
        """;

    private const string ThrowIfNullOfExpression = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                ArgumentNullException.ThrowIfNull(Normalize(name));
            }

            private static string Normalize(string value) => value;
        }
        """;

    private const string ThrowIfNullOrEmpty = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                ArgumentException.ThrowIfNullOrEmpty(name);
            }
        }
        """;

    private const string OtherExceptionThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                    throw new InvalidOperationException();
            }
        }
        """;

    private const string ThrownExpressionIsNotCreation = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                    throw Failure(name);
            }

            private static Exception Failure(string value) => new ArgumentNullException(value);
        }
        """;

    private const string NotANullCheck = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name.Length == 0)
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string NotEqualsNull = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name != null)
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string IsNotNullPattern = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is not null)
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string IsEmptyStringPattern = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is "")
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string MemberIsNullPattern = """
        using System;

        namespace Sample;

        public class Order
        {
            private string _name = "";

            public void Ship()
            {
                if (this._name is null)
                    throw new ArgumentNullException(nameof(_name));
            }
        }
        """;

    private const string ElseBranch = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                    throw new ArgumentNullException(nameof(name));
                else
                    Console.WriteLine(name);
            }
        }
        """;

    private const string TwoStatementBlock = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                {
                    Console.WriteLine("bad");
                    throw new ArgumentNullException(nameof(name));
                }
            }
        }
        """;

    private const string NoThrowAtAll = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                    Console.WriteLine("bad");
            }
        }
        """;

    private const string CoalesceOtherException = """
        using System;

        namespace Sample;

        public class Order
        {
            private readonly string _name;

            public Order(string name)
            {
                _name = name ?? throw new InvalidOperationException();
            }
        }
        """;

    private const string CoalesceOfExpression = """
        using System;

        namespace Sample;

        public class Order
        {
            private readonly string _name;

            public Order(string name)
            {
                _name = Normalize(name) ?? throw new ArgumentNullException(nameof(name));
            }

            private static string? Normalize(string value) => value;
        }
        """;

    private const string ThrowExpressionOutsideCoalesce = """
        using System;

        namespace Sample;

        public class Order
        {
            public int Ship(string name, bool ready) => ready ? 1 : throw new ArgumentNullException(nameof(name));
        }
        """;

    private const string ExistingGuardClausesUsing = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                    throw new ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string ExistingGuardClausesUsingFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.Null(name);
            }
        }
        """;

    private const string NoUsings = """
        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name is null)
                    throw new System.ArgumentNullException(nameof(name));
            }
        }
        """;

    private const string NoUsingsFixed = """
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.Null(name);
            }
        }
        """;

    private const string TwoCheckedParameters = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                if (name is null)
                    throw new ArgumentNullException(nameof(name));
                if (address is null)
                    throw new ArgumentNullException(nameof(address));
            }
        }
        """;

    private const string TwoCheckedParametersFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                Guard.Against.Null(name);
                Guard.Against.Null(address);
            }
        }
        """;

    public static class PG1001
    {
        public static TheoryData<AnalyzerCase> Cases =>
        [
            new("is-null-then-throw-argument-null-is-a-guard", IsNullThrow, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 9, 9)),
            new("equals-null-then-throw-argument-null-is-a-guard", EqualsNullThrow, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 9, 9)),
            new("the-null-literal-may-sit-on-either-side-of-the-comparison", NullEqualsThrow, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 9, 9)),
            new("a-braced-single-throw-is-the-same-check", BlockThrow, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 9, 9)),
            new("coalescing-into-a-throw-is-a-guard", CoalesceThrow, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 11, 17)),
            new("throw-if-null-is-a-guard", ThrowIfNull, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 9, 9)),
            new("throwing-another-exception-type-is-not-a-null-guard", OtherExceptionThrow, new AnalyzerExpected(true)),
            new("throwing-a-factory-made-exception-is-left-alone", ThrownExpressionIsNotCreation, new AnalyzerExpected(true)),
            new("a-length-check-is-not-a-null-check", NotANullCheck, new AnalyzerExpected(true)),
            new("an-inequality-check-is-not-a-null-check", NotEqualsNull, new AnalyzerExpected(true)),
            new("an-is-not-null-pattern-is-not-a-null-check", IsNotNullPattern, new AnalyzerExpected(true)),
            new("a-constant-pattern-other-than-null-is-not-a-null-check", IsEmptyStringPattern, new AnalyzerExpected(true)),
            new("only-a-plain-identifier-is-guarded-not-a-member-access", MemberIsNullPattern, new AnalyzerExpected(true)),
            new("an-if-with-an-else-branch-is-left-alone", ElseBranch, new AnalyzerExpected(true)),
            new("a-block-doing-more-than-throwing-is-left-alone", TwoStatementBlock, new AnalyzerExpected(true)),
            new("an-if-that-does-not-throw-is-left-alone", NoThrowAtAll, new AnalyzerExpected(true)),
            new("coalescing-into-another-exception-type-is-left-alone", CoalesceOtherException, new AnalyzerExpected(true)),
            new("coalescing-from-a-call-rather-than-an-identifier-is-left-alone", CoalesceOfExpression, new AnalyzerExpected(true)),
            new("a-throw-expression-outside-a-coalesce-is-left-alone", ThrowExpressionOutsideCoalesce, new AnalyzerExpected(true)),
            new("throw-if-null-with-an-explicit-param-name-is-left-alone", ThrowIfNullWithParamName, new AnalyzerExpected(true)),
            new("throw-if-null-of-a-call-rather-than-an-identifier-is-left-alone", ThrowIfNullOfExpression, new AnalyzerExpected(true)),
            new("a-different-throw-if-helper-is-left-alone", ThrowIfNullOrEmpty, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> WithoutPineGuardReferenceCases =>
        [
            new("no-pineguard-reference-means-no-suggestion", IsNullThrow, new AnalyzerExpected(true)),
            new("no-pineguard-reference-silences-throw-if-null-too", ThrowIfNull, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> InsidePineGuardCases =>
        [
            new("pineguard-never-reports-on-its-own-throw-helper", IsNullThrow, new AnalyzerExpected(true)),
            new("pineguard-never-reports-on-its-own-coalesce-throw", CoalesceThrow, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> FixCases =>
        [
            new("is-null-becomes-guard-against-null", IsNullThrow, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 9, 9, IsNullThrowFixed)),
            new("a-coalesce-throw-becomes-a-guard-that-returns-the-value", CoalesceThrow, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 11, 17, CoalesceThrowFixed)),
            new("throw-if-null-becomes-guard-against-null", ThrowIfNull, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 9, 9, ThrowIfNullFixed)),
            new("an-existing-guard-clauses-using-is-not-duplicated", ExistingGuardClausesUsing, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 10, 9, ExistingGuardClausesUsingFixed)),
            new("a-file-with-no-usings-gains-one", NoUsings, new AnalyzerExpected(false, NameMessage, UseGuardAgainstNull, 7, 9, NoUsingsFixed))
        ];

        public static TheoryData<AnalyzerCase> FixAllCases =>
        [
            new("two-checks-are-fixed-together-and-the-using-is-added-once", TwoCheckedParameters, new AnalyzerExpected(false, null, UseGuardAgainstNull, null, null, TwoCheckedParametersFixed))
        ];
    }
}
