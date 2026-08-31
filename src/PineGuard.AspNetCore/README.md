# PineGuard.AspNetCore

**One bad request, one 400, every failure listed — with a stable code on each.**

FluentValidation dropped official ASP.NET auto-validation because async validators cannot run inside synchronous model binding, and the community patchwork that replaced it mostly returns a body the client cannot program against. PineGuard.AspNetCore validates every action argument and every Minimal API parameter *after* binding and *before* your handler runs, aggregates the failures into one RFC 9457 `ValidationProblemDetails`, and keys the errors the way your app names things — `email`, not `Email`, when the app serialises camelCase.

**One rule library. Every call site in your architecture.** The `MustValidator<Order>` your request filter resolves is the same class your handler injects and your options binder validates at startup.

## Install

```bash
dotnet add package PineGuard.AspNetCore
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses), [PineGuard.Extensions.DependencyInjection](https://www.nuget.org/packages/PineGuard.Extensions.DependencyInjection) and the `Microsoft.AspNetCore.App` shared framework.

### Supported frameworks

Targets `net8.0` and `net10.0`. ASP.NET Core has no `netstandard` asset, so this package does not carry the `netstandard2.1` target the rest of PineGuard does.

`AddMustValidatorResolver()` — the `Microsoft.Extensions.Validation` integration — exists on `net10.0` only, because that is where the built-in validation pipeline ships.

## What you get

- **Minimal API auto-validation** — `.AddMustValidation()` on an endpoint or a whole `MapGroup`.
- **MVC auto-validation** — `AddControllers().AddMustValidation()`; the same body, and `ModelState` populated too.
- **RFC 9457 `ValidationProblemDetails`** with an optional `failures` array carrying a stable `code` per failure.
- **JSON-naming-policy awareness** — error keys *and* messages name the field the way the wire does.
- **A boundary-aware exception handler** — `MustValidationException` is a 400; a guard's `ArgumentException` stays a 500 unless you opt in.
- **.NET 10 platform integration** — plug PineGuard validators into `Microsoft.Extensions.Validation`.
- **A localisation seam** — resolve each message by its stable code through `IStringLocalizer`.

## Examples

### Register everything once

```csharp
using PineGuard.AspNetCore;

builder.Services.AddMustValidation(typeof(Program).Assembly);   // scans for IMustValidator<T>; options at their defaults

// or, with options:
builder.Services.AddMustValidation(options =>
{
    options.IncludeCodes = true;             // default
    options.HandleGuardExceptions = false;   // default — see below
}, typeof(Program).Assembly);
```

### Minimal API

```csharp
app.MapPost("/orders", (CreateOrder order) => TypedResults.Created($"/orders/{order.Id}"))
   .AddMustValidation();

app.MapGroup("/api").AddMustValidation();   // whole group
```

A request with a bad email and an empty line list gets HTTP 400:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "email": ["email must be a valid email address."],
    "lines[1].sku": ["lines[1].sku must not be null or whitespace."]
  },
  "failures": [
    { "property": "email", "code": "email.address.invalid", "message": "email must be a valid email address." },
    { "property": "lines[1].sku", "code": "text.content.blank", "message": "lines[1].sku must not be null or whitespace." }
  ]
}
```

The endpoint filter is registered at build time only when a parameter type actually has a validator, so endpoints without one pay nothing at run time. Every bound complex argument is validated regardless of binding source — an `[AsParameters] SearchQuery` is validated exactly like a body.

### MVC

```csharp
builder.Services.AddControllers().AddMustValidation();
```

Every action argument with a registered validator is validated before the action runs; the response body is identical to the Minimal API one, and the failures are also written to `ModelState`.

### Boundary exceptions

```csharp
app.UseExceptionHandler();
```

A handler that does `validator.Validate(order).ThrowIfFailed()` throws `MustValidationException`, and the registered `IExceptionHandler` turns it into the same 400 body.

Guard clauses keep throwing the `ArgumentException` family and keep being 500s, because a guard three layers deep is a bug in your code, not a bad request. Turning them into 400s is an explicit, documented opt-in:

```csharp
builder.Services.AddMustValidation(options => options.HandleGuardExceptions = true, typeof(Program).Assembly);
```

> **Warning.** `HandleGuardExceptions = true` maps every `ArgumentException`, `ArgumentNullException` and `ArgumentOutOfRangeException` reaching the exception handler to a 400 — including one thrown by a programmer error deep inside your own code or a dependency. That hides bugs behind a client-error status. Prefer `MustValidationResult.From(...).ThrowIfFailed()` at the boundary.

### Async rules

```csharp
public sealed class RegisterUserValidator : MustValidator<RegisterUser>
{
    public RegisterUserValidator(IUserDirectory users)
    {
        RuleFor(x => x.Email, e => Must.Be.Email(e));
        RuleForAsync(x => x.Email, (e, ct) => Must.Be.SatisfiesAsync(e, users.IsAvailableAsync, ct));
    }
}

builder.Services.AddMustValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Scoped);
```

Both filters call `ValidateAsync` with `HttpContext.RequestAborted`. A validator that consumes a scoped service must itself be scoped.

### .NET 10 built-in validation

```csharp
builder.Services.AddValidation(options => options.AddMustValidatorResolver());
```

PineGuard validators join Microsoft's own validation pipeline, so `[ValidatableType]`, `DisableValidation()` and the source-generated resolvers keep working: the resolver PineGuard adds runs your validators and then hands the value on to the rest of the chain, so it only ever *adds* validation. It is named for what it adds — one resolver — so it never reads as a second spelling of `AddMustValidatorsFromAssembly`, which adds validators.

Codes are **not** carried on this path — the built-in error shape is `Dictionary<string, string[]>` and has nowhere to put them. Use the filters when you need codes.

`Microsoft.Extensions.Validation`'s resolver seam is `[Experimental("ASP0029")]` in .NET 10. PineGuard absorbs that diagnostic internally, so `AddMustValidatorResolver()` compiles clean at your call site — but the underlying platform contract may still change in a future release.

### Localisation

```csharp
builder.Services.AddSingleton<IMustFailureMessageResolver, StringLocalizerMustFailureMessageResolver>();
```

Each failure's message is looked up by its stable `Code` through `IStringLocalizer`; the English template remains the default when no resource is found.

## A note on values

`MustFailure.Value` — the attempted value — is never serialised. A failure whose value is a password or a token cannot leak into a response body through this package.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Validator registration** → [PineGuard.Extensions.DependencyInjection](https://www.nuget.org/packages/PineGuard.Extensions.DependencyInjection)
- **Configuration** → [PineGuard.Extensions.Options](https://www.nuget.org/packages/PineGuard.Extensions.Options)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
