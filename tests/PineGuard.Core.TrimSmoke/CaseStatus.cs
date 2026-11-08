using PineGuard.Common;

namespace PineGuard.Core.TrimSmoke;

public sealed class CaseStatus : StringEnumeration
{
    public static readonly CaseStatus Lower = new("a", "Lower");
    public static readonly CaseStatus Upper = new("A", "Upper");

    private CaseStatus(string value, string name) : base(value, name) { }
}
