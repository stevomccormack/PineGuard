<!-- metadata_header
type: plan
id: new-surfaces-04-mediatr-result-bridges
version: 1.2
status: planned
last_updated: 2026-08-26
parent: new-surfaces-program
-->

# Plan 04 — Phase 4: `PineGuard.MediatR` and the result-pattern bridges

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · **04 MediatR & bridges** · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->

> **Status**: Planned | **Depends on**: the three result bridges — 1a + Track 0 only (Plan 00 unit **4-bridges**, Wave 1); MediatR — Phase 3 PR 1 (`PineGuard.Extensions.DependencyInjection`, async rules; Plan 00 unit **4-mediatr**, Wave 2). `MustValidationException` → 400 mapping (Phase 3 PR 2) is optional for both | **Unblocks**: nothing — leaf phase
>
> **Worktrees** (two PRs, independent): `.claude/worktrees/result-bridges` on `feature/result-bridges` (W1 for three scopes, W3–W5, W6–W7) and `.claude/worktrees/mediatr` on `feature/mediatr` (W1 for one scope, W2, W6–W7). The bridges never wait for ASP.NET; the keystone-freeze checkpoint (Plan 00 §10.2) needs them merged early.
>
> Read [Plan 00](new-surfaces-missing-validation-cases-00-program.md) first; onboarding per Plan 02 §3.4's log.

## 1. Business plan

### 1.1 The problem

The MediatR "validation behavior" is the single most copy-pasted validation snippet in .NET codebases — every team re-writes the same forty lines that run validators before a handler and either throw or return a failed result. Result-oriented codebases (ErrorOr, FluentResults, OneOf) meanwhile have to hand-write the same `MustResult<T>` → their-result conversion in every handler. Both are tiny surfaces with broad reach.

### 1.2 Value

- **Reach**: four micro-packages, each ≤ 150 lines, that let PineGuard drop into the most common application architectures with one registration.
- **Consistency**: validators stay on `IMustValidator<T>`; each adapter is a shim, so a team can switch from MediatR to another mediator, or from throwing to returning results, without touching a validator.
- **Codes travel**: every bridge carries `Code` and `PropertyPath` into the target library's error type, so the Phase 1 investment reaches result-oriented code unchanged.

### 1.3 Success metrics

- `services.AddMediatR(cfg => cfg.AddMustValidation())` validates every request that has a validator, aggregating all of them.
- `Must.Be.Email(x).ToErrorOr()`, `.ToResult()`, `.ToOneOf()` are one-liners; `MustValidationResult` bridges exist for each.
- Four packages onboarded; 100 %/100 % each.

## 2. Functional plan

### 2.1 User stories

1. **MediatR, throw mode (default).**

   ```csharp
   using PineGuard.Extensions.DependencyInjection;
   using PineGuard.MediatR;

   services.AddMustValidatorsFromAssemblyContaining<Program>();
   services.AddMediatR(cfg =>
   {
       cfg.RegisterServicesFromAssemblyContaining<Program>();
       cfg.AddMustValidation();          // open behavior, runs before handlers
   });
   ```

   A request with a registered validator that fails throws `MustValidationException` (which Phase 3's exception handler turns into a 400 when hosted in ASP.NET Core).

2. **MediatR, respond mode.** The consumer registers a factory and the behavior returns instead of throwing. Two seams, because Microsoft DI cannot map an open generic `IMustFailureResponseFactory<>` to an implementation that closes it as `IMustFailureResponseFactory<ErrorOr<T>>` (the type parameters do not line up — registration throws): a closed generic for one response type, and a non-generic one for a *family* of response types:

   ```csharp
   // one response type
   services.AddSingleton<IMustFailureResponseFactory<CreateOrderResult>, CreateOrderFailureFactory>();

   // a family — every ErrorOr<T>
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

   (The README ships this exact sample for ErrorOr; the package does not depend on ErrorOr.)

3. **ErrorOr.** `ErrorOr<string> email = Must.Be.Email(input).ToErrorOr();` → `Error.Validation(code: "email.address.invalid", description: "…")`; `List<Error> errors = result.ToErrors();` `ErrorOr<CreateOrder> r = result.ToErrorOr(order);`.
4. **FluentResults.** `Result<string> r = Must.Be.Email(input).ToResult();` — failures are `MustError` (a `FluentResults.Error` with `Code` and `PropertyPath` properties and the same values in `Metadata`); `Result r = result.ToResult();` `Result<T> r = result.ToResult(value);`.
5. **OneOf.** `OneOf<string, MustFailure> r = Must.Be.Email(input).ToOneOf();` `OneOf<CreateOrder, MustValidationResult> r = result.ToOneOf(order);`.

### 2.2 Acceptance criteria

- [ ] `MustValidationBehavior<TRequest,TResponse>` runs **all** `IMustValidator<TRequest>` registered, merges results, and either throws `MustValidationException` or returns `IMustFailureResponseFactory<TResponse>.Create(result)` when a factory is registered; requests without validators pass straight through with no allocation beyond the `IEnumerable` resolution.
- [ ] Bridge methods exist with the signatures in §3; success paths pass `Result`/value through; failure paths carry code, message and property path.
- [ ] Four packages published-ready with READMEs; four scopes onboarded; 100 %/100 %.

### 2.3 Not in this phase

MassTransit / Wolverine / source-generated Mediator shims (documented as patterns in the MediatR README; separate plans if demand appears). `HttpClient` response validation (deferred from Phase 3).

## 3. Technical plan

### 3.1 `PineGuard.MediatR`

| Item | Value |
|---|---|
| Path / namespace | `+ src/PineGuard.MediatR/`, `PineGuard.MediatR` |
| TFMs | inherited three (MediatR 12 ships `netstandard2.0`) |
| References | `PineGuard.Core`, `PineGuard.Extensions.DependencyInjection`; package `MediatR` pinned to the **12.5.x** line in `Directory.Packages.props` (the last Apache-2.0 line; 13.x moved to a commercial licence — the consumer may still use 13.x because the NuGet dependency range is `>= 12.5.0`) |
| Description | `IPipelineBehavior that validates MediatR requests with PineGuard validators — throw MustValidationException or return your own failure response.` |

```csharp
namespace PineGuard.MediatR;

public interface IMustFailureResponseFactory<out TResponse>          // one response type
{
    TResponse Create(MustValidationResult result);
}

public interface IMustFailureResponseFactory                          // a family of response types (open generics such as ErrorOr<T>)
{
    bool TryCreate(Type responseType, MustValidationResult result, out object? response);
}

public sealed class MustValidationBehavior<TRequest, TResponse>(
    IEnumerable<IMustValidator<TRequest>> validators,
    IEnumerable<IMustFailureResponseFactory<TResponse>> typedFactories,      // IEnumerable, never an optional parameter: container-agnostic and never silently drops a second registration
    IEnumerable<IMustFailureResponseFactory> familyFactories)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

public static class MediatRServiceConfigurationExtension
{
    public static MediatRServiceConfiguration AddMustValidation(this MediatRServiceConfiguration configuration);   // cfg.AddOpenBehavior(typeof(MustValidationBehavior<,>))
}
```

Behaviour: no validators → `await next()`; otherwise `ValidateAsync(request, cancellationToken)` on each in registration order, `MustValidationResult.Combine`; success → `next()`; failure → the first typed factory's `Create(result)`, else the first family factory whose `TryCreate(typeof(TResponse), result, out var r)` returns true (cast to `TResponse`), else `throw new MustValidationException(result)`. Resolution order is typed before family so a specific registration always wins. `RequestHandlerDelegate<TResponse>` is invoked as `next()` (MediatR 12 signature; 13 adds a token overload — the 12-style call compiles on both).

Files: `+ src/PineGuard.MediatR/IMustFailureResponseFactory.cs` (non-generic), `IMustFailureResponseFactoryOfT.cs` (the same named file-name exception as `IMustValidatorOfT.cs`), `MustValidationBehavior.cs`, `MediatRServiceConfigurationExtension.cs`, `README.md`, `AGENTS.md`.

### 3.2 `PineGuard.ErrorOr`

| Item | Value |
|---|---|
| Path / namespace | `+ src/PineGuard.ErrorOr/`, `PineGuard.ErrorOr` |
| References | `PineGuard.Core`; package `ErrorOr` (2.x) |
| File | `+ src/PineGuard.ErrorOr/ErrorOrExtension.cs` |

```csharp
public static class ErrorOrExtension
{
    public static ErrorOr<T> ToErrorOr<T>(this MustResult<T> result);       // success → result.Result!; failure → Error.Validation(result.Code, result.Message, metadata: { ["propertyPath"] = MustFailure.From(result).PropertyPath })
    public static Error ToError(this MustFailure failure);                   // Error.Validation(failure.Code, failure.Message, metadata: { ["propertyPath"] = failure.PropertyPath })
    public static List<Error> ToErrors(this MustValidationResult result);              // empty list on success
    public static ErrorOr<T> ToErrorOr<T>(this MustValidationResult result, T value);  // success → value; failure → ToErrors()
}
```

### 3.3 `PineGuard.FluentResults`

| Item | Value |
|---|---|
| Path / namespace | `+ src/PineGuard.FluentResults/`, `PineGuard.FluentResults` |
| References | `PineGuard.Core`; package `FluentResults` (3.x) |
| Files | `+ src/PineGuard.FluentResults/MustError.cs`, `+ src/PineGuard.FluentResults/FluentResultsExtension.cs` |

```csharp
public sealed class MustError : Error
{
    public MustError(string code, string propertyPath, string message);   // base(message); WithMetadata("code", code).WithMetadata("propertyPath", propertyPath) — camelCase metadata keys in every bridge (§3.5)
    public string Code { get; }
    public string PropertyPath { get; }
    public static MustError From(IMustResult result);   // failure only (ArgumentException otherwise)
    public static MustError From(MustFailure failure);
}

public static class FluentResultsExtension
{
    public static Result<T> ToResult<T>(this MustResult<T> result);
    public static Result ToResult(this MustValidationResult result);
    public static Result<T> ToResult<T>(this MustValidationResult result, T value);
}
```

### 3.4 `PineGuard.OneOf`

| Item | Value |
|---|---|
| Path / namespace | `+ src/PineGuard.OneOf/`, `PineGuard.OneOf` |
| References | `PineGuard.Core`; package `OneOf` (3.x) |
| File | `+ src/PineGuard.OneOf/OneOfExtension.cs` |

```csharp
public static class OneOfExtension
{
    public static OneOf<T, MustFailure> ToOneOf<T>(this MustResult<T> result);   // failure → MustFailure.From(result)
    public static OneOf<T, MustValidationResult> ToOneOf<T>(this MustValidationResult result, T value);
}
```

### 3.5 Shared notes

- Success with a `null` `Result` (e.g. `NullOrEmpty` succeeding on `null`) maps to the target library's value slot as `default!`; each README states that the bridges follow the clause's own `Result` contract.
- No bridge references another bridge; the MediatR README's ErrorOr sample is *consumer* code.
- Metadata keys are camelCase in every bridge (`"code"`, `"propertyPath"`) — metadata bags are wire-shaped and ErrorOr's own conventions are camel.
- Inside `namespace PineGuard.OneOf;` / `PineGuard.ErrorOr;` / `PineGuard.FluentResults;` the simple names `OneOf`, `ErrorOr`, `Error` bind to the enclosing namespace first: every target type is written `global::OneOf.OneOf<…>`, `global::ErrorOr.Error`, `global::FluentResults.Error` (Plan 00 §4.1; `naming-collisions.md`).
- `MustError` is the one sanctioned use of the `Error` noun: the target library's base type is `Error` (Plan 00 §5.2 carve-out).
- Scope identifiers: `mediatr`, `erroror`, `fluentresults`, `oneof`; PowerShell scopes `MediatR`, `ErrorOr`, `FluentResults`, `OneOf`. Dependabot groups `mediatr` and `result-libraries` (`ErrorOr`, `FluentResults`, `OneOf`).

## 4. Testing plan

Four test projects (Plan 00 §4.5). Base classes: `BaseMustValidationUnitTest` for result inputs where useful, otherwise `BaseUnitTest` with project-local `ReturnExpected`/`ReturnCase`-derived records. Every `XxxTests.cs` ships with `XxxTestData.cs` (Rule50).

| Project | Tests | Groups |
|---|---|---|
| `+ tests/PineGuard.MediatR.UnitTests/` (test-only packages: `Microsoft.Extensions.DependencyInjection`, `MediatR`) | `MustValidationBehaviorTests` | no validators → next called once; one validator valid → next; invalid → `MustValidationException` with the merged result; two validators both failing → both sets of failures in registration order; typed factory registered → returns `Create(result)` and next not called; family factory matches / does not match; typed wins over family; a spy validator records the `CancellationToken` it received and the test asserts it equals the token passed to `Handle`; `notnull` request |
| | `MediatRServiceConfigurationExtensionTests` | registers the open behavior; `IMediator.Send` end-to-end through a real `ServiceProvider` with a sample request/handler/validator |
| `+ tests/PineGuard.ErrorOr.UnitTests/` | `ErrorOrExtensionTests` | success value; failure code/description/metadata; `ToError`; `ToErrors` empty/many; `ToErrorOr(value)` both branches |
| `+ tests/PineGuard.FluentResults.UnitTests/` | `MustErrorTests`, `FluentResultsExtensionTests` | ctor/metadata; `From` both overloads incl. success → `ArgumentException`; the three `ToResult` shapes |
| `+ tests/PineGuard.OneOf.UnitTests/` | `OneOfExtensionTests` | both branches of both methods; `IsT0`/`IsT1` and values |

Samples for MediatR (`Samples/`): `CreateOrder : IRequest<Guid>`, its handler, a validator, a second validator for the same request, an `IMustFailureResponseFactory<Guid>`.

## 5. Playbook

**W0** Plan 00 §6 (`<slug> = mediatr-bridges`); read `docs/ai/specs/spec.md`, Plan 02 §3.4 log; baseline gates.

**W1** Onboard the scopes of the PR (three for `result-bridges`, one for `mediatr`; Plan 00 §8.1–8.3 via the registry) with empty projects; `dotnet build` clean; commit `build(bridges): add ErrorOr, FluentResults and OneOf projects and scopes` / `build(mediatr): add the MediatR project and scope`.

**W2** `PineGuard.MediatR` + tests → `-Scope MediatR` 100/100 → commit `feat(mediatr): add MustValidationBehavior and AddMustValidation`.

**W3** `PineGuard.ErrorOr` + tests → 100/100 → commit `feat(erroror): bridge MustResult and MustValidationResult to ErrorOr`.

**W4** `PineGuard.FluentResults` + tests → 100/100 → commit `feat(fluentresults): bridge MustResult and MustValidationResult to FluentResults`.

**W5** `PineGuard.OneOf` + tests → 100/100 → commit `feat(oneof): bridge MustResult and MustValidationResult to OneOf`.

**W6** Brain/agents for the PR's scopes (Rule11/12), READMEs, root README *Mediator and result bridges* subsection (each PR adds its rows); commit `docs(brain): onboard the erroror, fluentresults and oneof scopes` / `docs(brain): onboard the mediatr scope`.

**W7** Plan 00 §7 (`-Scope All` 100/100); PR; merge; cleanup.

## 6. Definition of Done

Plan 00 §7, plus: MediatR pinned to 12.5.x with the licence rationale in `Directory.Packages.props` as a comment; the ErrorOr factory sample in the MediatR README compiles as a test sample; no bridge package references another.

## 7. Risks

| Risk | Mitigation |
|---|---|
| MediatR 13 signature drift (`RequestHandlerDelegate` token overload) | Use the parameterless `next()` call, valid on both |
| Consumers on MediatR 13 hit licence surprises | Not PineGuard's concern beyond the README note; dependency range allows both |
| Bridging `T?` results into non-nullable value slots | Documented; tests pin `default!` behaviour |

## 8. Out of scope

Other mediators, message-bus filters, `HttpClient` handlers, bridges to further result libraries (add one micro-package per library on demand, same shape).

<!-- footer
last_verified: 2026-08-26
-->

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · **04 MediatR & bridges** · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->
