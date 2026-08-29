<!-- metadata_header
type: plan
id: new-surfaces-03-aspnetcore
version: 1.2
status: planned
last_updated: 2026-08-26
parent: new-surfaces-program
-->

# Plan 03 — Phase 3: `PineGuard.AspNetCore`, `PineGuard.Extensions.DependencyInjection` and the async seam

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · **03 ASP.NET Core** · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->

> **Status**: Planned | **Depends on**: PR 1 — 1a + Track 0 (Plan 00 §10.1; Plan 02's onboarding log is used if it has merged, otherwise this phase executes Plan 00 §8 itself and files the same log); PR 2 — PR 1 **and 1d** (the exception handler reads codes through `ExceptionExtension`) | **Unblocks**: Phase 4 MediatR (reuses the DI package and the async seam)
>
> **Worktrees** (two PRs, in this order):
> 1. `+ .claude/worktrees/must-async-di` on `feature/must-async-di` — W1–W3 (async seam, validation mode, `PineGuard.Extensions.DependencyInjection`).
> 2. `+ .claude/worktrees/aspnetcore` on `feature/aspnetcore` — W4–W8 (`PineGuard.AspNetCore`).
>
> Read [Plan 00](new-surfaces-missing-validation-cases-00-program.md) first; Plan 02 §3.4's onboarding log is the procedure for the two new scopes here.

## 1. Business plan

### 1.1 The problem

Request validation is where teams meet a validation library, and the .NET landscape shifted twice: FluentValidation **dropped** official ASP.NET auto-validation (async validators cannot run inside synchronous model binding), leaving a community patchwork; and .NET 10 shipped `Microsoft.Extensions.Validation` — Microsoft's own model validator for Minimal APIs — which validates *models* but ships almost no *rules*. The first is demand nobody serves; the second is a platform wave to ride rather than fight.

Two production failure modes matter as much as the integration itself: error keys that say `FirstName` while the client sent `firstName`, and a handler that turns a programmer's `ArgumentNullException` three layers deep into a friendly 400 that hides a bug.

### 1.2 Value

- **Auto-validation done right**: an async-safe MVC action filter and a Minimal API endpoint filter that aggregate every failure into one RFC 9457 `ValidationProblemDetails` — with stable codes — and respect the app's JSON naming policy.
- **Platform alignment**: a `IValidatableInfoResolver` that plugs PineGuard validators into .NET 10's built-in validation, which also makes the AOT/trimming story Microsoft's problem.
- **A correct boundary policy**: only `MustValidationException` is a 400 by default; Guard exceptions stay 500 unless the consumer opts in, so blanket mapping never masks a bug.
- **The async seam** every DB-uniqueness check needs, confined to the Must layer and above; Core stays synchronous and an audit rule keeps it that way.

### 1.3 Success metrics

- A Minimal API endpoint and an MVC action both return the same 400 body for the same bad request, keys camel-cased when the app uses camelCase.
- `SatisfiesAsync` and `RuleForAsync` exist; Rule14 proves Core contains no `async`/`Task`.
- Two new packages (`PineGuard.Extensions.DependencyInjection`, `PineGuard.AspNetCore`) onboarded via the Phase 2 procedure; 100 %/100 % each.

## 2. Functional plan

### 2.1 User stories

1. **Register everything once.**

   ```csharp
   using PineGuard.AspNetCore;
   using PineGuard.Extensions.DependencyInjection;

   builder.Services.AddMustValidation(typeof(Program).Assembly);   // scans for IMustValidator<T> implementations; options at their defaults

   // or, with options:
   builder.Services.AddMustValidation(options =>
   {
       options.IncludeCodes = true;             // default
       options.HandleGuardExceptions = false;   // default — see §4.3
   }, typeof(Program).Assembly);
   ```

2. **Minimal API (any .NET 8+).**

   ```csharp
   app.MapPost("/orders", (CreateOrder order) => TypedResults.Created($"/orders/{order.Id}"))
      .AddMustValidation();

   app.MapGroup("/api").AddMustValidation();   // whole group
   ```

   A request with a bad email and an empty line list gets HTTP 400 (messages are re-rendered with the transformed path, so keys and messages name the field the same way):

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

3. **MVC.** `builder.Services.AddControllers().AddMustValidation();` — every action argument with a registered validator is validated before the action runs; the same body as above is returned; failures are also written to `ModelState`. Both the `IMvcBuilder` and the endpoint-builder `AddMustValidation()` assume story 1's `services.AddMustValidation(...)` has run (options, resolver, validators); only story 5 additionally needs `app.UseExceptionHandler()`.
4. **.NET 10 built-in validation.** `builder.Services.AddValidation(options => options.AddMustValidators());` — PineGuard validators participate in Microsoft's validation pipeline (`[ValidatableType]`, `DisableValidation()` etc. keep working). Codes are not carried on this path (the built-in error shape is `Dictionary<string, string[]>`); the README says so.
5. **Boundary exception.** A handler that does `MustValidationResult.From(...).ThrowIfFailed()` — or a MediatR behavior (Phase 4) — throws `MustValidationException`; `app.UseExceptionHandler()` + the registered handler turn it into the same 400 body.
6. **Guards stay 500** unless `options.HandleGuardExceptions = true`, in which case the `ArgumentException` family becomes a 400 with one failure — `property` from `exception.GetMustPropertyPath()` (falling back to `ParamName`), `code` from `exception.TryGetMustCode(...)` stamped by `GuardFailure` in Phase 1d, falling back to `MustValidationOptions.UnknownGuardCode` (default `MustCodes.Value.Argument.Invalid`, the reserved catalogue constant no clause emits) for an argument exception PineGuard did not throw. The option's XML doc and README carry the warning from the parent plan verbatim.
7. **Async rule.**

   ```csharp
   public sealed class RegisterUserValidator : MustValidator<RegisterUser>
   {
       public RegisterUserValidator(IUserDirectory users)      // IUserDirectory.IsAvailableAsync : Func<string, CancellationToken, ValueTask<bool>>
       {
           RuleFor(x => x.Email, e => Must.Be.Email(e));
           RuleForAsync(x => x.Email, (e, ct) => Must.Be.SatisfiesAsync(e, users.IsAvailableAsync, ct));
       }
   }

   services.AddMustValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Scoped);   // a validator that consumes a scoped service is itself scoped
   ```

   Filters call `ValidateAsync` with `HttpContext.RequestAborted`. Validators with async rules almost always depend on scoped services (a `DbContext`-backed directory), so they are registered `Scoped`; ASP.NET's scope validation throws at first request if a singleton captures one.

8. **Non-body parameters.** `[AsParameters] SearchQuery query` and any bound complex argument are validated exactly like a body — the filters validate every argument that has a validator, regardless of binding source.
9. **Localisation seam.** `services.AddSingleton<IMustFailureMessageResolver, StringLocalizerMustFailureMessageResolver>()` resolves each failure's message by `Code` through `IStringLocalizer`; English templates remain the default when no resource is found.

### 2.2 Acceptance criteria

- [ ] All types in §4 exist with the listed signatures; 100 %/100 % on `DependencyInjection` and `AspNetCore` scopes.
- [ ] End-to-end tests (`TestServer`) prove stories 2, 3, 5 and 6 with the exact JSON shape above.
- [ ] Rule14 (Core stays sync) exists and is clean; Rule13 clean for the new async clauses.
- [ ] `docs/ai/specs/testing/unit-test.md` §5.1 records the async-test exception (`public async Task <Member>_BehavesAsExpected`).
- [ ] Plan 00 §7 in full; both scopes onboarded per Plan 02 §3.4.

### 2.3 Not in this phase

`HttpClient` response-contract validation (deferred — see §9), MassTransit/Wolverine shims, gRPC/SignalR/Hangfire/Functions entry points, translations (only the seam ships). Guard code stamping and the exception policy redesign are **not** here — they ship in Phase 1d (Plan 01 §4.14) and this phase merely reads the stamped code with `ExceptionExtension`. (`HandleGuardExceptions` was `MapGuardExceptions`; renamed so "map" stays the word for `GuardExceptionPolicy.Map` and "handle" — ASP.NET's own word, `IExceptionHandler` — names what this option does.)

## 3. Technical plan — the async seam and validation mode (Core + MustClauses + Fluent)

### 3.1 `MustValidator<T>` async rules (`src/PineGuard.Core/MustClauses/MustValidator.cs`)

```csharp
protected MustPropertyRule<T, TProperty> RuleForAsync<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check);
protected MustPropertyRule<T, TProperty> RuleForAsync<TProperty, TResult>(Expression<Func<T, TProperty>> expression, Func<T, TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check);
protected MustPropertyRule<T, TItem>   RuleForEachAsync<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<TItem, CancellationToken, ValueTask<MustResult<TResult>>> check);
protected MustPropertyRule<T, TItem>   RuleForEachAsync<TItem, TResult>(Expression<Func<T, IEnumerable<TItem>?>> expression, Func<T, TItem, CancellationToken, ValueTask<MustResult<TResult>>> check);
protected bool HasAsyncRules { get; }
```

- Explicit `Async` suffix so overload resolution never has to distinguish sync and async lambdas.
- `Validate(T)` throws `InvalidOperationException("… has async rules; call ValidateAsync.")` when `HasAsyncRules` — a programmer error, and the behaviour FluentValidation users expect.
- `ValidateAsync` runs all rules **sequentially in registration order** (deterministic failure order; no parallel fan-out), observing the token between rules. `InlineMustValidator<T>` re-exposes the four overloads.

### 3.2 Validation mode (fail-fast) — non-breaking via default interface members

```csharp
public enum MustValidationMode { Aggregate, StopOnFirstFailure }

// IMustValidator<in T> — added as DIMs that forward to the existing members
ValueTask<MustValidationResult> ValidateAsync(T value, MustValidationMode mode, CancellationToken cancellationToken = default) => ValidateAsync(value, cancellationToken);
// IMustValidator — likewise for object?

// MustValidator<T> overrides to honour the mode: stops after the first rule that emits a failure.
```

`Aggregate` remains the default everywhere; `MustValidationOptions.Mode` (§4.2) surfaces it per app. `MustValidationMode` lives in `+ src/PineGuard.Core/MustClauses/MustValidationMode.cs`.

### 3.3 Async predicate clause (`src/PineGuard.MustClauses/MustPredicateClauses.cs`)

```csharp
public static ValueTask<MustResult<T>> SatisfiesAsync<T>(this IMustClause _,
    T? value,
    Func<T, CancellationToken, ValueTask<bool>> predicate,
    CancellationToken cancellationToken = default,
    [CallerArgumentExpression(nameof(value))] string? paramName = null);

public static ValueTask<MustResult<T>> NotSatisfiesAsync<T>(/* same shape */);
```

- Null `predicate` → failure attributed to `nameof(predicate)` (fail-soft, like `Satisfies`). Null `value` → failure with the clause's code (no predicate call), matching `PredicateRules.Satisfies`.
- Codes: the async pair shares the sync pair's constants — `MustCodes.Predicate.Result.False` / `…Result.True` — because it is the same rule evaluated asynchronously (Plan 00 §5.4 rule 5). Rule13 (a) already matches `ValueTask<MustResult<…>>` signatures (Plan 01 §4.1); verify by deleting the constant from `SatisfiesAsync` and confirming Rule13 fails.
- Parity: add both names to `ignoreMethods` in `docs/ai/specs/language/vocabulary.json` with the comment *Guard is synchronous by design; DataAnnotations has no async contract*. FluentValidation **does** get them (§3.4).
- Method ordering: after `NotSatisfies`, positive before negative (Rule08).

### 3.4 FluentValidation async adapter (`src/PineGuard.FluentValidation/`)

- `Common/FluentExtension.cs`: `+ MustBeAsync<T, TProp, TResult>(this IRuleBuilder<T, TProp>, Func<TProp, CancellationToken, ValueTask<MustResult<TResult>>> check, string? message, string? code = null)` built on FluentValidation's `MustAsync((value, ct) => …)` with the same message/`WithErrorCode` handling as `MustBe`.
- `FluentPredicateExtensions`: `SatisfiesAsync` / `NotSatisfiesAsync` extensions.

### 3.5 Audit Rule14 — Core stays synchronous

`Test-Rule14-CoreSync.ps1` in `tools/audit-cli/rules/` (catalogue entry `Rule14 — Core Sync`, `-UsesFailOnFindings`, output `artifacts/audit/Rule14-core-sync.txt`): fails if any file under `src/PineGuard.Core/Rules/**` or `src/PineGuard.Core/Utils/**` contains `\basync\b`, `\bawait\b`, `\bValueTask\b`, `System.Threading.Tasks`, or `Task`/`Task<…>` **as a return or parameter type** outside comments. `Rules/TaskRules.cs` and `Utils/TaskUtility.cs` validate `Task` *values* and are allow-listed for the type-name token in `tools/audit-cli/test-audit-exceptions.json` (`Rule14: AllowTaskType`) — without that the rule is red on a clean tree. `MustValidator<T>` lives under `src/PineGuard.Core/MustClauses/`, which is deliberately outside the rule's scope — the invariant is *Rules/Utils are sync*, per the parent plan's risk table. Wire into `Run-AuditLibraryRules.ps1`, `.vscode/tasks.json`, `tools/audit-cli/README.md`, `docs/ai/specs/tools/audit-cli/spec.md`.

### 3.6 Test-spec addendum

`docs/ai/specs/testing/unit-test.md` §5.1 gains: *"Async members are tested with `public async Task <Member>_BehavesAsExpected(…)` — the only permitted return type other than `void`."* The §5.2 and §8.3 examples in the same spec are annotated "sync form shown; async members use `public async Task`" so the three places agree. Rule50 does not inspect return types; verify by running it.

## 4. Technical plan — `PineGuard.Extensions.DependencyInjection` and `PineGuard.AspNetCore`

### 4.1 `PineGuard.Extensions.DependencyInjection`

| Item | Value |
|---|---|
| Path / namespace | `+ src/PineGuard.Extensions.DependencyInjection/`, `PineGuard.Extensions.DependencyInjection` |
| TFMs | inherited three |
| References | `PineGuard.Core`; package `Microsoft.Extensions.DependencyInjection.Abstractions` (10.0.x) |
| Description | `Registers PineGuard validators in Microsoft.Extensions.DependencyInjection — one validator, or every IMustValidator<T> in an assembly.` |
| Files | `+ src/PineGuard.Extensions.DependencyInjection/MustValidatorServiceCollectionExtension.cs` (feature-prefixed because `PineGuard.AspNetCore` also extends `IServiceCollection` — Plan 00 §4.1), `+ src/PineGuard.Extensions.DependencyInjection/ServiceProviderExtension.cs`, `README.md`, `AGENTS.md` |

```csharp
public static class MustValidatorServiceCollectionExtension
{
    public static IServiceCollection AddMustValidator<TValidator>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TValidator : class, IMustValidator;
    public static IServiceCollection AddMustValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Singleton, Func<Type, bool>? filter = null);
    public static IServiceCollection AddMustValidatorsFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Singleton, Func<Type, bool>? filter = null);
    public static IServiceCollection AddMustValidatorsFromAssemblyContaining<TMarker>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton, Func<Type, bool>? filter = null);
}

public static class ServiceProviderExtension
{
    public static bool TryGetMustValidator(this IServiceProvider provider, Type validatedType, [NotNullWhen(true)] out IMustValidator? validator);
    public static IReadOnlyList<IMustValidator> GetMustValidators(this IServiceProvider provider, Type validatedType);   // every IMustValidator<validatedType>; empty when none
}
```

- `AddMustValidator<TValidator>` registers the concrete type once with the given lifetime and forwards `IMustValidator<T>` for **every** closed `IMustValidator<T>` the type implements (a validator may serve several types), plus the non-generic `IMustValidator`. `Add`, not `TryAdd`, so several validators per `T` are allowed (Phase 4's behavior resolves all of them).
- Scanning: non-abstract, non-open-generic classes implementing `IMustValidator<T>`; `filter` narrows. Annotate scanning members with `[RequiresUnreferencedCode]` under `#if NET8_0_OR_GREATER` (the attribute is net5+); the README says scanning is not trim-safe and `AddMustValidator<T>` is.
- `GetMustValidators` resolves `IEnumerable<IMustValidator<T>>` via `typeof(IEnumerable<>).MakeGenericType(typeof(IMustValidator<>).MakeGenericType(validatedType))` and casts.

### 4.2 `PineGuard.AspNetCore`

| Item | Value |
|---|---|
| Path / namespace | `+ src/PineGuard.AspNetCore/`, `PineGuard.AspNetCore` |
| TFMs | `net8.0;net10.0` (override `TargetFrameworks` in the csproj) |
| References | `PineGuard.Core`, `PineGuard.MustClauses`, `PineGuard.Extensions.DependencyInjection`; `<FrameworkReference Include="Microsoft.AspNetCore.App" />`; `Microsoft.Extensions.Validation` for `net10.0` only (add the `PackageVersion`; if the shared framework already provides it the reference is redundant and can be dropped) |
| Description | `Request validation for ASP.NET Core: Minimal API and MVC auto-validation, RFC 9457 ValidationProblemDetails with stable codes, a boundary-aware exception handler, and .NET 10 Microsoft.Extensions.Validation integration.` |

Files (all `+ src/PineGuard.AspNetCore/…`):

| File | Type |
|---|---|
| `MustValidationOptions.cs` | `public sealed class MustValidationOptions { JsonNamingPolicy? PropertyNamingPolicy; bool UseJsonNamingPolicy = true; bool IncludeCodes = true; MustValidationMode Mode = Aggregate; bool HandleGuardExceptions = false; string UnknownGuardCode = MustCodes.Value.Argument.Invalid; string Title = "One or more validation errors occurred."; Type? LocalizationResourceType; }` — registered by `services.Configure<MustValidationOptions>(configure)`; every filter, handler and the ProblemDetails builder receives it as `IOptions<MustValidationOptions>` and reads `.Value` once |
| `MustFailureDetail.cs` | `public sealed record MustFailureDetail(string PropertyPath, string Code, string Message)` with `[JsonPropertyName("property")]` on `PropertyPath` (wire key stays short; the member follows the program's `PropertyPath` vocabulary) — the `failures` extension item; never carries `Value` (a test serialises a failure whose `Value` is a secret and asserts the body does not contain it) |
| `ProblemDetailsExtension.cs` | `ToValidationProblemDetails(this MustValidationResult result, MustValidationOptions options, JsonNamingPolicy? namingPolicy, IMustFailureMessageResolver resolver, HttpContext httpContext)` → `ValidationProblemDetails` (`Status = 400`, `Title`, `Errors` keyed by `PropertyPathUtility.Transform(path, namingPolicy.ConvertName)`, each message obtained from `resolver.Resolve(failure, httpContext)` and — when a naming policy is in effect — re-rendered from `MessageTemplate` with the **transformed** path so key and message name the field the same way; `Extensions["failures"]` when `IncludeCodes`). Plus the convenience `ToValidationProblemDetails(this MustValidationResult result, HttpContext httpContext)` that pulls options, naming policy and resolver from `httpContext.RequestServices` — the overload a handler doing its own validation reaches for |
| `IMustFailureMessageResolver.cs`, `DefaultMustFailureMessageResolver.cs`, `StringLocalizerMustFailureMessageResolver.cs` | `string Resolve(MustFailure failure, HttpContext httpContext)`; default returns `failure.Message`; localizer looks up `failure.Code` in `IStringLocalizerFactory.Create(options.LocalizationResourceType ?? typeof(MustValidationOptions))`, renders `{paramName}` with the property path, falls back to `failure.Message` when `ResourceNotFound` |
| `MustValidationEndpointFilter.cs` | `IEndpointFilter`; the factory (`EndpointConventionBuilderExtension`) inspects `EndpointFilterFactoryContext.MethodInfo` parameters at build time and registers the filter only if at least one parameter type has a validator, so unvalidated endpoints pay nothing; at run time validates every argument with validators via `ValidateAsync(arg, options.Mode, HttpContext.RequestAborted)`, merges results, builds the body through `ProblemDetailsExtension` (resolver and options from DI), returns `TypedResults.Problem(problemDetails)` on failure |
| `EndpointConventionBuilderExtension.cs` | `AddMustValidation<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder` |
| `MustValidationActionFilter.cs` | `IAsyncActionFilter`; validates `ActionArguments` whose declared parameter type (or runtime type) has validators; on failure writes each failure to `ModelState` (`AddModelError(key, message)`) **and** sets `context.Result = new BadRequestObjectResult(problemDetails)` so MVC and Minimal API bodies are identical; otherwise `await next()` |
| `MvcBuilderExtension.cs` | `AddMustValidation(this IMvcBuilder builder)` → `Configure<MvcOptions>(o => o.Filters.Add<MustValidationActionFilter>())` |
| `MustValidationExceptionHandler.cs` | `IExceptionHandler`; `MustValidationException` → 400 body; when `HandleGuardExceptions`, `ArgumentException` (incl. `ArgumentNullException`, `ArgumentOutOfRangeException`) → 400 with one failure `(ex.GetMustPropertyPath() is { Length: > 0 } p ? p : ParamName ?? "", ex.TryGetMustCode(out var c) ? c : options.UnknownGuardCode, Message)` — the `ExceptionExtension` accessors from Phase 1d; everything else → `false` |
| `MustValidationServiceCollectionExtension.cs` | two overloads — `AddMustValidation(this IServiceCollection services, params Assembly[] validatorAssemblies)` and `AddMustValidation(this IServiceCollection services, Action<MustValidationOptions> configure, params Assembly[] validatorAssemblies)` (an optional parameter before `params` would make `AddMustValidation(typeof(Program).Assembly)` uncompilable) → `Configure<MustValidationOptions>`, `AddMustValidatorsFromAssembly` per assembly, `TryAddSingleton<IMustFailureMessageResolver, DefaultMustFailureMessageResolver>`, `AddExceptionHandler<MustValidationExceptionHandler>()` (the app still calls `app.UseExceptionHandler()`; README) |
| `MustValidatableInfoResolver.cs`, `ValidationOptionsExtension.cs` (`#if NET10_0_OR_GREATER`) | `IValidatableInfoResolver`: `TryGetValidatableTypeInfo(type, out info)` returns an `IValidatableInfo` whose `ValidateAsync(object? value, ValidateContext context, CancellationToken ct)` resolves validators through the `ValidationContext` service provider, runs them, and writes `context.ValidationErrors[PropertyPathUtility.Combine(context.CurrentValidationPath, failure.PropertyPath)]`; `TryGetValidatableParameterInfo` → `false`. `AddMustValidators(this ValidationOptions options)` inserts the resolver at index 0. **Member names of `Microsoft.Extensions.Validation` must be verified against the installed package at implementation time** — the shape above is from the .NET 10 release; adjust names, keep behaviour. Decision rule: if `IValidatableInfoResolver` cannot be satisfied without reflection, drop the resolver from this PR and file it as its own plan — the DoD does not block on it. Naming of `AddMustValidators()` is an open owner decision (Plan 00 §12: `AddMustValidatorResolver()` is the council's recommendation) |

Naming-policy resolution order (`internal static JsonNamingPolicy? ResolveNamingPolicy(HttpContext)`): `options.PropertyNamingPolicy` → if `UseJsonNamingPolicy`: `IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>.Value.SerializerOptions.PropertyNamingPolicy` → `IOptions<Microsoft.AspNetCore.Mvc.JsonOptions>.Value.JsonSerializerOptions.PropertyNamingPolicy` → `null` (paths unchanged). Only identifier segments are transformed; `[2]` indices and `[key]` keys are untouched.

### 4.3 Boundary policy (why the exception handler is shaped this way)

The parent plan requires a marker distinguishing "a guard at the API boundary" from "a guard three layers deep". No heuristic can tell them apart from an `ArgumentException`, so the marker is a **different exception type**: `MustValidationException` (Phase 1) is only thrown by `MustValidationResult.ThrowIfFailed()` and by adapters, i.e. by code that *means* "this request is invalid". Guards keep throwing argument exceptions and keep being 500s. Teams that want 400s from guards choose it explicitly with `HandleGuardExceptions = true` (documented as masking bugs) or, better, use `MustValidationResult.From(...).ThrowIfFailed()` at the boundary. Replacement exceptions for Guards (parent 1.2, fourth bullet) are delivered by Phase 1d's `GuardExceptionPolicy.Map` (Plan 01 §4.14.1) — nothing to add here. `Guard.Against.Invalid` is a guard and throws the argument-exception family, so it stays a 500 like every guard; the 400 spelling at a boundary is `validator.Validate(x).ThrowIfFailed()`.

### 4.4 Onboarding

Two scopes — `DependencyInjection` (`di`) and `AspNetCore` (`aspnetcore`) — via the Plan 02 §3.4 log. `ci.yml` run-if arms: `di` depends on `core`; `aspnetcore` depends on `core`, `must`, `di`. `.editorconfig` brace list gets both test projects.

### 4.5 Docs

`README.md` per package; root README *ASP.NET Core* subsection (stories 1–6 verbatim); the `di` and `aspnetcore` triads (`project.md`, `unit-test.md`, `coverage.md`) planned under docs/ai/specs/; `docs/ai/specs/must-clauses/project.md` async section (Async lives in Must and above; Core never; Rule14); `docs/ai/specs/fluent-validation/project.md` `MustBeAsync`; `docs/ai/specs/testing/unit-test.md` §5.1 addendum; `docs/ai/memory/*` durable patterns ("filters never read `MustFailure.Value`", "only `MustValidationException` maps to 400 by default").

## 5. Testing plan

### 5.1 Core / MustClauses / Fluent (existing projects)

- `MustValidatorTests` (extend): `RuleForAsync` ×2, `RuleForEachAsync` ×2, sequential order with an async rule between sync rules, `Validate` throws when async rules exist, cancellation observed between rules, `StopOnFirstFailure` mode stops after the first failing rule, DIM forwarders on both interfaces.
- `MustPredicateClausesTests` (extend): `SatisfiesAsync`/`NotSatisfiesAsync` — valid, invalid, null value, null predicate attribution, cancellation propagates `OperationCanceledException` (config error path, allowed to throw), `Code`.
- `FluentExtensionTests` (extend): `MustBeAsync` message/code; `FluentPredicateExtensionsTests`: the two async extensions via `ValidateAsync`.
- Existing scopes stay 100/100.

### 5.2 `+ tests/PineGuard.Extensions.DependencyInjection.UnitTests/`

Base `BaseUnitTest`; samples in `Samples/` (a validator for two types, an abstract validator, an open-generic validator, a non-validator). Groups: `AddMustValidator` (registers concrete + each closed interface + non-generic; lifetime honoured; twice → two registrations), `AddMustValidatorsFromAssembly` (finds the concrete validators only; `filter` excludes; abstract/open-generic skipped), `AddMustValidatorsFromAssemblyContaining`, `AddMustValidatorsFromAssemblies`, `TryGetMustValidator` (found / not found / null args → ANE), `GetMustValidators` (zero, one, two). Needs `Microsoft.Extensions.DependencyInjection` as a test-only package.

### 5.3 `+ tests/PineGuard.AspNetCore.UnitTests/`

`<FrameworkReference Include="Microsoft.AspNetCore.App" />` in the test csproj; test-only packages `Microsoft.AspNetCore.TestHost`, `Microsoft.Extensions.DependencyInjection`. Every `XxxTests.cs` ships with `XxxTestData.cs` (Rule50, CI-enforced); single-scenario groups (`MustValidationOptionsTests` defaults) use a `TheoryData` of one named case. Rule53 is a local convention check, not a CI gate — never add allowlist entries to `tools/audit-cli/test-audit-exceptions.json` for new code. Base `BaseMustValidationUnitTest` where a result is asserted, `BaseUnitTest` elsewhere; project-local `ProblemDetailsExpected(bool IsValid, int? Status = null, string[]? ErrorKeys = null, string[]? Codes = null)` for HTTP-shaped assertions.

| Tests | Groups |
|---|---|
| `ProblemDetailsExtensionTests` | naming policy null / camelCase (`Lines[1].Sku` → `lines[1].sku`), root path key `""`, `IncludeCodes` on/off, title, ordering |
| `MustValidationOptionsTests` | defaults (incl. `UnknownGuardCode == MustCodes.Value.Argument.Invalid`) |
| `MustValidationEndpointFilterTests` | `DefaultHttpContext` + `DefaultEndpointFilterInvocationContext`: no validators → `next` invoked unchanged; valid → `next`; invalid → `ProblemHttpResult` 400 with the body; two arguments merge; `Mode` honoured; token is `RequestAborted` |
| `EndpointConventionBuilderExtensionTests` | factory registers the filter only when a parameter type has a validator (inspect `EndpointBuilder.FilterFactories` count) |
| `MustValidationActionFilterTests` | constructed `ActionExecutingContext`; `ModelState` populated; `Result` is `BadRequestObjectResult` with `ValidationProblemDetails`; valid → `next` called |
| `MustValidationExceptionHandlerTests` | `MustValidationException` handled → 400 + body; other exception → `false`; `HandleGuardExceptions` false/true with `ArgumentNullException`, `ArgumentOutOfRangeException` |
| `DefaultMustFailureMessageResolverTests`, `StringLocalizerMustFailureMessageResolverTests` | found (template rendered with property path) / not found (fallback) / no factory |
| `ServiceCollectionExtensionTests` | registrations, scanning delegated, options configured, handler registered |
| `MustValidatableInfoResolverTests` (`#if NET10_0_OR_GREATER`) | `TryGetValidatableTypeInfo` true/false; `ValidateAsync` writes `ValidationErrors` with combined path; parameter info → false |

End-to-end coverage (`TestServer` via `WebApplication.CreateBuilder` + `builder.WebHost.UseTestServer()`) lives as `EndToEnd` operation groups inside `MustValidationEndpointFilterTests` (story 2 JSON verbatim, camelCase policy), `MustValidationActionFilterTests` (story 3, identical body) and `MustValidationExceptionHandlerTests` (story 5; story 6 both settings) — Rule53 maps a test file to a source class by name, so there is no `EndToEndTests` file. Async test methods use the §3.6 form.

## 6. Playbook

### PR 1 — `feature/must-async-di`

**W0** Plan 00 §6 (`<slug> = must-async-di`); read `docs/ai/specs/must-clauses/project.md`, `docs/ai/specs/fluent-validation/project.md`, `docs/ai/specs/testing/unit-test.md`, `docs/ai/specs/tools/spec.md`, Plan 02's onboarding log. Baseline gates.

**W1** Async rules + `MustValidationMode` in Core (§3.1–3.2); tests; `-Scope Core` 100/100; Rule14 script + catalogue + tasks + docs; `Run-All.ps1 -RuleId Rule14` clean. Commit `feat(core): add async validator rules and StopOnFirstFailure mode` and `feat(tools): add audit Rule14 keeping Core synchronous`.

**W2** `SatisfiesAsync`/`NotSatisfiesAsync` (+ `MustCodes`, Rule13), `vocabulary.json` ignores, `MustBeAsync` + Fluent extensions, the unit-test spec addendum; tests; `-Scope MustClauses` and `-Scope FluentValidation` 100/100; `Run-All.ps1 -RuleId Rule06,Rule08,Rule13,Rule50` clean. Commit `feat(must): add the async predicate seam` and `feat(fluent): adapt async Must predicates`.

**W3** Onboard `DependencyInjection` (Plan 02 §3.4 log) → package → tests → `-Scope DependencyInjection` 100/100 → Brain/agents (Rule11/12) → Plan 00 §7 → PR → merge. Commits: `build(di): …`, `feat(di): …`, `test(di): …`, `docs(brain): …`.

### PR 2 — `feature/aspnetcore`

**W4** Plan 00 §6 (`<slug> = aspnetcore`) from the merged main; onboard `AspNetCore` (csproj with the TFM override and `FrameworkReference`; test csproj with `FrameworkReference` + test packages; `ci.yml` etc.). Commit `build(aspnetcore): …`.

**W5** Options, `MustFailureDetail`, `ProblemDetailsExtension`, resolvers, `ServiceCollectionExtension`; tests; commit `feat(aspnetcore): add ValidationProblemDetails mapping and message resolvers`.

**W6** Endpoint filter + factory + convention extension; MVC filter + `IMvcBuilder` extension; exception handler; end-to-end groups; commit `feat(aspnetcore): add Minimal API and MVC auto-validation and the boundary exception handler`.

**W7** `#if NET10_0_OR_GREATER` resolver + `AddMustValidators`; net10-gated tests; commit `feat(aspnetcore): integrate with Microsoft.Extensions.Validation on .NET 10`.

**W8** `-Scope AspNetCore` 100/100, `-Scope All` 100/100; Brain/agents/README (Rule11/12); Plan 00 §7; PR; merge; worktree cleanup.

**W9 — removed.** Guard code stamping moved to Phase 1d (Plan 01 §4.14, W6b) so that every layer carries codes before any adapter ships; W6 here only reads `Exception.Data["pineguard.code"]` / `["pineguard.property-path"]` when `HandleGuardExceptions` is on.

## 7. Definition of Done

Plan 00 §7 for both PRs, plus: Rule14 exists and is clean; the story-2 JSON is produced byte-for-byte (modulo whitespace) by the end-to-end tests for both Minimal API and MVC; `HandleGuardExceptions` defaults to `false` and the README carries the warning; `docs/ai/specs/testing/unit-test.md` §5.1 addendum merged.

## 8. Risks

| Risk | Mitigation |
|---|---|
| `Microsoft.Extensions.Validation` API names differ from this plan | W7 verifies against the installed package; behaviour is specified, names are adjusted |
| `TestServer` end-to-end tests flaky on CI | They are deterministic in-process tests; no ports, no timing |
| Endpoint filter cost on unvalidated endpoints | Factory-time detection returns `next` untouched; test asserts it |
| JSON naming policy resolved from the wrong options type | Resolution order is explicit and tested for Minimal API and MVC |
| Async rules make an existing sync seam (Options) throw | By design; documented in Phase 2 and here |

## 9. Deferred (explicit)

- **`HttpClient` response validation** (`DelegatingHandler` asserting status class / content type / payload shape) and typed-client request validation — a follow-on plan after Phase 4; the roadmap's revised design (validate requests *before* serialization in the typed client, never re-deserialise outbound content in a handler) is the starting point.
- gRPC interceptors, SignalR hub filters, Hangfire job filters, Azure Functions middleware — demand-driven.

<!-- footer
last_verified: 2026-08-26
-->

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · **03 ASP.NET Core** · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->
