namespace PineGuard.Testing.UnitTests.Rules;

public sealed record RuleScenario<TInputs>(string Name, TInputs Inputs, bool IsValid)
{
    public bool IsNull => Inputs is null;
}
