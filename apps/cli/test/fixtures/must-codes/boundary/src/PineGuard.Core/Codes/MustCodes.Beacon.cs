namespace PineGuard.Codes;

// Serves: MustBeaconClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>beacon</c> domain: signal-state checks on beacons.</summary>
    public static class Beacon
    {
        /// <summary>The code prefix for this node (<c>"beacon"</c>).</summary>
        public const string Prefix = "beacon";

        /// <summary>The beacon's signal.</summary>
        public static class Signal
        {
            /// <summary>The code prefix for this node (<c>"beacon.signal"</c>).</summary>
            public const string Prefix = Beacon.Prefix + ".signal";

            /// <summary><c>beacon.signal.lost</c></summary>
            public const string Lost = Prefix + ".lost";
        }

        /// <summary>
        /// Reserved for a future satellite-uplink adapter that has not landed yet. No clause
        /// emits this directly.
        /// </summary>
        public static class Uplink
        {
            /// <summary>The code prefix for this node (<c>"beacon.uplink"</c>).</summary>
            public const string Prefix = Beacon.Prefix + ".uplink";

            /// <summary><c>beacon.uplink.pending</c></summary>
            public const string Pending = Prefix + ".pending";
        }

        /// <summary>Diagnostic counters for the beacon.</summary>
        public static class Diagnostics
        {
            /// <summary>The code prefix for this node (<c>"beacon.diagnostics"</c>).</summary>
            public const string Prefix = Beacon.Prefix + ".diagnostics";

            /// <summary><c>beacon.diagnostics.stale</c></summary>
            public const string Stale = Prefix + ".stale";
        }
    }
}
