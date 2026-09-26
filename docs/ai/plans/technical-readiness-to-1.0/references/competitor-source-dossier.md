# PineGuard competitor source dossier

Checked: 2026-09-26 (web access; date provided by task environment)
Scope: source research only. This is a companion evidence register for use in modular planning and the living competitive analysis. No PineGuard code was read by this researcher, edited, built, tested, or scanned. PineGuard context was limited to the requested AI Brain specs/plans and README/rubric.

## Selection frame and confidence

This is a purpose-selected comparison set, not an objective “top ten by popularity” ranking. Selection criteria: (1) recognizable .NET validation API relevant to one or more PineGuard surfaces (object/rule validation, guards, attributes, strong value-object validation); (2) at least one direct architectural analogue; (3) Microsoft platform defaults that shape .NET consumer decisions; and (4) Zod as the explicitly requested cross-language schema/type-inference reference. The set is: FluentValidation, System.ComponentModel.DataAnnotations, Ardalis.GuardClauses, Dawn.Guard, CommunityToolkit.Diagnostics Guard, Validot, Valit, ExpressiveAnnotations, Vogen, and Zod. It is intentionally unranked. Repository stars, NuGet counts, and benchmark figures are not used as ranking evidence.

Evidence labels: **Documented** means directly stated in an official project/runtime source; **Inference** is a bounded comparison derived from those statements; **Unknown** means not established from sources reviewed. Checked date applies to every entry. “Not found in reviewed sources” is not a claim of absence. Security limits below are only those expressly stated or apparent at the API boundary; no security audit was performed.

## Competitor summaries and factual evidence

### FluentValidation (.NET; direct object/rule validator)

- **Documented**: Rule-builder validator classes, property rules, built-in validators and custom predicates are the core model. Official docs cover built-in validator catalog, conditions, child/collection composition, error customization, DI registration, async rules (`MustAsync`, `CustomAsync`, `WhenAsync`) and manual ASP.NET invocation. See [FV built-in validators](https://docs.fluentvalidation.net/en/latest/built-in-validators.html), [conditions](https://docs.fluentvalidation.net/en/latest/conditions.html), [collections](https://docs.fluentvalidation.net/en/latest/collections.html), [custom validators](https://docs.fluentvalidation.net/en/latest/custom-validators.html), [DI](https://docs.fluentvalidation.net/en/latest/di.html), [async validation](https://docs.fluentvalidation.net/en/latest/async.html), [ASP.NET integration](https://docs.fluentvalidation.net/en/latest/aspnet.html).
- **Documented**: Async-containing validators must be invoked with `ValidateAsync`; invoking `Validate` throws. Built-in ASP.NET model-binding auto-validation is synchronous and not recommended for new projects; it is MVC-only, while async rules require manual async invocation. Docs describe action-filter/third-party auto-validation paths. See [async validation](https://docs.fluentvalidation.net/en/latest/async.html) and [ASP.NET integration](https://docs.fluentvalidation.net/en/latest/aspnet.html). This narrows the existing PineGuard plan’s claim about the framework integration.
- **Documented**: `FluentValidation.AspNetCore` repo README says the package remains available for legacy implementations and is no longer recommended for new projects because auto-validation is sync/MVC-only; package license stated as Apache-2.0. Main project license file is Apache-2.0. [Integration README](https://github.com/FluentValidation/FluentValidation.AspNetCore), [license](https://github.com/FluentValidation/FluentValidation/blob/main/License.txt).
- **Version/activity**: release page search returned no directly verified current latest release, so version and recent activity are **Unknown in this pass**. Do not cite any inferred “846M downloads” figure from the living plan without dated NuGet evidence.
- **Semantics/composition**: documented sync+async rule composition, conditional execution, property/nested/collection rules, custom validators, DI; null behavior is rule-specific and should be cited at the specific rule (e.g., `NotNull` vs optional validators), not generalized. Structured errors expose property name, error code/message and attempted value per result model (confirm against official API source before fine-grained claim).
- **Deployment/performance/security**: no specific NativeAOT/trimming guarantee was found in reviewed primary docs (**Unknown**); no benchmark result independently checked. Validator checks are application input validation; docs do not claim it sanitizes values or replaces domain/database authorization/security controls. **Strength** relative PineGuard goals: mature model composition and familiar rule DSL. **Tradeoff**: framework-specific rule model, no single Core→Must→Guard→Fluent→Annotations engine evident from docs; official async MVC integration boundary is a consumer concern. These are comparison inferences, not claims of missing features beyond reviewed sources.

### System.ComponentModel.DataAnnotations (.NET runtime; built-in attributes and APIs)

- **Documented**: Microsoft Learn lists DataAnnotations APIs/attributes and `ValidationAttribute.Validate` throws `ValidationException` when invalid; validation is an attribute/API contract. [Namespace API](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-10.0), [`ValidationAttribute.Validate`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute.validate?view=net-10.0).
- **Documented**: .NET runtime repository/package code uses MIT licensing (runtime source license model); exact package component terms should be checked for the target runtime distribution. [dotnet/core license information](https://github.com/dotnet/core/blob/main/license-information.md).
- **Current platform integration**: the PineGuard Brain’s competitive note mentions .NET 10 `Microsoft.Extensions.Validation`, `[ValidatableType]`, and generated validation; I have not yet completed a direct source verification for all those specifics in this draft. Treat those details as pending citation, not fact. (Microsoft Learn URL attempted but not fetchable in current search.)
- **Composition/null**: `ValidationAttribute` validates the annotated member; object validation, recursive behavior, null-skipping and requiredness depend on the particular validator/framework caller. Do not conflate a format attribute’s treatment of `null` with `[Required]`. Validation context supports application services for custom attributes; confirm source-specific details before asserting service injection semantics.
- **Deployment/performance/security**: Native runtime implementation; no blanket trimming/AOT claim checked for arbitrary custom attributes. No benchmark evidence collected. Validation reports do not establish SQL/XSS/SSRF safety.
- **Comparison inference**: strong baseline interoperability because it is a built-in ecosystem surface; PineGuard’s breadth/shared semantics can complement it through custom attributes. DataAnnotations’ attribute model is less naturally compositional for arbitrary multi-rule/cross-property asynchronous validation than dedicated object-validator DSLs, but do not present that as a blanket inability without scenario-specific evidence.

### Ardalis.GuardClauses (.NET; direct guard)

- **Documented**: official README describes a simple extensible package of guard-clause extensions for fail-fast argument checks. Typical usage is `Guard.Against.Null(...)`; extensions are added to `IGuardClause`. MIT license. [Repository README](https://github.com/ardalis/GuardClauses).
- **Documented release evidence**: official releases page showed v5.0 (release page surfaced Sep. 30; page is crawled state and year not shown in the snippet), with minimal changes after v4.6; release notes mention breaking changes and custom exception support. Exact access date 2026-09-26; date/year ambiguity must be resolved from tag metadata before reporting a calendar year. [Releases](https://github.com/ardalis/GuardClauses/releases).
- **Model/semantics**: synchronous throw-on-failure, guard methods return checked values in many cases (README examples assign return); extensible via extension methods. Core designed for argument preconditions rather than aggregate model validation. Null semantics are guard-specific. No official async rule/validator composition model was identified in reviewed README.
- **Integrations/deployment/performance/testing/security**: README documents NuGet distribution and extension guidance; no AOT/trimming guarantee or benchmark was verified. CI/tests details not evaluated. Exact target frameworks/version evidence remains **Unknown** in this pass. Guards are precondition checks and do not validate untrusted input safely by themselves.
- **Comparison inference**: good small-surface guard ergonomics/extensibility; PineGuard may offer broad shared predicates and configurable exception policy across more validation styles. Coverage counts and relative popularity are not established here.

### Dawn.Guard (formerly Guard; .NET; direct fluent argument guard)

- **Documented**: official repository describes fluent argument validation API (`Guard.Argument(value, name).NotNull().NotEmpty()`), read-only structs/extension pattern and standard/custom validations; NuGet package name `Dawn.Guard`. [Repository README](https://github.com/safakgur/guard).
- **Lifecycle**: official Releases page currently exposes v1.12.0; the snippets available so far show 2020 release history; the README explains author considered a v2 compile-time/source-generator path but “never got the chance.” Exact latest release date needs confirmation from page lines before final; do not call archived unless GitHub metadata says archived. [Releases](https://github.com/safakgur/guard/releases).
- **Model/semantics**: fluent guard argument wrappers; README says read-only structs are passed by reference to address allocations, and shows exceptions on failed predicates. Method chaining appears the primary composition form. Null/empty conventions are API-specific. No object-validation/async model established in reviewed sources.
- **Deployment/integration/testing/security**: target framework support, trimming/AOT, security guarantees, test suite evidence and exact license still need direct confirmation. There is no source-gen implementation evidence in reviewed README; it records an unimplemented experiment.
- **Comparison inference**: offers chainable precondition ergonomics; breadth and interoperability with other surfaces are not demonstrated by sources reviewed. Avoid saying “small ecosystem” absent evidence.

### CommunityToolkit.Diagnostics `Guard` (.NET; Microsoft-maintained helper API)

- **Documented**: CommunityToolkit describes `CommunityToolkit.Diagnostics` as helpers, specifically `Guard` and `ThrowHelper`, for argument validation/error checking. Microsoft Learn says guard APIs throw detailed exceptions and are designed to minimize caller code and inline; primitive overloads are generated with T4 templates. [Toolkit repository](https://github.com/CommunityToolkit/dotnet), [Guard class docs](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/diagnostics/guard), [IsNotNull API](https://learn.microsoft.com/en-us/dotnet/api/communitytoolkit.diagnostics.guard.isnotnull?view=dotnet-comm-toolkit-8.4).
- **Documented**: `Guard.IsNotNull` throws `ArgumentNullException`; API includes generic overloads for class and nullable value types, and docs say generic form avoids boxing. Toolkit is maintained/published by Microsoft and part of .NET Foundation per project README; code source identifies MIT license. [Toolkit repo](https://github.com/CommunityToolkit/dotnet), [source file/license](https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Diagnostics/Extensions/TypeExtensions.cs).
- **Model/integrations**: static guard API, primarily method argument preconditions; signatures generally return void, unlike value-returning guards. No model validator/attributes/FluentValidation integration evidenced by reviewed docs. Version surfaced as 8.4.0 API docs. Target support beyond docs, analyzer/AOT/trimming guarantees, security scope and full test evidence remain **Unknown**.
- **Comparison inference**: credible benchmark for low-overhead exception guards and primitive/value-type overloads; may serve common null/range preconditions. PineGuard’s broader multi-surface goal is different; do not claim faster/slower absent equivalent measurements.

### Validot (.NET; specification/object validation; archived)

- **Documented**: official README describes a declarative, fluent, performance-first validator supporting classes, structs, nested members, collections, nullable values, combinations/relations, translations, custom logic and DI. README says .NET Standard 2.0 and no extra dependencies; MIT license; repository itself states near-100% unit-test coverage and that BenchmarkDotNet/performance tests exist. These are project-reported, not independently verified by this review. [README](https://github.com/bartoszlenar/Validot).
- **Lifecycle evidence**: official Releases page explicitly says owner archived repository on 2025-05-26; latest displayed release v2.6.0 dated 2025-02-04. [Releases](https://github.com/bartoszlenar/Validot/releases). This corrects the date granularity in the PineGuard living plan’s “Archived May 2025” wording.
- **Semantics/composition**: docs/README support declaration/specification model, nested and collection paths, nullables, relationships, custom rules, translation and DI. Exact evaluation/fail-fast/aggregate result semantics, error path/code separation, and thread-safety claims need citations to `DOCUMENTATION.md` before detailed assertion; not established in this draft.
- **Deployment/security**: target .NET Standard 2.0 is documented, but no explicit NativeAOT/trimming/source-generation contract checked. Benchmarks exist according to README, but numeric results must not be repeated without exact workload/methodology and source. Validation remains distinct from security sanitization.
- **Comparison inference**: valuable source of composition/localization/performance hypotheses; archived status makes it a historical design comparator rather than an actively maintained dependency candidate. It directly challenges “nobody else provides layered/full-object composition” and should be incorporated as an adjacent architecture example.

### Valit (.NET; fluent object validation; lifecycle evidence stale/uncertain)

- **Documented**: repository README says NuGet version 2.0.0 and fluent property selection/rules, configurable strategies, custom errors/error codes, tags/conditions, and `Validate()` returning a result. README says MIT. [Repository README](https://github.com/valit-stack/Valit); [NuGet 2.0.0](https://www.nuget.org/packages/Valit/2.0.0).
- **Current state**: official GitHub page is live, shows 316 commits and repo not explicitly labeled archived; current README still prescribes 2.0.0, NuGet page lists computed modern frameworks, but this does not prove target package compatibility. Exact latest push/release date and current maintenance status are **Unknown**; avoid declaring dead/abandoned based only on old README wording. [Repository](https://github.com/valit-stack/Valit), [NuGet](https://www.nuget.org/packages/Valit).
- **Semantics/composition**: fluent property rules, rules can be tagged/conditioned and validation strategy chosen; all-failure behavior and exact error code/message/path semantics not fully reviewed. Null, localization, async, DI and nested/collection composition need direct docs verification before claims.
- **Deployment/security**: repository MIT; package compatibility/current TFM, AOT/trimming and performance evidence **Unknown**. No security guarantees.
- **Comparison inference**: a closer object-validator analogue than a guard library; potentially useful property-rule/error customization reference, but its present release/support posture is unresolved.

### ExpressiveAnnotations (.NET + JavaScript; conditional DataAnnotations)

- **Documented**: official README describes annotation-based conditional validation using string expressions. It parses expression text, builds a compiled delegate, caches it on attribute instances for repeated evaluations, and exposes MVC unobtrusive client validation that evaluates the expression string in JavaScript using `eval()`. The README explicitly warns server-side C# and JavaScript null semantics differ, giving examples. MIT license. [Repository README](https://github.com/jwaliszko/ExpressiveAnnotations).
- **Composition/integration**: attributes can express conditional requirements/assertions; core and MVC unobtrusive packages/scripts are documented; README says compile expressive attributes collectively to detect expression syntax/type errors. This is a focused adapter, not a general-purpose rule DSL.
- **Lifecycle/security**: exact active version/release/activity, supported .NET/ASP.NET versions, trimming/AOT, test evidence and security review are **Unknown** in this pass. The documented client `eval()` behavior plus server/client semantic mismatch is a concrete trust/consistency boundary; it is not proof of exploitable injection, and expressions should not be treated as trusted arbitrary client input. No other security claims.
- **Comparison inference**: useful point of comparison for cross-property conditions and attribute-driven UX, but runtime expression parsing/evaluation and server/client consistency need explicit consumer decisions. It also disproves a blanket “nobody supports conditional composition” statement.

### Vogen (.NET; value-object source generator/analyzer; adjacent)

- **Documented**: Vogen generates primitive-backed domain value objects plus analyzer diagnostics that block uninitialized construction patterns; it supports custom validation via generated factory and configurable validation exception; default conversion docs include TypeConverter and System.Text.Json, with optional Dapper/EF Core/etc. [README](https://github.com/SteveDunn/Vogen), [wiki](https://stevedunn.github.io/Vogen/vogen.html).
- **Validation semantics**: invalid factory input returns/throws according to Vogen validation contract (README says invalid input becomes `ValueObjectValidationException`; validation APIs return a `Validation` result); normalization hook is supported. Analyzer cannot prevent every runtime defaulting/reflection/serialization path by assertion alone; README itself mentions default handling and generated analyzers. Treat it as invariant/type construction, not general input validator.
- **Build/deployment**: package is a .NET source generator/analyzer, generator targets .NET Standard 2.0 per README and generated code supports C# 6+; current target/release version not checked yet. Source generation means runtime reflection is not required for value-object generation, but that is **not equivalent to an explicit NativeAOT/trimming guarantee**. Tests/benchmarks are documented in repo; README’s historical numeric benchmark uses old hardware/runtime and must not be presented as current evidence. MIT license needs direct citation check before final.
- **Comparison inference**: relevant to PineGuard’s “parsed/value output” and domain invariants; can make valid values representable in types, while broad field/request validation and errors remain different responsibilities. Stronger differentiation than a direct competitor label.

### Zod (TypeScript; conceptual schema/type-inference comparator)

- **Documented**: Zod describes schema-first runtime validation with inferred static TypeScript types; `.parse` validates and returns parsed/cloned data, `.safeParse` returns discriminated success/error result; async refinements/transforms require async parse; transforms and coercion can change input/output types. API also supports JSON Schema conversion and immutable schema methods. [Repository README](https://github.com/colinhacks/zod), [docs](https://zod.dev/).
- **Version/activity**: official releases page displayed v4.6.5 as latest when checked 2026-09-26; page release entry dated Sep. 13 but year not visible in snippet; retrieve exact release date/year before final. [Releases](https://github.com/colinhacks/zod/releases).
- **License**: needs direct LICENSE file confirmation before final; README license badge/result not yet checked. Integration ecosystem/AOT is not comparable to .NET deployment target and should not be mapped as a simple feature parity score.
- **Security/performance**: GitHub README reports compiled-schema benchmark figures; these are project claims with a particular 55-schema set and should only be quoted with exact original methodology (not comparable to PineGuard); README release notes also describe memory-retention work. No security validation/sanitization guarantee; parsing validates/transforms structures.
- **Comparison inference**: strongest cross-language reference for schema/type coupling, immutable composition and parse result ergonomics, but PineGuard’s multi-adapter/reusable rule engine is an architectural comparison rather than a feature-for-feature replacement.

## Interim synthesis (facts separated from planning suggestions)

**Documented landscape facts**: direct analogues exist in multiple PineGuard niches: FluentValidation for model/rule composition and async, DataAnnotations for attributes/platform interoperability, several dedicated guard libraries, Validot/Valit for specification/property validation, ExpressiveAnnotations for conditional attributes, Vogen for invariants at type construction, and Zod for inferred schema parse/transform. The selected set is intentionally not ordered by adoption.

**Planning implications (inference, not external fact)**: avoid absolute positioning such as “every competitor does one layer” or “no other library does this” without a reproducible competitor set and search protocol. Prefer the defensible differentiation hypothesis: one core rule contract is intended to project through several .NET call-site styles; verify this in PineGuard’s source and public package behavior separately. Use the reference designs to inform tradeoff review, not as evidence PineGuard works better. Keep inactive sources like Validot as historical patterns, not adoption alternatives. The product engineering rubric explicitly separates market/adoption from technical readiness.

**Coverage still needed before final**: direct exact release/activity and target support for remaining projects; Valit docs/null/result behavior; Dawn license/current date; FluentValidation current release/AOT/testing claims; System.Extensions.Validation source, DataAnnotations semantics; exact Zod license and more narrow docs; Vogen license/release/AOT; build direct reference register and section titles. No current official benchmark has been independently rerun.

## Reference register

| ID | Project/source | Direct URL | Sections used / intended | Checked | Confidence |
|---|---|---|---|---|---|
| FV-01 | FluentValidation built-in validators | https://docs.fluentvalidation.net/en/latest/built-in-validators.html | Built-in Validators | 2026-09-26 | High for documented features |
| FV-02 | FluentValidation conditions | https://docs.fluentvalidation.net/en/latest/conditions.html | Conditions | 2026-09-26 | High |
| FV-03 | FluentValidation collections | https://docs.fluentvalidation.net/en/latest/collections.html | RuleForEach/collections | 2026-09-26 | High |
| FV-04 | FluentValidation custom validators | https://docs.fluentvalidation.net/en/latest/custom-validators.html | Custom validators | 2026-09-26 | High |
| FV-05 | FluentValidation DI | https://docs.fluentvalidation.net/en/latest/di.html | DI/extensions | 2026-09-26 | High |
| FV-06 | FluentValidation async | https://docs.fluentvalidation.net/en/latest/async.html | MustAsync/ValidateAsync constraints | 2026-09-26 | High |
| FV-07 | FluentValidation ASP.NET docs | https://docs.fluentvalidation.net/en/latest/aspnet.html | ASP.NET MVC/manual/action filter guidance | 2026-09-26 | High |
| FV-08 | FV ASP.NET Core repo | https://github.com/FluentValidation/FluentValidation.AspNetCore | README, supported platform, auto validation, license | 2026-09-26 | High |
| FV-09 | FV main license | https://github.com/FluentValidation/FluentValidation/blob/main/License.txt | Apache-2.0 | 2026-09-26 | High |
| DA-01 | DataAnnotations API namespace | https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-10.0 | API/type index | 2026-09-26 | High |
| DA-02 | ValidationAttribute.Validate | https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute.validate?view=net-10.0 | exceptions, overloads | 2026-09-26 | High |
| DA-03 | .NET license information | https://github.com/dotnet/core/blob/main/license-information.md | package/source licenses | 2026-09-26 | Medium (runtime distribution distinctions) |
| AG-01 | Ardalis GuardClauses repo | https://github.com/ardalis/GuardClauses | README API/extension/license | 2026-09-26 | High |
| AG-02 | Ardalis releases | https://github.com/ardalis/GuardClauses/releases | release versions/notes (date year unresolved) | 2026-09-26 | Medium |
| DG-01 | Dawn.Guard repo | https://github.com/safakgur/guard | README model/design notes | 2026-09-26 | High |
| DG-02 | Dawn.Guard releases | https://github.com/safakgur/guard/releases | versions/history | 2026-09-26 | Medium (release date still verify) |
| CT-01 | CommunityToolkit repo | https://github.com/CommunityToolkit/dotnet | diagnostics package/lifecycle/license | 2026-09-26 | High |
| CT-02 | Guard class docs | https://learn.microsoft.com/en-us/dotnet/communitytoolkit/diagnostics/guard | design goals/API pattern | 2026-09-26 | High |
| CT-03 | IsNotNull docs | https://learn.microsoft.com/en-us/dotnet/api/communitytoolkit.diagnostics.guard.isnotnull?view=dotnet-comm-toolkit-8.4 | null exceptions/value-type boxing | 2026-09-26 | High |
| VO-01 | Validot repo | https://github.com/bartoszlenar/Validot | README scope, framework, claims, license | 2026-09-26 | High for owner claims |
| VO-02 | Validot releases | https://github.com/bartoszlenar/Validot/releases | archive notice, latest release | 2026-09-26 | High |
| VA-01 | Valit repo | https://github.com/valit-stack/Valit | README version/API/license/status | 2026-09-26 | High for README; maintenance only medium |
| VA-02 | Valit NuGet page | https://www.nuget.org/packages/Valit | published version/framework listing | 2026-09-26 | Medium pending version interpretation |
| EA-01 | ExpressiveAnnotations repo | https://github.com/jwaliszko/ExpressiveAnnotations | README behavior, cache, client-side eval/null mismatch/license | 2026-09-26 | High |
| VG-01 | Vogen repo | https://github.com/SteveDunn/Vogen | README source gen, validation, conversions, perf claims | 2026-09-26 | High for documented claims |
| VG-02 | Vogen wiki | https://stevedunn.github.io/Vogen/vogen.html | detailed generation/integration docs | 2026-09-26 | Pending content review |
| VG-03 | Vogen releases | https://github.com/SteveDunn/Vogen/releases | release evidence | 2026-09-26 | Pending content review |
| Z-01 | Zod repo | https://github.com/colinhacks/zod | README parse, safeParse, async, transforms, claimed benchmark | 2026-09-26 | High for project claims |
| Z-02 | Zod docs | https://zod.dev/ | API details | 2026-09-26 | Pending section review |
| Z-03 | Zod releases | https://github.com/colinhacks/zod/releases | latest version/date | 2026-09-26 | Medium (year still confirm) |
| SNAP-01 | User-provided PineGuard snapshot | https://chatgpt.com/s/cx_6ab7452cf0548191b522d3b8f4b380c4 | Public snapshot inaccessible; open returned cache miss | 2026-09-26 | High for access result only |

## Batch 2 evidence corrections and added references (checked 2026-09-26)

These sources resolve open notes above and supersede any conflicting “coverage still needed” wording. The sections above remain intentionally conservative on claims whose direct documentation has not been reviewed.

- **Dawn.Guard lifecycle is now confirmed:** repository archive notice says archived 2025-12-10, read-only; Releases lists v1.12.0 as latest dated 2020-03-31. README states MIT. Its README’s later “v2” discussion expressly says the author considered source generation/IL weaving, but did not implement it. Refs DG-01 and DG-02 now support archive, license and exact latest-release date.
- **DataAnnotations / modern .NET validation details now confirmed:** Microsoft’s ASP.NET Core validation overview (10.0) says `Microsoft.Extensions.Validation` uses Roslyn generation; Minimal API model types are discovered from endpoint signatures, generated metadata is assembly-local, and if it is missing Minimal APIs perform no automatic validation. In Blazor the fallback path is top-level only and does not support nested/collection validation. A separate .NET 10 release note describes DataAnnotations/IValidatableObject support, `AddValidation`, endpoint filter, HTTP 400 errors, and the validation API package/namespace move. This is a specific platform path, not a universal DataAnnotations AOT claim. `Microsoft.Extensions.Options` validation source generator docs explicitly say generated validators avoid reflection and are AOT-compatible; scope is options validation. Refs DA-04–06 below.
- **FluentValidation null/error/short-circuit details:** built-in `NotNull` is null-only; `NotEmpty` also treats empty/whitespace strings, default value types and empty enumerables as empty. `Validate` returns `ValidationResult` with `Errors`; `ValidateAndThrow` is available. Error code is a separate `ValidationFailure.ErrorCode`, with default validator code or explicit `WithErrorCode`; cascade mode defaults to Continue, with Stop options. Nested child validator is skipped when the child property is null, but inline property chains do not automatically null-check parents. Refs FV-10–14 below.
- **Zod package license/version is verified:** `packages/zod/package.json` declares version `4.6.5` and MIT. See Z-04. Exact latest release year/date still not established by snippet; do not state it.
- **Do not compare reported speed figures:** README/release figures remain project-reported with different corpus/runtime/hardware. No cross-project benchmark was created for this dossier.

### Added reference register rows

| ID | Project/source | Direct URL | Sections used / intended | Checked | Confidence |
|---|---|---|---|---|---|
| FV-10 | FluentValidation error codes | https://docs.fluentvalidation.net/en/latest/error-codes.html | WithErrorCode and ValidationFailure.ErrorCode | 2026-09-26 | High |
| FV-11 | FluentValidation cascade | https://docs.fluentvalidation.net/en/latest/cascade.html | Continue/Stop behavior/defaults | 2026-09-26 | High |
| FV-12 | FluentValidation getting started | https://docs.fluentvalidation.net/en/latest/start.html | ValidationResult/Errors/ValidateAndThrow/nested child null behavior | 2026-09-26 | High |
| FV-13 | FluentValidation configuring | https://docs.fluentvalidation.net/en/latest/configuring.html | message and property-name customization | 2026-09-26 | High |
| FV-14 | FluentValidation built-in validators | https://docs.fluentvalidation.net/en/latest/built-in-validators.html | precise NotNull/NotEmpty semantics | 2026-09-26 | High |
| DA-04 | ASP.NET Core validation overview | https://learn.microsoft.com/en-us/aspnet/core/validation/overview?view=aspnetcore-10.0 | Register service, generated metadata, supported paths, fallback behavior, assembly scoping | 2026-09-26 | High |
| DA-05 | ASP.NET Core 10 release notes | https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0?view=aspnetcore-10.0 | Minimal API validation and package/namespace move | 2026-09-26 | High |
| DA-06 | Options validation source generator | https://learn.microsoft.com/en-us/dotnet/core/extensions/options-validation-generator | reflection-free AOT-compatible options validation generator | 2026-09-26 | High |
| DG-03 | Dawn.Guard repository archive notice | https://github.com/safakgur/guard | archive date/read-only, MIT metadata, README/source-gen v2 not implemented | 2026-09-26 | High |
| DG-04 | Dawn.Guard v1.12 release history | https://github.com/safakgur/guard/releases | v1.12.0 latest, dated Mar 31, 2020 | 2026-09-26 | High |
| Z-04 | Zod package manifest | https://github.com/colinhacks/zod/blob/main/packages/zod/package.json | version 4.6.5, MIT | 2026-09-26 | High |
| Z-05 | Zod contributor/test guide | https://github.com/colinhacks/zod/blob/main/CONTRIBUTING.md | test commands, Vitest/type/build checks; not test results | 2026-09-26 | High |
| VG-04 | Vogen repository license | https://github.com/SteveDunn/Vogen/blob/main/LICENSE | declared repository license; file content not separately reviewed this pass | 2026-09-26 | Medium |

## Release and activity snapshot (supersedes earlier unknowns where marked)

Checked 2026-09-26. GitHub release pages sometimes omit the year in the rendered snippet; version claims below are safer than inferring year from month/day alone.

- **FluentValidation:** official GitHub releases identifies v12.1.0 as latest. v12 release notes set minimum supported platform to .NET 8 and list removal of deprecated transforms/DI APIs; date appears “03 Nov” but year is not visible in extracted page. [Releases](https://github.com/FluentValidation/FluentValidation/releases). This supersedes “version/recent activity unknown.”
- **Ardalis.GuardClauses:** v5.0 is Latest; release snippet date “30 Sep” with year not visible. Don't state exact year from the snippet. [Releases](https://github.com/ardalis/GuardClauses/releases).
- **CommunityToolkit:** repository releases lists v8.4.2 latest; date shows “25 Mar” with year not visible in extracted page. The Guard docs cited above show 8.4.0 API surface. [Releases](https://github.com/CommunityToolkit/dotnet/releases).
- **Dawn.Guard:** repository marked Public archive, archived 2025-12-10. v1.12.0 Latest, exact date 2020-03-31. MIT. [Repo](https://github.com/safakgur/guard), [releases](https://github.com/safakgur/guard/releases).
- **Validot:** repository marked Public archive, archived 2025-05-26. v2.6.0 latest release dated 2025-02-04. [Releases](https://github.com/bartoszlenar/Validot/releases).
- **Valit:** repository README prescribes 2.0.0; NuGet page presents Valit 2.0.0. Do not infer support of computed TFMs or ongoing maintenance from that data. [Repo](https://github.com/valit-stack/Valit), [NuGet](https://www.nuget.org/packages/Valit/2.0.0).
- **ExpressiveAnnotations:** v2.9.6 Latest; date appears “20 Sep” without year in extracted page. It packages distinct core/MVC-unobtrusive/JS component versions. [Releases](https://github.com/jwaliszko/ExpressiveAnnotations/releases).
- **Vogen:** releases page lists v9.0.0-beta.2 as newest prerelease; page shows v8.0.6 with a Latest badge, while also rendering an 8.0.7 hotfix section. For consumer version pinning verify NuGet/package release before making a current stable version claim. [Releases](https://github.com/SteveDunn/Vogen/releases).
- **Zod:** repo package manifest states v4.6.5 and MIT. Releases page displays 4.6.5 latest, but extracted date year not visible. [manifest](https://github.com/colinhacks/zod/blob/main/packages/zod/package.json), [releases](https://github.com/colinhacks/zod/releases).

### Added release-source register rows

| ID | Project/source | Direct URL | Sections used / intended | Checked | Confidence |
|---|---|---|---|---|---|
| FV-15 | FluentValidation releases | https://github.com/FluentValidation/FluentValidation/releases | latest version, v12 support floor/changelog | 2026-09-26 | High for displayed version; year unresolved |
| AG-03 | Ardalis releases | https://github.com/ardalis/GuardClauses/releases | current displayed v5.0 | 2026-09-26 | High for version; year unresolved |
| CT-04 | CommunityToolkit releases | https://github.com/CommunityToolkit/dotnet/releases | current displayed v8.4.2 | 2026-09-26 | High for version; year unresolved |
| EA-02 | ExpressiveAnnotations releases | https://github.com/jwaliszko/ExpressiveAnnotations/releases | current v2.9.6 and component versions | 2026-09-26 | High for version; year unresolved |
