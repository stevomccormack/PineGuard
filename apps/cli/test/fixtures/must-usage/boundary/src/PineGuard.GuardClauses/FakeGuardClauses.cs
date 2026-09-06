using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

// Legacy note: this Guard clause used to call Must.Be.Qux(value) directly;
// kept here only as a comment so the boundary fixture can prove a comment
// mention of the call shape is not treated as a real call site.
/// <summary>Fake Guard clause — must-usage boundary/ fixture. Never actually calls Must.Be.Qux.</summary>
public static class FakeGuardClauses
{
    public static void AgainstQux(bool value)
    {
        var note = "Must.Be.Qux(value)"; // string literal only — still not a real call
        System.Diagnostics.Debug.WriteLine(note);
    }
}
