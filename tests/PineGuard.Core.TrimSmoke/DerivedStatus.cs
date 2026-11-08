namespace PineGuard.Core.TrimSmoke;

public sealed class DerivedStatus : BaseStatus
{
    public static readonly DerivedStatus Own = new("own", "Own");

    internal DerivedStatus(string value, string name) : base(value, name) { }
}
