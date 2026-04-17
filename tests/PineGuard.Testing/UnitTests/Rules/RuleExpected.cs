using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.Rules;

public sealed record RuleExpected(bool IsValid) : IExpectedResult;
