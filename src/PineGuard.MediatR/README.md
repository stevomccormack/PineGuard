# PineGuard.MediatR

**Delete the forty-line validation behavior every MediatR codebase re-writes.**

Almost every MediatR application ends up with the same hand-rolled `IPipelineBehavior` that resolves validators, runs them, merges the failures and throws. It is copied between projects, drifts between them, and the copy that forgets to merge two validators' results ships a 400 that only mentions the first mistake. PineGuard.MediatR is that behavior, written once: it runs **every** `IMustValidator<TRequest>` registered for a request, merges the results so one response carries every failure, and then either throws `MustValidationException` or hands back a failure response you define.

**One rule library. Every call site in your architecture.** The `OrderValidator` your handler pipeline runs here is the same class your options binding, your request filters and your domain constructors already use.

## Install

```bash
dotnet add package PineGuard.MediatR
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [PineGuard.Extensions.DependencyInjection](https://www.nuget.org/packages/PineGuard.Extensions.DependencyInjection) and `MediatR`.

`MediatR` is referenced as `>= 12.5.0`. The 12.5.x line is the last one published under Apache-2.0; MediatR 13 moved to a commercial licence. Both lines work here — restore whichever your project is licensed for.

### Supported frameworks

Targets `net8.0`, `net10.0`, and `netstandard2.1`.

## What you get

- `cfg.AddMustValidation()` — one line inside `AddMediatR(...)` registers `MustValidationBehavior<,>` as an open behavior, so every request in the pipeline is covered.
- `MustValidationBehavior<TRequest, TResponse>` — runs every registered validator for the request in registration order, merges the results, and short-circuits on failure. A request with no validator is untouched and pays nothing.
- `IMustFailureResponseFactory<TResponse>` — return a failure response of one response type instead of throwing.
- `IMustFailureResponseFactory` — the same, for a whole *family* of response types such as `ErrorOr<T>`.

## Examples

### Throw mode (the default)

```csharp
using PineGuard.Extensions.DependencyInjection;
using PineGuard.MediatR;

builder.Services.AddMustValidatorsFromAssemblyContaining<Program>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.AddMustValidation();          // open behavior, runs before handlers
});
```

A request whose validator fails never reaches its handler — the behavior throws `MustValidationException`, carrying the full `MustValidationResult`. Hosted in ASP.NET Core, PineGuard's exception handler turns that into an RFC 9457 400 response with every failure and its code.

Two validators registered for the same request both run, and the failures arrive together in registration order:

```csharp
builder.Services.AddMustValidator<CreateOrderValidator>();
builder.Services.AddMustValidator<CreateOrderQuotaValidator>();
```

### Respond mode, one response type

Register a factory for the response type and the behavior returns instead of throwing:

```csharp
public sealed class CreateOrderFailureFactory : IMustFailureResponseFactory<CreateOrderResult>
{
    public CreateOrderResult Create(MustValidationResult result) =>
        CreateOrderResult.Rejected(result.Failures.Select(f => (f.PropertyPath, f.Code, f.Message)));
}

services.AddSingleton<IMustFailureResponseFactory<CreateOrderResult>, CreateOrderFailureFactory>();
```

### Respond mode, a family of response types

Microsoft DI cannot map an open generic `IMustFailureResponseFactory<>` onto an implementation that closes it as `IMustFailureResponseFactory<ErrorOr<T>>` — the type parameters do not line up and registration throws. Register the non-generic seam instead, which is handed the runtime response type and decides whether it serves it:

```csharp
public sealed class ErrorOrFailureResponseFactory : IMustFailureResponseFactory
{
    public bool TryCreate(Type responseType, MustValidationResult result, out object? response)
    {
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(global::ErrorOr.ErrorOr<>))
        {
            var errors = result.ToErrors();
            response = typeof(global::ErrorOr.ErrorOr<>).MakeGenericType(responseType.GenericTypeArguments[0])
                .GetMethod("From", [typeof(List<global::ErrorOr.Error>)])!.Invoke(null, [errors]);
            return true;
        }

        response = null;
        return false;
    }
}

services.AddSingleton<IMustFailureResponseFactory, ErrorOrFailureResponseFactory>();
```

That sample is consumer code: it uses [PineGuard.ErrorOr](https://www.nuget.org/packages/PineGuard.ErrorOr)'s `ToErrors()`, but this package depends on neither it nor `ErrorOr`. The same shape adapts to any result family — swap the open generic and the factory method.

A typed factory always wins over a family factory, so a specific registration overrides a general one. With neither registered, the behavior throws.

### Other mediators

MassTransit, Wolverine and the source-generated `Mediator` all expose an equivalent middleware seam; the body of `MustValidationBehavior.Handle` ports across unchanged — resolve `IEnumerable<IMustValidator<TRequest>>`, `await` each `ValidateAsync`, `MustValidationResult.Combine`, then continue or short-circuit.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Validator registration** → [PineGuard.Extensions.DependencyInjection](https://www.nuget.org/packages/PineGuard.Extensions.DependencyInjection)
- **Configuration** → [PineGuard.Extensions.Options](https://www.nuget.org/packages/PineGuard.Extensions.Options)
- **Result-oriented handlers** → [PineGuard.ErrorOr](https://www.nuget.org/packages/PineGuard.ErrorOr), [PineGuard.FluentResults](https://www.nuget.org/packages/PineGuard.FluentResults), [PineGuard.OneOf](https://www.nuget.org/packages/PineGuard.OneOf)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
