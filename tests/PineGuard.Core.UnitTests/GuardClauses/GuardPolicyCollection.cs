namespace PineGuard.Core.UnitTests.GuardClauses;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class GuardPolicyCollection
{
    public const string Name = "Guard policy";
}

