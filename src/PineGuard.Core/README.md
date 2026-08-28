# PineGuard.Core

**The validation engine behind every PineGuard layer — no third-party dependencies, zero exceptions on the happy path, zero IO.**

Core gives you pure static predicates and parsers for the data your app actually deals with: strings, numbers, dates, collections, URIs, emails, network identifiers, and OWASP-safe input. Fast, allocation-light, and safe to call from anywhere — hot paths, constructors, loops.

Most apps don't install Core directly. They install a higher layer (Must, Guard, Fluent, DataAnnotations) that wraps Core in the style that fits the call site. Install `PineGuard.Core` when you want the raw predicates without any opinion on how failures are reported.

## Install

```bash
dotnet add package PineGuard.Core
```

Targets `net8.0`, `net10.0`, and `netstandard2.1`. The only package references are the Microsoft first-party BCL packages `System.ComponentModel.Annotations` and `System.Text.Json`.

## Examples

```csharp
using PineGuard.Rules;

// Ask a question, get a bool
bool ok = EmailRules.IsEmail("alice@example.com");
bool safe = OwaspRules.IsOwaspSafe(userInput);
bool https = UriRules.IsHttpsUrl("https://example.com");

// Drill into specific OWASP categories
bool xssSafe = OwaspRules.IsXssSafe(userInput);
bool sqlSafe = OwaspRules.IsSqlInjectionSafe(userInput);

// Choose your validation strictness
bool strict = EmailRules.IsStrictEmail("alice@example.com");
```

## The object-validation keystone lives here too

`MustResult<T>`, `Must`/`IMustClause`, and the `MustValidator<T>` object-validation base class all live in
Core — the `Must.Be.*` clause catalogue (`PineGuard.MustClauses`) is a consumer of these types, not their
owner. That means you can derive from `MustValidator<T>` and build `MustResult<T>.Ok`/`Fail` results from
your own predicates without installing any other PineGuard package:

```csharp
using PineGuard.MustClauses;

public sealed class WidgetValidator : MustValidator<Widget>
{
    public WidgetValidator() =>
        RuleFor(x => x.Name, name => MustResult<string>.FromBool(
            !string.IsNullOrWhiteSpace(name), "widget.name.blank", "{paramName} must not be blank.", nameof(name), name));
}
```

Every failure — whether it comes from a hand-rolled predicate here or a `Must.Be.*` clause in
`PineGuard.MustClauses` — carries a stable, three-segment `Code` (`<domain>.<aspect>.<condition>`) alongside
its message, so callers can branch on *which rule failed* without parsing prose.

## Prefer a higher layer for application code

- **[PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)** — result-based, composable, never throws
- **[PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)** — fail-fast with typed return values
- **[PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)** — `AbstractValidator` rule-builder extensions
- **[PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)** — `[ValidationAttribute]` for DTOs and forms

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
