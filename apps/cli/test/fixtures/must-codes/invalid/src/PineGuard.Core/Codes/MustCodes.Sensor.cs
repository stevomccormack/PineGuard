namespace PineGuard.Codes;

// Serves: MustSensorClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>sensor</c> domain: reading validity checks on sensors.</summary>
    public static class Sensor
    {
        /// <summary>The code prefix for this node (<c>"sensor"</c>).</summary>
        public const string Prefix = "sensor";

        /// <summary>The sensor's most recent reading.</summary>
        public static class Reading
        {
            /// <summary>The code prefix for this node (<c>"sensor.reading"</c>).</summary>
            public const string Prefix = Sensor.Prefix + ".reading";

            /// <summary><c>sensor.reading.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary>
            /// <c>sensor.reading.orphaned</c> — (b) violation fixture: this constant is declared
            /// but never referenced by any clause, DataAnnotations attribute, or usage-root call
            /// site anywhere in this fixture tree, and its doc comment does not exempt it.
            /// </summary>
            public const string Orphaned = Prefix + ".orphaned";
        }
    }
}
