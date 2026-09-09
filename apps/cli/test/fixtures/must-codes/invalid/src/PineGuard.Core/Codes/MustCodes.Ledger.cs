using PineGuard.Rules; // (g) violation: Codes/ must stay a dependency-free leaf — this "using PineGuard..." line should not be here.

namespace PineGuard.Codes;

// Serves: MustLedgerClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>ledger</c> domain: reconciliation checks on ledger entries.</summary>
    public static class Ledger
    {
        /// <summary>The code prefix for this node (<c>"ledger"</c>).</summary>
        public const string Prefix = "ledger";

        /// <summary>A single ledger entry.</summary>
        public static class Entry
        {
            /// <summary>The code prefix for this node (<c>"ledger.entry"</c>).</summary>
            public const string Prefix = Ledger.Prefix + ".entry";

            /// <summary><c>ledger.entry.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
