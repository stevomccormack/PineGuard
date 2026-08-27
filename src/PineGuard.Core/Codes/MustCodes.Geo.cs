namespace PineGuard.Codes;

// Serves: MustGeoLocationClauses.cs, MustStringGeoLocationClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>geo</c> domain: geographic coordinates, whether supplied as numbers or as strings.</summary>
    public static class Geo
    {
        /// <summary>The code prefix for this node (<c>"geo"</c>).</summary>
        public const string Prefix = "geo";

        /// <summary>The latitude component, on its own.</summary>
        public static class Latitude
        {
            /// <summary>The code prefix for this node (<c>"geo.latitude"</c>).</summary>
            public const string Prefix = Geo.Prefix + ".latitude";

            /// <summary><c>geo.latitude.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The longitude component, on its own.</summary>
        public static class Longitude
        {
            /// <summary>The code prefix for this node (<c>"geo.longitude"</c>).</summary>
            public const string Prefix = Geo.Prefix + ".longitude";

            /// <summary><c>geo.longitude.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }

        /// <summary>The latitude and longitude pair taken together.</summary>
        public static class Coordinate
        {
            /// <summary>The code prefix for this node (<c>"geo.coordinate"</c>).</summary>
            public const string Prefix = Geo.Prefix + ".coordinate";

            /// <summary><c>geo.coordinate.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
