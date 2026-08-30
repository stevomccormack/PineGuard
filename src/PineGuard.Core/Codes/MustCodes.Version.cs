namespace PineGuard.Codes;

// Serves: MustVersionClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>version</c> domain: version identifiers a release is published under.</summary>
    public static class Version
    {
        /// <summary>The code prefix for this node (<c>"version"</c>).</summary>
        public const string Prefix = "version";

        /// <summary>The Semantic Versioning 2.0.0 form: <c>major.minor.patch</c> with optional pre-release and build metadata.</summary>
        public static class Semver
        {
            /// <summary>The code prefix for this node (<c>"version.semver"</c>).</summary>
            public const string Prefix = Version.Prefix + ".semver";

            /// <summary><c>version.semver.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
