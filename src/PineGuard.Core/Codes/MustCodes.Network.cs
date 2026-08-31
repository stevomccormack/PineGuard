namespace PineGuard.Codes;

// Serves: MustNetworkClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>network</c> domain: IP addresses, hostnames, port numbers, and CIDR range membership.</summary>
    public static class Network
    {
        /// <summary>The code prefix for this node (<c>"network"</c>).</summary>
        public const string Prefix = "network";

        /// <summary>The IP address the value carries and the family it belongs to.</summary>
        public static class Address
        {
            /// <summary>The code prefix for this node (<c>"network.address"</c>).</summary>
            public const string Prefix = Network.Prefix + ".address";

            /// <summary><c>network.address.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>network.address.not-ipv4</c></summary>
            public const string NotIpv4 = Prefix + ".not-ipv4";

            /// <summary><c>network.address.not-ipv6</c></summary>
            public const string NotIpv6 = Prefix + ".not-ipv6";

            /// <summary><c>network.address.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";

            /// <summary><c>network.address.ipv4</c></summary>
            public const string Ipv4 = Prefix + ".ipv4";

            /// <summary><c>network.address.ipv6</c></summary>
            public const string Ipv6 = Prefix + ".ipv6";
        }

        /// <summary>The DNS hostname the value carries.</summary>
        public static class Hostname
        {
            /// <summary>The code prefix for this node (<c>"network.hostname"</c>).</summary>
            public const string Prefix = Network.Prefix + ".hostname";

            /// <summary><c>network.hostname.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>network.hostname.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";
        }

        /// <summary>The transport port number the value carries.</summary>
        public static class Port
        {
            /// <summary>The code prefix for this node (<c>"network.port"</c>).</summary>
            public const string Prefix = Network.Prefix + ".port";

            /// <summary><c>network.port.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>network.port.well-formed</c></summary>
            public const string WellFormed = Prefix + ".well-formed";
        }

        /// <summary>Membership of the address in a CIDR range.</summary>
        public static class Cidr
        {
            /// <summary>The code prefix for this node (<c>"network.cidr"</c>).</summary>
            public const string Prefix = Network.Prefix + ".cidr";

            /// <summary><c>network.cidr.out-of-range</c></summary>
            public const string OutOfRange = Prefix + ".out-of-range";

            /// <summary><c>network.cidr.in-range</c></summary>
            public const string InRange = Prefix + ".in-range";
        }

        /// <summary>The MAC (media access control) address the value carries.</summary>
        public static class Mac
        {
            /// <summary>The code prefix for this node (<c>"network.mac"</c>).</summary>
            public const string Prefix = Network.Prefix + ".mac";

            /// <summary><c>network.mac.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
