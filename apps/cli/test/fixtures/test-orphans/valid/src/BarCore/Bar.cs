namespace Sample.Bar;

/// <summary>
/// Deliberately lives under src/BarCore, not src/Bar — there is no
/// same-named "Bar" source project in this fixture at all. Only resolving
/// BarTests.cs's "Bar" via Bar.UnitTests.csproj's &lt;ProjectReference&gt; to
/// BarCore proves the cross-project fix, not just the same-project case.
/// </summary>
public sealed class Bar
{
    public string Name { get; init; } = string.Empty;
}
