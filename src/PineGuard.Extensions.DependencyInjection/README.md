# PineGuard.Extensions.DependencyInjection

**Every validator in your assembly, registered in one line.**

A `MustValidator<T>` is only useful once something can resolve it. Hand-registering `IMustValidator<Order>`, `IMustValidator<Customer>` and the twenty that follow is the kind of file nobody remembers to update, and the failure mode is silent: the validator exists, nothing resolves it, the rule never runs. PineGuard.Extensions.DependencyInjection registers one validator explicitly or scans an assembly for all of them, and forwards each to **every** service type it can legitimately be asked for — the concrete class, each closed `IMustValidator<T>` it implements, and the non-generic `IMustValidator` used for runtime dispatch by `Type`.

**One rule library. Every call site in your architecture.** The same validator your ASP.NET Core filter resolves by `Type` at the request boundary is the one your handler injects as `IMustValidator<Order>`.

## Install

```bash
dotnet add package PineGuard.Extensions.DependencyInjection
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core) and `Microsoft.Extensions.DependencyInjection.Abstractions`.

### Supported frameworks

Targets `net8.0`, `net10.0`, and `netstandard2.1`.

## What you get

- `AddMustValidator<TValidator>()` — registers one validator under the concrete type, every closed `IMustValidator<T>` it implements, and `IMustValidator`. Explicit, and **trim-safe**.
- `AddMustValidatorsFromAssembly()`, `AddMustValidatorsFromAssemblies()`, `AddMustValidatorsFromAssemblyContaining<TMarker>()` — the same registration for every validator found by scanning, with an optional `filter`. Scanning uses reflection and is **not** trim-safe.
- `IServiceProvider.TryGetMustValidator(type, out validator)` and `IServiceProvider.GetMustValidators(type)` — resolve by `Type` when the validated type is only known at run time, which is what request filters and pipeline behaviours need.
- Lifetime is yours: `ServiceLifetime.Singleton` by default, `Scoped` when a validator consumes a scoped dependency.

## Examples

Register one validator:

```csharp
using PineGuard.Extensions.DependencyInjection;

builder.Services.AddMustValidator<OrderValidator>();
```

`OrderValidator` is now resolvable as `OrderValidator`, as `IMustValidator<Order>`, and as `IMustValidator`:

```csharp
public sealed class OrderHandler(IMustValidator<Order> validator)
{
    public async Task HandleAsync(Order order, CancellationToken cancellationToken)
    {
        (await validator.ValidateAsync(order, cancellationToken)).ThrowIfFailed();
        // ...
    }
}
```

Register every validator in an assembly:

```csharp
builder.Services.AddMustValidatorsFromAssemblyContaining<Program>();

// or, narrowed:
builder.Services.AddMustValidatorsFromAssembly(
    typeof(Program).Assembly,
    filter: type => type.Namespace?.StartsWith("Acme.Orders", StringComparison.Ordinal) == true);
```

Scanning finds non-abstract, non-open-generic classes that implement a closed `IMustValidator<T>`. Abstract bases and open-generic validators are skipped — they are not registerable service implementations.

A validator that consumes a scoped service must itself be scoped, or the container's scope validation throws on the first request:

```csharp
builder.Services.AddMustValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Scoped);
```

One validator can serve several types, and both registrations are made:

```csharp
public sealed class ContactValidator : IMustValidator<Customer>, IMustValidator<Supplier> { /* ... */ }

builder.Services.AddMustValidator<ContactValidator>();

provider.GetRequiredService<IMustValidator<Customer>>();   // ContactValidator
provider.GetRequiredService<IMustValidator<Supplier>>();   // the same registration
```

Resolve by `Type` when the validated type is only known at run time:

```csharp
if (httpContext.RequestServices.TryGetMustValidator(argument.GetType(), out var validator))
{
    var result = await validator.ValidateAsync(argument, httpContext.RequestAborted);
    // ...
}

// every validator registered for the type, in registration order:
IReadOnlyList<IMustValidator> all = provider.GetMustValidators(typeof(Order));
```

### A note on registering twice

Every method uses `Add`, not `TryAdd`. Registering two validators for the same `T` is deliberate and supported — `GetMustValidators` returns both, and `GetRequiredService<IMustValidator<T>>()` returns the last one registered, which is how `Microsoft.Extensions.DependencyInjection` behaves everywhere else. Calling `AddMustValidator<TValidator>()` twice therefore produces two sets of descriptors rather than being silently ignored; register once.

### A note on trimming

`AddMustValidator<TValidator>()` names its type at compile time and survives trimming. The three scanning overloads enumerate an assembly's types reflectively and are annotated `[RequiresUnreferencedCode]` on `net8.0` and later — a trimmed or AOT-published application should register validators explicitly.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Configuration** → [PineGuard.Extensions.Options](https://www.nuget.org/packages/PineGuard.Extensions.Options)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
