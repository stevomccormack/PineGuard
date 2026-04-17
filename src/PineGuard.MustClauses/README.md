# PineGuard.MustClauses

**Validation that returns an answer, not an exception.**

`Must.Be.Email(value)` gives you a `MustResult<T>`. Inspect it, compose it, or escalate it to whatever exception your domain speaks. PineGuard never throws on your behalf — your API layer, your call.

Use MustClauses anywhere the caller should decide what happens on failure — API endpoints returning `400 Bad Request`, MediatR pipeline steps collecting multiple errors, services where failed validation isn't exceptional, and any Result-pattern architecture. It's also the composable primitive every other PineGuard layer is built on: Guard clauses, FluentValidation extensions, and DataAnnotations attributes all call into `Must.Be.*` under the hood.

**One rule library. Every call site in your architecture.** The same `Must.Be.Email` your services call powers `Guard.Against.NotEmail` in your domain constructors, `RuleFor(x => x.Email).Email()` in your validators, and `[Email]` on your DTOs. No parallel dialects. No drift. When the rule changes, every layer changes with it.

## Install

```bash
dotnet add package PineGuard.MustClauses
```

Targets `net8.0`, `net10.0`, and `netstandard2.1`. Depends only on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core).

## Examples

```csharp
using PineGuard.MustClauses;

// The four canonical rules, result-style
MustResult<string> email       = Must.Be.Email(userEmail);
MustResult<string> strictEmail = Must.Be.StrictEmail(userEmail);
MustResult<string> safe        = Must.Be.OwaspSafe(userInput);
MustResult<Uri>    callbackUri = Must.Be.HttpsUrl(callback);

// Inspect the result
if (email.Failed)
    return BadRequest(email.Message);

// Or collapse to a value-or-throw in one line
var url = Must.Be.HttpsUrl(callback).OrThrow();

// Escalate with your own exception type
Must.Be.Email(userEmail).ThrowIfFailed((message, paramName) =>
    new BusinessException($"{paramName}: {message}"));

// Compose
var result = Must.Be.NotNull(orderId)
    .AndThen(id => Must.Be.GuidV4(id));
```

## Other layers, same rule library

- **Constructors and service boundaries that should fail fast** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Already using FluentValidation** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)
- **Attribute-driven models (DTOs, MVC binding, Blazor)** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete `Must.Be.*` catalog.

## License

MIT © Steve McCormack
