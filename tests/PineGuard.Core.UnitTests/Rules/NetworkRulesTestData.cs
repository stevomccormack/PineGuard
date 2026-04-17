using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NetworkRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class NetworkRulesTestData
{
    public static class IsIpAddress
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsIpAddress.AllScenarios.ToRuleCases();
    }

    public static class IsIpv4
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsIpv4.AllScenarios.ToRuleCases();
    }

    public static class IsIpv6
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsIpv6.AllScenarios.ToRuleCases();
    }

    public static class IsInCidr
    {
        public static TheoryData<RuleCase<(string? ip, string cidr)>> Cases => F.IsInCidr.AllScenarios.ToRuleCases();
    }

    public static class IsValidHostname
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsValidHostname.AllScenarios.ToRuleCases();
    }

    public static class IsPortNumber
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsPortNumber.AllScenarios.ToRuleCases();
    }
}
