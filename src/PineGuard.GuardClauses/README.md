# PineGuard.GuardClauses

**Stop bad input at the door. Keep the parsed value. Throw your exceptions, not `ArgumentException`.**

`Guard.Against.NotHttpsUrl(url)` takes a string and hands back a `Uri`. If the input wasn't a valid HTTPS URL, it throws — if it was, you get the parsed value with no second `new Uri(...)` call. Every guard in PineGuard works this way: bad input stops at the boundary, and the parsed, typed value flows on.

Guards fit anywhere invalid input should stop execution immediately — constructors, factory methods, service methods, API layer boundaries, and parameter checks on any public method. They're a **perfect fit for Domain-Driven Design**, where the always-valid-state principle means entities, value objects, and aggregates can't exist in an invalid state. Guards enforce that invariant right in the constructor, so bad data never crosses into your domain.

**Where PineGuard goes further than any other guard library: your exception, not `ArgumentException`.** Replace the default globally, per-scope, or per-call — so `new Order(...)` throws `InvalidOrderException` and `new Checkout(...)` throws `CheckoutException`. Other libraries stop at a per-call override; PineGuard's scoped and global policies let your whole domain speak one exception vocabulary without each call site opting in.

## Install

```bash
dotnet add package PineGuard.GuardClauses
```

Targets `net8.0`, `net10.0`, and `netstandard2.1`. Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core) and [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses).

## Examples

```csharp
using PineGuard.GuardClauses;

// The four canonical rules — each throws on failure, returns the parsed value on success
string email       = Guard.Against.NotEmail(userEmail);
string strictEmail = Guard.Against.NotStrictEmail(userEmail);
string safe        = Guard.Against.OwaspUnsafe(userInput);
Uri    callbackUri = Guard.Against.NotHttpsUrl(callback);  // returns Uri, not string

// Compose at a service boundary
public EndpointConfiguration Create(string callback, string userEmail, string userInput)
{
    return new EndpointConfiguration(
        Guard.Against.NotHttpsUrl(callback),
        Guard.Against.NotEmail(userEmail),
        Guard.Against.OwaspUnsafe(userInput));
}
```

### Your domain, your exception story

```csharp
// Replace globally
GuardExceptionPolicy.ExceptionReplacer = ex => new DomainValidationException(ex.Message, ex);
GuardExceptionPolicy.ReplaceDefaultExceptions = true;

// Or just for a block
using (GuardExceptionPolicy.BeginScope(o =>
    o.ExceptionReplacer = ex => new CheckoutException(ex.Message, ex)))
{
    Guard.Against.NotNull(orderId);
}

// Or just for one call
Guard.Against.NotNull(orderId,
    exceptionCreator: () => new CheckoutException("Order id is required."));
```

## Other layers, same rule library

- **Caller should decide what to do on failure** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Pipeline-style request validation** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)
- **Attribute-driven models** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete `Guard.Against.*` catalog and exception-policy options.

## License

MIT © Steve McCormack
