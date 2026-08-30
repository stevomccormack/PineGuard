# PineGuard.FluentResults

**Your validation result, spelled the way your domain already speaks.**

A codebase built on [FluentResults](https://github.com/altmann/FluentResults) returns `Result` and `Result<T>` and reasons about failure through `IError`. PineGuard returns `MustResult<T>` and `MustValidationResult`. Without a bridge, every handler hand-writes the same conversion — and usually loses the error code doing it. This package is that bridge, and nothing else: one error type and three extension methods that carry PineGuard's rule **code**, **message** and **property path** into a `FluentResults.Error`.

**One rule library. Every call site in your architecture.** The same `Must.Be.Email` your constructors guard with becomes a `Result<string>` in your handler, with the code intact.

## Install

```bash
dotnet add package PineGuard.FluentResults
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core) and `FluentResults`. Add [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses) too if you want to write `Must.Be.*` at the call site — this package deliberately does not force that dependency on you.

### Supported frameworks

Targets `net8.0`, `net10.0`, and `netstandard2.1`. `FluentResults` ships `netstandard2.0` and `netstandard2.1` assets, so every one of those three resolves.

## What you get

- `MustError` — a `FluentResults.Error` with `Code` and `PropertyPath` properties, mirrored into `Metadata["code"]` and `Metadata["propertyPath"]` so error handling that only knows `IError` still reads them.
- `MustError.From(IMustResult)` and `MustError.From(MustFailure)` — build one from either shape.
- `MustResult<T>.ToResult()` — success carries the typed `Result` through; failure fails with one `MustError`.
- `MustValidationResult.ToResult()` and `.ToResult(value)` — success is `Result.Ok()` / `Result.Ok(value)`; failure carries a `MustError` per failure.

## Examples

One value, one rule:

```csharp
using FluentResults;
using PineGuard.FluentResults;
using PineGuard.MustClauses;

Result<string> email = Must.Be.Email(input).ToResult();

// email.IsFailed                                  -> true
// email.Errors.OfType<MustError>().First().Code   -> "email.address.invalid"
// email.Errors[0].Metadata["propertyPath"]        -> "input"
```

A whole object, every failure kept:

```csharp
MustValidationResult validation = new CreateOrderValidator().Validate(command);

Result<CreateOrder> order = validation.ToResult(command);

return order.IsSuccess
    ? Results.Ok(order.Value)
    : Results.ValidationProblem(order.Errors.OfType<MustError>()
        .ToDictionary(e => e.PropertyPath, e => new[] { e.Message }));
```

Or bridge without a value, when the caller only needs pass/fail:

```csharp
Result gate = validation.ToResult();
gate.Bind(() => _repository.Save(command));
```

`MustError.From` is deliberately strict: passing a *successful* `IMustResult` throws `ArgumentException`, because a success has no code, no message and nothing to report.

### A note on `null` results

The bridges follow the clause's own `Result` contract. A clause that succeeds with a `null` result — `Must.Be.NullOrEmpty(null)`, for example — produces a `Result<T>` whose `Value` is `default`. That is the clause's answer, faithfully carried across, not a conversion failure.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
