# PineGuard.OneOf

**Your validation result, spelled the way your domain already speaks.**

A codebase built on [OneOf](https://github.com/mcintyre321/OneOf) returns a discriminated union from every method and lets the compiler force the caller to handle both arms. PineGuard returns `MustResult<T>` and `MustValidationResult`. Without a bridge, every handler hand-writes the same conversion. This package is that bridge, and nothing else: two extension methods that put the validated value in the first arm and PineGuard's own failure type — code, message, property path and all — in the second.

**One rule library. Every call site in your architecture.** The same `Must.Be.Email` your constructors guard with becomes a `OneOf<string, MustFailure>` in your handler, with the code intact.

## Install

```bash
dotnet add package PineGuard.OneOf
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core) and `OneOf`. Add [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses) too if you want to write `Must.Be.*` at the call site — this package deliberately does not force that dependency on you.

### Supported frameworks

Targets `net8.0`, `net10.0`, and `netstandard2.1`. `OneOf` ships a `netstandard2.0` asset, so every one of those three resolves.

## What you get

- `MustResult<T>.ToOneOf()` → `OneOf<T, MustFailure>` — success is `T`, failure is the `MustFailure` with its `Code`, `Message` and `PropertyPath`.
- `MustValidationResult.ToOneOf(value)` → `OneOf<T, MustValidationResult>` — success is `value`, failure is the whole result, so every failure survives the crossing.

The second arm is PineGuard's own type in both cases: there is no new error class to learn, and no code, message or path is dropped on the way across.

## Examples

One value, one rule:

```csharp
using OneOf;
using PineGuard.MustClauses;
using PineGuard.OneOf;

OneOf<string, MustFailure> email = Must.Be.Email(input).ToOneOf();

return email.Match(
    address => Results.Ok(address),
    failure => Results.Problem(failure.Message, type: failure.Code));
```

A whole object, every failure kept:

```csharp
MustValidationResult validation = new CreateOrderValidator().Validate(command);

OneOf<CreateOrder, MustValidationResult> order = validation.ToOneOf(command);

// order.IsT0 -> the command, validated
// order.IsT1 -> the MustValidationResult, with every MustFailure in rule order
```

### A note on `null` results

The bridges follow the clause's own `Result` contract. A clause that succeeds with a `null` result — `Must.Be.NullOrEmpty(null)`, for example — produces a `OneOf` whose first arm holds `default`. That is the clause's answer, faithfully carried across, not a conversion failure.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
