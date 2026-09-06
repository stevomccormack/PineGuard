namespace Sample.Qux.Core;

// The referenced project also doesn't declare "Qux" anywhere — proving the
// cross-project fix doesn't accidentally rescue a genuine orphan just
// because *some* project reference exists to check.
public sealed class QuxCoreThing
{
    public string Value { get; init; } = string.Empty;
}
