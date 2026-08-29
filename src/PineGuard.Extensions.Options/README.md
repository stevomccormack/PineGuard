# PineGuard.Extensions.Options

**Configuration that refuses to start wrong.**

`services.AddOptions<T>().ValidateDataAnnotations()` gives you `[Required]` and `[Range]`. Anything richer — a hostname, an HTTPS URL, a port, an email sender — is usually a hand-written `Validate(o => …)` lambda, or nothing at all. PineGuard.Extensions.Options brings the whole `Must.Be.*` vocabulary to `IOptions<T>`: one call, `.ValidateMustRules()`, and `ValidateOnStart()` makes the host refuse to start while listing **every** violation in one exception — not just the first one an operator trips over.

**One rule library. Every call site in your architecture.** The same `Must.Be.Hostname` your services call validates the `Hostname` setting in `appsettings.json` before the host ever finishes starting.

## Install

```bash
dotnet add package PineGuard.Extensions.Options
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses), and `Microsoft.Extensions.Options`.

### Supported frameworks

Targets `net8.0`, `net10.0`, and `netstandard2.1`. `ValidateOnStart()` and `IStartupValidator` ship inside `Microsoft.Extensions.Options` itself on every one of those three — confirmed against the package this project actually restores — so this package adds no dependency on `Microsoft.Extensions.Hosting` anywhere in `src/`. Your application already references `Microsoft.Extensions.Hosting` (directly, or via `Microsoft.NET.Sdk.Web`) because that is what actually calls `IStartupValidator.Validate()` during `Host.StartAsync()`; this package's job stops at registering the validator and formatting its failures.

## What you get

- `MustRulesValidateOptions<TOptions>` — an `IValidateOptions<TOptions>` that runs an `IMustValidator<TOptions>` and turns every `MustFailure` into a `"{TypeName}.{PropertyPath}: {Message} [{Code}]"` line (`"{TypeName}: {Message} [{Code}]"` at the root).
- `ValidateMustRules()` — three `OptionsBuilder<TOptions>` extension overloads: resolve the validator from DI, pass an instance, or configure one inline with `InlineMustValidator<TOptions>`.
- Named options support: `AddOptions<T>("Name")` validates only that name, exactly like `ValidateDataAnnotations()`.
- No new behaviour around `ValidateOnStart()` itself — chain it straight from `Microsoft.Extensions.Options` and every violation from every rule is listed in one `OptionsValidationException`.

## Examples

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

(`Must.Be.Positive` is a net8.0+ clause — a `netstandard2.1`-only consumer will need a different rule here.)

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

### A note on validator lifetime

Every `ValidateMustRules()` overload registers the `IValidateOptions<TOptions>` adapter as a **singleton** — the same lifetime `ValidateDataAnnotations()` uses. That is safe because a `MustValidator<T>` is immutable once constructed: rules are only ever added in the constructor, so one instance can validate every request without shared mutable state. If your own `IMustValidator<TOptions>` is registered with a shorter lifetime (for example `scoped`) and gets resolved from the root provider — which is what `ValidateOnStart()` and the first read of `IOptions<TOptions>` both do — ASP.NET Core's scope validation throws in development. That exception is the framework reporting the same lifetime mismatch, not a bug in this package.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
