using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>Fake Fluent extension — must-usage boundary/ fixture. Never actually calls Must.Be.Qux, only mentions it in a comment.</summary>
public static class FakeFluentExtensions
{
    // TODO: consider validating via Must.Be.Qux(value) here eventually.
    public static bool ValidateQux(bool value) => value;
}
