namespace PineGuard.Analyzers.UnitTests;

public static class DiscardedMustResultAnalyzerTestData
{
    private const string DiscardedMustResult = "PG2001";
    private const string NotNullDiscardedMessage = "The MustResult from 'NotNull' is discarded, so a failed check passes unnoticed";

    private const string MustResultDiscarded = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Must.Be.NotNull(name);
            }
        }
        """;

    private const string MustResultThrown = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Must.Be.NotNull(name).ThrowIfFailed();
            }
        }
        """;

    private const string MustResultAssigned = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                var result = Must.Be.NotNull(name);
            }
        }
        """;

    private const string MustResultDiscardedIntoAWildcard = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                _ = Must.Be.NotNull(name);
            }
        }
        """;

    private const string MustResultReassigned = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                var result = Must.Be.NotNull(name);
                result = Must.Be.NotNull(address);
            }
        }
        """;

    private const string MustResultReturned = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public MustResult<string> Check(string name)
            {
                return Must.Be.NotNull(name);
            }
        }
        """;

    private const string MustResultAwaited = """
        using System.Threading.Tasks;
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public async Task ShipAsync(string name)
            {
                await Task.FromResult(Must.Be.NotNull(name));
            }
        }
        """;

    // Named nothing from PineGuard, so the one snippet serves both the "referenced but irrelevant"
    // and the "not referenced at all" groups.
    private const string VoidCallDiscarded = """
        using System;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Console.WriteLine(name);
            }
        }
        """;

    private const string LookAlikeResultDiscarded = """
        namespace Sample;

        public sealed class MustResult<T>
        {
        }

        public class Order
        {
            public void Ship(string name)
            {
                Check(name);
            }

            private static MustResult<string> Check(string value) => new();
        }
        """;

    private const string MustResultThrownFixed = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                Must.Be.NotNull(name).ThrowIfFailed();
            }
        }
        """;

    private const string MustResultAssignedFixed = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                var result = Must.Be.NotNull(name);
            }
        }
        """;

    private const string MustResultDiscardedBesideATakenName = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                var result = name;
                Must.Be.NotNull(name);
            }
        }
        """;

    private const string MustResultDiscardedBesideATakenNameFixed = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name)
            {
                var result = name;
                var result2 = Must.Be.NotNull(name);
            }
        }
        """;

    private const string TwoDiscardedMustResults = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                Must.Be.NotNull(name);
                Must.Be.NotNull(address);
            }
        }
        """;

    private const string TwoDiscardedMustResultsFixed = """
        using PineGuard.MustClauses;

        namespace Sample;

        public class Order
        {
            public void Ship(string name, string address)
            {
                Must.Be.NotNull(name).ThrowIfFailed();
                Must.Be.NotNull(address).ThrowIfFailed();
            }
        }
        """;

    public static class PG2001
    {
        public static TheoryData<AnalyzerCase> Cases =>
        [
            new("a-must-call-on-its-own-line-checks-nothing", MustResultDiscarded, new AnalyzerExpected(false, NotNullDiscardedMessage, DiscardedMustResult, 9, 9)),
            new("chaining-throw-if-failed-uses-the-result", MustResultThrown, new AnalyzerExpected(true)),
            new("assigning-the-result-keeps-it-for-inspection", MustResultAssigned, new AnalyzerExpected(true)),
            new("discarding-into-a-wildcard-is-a-deliberate-act", MustResultDiscardedIntoAWildcard, new AnalyzerExpected(true)),
            new("assigning-over-an-existing-local-keeps-the-result", MustResultReassigned, new AnalyzerExpected(true)),
            new("returning-the-result-hands-the-check-to-the-caller", MustResultReturned, new AnalyzerExpected(true)),
            new("an-awaited-result-is-not-a-bare-call-and-is-left-alone", MustResultAwaited, new AnalyzerExpected(true)),
            new("a-call-that-returns-nothing-discards-nothing", VoidCallDiscarded, new AnalyzerExpected(true)),
            new("a-result-type-of-the-same-name-from-another-namespace-is-left-alone", LookAlikeResultDiscarded, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> WithoutPineGuardReferenceCases =>
        [
            new("no-pineguard-reference-means-no-warning", VoidCallDiscarded, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> InsidePineGuardCases =>
        [
            new("pineguard-never-warns-about-its-own-must-calls", MustResultDiscarded, new AnalyzerExpected(true))
        ];

        public static TheoryData<AnalyzerCase> ThrowIfFailedFixCases =>
        [
            new("a-discarded-result-gains-throw-if-failed", MustResultDiscarded, new AnalyzerExpected(false, NotNullDiscardedMessage, DiscardedMustResult, 9, 9, MustResultThrownFixed))
        ];

        public static TheoryData<AnalyzerCase> AssignResultFixCases =>
        [
            new("a-discarded-result-becomes-a-local", MustResultDiscarded, new AnalyzerExpected(false, NotNullDiscardedMessage, DiscardedMustResult, 9, 9, MustResultAssignedFixed)),
            new("a-local-named-result-already-in-the-method-is-not-shadowed", MustResultDiscardedBesideATakenName, new AnalyzerExpected(false, NotNullDiscardedMessage, DiscardedMustResult, 10, 9, MustResultDiscardedBesideATakenNameFixed))
        ];

        public static TheoryData<AnalyzerCase> ThrowIfFailedFixAllCases =>
        [
            new("every-discarded-result-in-the-method-gains-throw-if-failed", TwoDiscardedMustResults, new AnalyzerExpected(false, null, DiscardedMustResult, null, null, TwoDiscardedMustResultsFixed))
        ];
    }
}
