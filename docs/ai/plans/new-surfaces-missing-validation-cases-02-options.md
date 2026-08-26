<!-- metadata_header
type: plan
id: new-surfaces-02-options
version: 1.2
status: planned
last_updated: 2026-08-26
parent: new-surfaces-program
-->

# Plan 02 — Phase 2: `PineGuard.Extensions.Options`

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · **02 Options** · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->

> **Status**: Planned | **Depends on**: Phase 1 (`IMustValidator<T>`, `MustValidationResult`, `InlineMustValidator<T>`) | **Unblocks**: Phases 3–4 reuse the scope-onboarding procedure this phase proves
>
> **Worktree**: `.claude/worktrees/options` on `feature/options`.
>
> Read [Plan 00](new-surfaces-missing-validation-cases-00-program.md) first — §4 package conventions, §5 naming canon, §6 worktree protocol, §7 Definition of Done, §8 new-scope checklist apply verbatim. This is the **first new package**, so it also executes §8 in full and records anything the checklist missed.

## 1. Business plan

### 1.1 The problem

Configuration is the input every .NET application validates last and worst. `services.AddOptions<T>().ValidateDataAnnotations()` gives `[Required]`/`[Range]`; anything richer (a hostname, an HTTPS URL, a port, a cron string, an email sender) is hand-written `Validate(o => …)` lambdas or nothing. FluentValidation has no first-party options integration. When validation is missing, the failure surfaces at first use — in production, at 3 a.m., as a `NullReferenceException` three layers away from the bad setting. When it is present but validates one field at a time, an operator fixes one setting, restarts, and hits the next.

### 1.2 Value

- **Consumer**: one line — `.ValidateMustRules()` — brings the whole `Must.Be.*` vocabulary to configuration, and `ValidateOnStart()` makes the host refuse to start with **every** violation listed in one exception message.
- **Program**: the smallest possible adapter over the Phase 1 keystone. If `IMustValidator<T>` + `MustValidationResult` are wrong, this phase finds out for ~150 lines of production code rather than for the ASP.NET package.
- **Competitive**: no validation library in the .NET ecosystem ships this first-party.

### 1.3 Success metrics

- `PineGuard.Extensions.Options` published with `netstandard2.1;net8.0;net10.0` assets and a README whose examples are tests.
- Host start-up with two invalid settings throws one `OptionsValidationException` naming both, with codes.
- The Plan 00 §8 checklist is executed once and corrected where it was wrong; the correction is committed so Phases 3–4 inherit it.
- 100 %/100 % for the new scope; every existing scope unchanged.

## 2. Functional plan

### 2.1 User stories

1. As a developer I register my validator in DI and wire it with one call:

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

   (Clause names above are the existing ones; confirm each against `src/PineGuard.MustClauses/` when writing the README. `Must.Be.Positive`/`PortNumber` are net8+ clauses — say so beside the example. Once `PineGuard.Extensions.DependencyInjection` ships (Phase 3), the preferred registration is `builder.Services.AddMustValidator<SmtpOptionsValidator>()`; Plan 03 W3 updates this README.)

2. As a developer with a small options class I validate inline without a class:

   ```csharp
   builder.Services.AddOptions<CacheOptions>()
       .BindConfiguration("Cache")
       .ValidateMustRules(v => v.RuleFor(o => o.TtlSeconds, ttl => Must.Be.Positive(ttl)))
       .ValidateOnStart();
   ```

3. As a developer I pass an instance: `.ValidateMustRules(new SmtpOptionsValidator())`.
4. As an operator, when `Smtp:Host` and `Smtp:From` are both wrong, the host fails to start with:

   ```text
   OptionsValidationException: SmtpOptions.Host: Host must be a valid hostname. [network.hostname.invalid]; SmtpOptions.From: From must be a valid email address. [email.address.invalid]
   ```

5. As a developer using named options (`AddOptions<SmtpOptions>("Marketing")`) the validator runs only for that name.
6. As a developer using `IOptionsMonitor<T>` reloads, a reloaded invalid section throws on next access, exactly as `ValidateDataAnnotations()` does — the package adds no behaviour of its own here.

### 2.2 Acceptance criteria

- [ ] The three `ValidateMustRules` overloads exist with the signatures in §3.3 and chain (`return builder`).
- [ ] `MustRulesValidateOptions<TOptions>` implements `IValidateOptions<TOptions>` with `Skip` for non-matching names, `Success`, and `Fail` listing **all** failures in result order in the format of §3.2.
- [ ] `ValidateOnStart()` composes: `IStartupValidator.Validate()` throws one `OptionsValidationException` whose `Failures` has one entry per `MustFailure`.
- [ ] `null` options → `ArgumentNullException` (matches Microsoft's validators); a missing `IMustValidator<TOptions>` registration → `InvalidOperationException` from DI at validation time with the standard "no service registered" message (do not wrap it).
- [ ] README examples are the test samples.
- [ ] Plan 00 §7 in full; Plan 00 §8 executed and, where wrong, corrected in this PR.

### 2.3 Not in this phase

Assembly scanning / `AddMustValidator<T>()` sugar — Phase 3's `PineGuard.Extensions.DependencyInjection` (plain `services.AddSingleton<IMustValidator<T>, TValidator>()` is idiomatic and sufficient here). Async validators — the `IValidateOptions` contract is synchronous. The instance and inline overloads of `ValidateMustRules` reject a validator with async rules **at registration** (`InvalidOperationException` naming the validator and the first async rule's property path — `MustValidator<T>.HasAsyncRules`, Phase 3), so the mistake fails at composition, not at bind; the DI-resolved overload can only detect it at first validation and documents that. Localised failure text — Phase 3 seam.

## 3. Technical plan

### 3.1 Package

| Item | Value |
|---|---|
| Path | `+ src/PineGuard.Extensions.Options/` |
| Package / assembly / namespace | `PineGuard.Extensions.Options` |
| TFMs | inherited `netstandard2.1;net8.0;net10.0` |
| ProjectReferences | `PineGuard.Core`, `PineGuard.MustClauses` |
| PackageReferences | `Microsoft.Extensions.Options` (adds `PackageVersion` 10.0.x to `Directory.Packages.props`; it transitively brings `Microsoft.Extensions.DependencyInjection.Abstractions` and `Microsoft.Extensions.Primitives`) |
| Description | `IValidateOptions<T> for configuration validated by PineGuard. AddOptions<T>().ValidateMustRules().ValidateOnStart() fails fast at host start with every violation listed.` |
| Tags | `$(PackageTags);options;configuration;ioptions;validateonstart;startup-validation` |
| Files | `+ src/PineGuard.Extensions.Options/MustRulesValidateOptions.cs`, `+ src/PineGuard.Extensions.Options/OptionsBuilderExtension.cs`, `README.md`, `AGENTS.md` |

**Verify at first build** where `ValidateOnStart()` and `IStartupValidator` live for the pinned package line (they have moved between `Microsoft.Extensions.Options` and `Microsoft.Extensions.Hosting` across majors): the package itself never calls either, so no Hosting reference is needed in `src/`; the README states the consumer-side package per TFM from what the build shows, and the test project references `Microsoft.Extensions.Hosting` so `ValidateOnStart()` is exercised end-to-end regardless.

### 3.2 `MustRulesValidateOptions<TOptions>`

```csharp
namespace PineGuard.Extensions.Options;

public sealed class MustRulesValidateOptions<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    public MustRulesValidateOptions(string? name, IMustValidator<TOptions> validator);   // validator null → ArgumentNullException

    public string? Name { get; }                                                   // null = every named instance

    public ValidateOptionsResult Validate(string? name, TOptions options);
}
```

Behaviour, in order:

1. `Name is not null && !string.Equals(name, Name, StringComparison.Ordinal)` → `ValidateOptionsResult.Skip`. (Microsoft's `ValidateOptions<T>` uses the same rule; `OptionsBuilder<T>.Name` is `Options.DefaultName` (`""`) for unnamed options, so `""` is compared literally.)
2. `options is null` → `ArgumentNullException(nameof(options))`.
3. `var result = validator.Validate(options);` → `Success` ⇒ `ValidateOptionsResult.Success`.
4. Otherwise `ValidateOptionsResult.Fail(result.Failures.Select(f => FormatFailure(f)))` where

   ```text
   FormatFailure: "{TypeName}.{PropertyPath}: {Message} [{Code}]"     when PropertyPath is non-empty
                  "{TypeName}: {Message} [{Code}]"                  when PropertyPath is ""
   TypeName = typeof(TOptions).Name
   ```

   `internal static string FormatFailure(MustFailure failure)` so tests pin the format. `ValidateOptionsResult.Fail(IEnumerable<string>)` joins with `"; "` for `FailureMessage`, which is what `OptionsValidationException.Message` shows.

No catching, no wrapping: an async-only validator (Phase 3) throwing `InvalidOperationException` from `Validate` is a programmer error and must surface as-is.

### 3.3 `OptionsBuilderExtension`

```csharp
namespace PineGuard.Extensions.Options;

public static class OptionsBuilderExtension
{
    /// Resolves IMustValidator<TOptions> from the container when validation runs.
    public static OptionsBuilder<TOptions> ValidateMustRules<TOptions>(this OptionsBuilder<TOptions> builder)
        where TOptions : class;

    /// Uses the given validator instance.
    public static OptionsBuilder<TOptions> ValidateMustRules<TOptions>(this OptionsBuilder<TOptions> builder, IMustValidator<TOptions> validator)
        where TOptions : class;

    /// Builds an InlineMustValidator<TOptions> once, at registration time, from the configure delegate.
    public static OptionsBuilder<TOptions> ValidateMustRules<TOptions>(this OptionsBuilder<TOptions> builder, Action<InlineMustValidator<TOptions>> configure)
        where TOptions : class;
}
```

Implementation:

- Overload 1: `builder.Services.AddSingleton<IValidateOptions<TOptions>>(sp => new MustRulesValidateOptions<TOptions>(builder.Name, sp.GetRequiredService<IMustValidator<TOptions>>()))`. `AddSingleton` (not `TryAdd`): several `IValidateOptions<T>` may coexist, exactly as `ValidateDataAnnotations()` + `Validate(...)` do.
- Overload 2: `AddSingleton<IValidateOptions<TOptions>>(new MustRulesValidateOptions<TOptions>(builder.Name, validator))`.
- Overload 3: `var validator = new InlineMustValidator<TOptions>(); configure(validator);` then overload 2. `configure` runs exactly once.
- All three: `ThrowHelper.ThrowIfNull(builder)` and of the second argument; return `builder`.
- Why a `GetRequiredService` factory rather than resolving at registration: validators may depend on other services; resolving lazily keeps the registration order irrelevant. Validators are immutable after construction (Phase 1), so **singleton** is the documented lifetime; the README says so and explains that a scoped validator resolved from the root provider throws in development (ASP.NET's scope validation) — which is the framework telling the consumer the same thing.

### 3.4 Scope onboarding (Plan 00 §8) — executed here for the first time

Do §8.1–§8.3 in this order, and log each item as *done*, *n/a*, or *checklist was wrong: fixed as …* in the PR body:

- §8.1 1–6 (solution via `dotnet sln`, `Directory.Packages.props`, `.editorconfig` brace list, no `InternalsVisibleTo` in Core — this package uses no `[CallerArgumentExpression]`, no Sonar entries needed, dependabot `microsoft-extensions` group).
- §8.2 7 — `ci.yml`: filter `options`, output, matrix entry `Options` with `run-if: core-or-must-or-options-or-testing-or-main`, `case` arm.
- §8.3 8–21 — add the `Options` entry to `Get-PineGuardScope` (`SourceDir`, `TestProject`, `IncludePattern`, `PathRegex`, `QodanaConfig`, `QodanaSlug`, `PackageId`) and the `Options` token to each script's `ValidateSet` (PowerShell needs the literal); `Commit-Options.ps1` in `tools/git/` and the `Run-Commits.ps1` switch; Qodana yaml + slnx; release scripts; tasks.json.
- §8.4 22–29 — the Brain: `docs/ai/specs/options/{project,unit-test,coverage}.md` (templates in `docs/ai/meta/`), `docs/ai/rules/options.md` plus the rules-only adapter layer maps and `.claude/rules/options.md` / `.cursor/rules/options.mdc` / `.github/instructions/options.instructions.md`, taxonomy §N.4 row `options → PineGuard.Extensions.Options`, the nine agents cascaded with `docs/ai/skills/scaffold-workflow/SKILL.md`, command rows, workflow/all-agent tables, `coverage.md` library list, gold-standard row, root README.

Record the actual list of files touched in the PR body; Phase 3 copies it.

### 3.5 Docs

- `+ src/PineGuard.Extensions.Options/README.md`: masthead *"Configuration that refuses to start wrong."*; install; the §2.1 examples; a *What you get* list; *Supported frameworks*; the standard closer.
- Root `README.md`: package table row, install snippet line, a short *Options validation* subsection under the surfaces list.
- `docs/ai/specs/options/project.md`: the §3.2/§3.3 contract, the failure-format rule, the singleton-lifetime rule, and "adapters never catch validator exceptions".

## 4. Testing plan

Project `+ tests/PineGuard.Extensions.Options.UnitTests/` (Plan 00 §4.5 skeleton) with additional test-only packages `Microsoft.Extensions.DependencyInjection`, `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Options.ConfigurationExtensions`, `Microsoft.Extensions.Hosting` (all need `PackageVersion` entries). Every `XxxTests.cs` ships with `XxxTestData.cs` (Rule50 — the CI-enforced audit rule); single-scenario groups use a `TheoryData` of one named case so the README's `BindConfiguration` path is exercised end-to-end from an in-memory configuration.

Base class: `BaseUnitTest` (there is no layer base for `ValidateOptionsResult`); project-local expectation types (precedent: `tests/PineGuard.DataAnnotations.UnitTests/ThrowsCase.cs`):

```csharp
public sealed record ValidateOptionsExpected(bool IsValid, string? Message = null, bool Skipped = false, IReadOnlyList<string>? Failures = null) : ReturnExpected(IsValid, Message);   // family shape (testing/project.md §3 rule 2); a list, not an array, so record equality is by value
public sealed record ValidateOptionsCase<TValue>(string Name, TValue Value, ValidateOptionsExpected Expected) : ReturnCase<TValue, ValidateOptionsExpected>(Name, Value, Expected);
```

with a `private static void AssertResult(ValidateOptionsCase<T> tc, ValidateOptionsResult result)` in each test class (asserts `Succeeded`/`Skipped`/`Failed` and, when `Failures` is given, the exact ordered strings).

Samples in `+ tests/PineGuard.Extensions.Options.UnitTests/Samples/` — `SmtpOptions`, `SmtpOptionsValidator` (the README code), `CacheOptions`.

| Tests / TestData | Operation groups |
|---|---|
| `MustValidateOptionsTests` | `Constructor` (null validator → ANE via `ThrowsCase`); `Validate` — default name match, named match, named mismatch → Skipped, `Name == null` validates any name, null options → ANE, valid → Success, one failure, three failures in result order, root-path failure format; `FormatFailure` (internal, `InternalsVisibleTo`) — with and without property path |
| `OptionsBuilderExtensionTests` | `ValidateMustRules` (DI-resolved: registers `IValidateOptions<T>`; `IOptions<T>.Value` throws `OptionsValidationException` with expected `Failures`; missing validator registration → `InvalidOperationException` — assert the type and that the message contains `nameof(IMustValidator<SmtpOptions>)`, never the framework's full sentence); `ValidateMustRulesInstance`; `ValidateMustRulesInline` (configure invoked once; rules honoured); `ValidateOnStart` (in-memory `IConfiguration` → `BindConfiguration` → `IStartupValidator.Validate()` throws once listing every failure); null-argument cases via `ThrowsCase` |

Coverage: `pwsh … Run-CodeCoverage.ps1 -Mode GenerateAndAnalyze -Scope Options -SkipHtml -Enforce100` → 100/100 (the `Options` scope must exist in the coverage scripts before this can run — §3.4 first).

## 5. Playbook

`<wt>` = `.claude/worktrees/options`.

### W0 — Set up
1. Plan 00 §6 steps 0–2 (`<slug> = options`). Confirm `main` contains Phase 1 (`git log --oneline -20 | grep -i "MustValidator"`).
2. Read `docs/ai/specs/spec.md`, `docs/ai/specs/project.md`, `docs/ai/specs/testing/unit-test.md`, `docs/ai/specs/testing/project.md`, `docs/ai/specs/tools/spec.md`, `docs/ai/meta/adapter-surfaces.md`, `docs/ai/skills/scaffold-workflow/SKILL.md`.
3. Baseline build + test (Plan 00 §9).

### W1 — Track 0 must be on `main`

> The `Get-PineGuardScope` registry is **Track 0** in Plan 00 §10 (approved, §12) and is executed there, not here. W1 is a check: `git log --oneline origin/main | grep -i "scope registry"`. If Track 0 has not merged, execute it now as its own worktree/PR (`.claude/worktrees/track-0`, `feature/tooling-scope-registry`): add `Get-PineGuardScope` to `tools/.shared/dotnet-projects.ps1` seeded with the six existing scopes; refactor items 8–15 of Plan 00 §8.3 to consume it with identical behaviour (run `Run-CodeCoverage.ps1 -Scope Core -SkipHtml -Enforce100 -Framework net10.0` and `-Scope MustClauses` before and after, compare summaries); apply Plan 00 §8.2 items 7(e)–7(g); Rule09/Rule10 clean; commit `refactor(tools): centralise per-scope project paths in one registry`; merge; then continue here.

### W2 — Onboard the `Options` scope
1. Create `src/PineGuard.Extensions.Options/PineGuard.Extensions.Options.csproj` (Plan 00 §4.4) and `tests/PineGuard.Extensions.Options.UnitTests/PineGuard.Extensions.Options.UnitTests.csproj` (§4.5 + the test-only packages); `dotnet sln PineGuard.slnx add …` for both (tests into the `tests` solution folder).
2. Plan 00 §8.1–8.3 for `Options`. `dotnet build <wt>/PineGuard.slnx -c Release` clean with the empty projects.
3. Commit `build(options): add PineGuard.Extensions.Options project, tests and tooling scope`.

### W3 — Package
1. `MustRulesValidateOptions<TOptions>`, `OptionsBuilderExtension`, XML docs on everything, `README.md`, `AGENTS.md`.
2. Commit `feat(options): add IValidateOptions adapter and ValidateMustRules`.

### W4 — Tests
1. Samples, TestData, Tests per §4. `pwsh … Run-Tests.ps1 -Project "<wt>/tests/PineGuard.Extensions.Options.UnitTests/PineGuard.Extensions.Options.UnitTests.csproj"`.
2. Coverage `-Scope Options` → 100/100; then `-Scope All` → still 100/100.
3. Commit `test(options): cover MustRulesValidateOptions and the builder extensions`.

### W5 — Brain and adapters
1. Plan 00 §8.4 for `options`. `Run-All.ps1 -RuleId Rule11,Rule12` clean.
2. Commit `docs(brain): onboard the options scope (specs, rules, agents, commands)`.

### W6 — Gates, PR, merge
1. Plan 00 §7; `dotnet format` then `--verify-no-changes`; `Run-All.ps1 -RuleId Rule50`.
2. Plan 00 §6 steps 6–9. PR body includes the §3.4 onboarding log.

## 6. Definition of Done

Plan 00 §7, plus:

- [ ] `Options` is a first-class scope in every script `ValidateSet`, `ci.yml`, `.editorconfig`, VS Code tasks, commit tooling, release tooling, Qodana, the Brain and every full adapter (Rule12 clean).
- [ ] The onboarding log in the PR body corrects Plan 00 §8 where it was wrong, and Plan 00 §8 itself is amended in the same PR.
- [ ] README examples compile as the test samples.

## 7. Risks

| Risk | Mitigation |
|---|---|
| `ValidateOnStart` missing on the `netstandard2.1` asset of `Microsoft.Extensions.Options` | Checked in W3's first build; README fallback text ready |
| Consumer registers a scoped validator | Documented; ASP.NET's scope validation throws a clear message in development |
| Track 0's registry refactor changed coverage output for existing scopes | Track 0's before/after comparison is mandatory; W1 here re-checks `-Scope Core` before onboarding |
| The checklist in Plan 00 §8 is incomplete | This phase's job is to find that out cheaply; corrections are part of the DoD |

## 8. Out of scope

DI scanning helpers, async validators, localisation, `IConfiguration`-level validation without `IOptions`, validation of `IOptionsMonitor` change tokens beyond what `IValidateOptions` already provides.

<!-- footer
last_verified: 2026-08-26
-->

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · **02 Options** · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · [06 Analyzers](new-surfaces-missing-validation-cases-06-analyzers.md)
<!-- /plan-nav -->
