namespace PineGuard.MustClauses;

/// <summary>
/// Default implementation of <see cref="IMustClause"/>, used as the receiver for all
/// <c>Must.Be.*</c> extension methods.
/// </summary>
/// <remarks>
/// Do not instantiate this class directly. Use <see cref="Must.Be"/> instead.
/// </remarks>
/// <seealso cref="Must"/>
/// <seealso cref="IMustClause"/>
public sealed class MustClause : IMustClause
{
    internal MustClause() { }
}
