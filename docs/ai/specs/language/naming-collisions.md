---
spec:
  id: pineguard.ai.language.naming-collisions
  title: "Naming Collisions"
  version: 2
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/**"
---

# Naming Collisions

This specification defines how PineGuard names must avoid collisions: adapter surfaces against framework-native APIs (§Rule–§Family rule, v1), and importable type names against the types consumers and other frameworks already own (§Type names vs vocabulary, v2).

## Rule

When an adapter name would collide with, closely mimic, or visually disappear into a framework-native validator, prefer a clearer PineGuard-specific public name.

Priorities:

1. Avoid exact framework collisions.
2. Avoid strong semantic near-collisions that imply framework-native behavior.
3. Prefer explicitness over mechanical parity with Must naming.
4. Do not rely on documentation to rescue an ambiguous public API.

## DataAnnotations

For `PineGuard.DataAnnotations`, compare public attribute usage names after dropping the `Attribute` suffix.

Examples:

- Prefer `WebUrlAttribute` over `UrlAttribute` to avoid colliding with framework-native `[Url]`.
- Prefer type/domain-qualified names when a short adapter name would be misleading in attribute form.

## FluentValidation

For `PineGuard.FluentValidation`, compare public extension method names against FluentValidation built-ins and strong IntelliSense expectations.

Examples:

- Prefer an explicit collision-safe name such as `WebUrl()` when a shorter name like `Url()` would be ambiguous in IntelliSense.
- Prefer `Required()` / `NotRequired()` over `NotNull()` / `Null()` when PineGuard adapts null semantics into FluentValidation, because the built-in validators already own the shorter names.
- Adapter method names may intentionally diverge from Must naming when collision avoidance materially improves clarity.
- When explicitly chosen for ergonomics, a short alias may remain as a thin facade that forwards to the clearer preferred method.

## Family rule

Rename the whole adapter family only when that materially improves clarity. Otherwise, a targeted rename for the collided adapter surface is acceptable.

When both names exist, document one explicit name as preferred and keep any alternate name as a thin forwarder only.

## Type names vs vocabulary

Ecosystem alignment and collision risk rise together: the more universal a word, the more likely a consumer or another framework already owns it. A name competes only with what shares its scope, so the rule splits by scope.

| Scope | Competes with | Rule | Examples |
|---|---|---|---|
| Members, verbs, parameters, patterns | nothing in the consumer's type list | **Align** with the ecosystem word | `RuleFor`, `PropertyPath`, `BeginScope`, `.WithErrorCode`, `Validate` |
| Importable public type names | every other `using` in the consumer's file | **Distinguish** with the mechanism qualifier (`Must`, `Guard`, …) | `MustCodes` not `ErrorCodes`; `MustPropertyRule` not `PropertyRule` (FluentValidation); `MustValidationResult` not `ValidationResult` (DataAnnotations, FluentValidation); `MustFailure` / `GuardFailure` not `Failure` |
| Namespaces | the consumer's own folder/namespace vocabulary | Prefer the artefact word over the populace folder word | `PineGuard.Codes` over `PineGuard.Errors` (`Errors` is the Clean-Architecture folder name) |

Before adopting a type name that "everyone uses", treat that popularity as a collision signal and check what FluentValidation, DataAnnotations, ErrorOr, FluentResults, MediatR, ASP.NET Core and a typical Clean-Architecture solution already call a type by that name. Record the check in the name's rejected-alternatives entry.

Cost model: a qualifier costs the library four characters once; a collision costs every implementer a CS0104 ambiguity and an alias in every affected file, forever.

Two corollaries:

- **Bridge packages whose last namespace segment equals a target type name** (`PineGuard.OneOf` vs `OneOf<…>`, `PineGuard.ErrorOr` vs `ErrorOr<T>`/`Error`, `PineGuard.FluentResults` vs `Error`) fully qualify every target type with `global::` in source, because inside `namespace PineGuard.OneOf;` the simple name `OneOf` binds to the enclosing namespace first.
- **Analyzer assemblies are exempt from the distinguish rule** for their internal helper types (`DiagnosticIds`, `DiagnosticDescriptors`): they ship as a development dependency, are never referenced by consumer code, and are `internal`.

### Decision record — `PineGuard.Codes.MustCodes` (2026-08-26)

- **Context.** The error-code catalogue (Plan 00 §5.4 grammar; Plan 01 §4.1) needed a type and namespace. A full alternatives pass ranked `ErrorCodes` in `PineGuard.Errors` as the more recognisable pair: "error codes" is the universal phrase (Stripe, Azure SDK, FluentValidation's `ErrorCode`), it reads naturally with `.WithErrorCode(ErrorCodes.Email.Address.Invalid)`, and `Errors` is the populace folder word.
- **Decision.** `MustCodes` in `PineGuard.Codes`. Deciding factor: `ErrorCodes` (and `Errors`) are type names that consumer applications and other frameworks routinely own; `MustCodes` collides with nothing in the ecosystem. Secondary: every other public type in the program is Must-named (`MustResult`, `MustFailure`, `MustValidator`, `MustValidationResult`), and the codes are born in Must — Guard, FluentValidation and DataAnnotations all call Must — so the name is true as well as safe.
- **Consequences.** Consumers write `using PineGuard.Codes;` and `MustCodes.Email.Address.Invalid` in every layer; no aliasing is ever required. Accepted trade-off: "Codes" could be misread as the ISO/postal codes the library validates; the `MustCodes` type name at every use site resolves it. This record sets the precedent the table above generalises; the per-program trail is Plan 00 §12.
