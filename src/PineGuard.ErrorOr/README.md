# PineGuard.ErrorOr

**Your validation result, spelled the way your domain already speaks.**

A codebase built on [ErrorOr](https://github.com/amantinband/error-or) returns `ErrorOr<T>` from every handler and never throws for a validation failure. PineGuard returns `MustResult<T>` and `MustValidationResult`. Without a bridge, every handler hand-writes the same six lines of conversion. This package is that bridge, and nothing else: four extension methods that carry PineGuard's rule **code**, **message** and **property path** into `ErrorOr`'s own `Error.Validation`.

**One rule library. Every call site in your architecture.** The same `Must.Be.Email` your constructors guard with becomes an `ErrorOr<string>` in your handler, with the code intact.

## Install

```bash
dotnet add package PineGuard.ErrorOr
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core) and `ErrorOr`. Add [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses) too if you want to write `Must.Be.*` at the call site — this package deliberately does not force that dependency on you.

### Supported frameworks

Targets `net8.0`, `net10.0`, and `netstandard2.1`. `ErrorOr` ships a `netstandard2.0` asset, so every one of those three resolves.

## What you get

- `MustResult<T>.ToErrorOr()` — success carries the typed `Result` through; failure becomes a single `Error.Validation(code, message)`.
- `MustFailure.ToError()` — one failure, one `Error`.
- `MustValidationResult.ToErrors()` — every failure as a `List<Error>`; an empty list on success.
- `MustValidationResult.ToErrorOr(value)` — success carries `value`; failure carries every `Error`.

Every produced `Error` has `Code` set to PineGuard's three-segment rule address (`email.address.invalid`), `Description` set to the rendered message, `Type` set to `ErrorType.Validation`, and `Metadata["propertyPath"]` set to where in the object the failure was found (`""` at the root).

## Examples

One value, one rule:

```csharp
using ErrorOr;
using PineGuard.ErrorOr;
using PineGuard.MustClauses;

ErrorOr<string> email = Must.Be.Email(input).ToErrorOr();

// email.IsError               -> true
// email.FirstError.Code       -> "email.address.invalid"
// email.FirstError.Type       -> ErrorType.Validation
```

A whole object, every failure kept:

```csharp
MustValidationResult result = new CreateOrderValidator().Validate(command);

ErrorOr<CreateOrder> order = result.ToErrorOr(command);

return order.Match(
    value => Results.Ok(value),
    errors => Results.ValidationProblem(errors.ToDictionary(e => e.Code, e => new[] { e.Description })));
```

Or take the errors on their own, to merge with errors your own code produced:

```csharp
List<Error> errors = result.ToErrors();      // empty when the result succeeded
errors.AddRange(inventory.Errors);
```

### A note on `null` results

The bridges follow the clause's own `Result` contract. A clause that succeeds with a `null` result — `Must.Be.NullOrEmpty(null)`, for example — produces an `ErrorOr<T>` whose `Value` is `default`. That is the clause's answer, faithfully carried across, not a conversion failure.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
