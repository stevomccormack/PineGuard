namespace PineGuard.Codes;

// Serves: MustTokenClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>token</c> domain: security tokens carried between parties.</summary>
    public static class Token
    {
        /// <summary>The code prefix for this node (<c>"token"</c>).</summary>
        public const string Prefix = "token";

        /// <summary>The JSON Web Token compact serialization: header, payload and signature.</summary>
        public static class Jwt
        {
            /// <summary>The code prefix for this node (<c>"token.jwt"</c>).</summary>
            public const string Prefix = Token.Prefix + ".jwt";

            /// <summary><c>token.jwt.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";
        }
    }
}
