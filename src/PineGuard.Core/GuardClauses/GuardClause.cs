namespace PineGuard.GuardClauses;

/// <summary>
/// Default implementation of <see cref="IGuardClause"/>, used as the receiver for all
/// <c>Guard.Against.*</c> extension methods.
/// </summary>
/// <remarks>
/// Do not instantiate this class directly. Use <see cref="Guard.Against"/> instead.
/// </remarks>
/// <seealso cref="Guard"/>
/// <seealso cref="IGuardClause"/>
public sealed class GuardClause : IGuardClause
{
    internal GuardClause() { }
}
