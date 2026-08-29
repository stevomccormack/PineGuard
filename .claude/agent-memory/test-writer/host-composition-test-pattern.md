---
name: host-composition-test-pattern
description: How to end-to-end test a package's composition with a first-party Microsoft.Extensions.* extension the package itself never calls (e.g. ValidateOnStart()) — confirmed empirically against Microsoft.Extensions.Hosting/Options 10.0.11.
metadata:
  type: feedback
---

Confirmed (2026-08-30, `PineGuard.Extensions.Options` Plan 02 §4 ValidateOnStart gap, real `dotnet test`
run against `Microsoft.Extensions.Hosting`/`Microsoft.Extensions.Options` 10.0.11 — not just recalled
from training data):

- `OptionsBuilder<T>.ValidateOnStart()` (in `Microsoft.Extensions.Options`) registers a singleton
  `IStartupValidator`. **`IHost.StartAsync()`** (in `Microsoft.Extensions.Hosting`) resolves that
  service, if present, and calls `.Validate()` *before* starting hosted services.
- When exactly one `IValidateOptions<T>` registration fails for exactly one bound options type,
  `StartupValidator.Validate()` rethrows that **exact** `OptionsValidationException` (preserved via
  `ExceptionDispatchInfo`, not wrapped) — so `await Assert.ThrowsAsync<OptionsValidationException>(() =>
  host.StartAsync())` catches it directly, no `AggregateException` unwrapping needed for the
  single-options-type case.
- `MustRulesValidateOptions<T>.Validate()` aggregates **all** `Must.Be.*` failures for one options
  instance into a single `ValidateOptionsResult.Fail(...)` call (per `MustRulesValidateOptions`'s own
  contract), which becomes a single `OptionsValidationException` whose `.Message` is the `"; "`-joined
  `FormatFailure` strings — so one failing options type with two bad properties surfaces as ONE
  exception listing both, not two.

### Test shape that worked (project-local, no shared base — see [[plain-object-model-test-pattern]] for
the sibling "(Other)"/BaseUnitTest convention this follows)

```csharp
using var host = new HostBuilder()
    .ConfigureAppConfiguration(c => c.AddInMemoryCollection(tc.Value))   // tc.Value: Dictionary<string,string?>
    .ConfigureServices(services =>
    {
        services.AddSingleton<IMustValidator<TOptions>, TValidator>();
        services.AddOptions<TOptions>().BindConfiguration("Section").ValidateMustRules().ValidateOnStart();
    })
    .Build();

// success path: await host.StartAsync(); await host.StopAsync();
// failure path: var ex = await Assert.ThrowsAsync(expected.ExceptionType, () => host.StartAsync());
```

`new HostBuilder()` (not `Host.CreateDefaultBuilder()`) keeps the test deterministic — no real
appsettings.json/env-var/console-logging providers pulled in, only the in-memory config explicitly
added. `Assert.ThrowsAsync(Type, Func<Task>)` (non-generic overload) exists in xunit.assert 2.9.3 —
confirmed compiling/passing, mirrors the sync `Assert.Throws(Type, Action)` already used elsewhere in
this file.

### Extending an existing local `Expected` record instead of restructuring
`OptionsBuilderExtensionTestData.ResolveExpected(Type? ExceptionType, string? MessageContains)` already
served three other operation groups. Rather than forcing a "list of substrings" case type onto all of
them, added one more optional trailing field —
`IReadOnlyList<string>? MessageContainsAll = null` — so old positional call sites (2-arg) are untouched
and the new multi-failure case just passes a 3rd positional arg (`null` for the unused 2nd slot to keep
it positional, no named args — matches the project's "no named arguments in test case records" rule).
Factored the shared substring-checking logic into one private `AssertMessage(expected, ex)` called by
both the existing sync `AssertResult` and the new async `AssertStartupResult`, instead of duplicating it.

### Reuse for later phases
This phase's plan explicitly said the `Microsoft.Extensions.Hosting` test-only package reference exists
*so ValidateOnStart() is exercised end-to-end* — expect Phase 3 (ASP.NET Core) and Phase 4
(MediatR/bridges) to need the same `HostBuilder` + `Assert.ThrowsAsync(Type, Func<Task>)` shape for any
surface that composes with another first-party host/DI extension the package itself doesn't call.
