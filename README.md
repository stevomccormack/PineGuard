<p align="center">
  <img src="https://img.shields.io/badge/.NET-netstandard2.1%20%7C%20net8.0%20%7C%20net10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="netstandard2.1 | net8.0 | net10.0" />
  <a href="https://github.com/stevomccormack/PineGuard/actions/workflows/ci.yml"><img src="https://img.shields.io/github/actions/workflow/status/stevomccormack/PineGuard/ci.yml?branch=main&style=for-the-badge&label=CI" alt="CI" /></a>
  <img src="https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge" alt="MIT License" />
</p>

<h1 align="center">PineGuard</h1>

<p align="center">
  <strong>Validation that thinks like you do.</strong><br />
  330+ rules. 550+ Must clauses. 530+ Guard clauses. 13,000+ tests.<br />
  Built by AI. Loved by AI. Trusted by engineers.
</p>

---

## Why PineGuard?

PineGuard gives you a **single validation model** you can reuse across every .NET boundary that matters:

- **`Must.Be.*()`** when you want result-based validation you can compose
- **`Guard.Against.*()`** when you want fail-fast guards and parsed return values
- **FluentValidation adapters** when you want request validation that reads naturally
- **DataAnnotations attributes** when you want model-level validation without duplicating rules

That means you don’t end up maintaining four different validation dialects for the same business rule.

### Why teams choose PineGuard

- **One mental model, multiple delivery styles.** Write validation once, then use it as a result, a guard, a FluentValidation rule, or a DataAnnotations attribute.
- **Broad coverage without a fragmented API.** Strings, numbers, dates and date/time ranges, time spans, geo-locations, collections, URIs, OWASP-safe input, network identifiers, and more.
- **Security built in.** PineGuard includes real OWASP, URI, hostname, and reference-data validations instead of stopping at trivial string checks.
- **Exception policy without forking the library.** Guard exceptions can stay default, be replaced globally, be scoped to an operation, or be overridden per call.

> **One validation model. Every .NET boundary.**

### Choose the surface that fits the call site

| Surface | Best when you want... | Example |
|---|---|---|
| `Must.Be.*()` | result-based validation and composable flow control | `Must.Be.Email(email)` |
| `Guard.Against.*()` | fail-fast validation and typed/parsed return values | `Guard.Against.NotHttpsUrl(callback)` |
| `PineGuard.FluentValidation` | request validators that read naturally in pipelines | `RuleFor(x => x.Website).WebUrl()` |
| `PineGuard.DataAnnotations` | attribute-driven validation on DTOs and models | `[WebUrl]` |
| `PineGuard.Testing` | writing tests against PineGuard-based validation | `ReturnCase<string, bool>` fixtures |

---

## Quick Start

### Install

```bash
# Core validation rules (no third-party dependencies)
dotnet add package PineGuard.Core

# Must clauses — result-based fluent validation
dotnet add package PineGuard.MustClauses

# Guard clauses — throw-on-failure guards
dotnet add package PineGuard.GuardClauses

# FluentValidation adapter
dotnet add package PineGuard.FluentValidation

# DataAnnotations attributes
dotnet add package PineGuard.DataAnnotations

# Test fixtures, base classes and assertion helpers
dotnet add package PineGuard.Testing
```

`PineGuard.Core` takes no third-party dependencies — only the Microsoft first-party
`System.Text.Json` and `System.ComponentModel.Annotations` packages, which the .NET runtime already
ships on modern targets.

### Must Clauses &mdash; result-based, explicit, composable

**Best for:** APIs, services, pipelines, and places where you want to decide what happens next.

```csharp
using PineGuard.MustClauses;

var emailResult = Must.Be.Email(email);
if (emailResult.Failed)
    return BadRequest(emailResult.Message);

var callbackUri = Must.Be.HttpsUrl(httpsCallback).OrThrow();
var safeInput = Must.Be.OwaspSafe(input).OrThrow();

Must.Be.Email(email).ThrowIfFailed((message, paramName) =>
    new BusinessException($"{paramName}: {message}"));
```

### Guard Clauses &mdash; fail fast, and return parsed data when useful

**Best for:** application boundaries, constructors, service methods, and anywhere invalid input should stop immediately.

```csharp
using PineGuard.GuardClauses;

public sealed record EndpointConfiguration(
    string DisplayName,
    Uri WebsiteUri,
    Uri CallbackUri,
    string Hostname,
    string SafeInput);

public sealed class EndpointService
{
    public EndpointConfiguration Create(
        string displayName,
        string website,
        string httpsCallback,
        string hostname,
        string input)
    {
        var name = Guard.Against.NotNull(displayName);
        var websiteUri = Guard.Against.NotUrl(website);           // accepts http:// or https:// and returns Uri
        var callbackUri = Guard.Against.NotHttpsUrl(httpsCallback); // returns Uri and enforces HTTPS only
        var host = Guard.Against.Hostname(hostname);             // domain-only, e.g. openai.com
        var safeInput = Guard.Against.OwaspUnsafe(input);

        return new EndpointConfiguration(name, websiteUri, callbackUri, host, safeInput);
    }
}
```

> Guard names the **forbidden state**. That’s why PineGuard uses `NotUrl(...)` / `NotHttpsUrl(...)` on the Guard surface while still returning the parsed, valid result.

### Guard Exception Policy &mdash; your domain, your exception story

**Best for:** teams who want a single validation library but need exceptions that match their application language.

```csharp
using PineGuard.GuardClauses;
using PineGuard.MustClauses;

// Default behavior: built-in ArgumentException / ArgumentNullException
Guard.Against.NotNull(orderId);

// Global replacement
GuardExceptionPolicy.ExceptionReplacer = ex => new DomainValidationException(ex.Message, ex);
GuardExceptionPolicy.ReplaceDefaultExceptions = true;

Guard.Against.NotNull(orderId);

// Scoped replacement
using (GuardExceptionPolicy.BeginScope(options =>
{
    options.ExceptionReplacer = ex => new CheckoutException(ex.Message, ex);
    options.ReplaceDefaultExceptions = true;
}))
{
    Guard.Against.NotNull(orderId);
    Guard.Against.OwaspUnsafe(input);
}

// Per-call override wins for this invocation only
Guard.Against.NotNull(
    orderId,
    exceptionCreator: () => new CheckoutException("Order id is required."));

// If you're already working with MustResult<T>, throw from there instead
Must.Be.OwaspSafe(input).ThrowIfFailed((message, paramName) =>
    new CheckoutException($"{paramName}: {message}"));
```

### FluentValidation Integration &mdash; expressive pipeline validation

**Best for:** application requests, commands, DTOs, and APIs that already use FluentValidation.

```csharp
using FluentValidation;
using PineGuard.FluentValidation;

public sealed record CreateEndpointRequest(
    string? DisplayName,
    string? LegacyAlias,
    string? Website,
    string? HttpCallback,
    string? HttpsCallback,
    string? Hostname,
    string? Email,
    string? StrictEmailAddress,
    string? UserInput);

public sealed class CreateEndpointRequestValidator : AbstractValidator<CreateEndpointRequest>
{
    public CreateEndpointRequestValidator()
    {
        RuleFor(x => x.DisplayName).Required();       // Fluent-specific name to avoid NotNull()/Null() collisions
        RuleFor(x => x.LegacyAlias).NotRequired();    // value must be null

        RuleFor(x => x.Website).WebUrl();             // accepts http:// or https://
        RuleFor(x => x.HttpCallback).HttpUrl();       // HTTP only
        RuleFor(x => x.HttpsCallback).HttpsUrl();     // HTTPS only
        RuleFor(x => x.Hostname).Hostname();          // domain-only, e.g. openai.com

        RuleFor(x => x.Email).Required().Email();
        RuleFor(x => x.StrictEmailAddress).Required().StrictEmail();
        RuleFor(x => x.UserInput).Required().OwaspSafe();
    }
}
```

### DataAnnotations &mdash; attribute-driven validation on models

**Best for:** DTOs, input models, MVC binding, and codebases that prefer declarative attributes.

```csharp
using PineGuard.DataAnnotations;

public sealed class CreateEndpointRequest
{
    [NotNull]
    public string DisplayName { get; init; } = string.Empty;

    [Null] // explicit: this value must remain null
    public string? LegacyAlias { get; init; }

    [WebUrl] // accepts http:// or https://
    public string? Website { get; init; }

    [HttpUrl]
    public string? HttpCallback { get; init; }

    [HttpsUrl]
    public string? HttpsCallback { get; init; }

    [Hostname] // domain-only, e.g. openai.com
    public string? Hostname { get; init; }

    [NotNull]
    [Email]
    public string Email { get; init; } = string.Empty;

    [NotNull]
    [StrictEmail]
    public string StrictEmailAddress { get; init; } = string.Empty;

    [NotNull]
    [OwaspSafe]
    public string UserInput { get; init; } = string.Empty;
}
```

> DataAnnotations format attributes allow `null` by default. Pair them with `[NotNull]` when the property must be present and valid.

> FluentValidation uses `Required()` / `NotRequired()` for presence checks to avoid built-in naming collisions. DataAnnotations uses `[NotNull]` / `[Null]` for the equivalent presence semantics today.

---

## Architecture

PineGuard is layered, and each layer adapts the one below it rather than restating its logic:

```
PineGuard.Core            pure boolean rules + parsing utilities
  └─ PineGuard.MustClauses    result-returning, never throws
       └─ PineGuard.GuardClauses    throws, returns parsed values
            ├─ PineGuard.FluentValidation    IRuleBuilder extensions
            └─ PineGuard.DataAnnotations     ValidationAttribute adapters

PineGuard.Testing         fixtures, base test classes, assertion helpers
```

A new validation is implemented across *all* layers, plus tests, in that order.

## Supported frameworks

The five validation packages multi-target `netstandard2.1`, `net8.0`, and `net10.0`.
`PineGuard.Testing` targets `net8.0` and `net10.0` only — its fixtures use `TimeOnly`.

## Documentation

The canonical engineering specs, conventions, and agent playbooks live in
**[docs/ai/](docs/ai/README.md)** — start there before changing conventions.

## Contributing

See **[CONTRIBUTING.md](CONTRIBUTING.md)** for build, test, and formatting instructions and the
quality gates every pull request must clear
([CI workflow](https://github.com/stevomccormack/PineGuard/actions/workflows/ci.yml)).

## Security

Please do not open public issues for vulnerabilities — see **[SECURITY.md](SECURITY.md)**.

## License

MIT — see **[LICENSE](LICENSE)**.

