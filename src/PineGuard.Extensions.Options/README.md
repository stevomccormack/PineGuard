# PineGuard.Extensions.Options

**Configuration that refuses to start wrong.**

`services.AddOptions<T>().ValidateDataAnnotations()` gives you `[Required]` and `[Range]`. Anything richer — a hostname, an HTTPS URL, a port, an email sender — is usually a hand-written `Validate(o => …)` lambda, or nothing at all. PineGuard.Extensions.Options brings the whole `Must.Be.*` vocabulary to `IOptions<T>`: one call, `.ValidateMustRules()`, and `ValidateOnStart()` makes the host refuse to start while listing **every** violation in one exception — not just the first one an operator trips over.

**One rule library. Every call site in your architecture.** The same `Must.Be.Hostname` your services call validates the `Hostname` setting in `appsettings.json` before the host ever finishes starting.

## Install

```bash
dotnet add package PineGuard.Extensions.Options
```

Targets `net8.0`, `net10.0`, and `netstandard2.1`. Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses), and `Microsoft.Extensions.Options`.

> **Status: coming in this phase.** The API below is the shape this package is being built to (Plan 02, Phase 2 of the New Surfaces program) — it is not published yet. This README will be the source for the package's tests once `ValidateMustRules()` and `MustRulesValidateOptions<TOptions>` land; nothing here has shipped to NuGet yet.

## Examples (planned shape)

Register a validator once, wire it with one call:

```csharp
using PineGuard.MustClauses;
using PineGuard.Extensions.Options;

public sealed class SmtpOptions
{
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? From { get; set; }
    public bool UseTls { get; set; }
}

public sealed class SmtpOptionsValidator : MustValidator<SmtpOptions>
{
    public SmtpOptionsValidator()
    {
        RuleFor(o => o.Host, host => Must.Be.Hostname(host));
        RuleFor(o => o.Port, port => Must.Be.PortNumber(port));
        RuleFor(o => o.From, from => Must.Be.Email(from));
        RuleFor(o => o.Port, port => Must.Be.EqualTo(port, 465)).When(o => o.UseTls);
    }
}

builder.Services.AddSingleton<IMustValidator<SmtpOptions>, SmtpOptionsValidator>();
builder.Services.AddOptions<SmtpOptions>()
    .BindConfiguration("Smtp")
    .ValidateMustRules()
    .ValidateOnStart();
```

Validate a small options class inline, without a dedicated validator class:

```csharp
builder.Services.AddOptions<CacheOptions>()
    .BindConfiguration("Cache")
    .ValidateMustRules(v => v.RuleFor(o => o.TtlSeconds, ttl => Must.Be.Positive(ttl)))
    .ValidateOnStart();
```

Pass a validator instance directly:

```csharp
builder.Services.AddOptions<SmtpOptions>()
    .BindConfiguration("Smtp")
    .ValidateMustRules(new SmtpOptionsValidator())
    .ValidateOnStart();
```

When `Smtp:Host` and `Smtp:From` are both wrong, the host fails to start with every violation named, not just the first:

```text
OptionsValidationException: SmtpOptions.Host: Host must be a valid hostname. [network.hostname.invalid]; SmtpOptions.From: From must be a valid email address. [email.address.invalid]
```

Named options (`AddOptions<SmtpOptions>("Marketing")`) validate only the matching name — the same rule `ValidateDataAnnotations()` follows.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
