namespace PineGuard.Codes;

// Serves: MustGammaClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>gamma</c> domain: trend checks on a measured level.</summary>
    public static class Gamma
    {
        /// <summary>The code prefix for this node (<c>"gamma"</c>).</summary>
        public const string Prefix = "gamma";

        /// <summary>The measured level.</summary>
        public static class Level
        {
            /// <summary>The code prefix for this node (<c>"gamma.level"</c>).</summary>
            public const string Prefix = Gamma.Prefix + ".level";

            /// <summary><c>gamma.level.low</c></summary>
            public const string Low = Prefix + ".low";

            /// <summary><c>gamma.level.high</c></summary>
            public const string High = Prefix + ".high";
        }
    }
}
