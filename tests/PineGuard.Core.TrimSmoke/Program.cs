using PineGuard.Common;

namespace PineGuard.Core.TrimSmoke;

internal static class Program
{
    private static void Main()
    {
        var derived = Enumeration<string>.GetAll<DerivedStatus>();
        Require(derived.Count == 2, "Inherited and declared eligible fields must survive trimming.");
        Require(derived.Select(x => x.Name).SequenceEqual(new[] { "Inherited", "Own" }), "Catalogue order must remain ordinal.");
        Require(Enumeration<string>.FromName<DerivedStatus>("INHERITED")?.Value == "inherited", "Inherited names remain case-insensitive.");
        Require(Enumeration<string>.FromValue<DerivedStatus>("own")?.Name == "Own", "Declared values remain discoverable.");
        Require(Enumeration<string>.GetAll<BaseStatus>().Count == 2, "Base query discovers its declared eligible fields.");
        Require(Enumeration<string>.GetAll<CaseStatus>().Count == 2, "Case-distinct string identities remain separate.");
        Require(Enumeration<string>.FromValue<CaseStatus>("a")?.Name == "Lower", "Lower identity survives trimming.");
        Require(Enumeration<string>.FromValue<CaseStatus>("A")?.Name == "Upper", "Upper identity survives trimming.");
        Require(ReferenceEquals(derived, Enumeration<string>.GetAll<DerivedStatus>()), "Completed catalogue is cached.");
        Console.WriteLine("Enumeration trimmed consumer passed.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
