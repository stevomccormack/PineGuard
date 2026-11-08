using PineGuard.Common;

namespace PineGuard.Core.TrimSmoke;

public class BaseStatus : StringEnumeration
{
    public static readonly BaseStatus Base = new("base", "Base");
    public static readonly DerivedStatus Inherited = new("inherited", "Inherited");

    protected BaseStatus(string value, string name) : base(value, name) { }
}
