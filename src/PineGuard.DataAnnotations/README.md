# PineGuard.DataAnnotations

**Every PineGuard rule as a `[ValidationAttribute]`. Declarative validation for MVC, Blazor, and anywhere .NET reads DataAnnotations.**

PineGuard.DataAnnotations exposes every PineGuard rule — `[Email]`, `[HttpsUrl]`, `[OwaspSafe]`, and the rest — as attributes that plug into the .NET validation infrastructure you already have. ASP.NET Core MVC model binding runs them on `[FromBody]` with `[ApiController]`, Blazor `EditForm` renders errors via `DataAnnotationsValidator`, and `Validator.TryValidateObject` runs them anywhere else. Zero custom middleware, zero hand-rolled wiring.

DataAnnotations are the .NET idiom for input-model validation — request DTOs, view models, and Blazor form models. They're a **perfect fit for Clean Architecture**, where the always-valid-state principle holds at the Presentation boundary: model binding runs the attributes automatically, so by the time the DTO reaches your Application layer, the input is always valid.

**One rule library. Every call site in your architecture.** The same `Must.Be.Email` your services call and the same `Guard.Against.NotEmail` your domain constructors call power `[Email]` on your DTOs. No parallel dialects. No drift. When the rule changes, every layer changes with it.

## Install

```bash
dotnet add package PineGuard.DataAnnotations
```

Targets `net8.0`, `net10.0`, and `netstandard2.1`. Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses), and `System.ComponentModel.DataAnnotations`.

## Examples

```csharp
using PineGuard.DataAnnotations;

public sealed class CreateUserRequest
{
    // The four canonical rules, as [ValidationAttribute]s
    [NotNull, Email]
    public string Email { get; init; } = string.Empty;

    [NotNull, StrictEmail]
    public string StrictEmailAddress { get; init; } = string.Empty;

    [NotNull, OwaspSafe]
    public string UserInput { get; init; } = string.Empty;

    [NotNull, HttpsUrl]
    public string Callback { get; init; } = string.Empty;
}
```

### Chain with built-in DataAnnotations

PineGuard attributes compose with every built-in `System.ComponentModel.DataAnnotations` attribute — `[Required]`, `[StringLength]`, `[MaxLength]`, `[Range]`, `[RegularExpression]`, and the rest — so you can stack PineGuard content validators alongside length limits and .NET's idiomatic presence checks on the same property.

```csharp
using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations;

public sealed class UpdateProfileRequest
{
    [Required]
    [MaxLength(256)]
    [HttpsUrl]
    public string Website { get; init; } = string.Empty;

    [Required]
    [StringLength(320)] // RFC 5321 email total-length limit
    [StrictEmail]
    public string CompanyEmail { get; init; } = string.Empty;

    [MaxLength(500)]
    [OwaspSafe]
    public string Bio { get; init; } = string.Empty;
}
```

### Presence semantics: `[Required]` vs `[NotNull]` vs `[Null]`

Both PineGuard and built-in DataAnnotations cover presence, with slightly different semantics:

- **`[Required]`** (built-in) — rejects `null` *and* empty string (unless `AllowEmptyStrings = true`).
- **`[NotNull]`** (PineGuard) — rejects `null` only; allows empty string.
- **`[Null]`** (PineGuard) — the inverse; the value must remain `null`.

Pick per property. PineGuard's content attributes (`[Email]`, `[HttpsUrl]`, `[OwaspSafe]`, ...) skip null/empty by default, so they compose correctly with any of these.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete attribute catalog.

## License

MIT © Steve McCormack
