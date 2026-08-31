namespace PineGuard.Analyzers.UnitTests;

public static class PreferGuardAnalyzerTestData
{
    private const string UseGuardAgainstNull = "PG1001";
    private const string UseGuardAgainstNullOrWhiteSpace = "PG1002";
    private const string UseGuardAgainstNullOrEmpty = "PG1003";
    private const string NameMessage = "Replace this null check with Guard.Against.Null(name)";
    private const string NameWhiteSpaceMessage = "Replace this null-or-whitespace check with Guard.Against.NullOrWhiteSpace(name)";
    private const string NameEmptyMessage = "Replace this null-or-empty check with Guard.Against.NullOrEmpty(name)";

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

    private const string CoalesceFactoryThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            private readonly string _name;

            public Order(string name)
            {
                _name = name ?? throw Failure(name);
            }

            private static Exception Failure(string value) => new ArgumentNullException(value);
        }
        """;

    private const string CustomThrowIfNull = """
        using System;

        namespace Sample;

        public static class Text
        {
            public static void ThrowIfNull(string value) => Console.WriteLine(value);
        }

        public class Order
        {
            public void Ship(string name)
            {
                Text.ThrowIfNull(name);
            }
        }
        """;

    private const string IsNullOrWhiteSpaceThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string IsNullOrWhiteSpaceThrowFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.NullOrWhiteSpace(name);
            }
        }
        """;

    private const string IsNullOrWhiteSpaceBlockThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("A name is required.", nameof(name));
                }
            }
        }
        """;

    private const string ThrowIfNullOrWhiteSpace = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(name);
            }
        }
        """;

    private const string ThrowIfNullOrWhiteSpaceFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.NullOrWhiteSpace(name);
            }
        }
        """;

    private const string IsNullOrWhiteSpaceOtherException = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new InvalidOperationException();
            }
        }
        """;

    private const string IsNullOrWhiteSpaceOfExpression = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrWhiteSpace(Normalize(name)))
                    throw new ArgumentException("A name is required.", nameof(name));
            }

            private static string Normalize(string value) => value;
        }
        """;

    private const string NegatedIsNullOrWhiteSpace = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (!string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string CustomIsNullOrWhiteSpace = """
        using System;

        namespace Sample;

        public static class Text
        {
            public static bool IsNullOrWhiteSpace(string value) => value.Length == 0;
        }

        public class Order
        {
            public void Ship(string name)
            {
                if (Text.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string StringContainsThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string forbidden)
            {
                if (name.Contains(forbidden))
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string LengthCheckArgumentException = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (name.Length == 0)
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string CustomThrowIfNullOrWhiteSpace = """
        using System;

        namespace Sample;

        public static class Text
        {
            public static void ThrowIfNullOrWhiteSpace(string value) => Console.WriteLine(value);
        }

        public class Order
        {
            public void Ship(string name)
            {
                Text.ThrowIfNullOrWhiteSpace(name);
            }
        }
        """;

    private const string TwoWhiteSpaceCheckedParameters = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("A name is required.", nameof(name));
                if (string.IsNullOrWhiteSpace(address))
                    throw new ArgumentException("An address is required.", nameof(address));
            }
        }
        """;

    private const string TwoWhiteSpaceCheckedParametersFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                Guard.Against.NullOrWhiteSpace(name);
                Guard.Against.NullOrWhiteSpace(address);
            }
        }
        """;

    private const string IsNullOrEmptyThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string IsNullOrEmptyThrowFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.NullOrEmpty(name);
            }
        }
        """;

    private const string IsNullOrEmptyBlockThrow = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("A name is required.", nameof(name));
                }
            }
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

    private const string ThrowIfNullOrEmptyFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Guard.Against.NullOrEmpty(name);
            }
        }
        """;

    private const string ThrowIfNullOrEmptyWithParamName = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            }
        }
        """;

    private const string IsNullOrEmptyOtherException = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrEmpty(name))
                    throw new InvalidOperationException();
            }
        }
        """;

    private const string IsNullOrEmptyOfExpression = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (string.IsNullOrEmpty(Normalize(name)))
                    throw new ArgumentException("A name is required.", nameof(name));
            }

            private static string Normalize(string value) => value;
        }
        """;

    private const string NegatedIsNullOrEmpty = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                if (!string.IsNullOrEmpty(name))
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string CustomIsNullOrEmpty = """
        using System;

        namespace Sample;

        public static class Text
        {
            public static bool IsNullOrEmpty(string value) => value.Length == 0;
        }

        public class Order
        {
            public void Ship(string name)
            {
                if (Text.IsNullOrEmpty(name))
                    throw new ArgumentException("A name is required.", nameof(name));
            }
        }
        """;

    private const string CustomThrowIfNullOrEmpty = """
        using System;

        namespace Sample;

        public static class Text
        {
            public static void ThrowIfNullOrEmpty(string value) => Console.WriteLine(value);
        }

        public class Order
        {
            public void Ship(string name)
            {
                Text.ThrowIfNullOrEmpty(name);
            }
        }
        """;

    private const string TwoEmptyCheckedParameters = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("A name is required.", nameof(name));
                if (string.IsNullOrEmpty(address))
                    throw new ArgumentException("An address is required.", nameof(address));
            }
        }
        """;

    private const string TwoEmptyCheckedParametersFixed = """
        using System;
        using PineGuard.GuardClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                Guard.Against.NullOrEmpty(name);
                Guard.Against.NullOrEmpty(address);
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
            new("coalescing-into-a-factory-made-exception-is-left-alone", CoalesceFactoryThrow, new AnalyzerExpected(true)),
            new("a-throw-if-null-declared-somewhere-other-than-the-framework-is-left-alone", CustomThrowIfNull, new AnalyzerExpected(true)),
            new("a-throw-expression-outside-a-coalesce-is-left-alone", ThrowExpressionOutsideCoalesce, new AnalyzerExpected(true)),
            new("throw-if-null-with-an-explicit-param-name-is-left-alone", ThrowIfNullWithParamName, new AnalyzerExpected(true)),
            new("throw-if-null-of-a-call-rather-than-an-identifier-is-left-alone", ThrowIfNullOfExpression, new AnalyzerExpected(true))
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

    public static class PG1002
    {
        public static TheoryData<AnalyzerCase> Cases =>
        [
            new("is-null-or-white-space-then-throw-argument-is-a-guard", IsNullOrWhiteSpaceThrow, new AnalyzerExpected(false, NameWhiteSpaceMessage, UseGuardAgainstNullOrWhiteSpace, 9, 9)),
            new("a-braced-single-throw-is-the-same-check", IsNullOrWhiteSpaceBlockThrow, new AnalyzerExpected(false, NameWhiteSpaceMessage, UseGuardAgainstNullOrWhiteSpace, 9, 9)),
            new("throw-if-null-or-white-space-is-a-guard", ThrowIfNullOrWhiteSpace, new AnalyzerExpected(false, NameWhiteSpaceMessage, UseGuardAgainstNullOrWhiteSpace, 9, 9)),
            new("throwing-another-exception-type-is-not-an-argument-guard", IsNullOrWhiteSpaceOtherException, new AnalyzerExpected(true)),
            new("checking-a-call-rather-than-an-identifier-is-left-alone", IsNullOrWhiteSpaceOfExpression, new AnalyzerExpected(true)),
            new("a-negated-check-asserts-the-opposite-and-is-left-alone", NegatedIsNullOrWhiteSpace, new AnalyzerExpected(true)),
            new("an-is-null-or-white-space-declared-somewhere-other-than-string-is-left-alone", CustomIsNullOrWhiteSpace, new AnalyzerExpected(true)),
            new("another-string-predicate-is-not-an-emptiness-check", StringContainsThrow, new AnalyzerExpected(true)),
            new("a-condition-that-is-not-a-call-at-all-is-left-alone", LengthCheckArgumentException, new AnalyzerExpected(true)),
            new("a-throw-if-null-or-white-space-declared-somewhere-other-than-the-framework-is-left-alone", CustomThrowIfNullOrWhiteSpace, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> WithoutPineGuardReferenceCases =>
        [
            new("no-pineguard-reference-means-no-suggestion", IsNullOrWhiteSpaceThrow, new AnalyzerExpected(true)),
            new("no-pineguard-reference-silences-throw-if-null-or-white-space-too", ThrowIfNullOrWhiteSpace, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> InsidePineGuardCases =>
        [
            new("pineguard-never-reports-on-its-own-white-space-check", IsNullOrWhiteSpaceThrow, new AnalyzerExpected(true)),
            new("pineguard-never-reports-on-its-own-throw-if-null-or-white-space", ThrowIfNullOrWhiteSpace, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> FixCases =>
        [
            new("is-null-or-white-space-becomes-guard-against-null-or-white-space", IsNullOrWhiteSpaceThrow, new AnalyzerExpected(false, NameWhiteSpaceMessage, UseGuardAgainstNullOrWhiteSpace, 9, 9, IsNullOrWhiteSpaceThrowFixed)),
            new("throw-if-null-or-white-space-becomes-guard-against-null-or-white-space", ThrowIfNullOrWhiteSpace, new AnalyzerExpected(false, NameWhiteSpaceMessage, UseGuardAgainstNullOrWhiteSpace, 9, 9, ThrowIfNullOrWhiteSpaceFixed))
        ];

        public static TheoryData<AnalyzerCase> FixAllCases =>
        [
            new("two-checks-are-fixed-together-and-the-using-is-added-once", TwoWhiteSpaceCheckedParameters, new AnalyzerExpected(false, null, UseGuardAgainstNullOrWhiteSpace, null, null, TwoWhiteSpaceCheckedParametersFixed))
        ];
    }

    public static class PG1003
    {
        public static TheoryData<AnalyzerCase> Cases =>
        [
            new("is-null-or-empty-then-throw-argument-is-a-guard", IsNullOrEmptyThrow, new AnalyzerExpected(false, NameEmptyMessage, UseGuardAgainstNullOrEmpty, 9, 9)),
            new("a-braced-single-throw-is-the-same-check", IsNullOrEmptyBlockThrow, new AnalyzerExpected(false, NameEmptyMessage, UseGuardAgainstNullOrEmpty, 9, 9)),
            new("throw-if-null-or-empty-is-a-guard", ThrowIfNullOrEmpty, new AnalyzerExpected(false, NameEmptyMessage, UseGuardAgainstNullOrEmpty, 9, 9)),
            new("throwing-another-exception-type-is-not-an-argument-guard", IsNullOrEmptyOtherException, new AnalyzerExpected(true)),
            new("checking-a-call-rather-than-an-identifier-is-left-alone", IsNullOrEmptyOfExpression, new AnalyzerExpected(true)),
            new("a-negated-check-asserts-the-opposite-and-is-left-alone", NegatedIsNullOrEmpty, new AnalyzerExpected(true)),
            new("an-is-null-or-empty-declared-somewhere-other-than-string-is-left-alone", CustomIsNullOrEmpty, new AnalyzerExpected(true)),
            new("a-throw-if-null-or-empty-declared-somewhere-other-than-the-framework-is-left-alone", CustomThrowIfNullOrEmpty, new AnalyzerExpected(true)),
            new("throw-if-null-or-empty-with-an-explicit-param-name-is-left-alone", ThrowIfNullOrEmptyWithParamName, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> WithoutPineGuardReferenceCases =>
        [
            new("no-pineguard-reference-means-no-suggestion", IsNullOrEmptyThrow, new AnalyzerExpected(true)),
            new("no-pineguard-reference-silences-throw-if-null-or-empty-too", ThrowIfNullOrEmpty, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> InsidePineGuardCases =>
        [
            new("pineguard-never-reports-on-its-own-emptiness-check", IsNullOrEmptyThrow, new AnalyzerExpected(true)),
            new("pineguard-never-reports-on-its-own-throw-if-null-or-empty", ThrowIfNullOrEmpty, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> FixCases =>
        [
            new("is-null-or-empty-becomes-guard-against-null-or-empty", IsNullOrEmptyThrow, new AnalyzerExpected(false, NameEmptyMessage, UseGuardAgainstNullOrEmpty, 9, 9, IsNullOrEmptyThrowFixed)),
            new("throw-if-null-or-empty-becomes-guard-against-null-or-empty", ThrowIfNullOrEmpty, new AnalyzerExpected(false, NameEmptyMessage, UseGuardAgainstNullOrEmpty, 9, 9, ThrowIfNullOrEmptyFixed))
        ];

        public static TheoryData<AnalyzerCase> FixAllCases =>
        [
            new("two-checks-are-fixed-together-and-the-using-is-added-once", TwoEmptyCheckedParameters, new AnalyzerExpected(false, null, UseGuardAgainstNullOrEmpty, null, null, TwoEmptyCheckedParametersFixed))
        ];
    }
}
