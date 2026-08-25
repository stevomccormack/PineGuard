---
spec:
  id: pineguard.ai.spec
  title: "PineGuard AI Root Spec (Cascading)"
  version: 1
  dependencies:
    - dependencies.md
applies_to:
  - "docs/ai/specs/**"
---

# PineGuard AI Root Spec (Cascading)

This is the **root specification** for every AI generation spec under `docs/ai/**` — the
production code generation specs and the testing + coverage specs alike.

Read this first. Child specs assume these defaults and only describe what’s _different_ for their domain.

Also required (read these at the start of any PineGuard task - non-negotiable to avoid context loss or summarisation over time - read all specs - its not long):

- `docs/ai/specs/dependencies.md` (what depends on what)
- `docs/ai/specs/orchestration.md` (process/logging)
- `docs/ai/specs/safety.md` (destructive operations safety — three-tier classification)

---

## Fast start (what an agent should do)

When asked to implement or modify PineGuard code using the AI specs:

1. Read the required AI docs:

- `docs/ai/specs/spec.md` (this file)
- `docs/ai/specs/orchestration.md` (process/logging)
- `docs/ai/specs/dependencies.md` (what depends on what)
- `docs/ai/specs/safety.md` (destructive operations safety)

2. Identify the relevant child spec(s) under `docs/ai/specs/**`.
3. Read the child spec(s) you will apply.
4. Follow the “cascading” precedence rules (below) to resolve conflicts.
5. Make the smallest correct change that satisfies the spec.
6. Verify via build/tests appropriate for the scope.

Goal: ship correct, deterministic changes with reviewable diffs.

---

## 1) Cascading model (“CSS for specs”)

Treat `docs/ai/**` like a cascading stylesheet:

- **Parent specs apply to all descendants.**
- **Child specs may narrow/override parent rules** for their specific domain.
- When rules conflict, the **most specific** applicable rule wins.

This is documentation-driven (not enforced by tooling). The point is predictability.

### 1.1 Precedence order (practical)

When deciding what instructions apply, use this order:

1. The most specific leaf spec you are executing.
2. Its parent folder/domain specs.
3. `docs/ai/specs/spec.md` (this file).
4. `docs/ai/specs/orchestration.md` (process/logging).
5. `docs/ai/specs/dependencies.md` (dependency map).
6. Repo-level agent instructions (`AGENTS.md`, `.github/copilot-instructions.md`, etc.) unless contradicted by a more specific AI spec.

### 1.2 Required: YAML “Spec Header” in child specs

Every spec under `docs/ai/specs/**` must begin with YAML front matter that links back to the root spec.

Minimum required header:

```yaml
---
spec:
  id: pineguard.ai.<domain>.<name>
  title: "<Human readable title>"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../../spec.md
  dependencies:
    - ../../dependencies.md
---
```

Notes:

- `parent` is a list to support multiple inheritance if ever needed.
- Child specs should add `applies_to` so it’s obvious what code the spec governs.
- `spec.template` is required for the **per-project spec families** (`<domain>/project.md`,
  `<domain>/unit-test.md`, `<domain>/coverage.md`) and should point at the matching template:
  - `docs/ai/meta/template-project.md`
  - `docs/ai/meta/template-unit-test.md`
  - `docs/ai/meta/template-coverage.md`

  Root and process specs (this file, `protocol.md`, `orchestration.md`, `dependencies.md`,
  `safety.md`, `coding-standard.md`, `council.md`, and the `language/`, `scan/`, `tools/`
  specs) have no template and omit the field.
- All header paths are relative to the spec's own location, so the depth varies: `../meta/…` and `parent: spec.md` for a spec directly under `docs/ai/specs/`, `../../meta/…` and `parent: ../spec.md` one directory deeper, and so on.

Recommended extended header:

```yaml
---
spec:
  id: pineguard.ai.<domain>.<name>
  title: "<Human readable title>"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/<Project>/**"
  - "tests/<Project>.UnitTests/**"
---
```

---

## 2) What “AI generation specs” are

In PineGuard, AI generation specs are **source-of-truth engineering documents**.

They exist to make AI-assisted changes:

- consistent across sessions,
- deterministic and reviewable,
- aligned with the library’s intended public API language.

Specs define (non-exhaustive):

- the _public API language_ (names, symmetry, complements),
- layering rules (Rules/Utils ↔ MustClauses ↔ GuardClauses ↔ integrations),
- code structure and location rules,
- determinism and correctness rules,
- testing and coverage rules.

Shared naming vocabulary (required):

- Use `docs/ai/specs/language/vocabulary.md` (human guidance) and `docs/ai/specs/language/vocabulary.json` (tooling map) to resolve questions like `Invalid*` vs `Not*` vs `Non*`, and opposite-term handling.

Out of scope:

- project management commentary,
- speculative behavior presented as fact,
- instructions that cannot be executed deterministically.

---

## 3) Feature Implementation Checklist (Master)

When adding a new feature (e.g., `JsonRules`), follow this **strict order of operations**. This ensures the layering (Utils -> Rules -> Must -> Guard -> Integrations) is respected.

1.  **Core Utils (`src/PineGuard.Core/Utils/**`)\*\*
    - Add/extend `Try*` parsing & normalization helpers.
    - _No throwing, pure logic._

2.  **Core Rules (`src/PineGuard.Core/Rules/**`)\*\*
    - Add pure predicates (`Is*`, `Has*`) that call Utils.
    - _No throwing, no messages._

3.  **MustClauses (`src/PineGuard.MustClauses/**`)\*\*
    - Add fluent API `Must.Be.Xxx`.
    - Call Core Rules/Utils.
    - **Own the canonical user-facing message.**
    - Return `MustResult<T>`.

4.  **GuardClauses (`src/PineGuard.GuardClauses/**`)\*\*
    - Add fluent API `Guard.Against.Xxx`.
    - **Call the corresponding MustClause.**
    - Throw on failure using `MustResult.Message`.

5.  **FluentValidation (`src/PineGuard.FluentValidation/**`)\*\*
    - Add the `IRuleBuilder` extension method.
    - Adapt the MustClause (reuse message & logic).

6.  **DataAnnotations (`src/PineGuard.DataAnnotations/**`)\*\*
    - Add the `ValidationAttribute` adapting the same MustClause.
    - Reuse the Must message; never invent new message text.

---

## 4) Defaults: technology and tooling

Unless a child spec explicitly states otherwise:

- Language: C#
- Runtime: .NET
- Test framework: xUnit
- Primary build system: `dotnet` CLI

Nullability default (required):

- Nullable reference types (NRT) are enabled across PineGuard projects.
- New/modified code must be NRT-correct:
  - Use `string?` / `T?` only when null is actually permitted.
  - Prefer explicit guard checks over suppressions.

Logging / Temporary Files (required):

- **Do not create log files in the repo root.**
- Always use `logs/` or `artifacts/` for any temporary output files.
- If the directory does not exist, create it.

Context Maintenance (required):

- **Rehydrate context continuously:** When summarising progress or starting a new deep-dive, run the "Context refresh" checklist in `docs/ai/specs/orchestration.md` — that file owns the procedure; this bullet only mandates that you do it.
- Do not rely solely on your own generated summaries; refer back to source-of-truth specs.

Verification defaults (choose the tightest checks that prove your change):

- Prefer targeted `dotnet test` runs for the changed project(s).

### 4.1 Strict Coding Standards (Required)

All generated C# code must follow these conventions:

- **Files**:
  - `using` statements always at the very top (before namespace).
  - Sort `using` statements alphabetically (System first if configured, but consistent).
  - Remove unused `using` statements.
  - Use **File-scoped namespaces** (`namespace PineGuard.Rules;`).

- **Structure**:
  - Use **Arrow functions** (`=>`) for implementation when valid (single line expressions).
  - Use **Block bodies** without braces for single-line control flow statements found on a new line:
    ```csharp
    if (value is null)
        return false;
    ```
  - Use `private` access modifiers where possible.
  - Remove redundant variables/arguments (e.g., inline return values if the variable adds no clarity).

- **Naming**:
  - **Parameters**: Validated input parameter must be named `value` (not `input`).
  - **Tests**: Use strict **AAA** (Arrange, Act, Assert) pattern.
  - **Test Data**: Follow the per-layer dataset model in `docs/ai/specs/testing/unit-test.md` §4.1
    (the dataset split differs by layer — do not assume a Valid/Edge/Invalid split).

Analyzer-level rules (ReSharper/SonarQube optimisations, nullability, suppression policy) live in
[`coding-standard.md`](./coding-standard.md) — see §10.

---

## 5) PineGuard invariants (global)

These invariants apply across all specs (unless explicitly overridden by a narrower spec).

### 5.1 Single source of truth for validation/parsing

- `Rules` and `Utils` are the single source of truth for validation/parsing logic.
- `Rules` are pure predicates (no user-facing messages).
- `Utils` contain parsing/normalization helpers via `Try*` methods.

### 5.2 Must/Guard relationship

- MustClauses call `Rules`/`Utils` and own canonical user-facing messages.
- GuardClauses call MustClauses and throw using `MustResult.Message`.

### 5.3 Determinism (no environment-dependent “validation”)

- No IO-dependent validation (filesystem/network/environment) inside Core `Rules`/`Utils`.
- Prefer BCL `Try*` APIs; avoid throwing for invalid inputs in Must/Rules/Utils.

Clarification:

- MustClauses should return failure results for invalid inputs (not throw).
- GuardClauses should throw, and should prefer throwing based on `MustResult.Message`.

### 5.4 Validated value vs configuration/dependency parameters (critical)

Many APIs take both:

- a **validated value** (the thing being checked), and
- one or more **configuration/dependency parameters** (regexes, predicates, providers, option arrays, etc.).

These have different semantics.

#### 5.4.1 Validated value (never throw)

For invalid _validated values_ (null/empty/format/range/etc):

- `Rules` return `false`.
- `Utils.Try*` return `false` and set out params to safe defaults.
- MustClauses return `MustResult.Fail(...)`.
- GuardClauses throw (via MustClauses canonical messages).

Rationale: invalid values are normal input scenarios; the library must be deterministic and non-explosive.

#### 5.4.2 Configuration/dependency parameters (may throw, but Must must not)

For invalid _configuration/dependency parameters_ (e.g., `Regex pattern` is null, `Func<T,bool> predicate` is null, provider interface is null):

- `Rules`/`Utils` **may throw** `ArgumentNullException` (treat as programmer error) OR may return `false` if the API is explicitly designed to accept null.
- MustClauses must **not** allow these throws to bubble:
  - MustClauses must validate configuration/dependency parameters up-front and return `MustResult.Fail(...)` attributing failure to the **failing parameter**, not the validated value.
- - Attribute the failing parameter via `nameof(parameter)` by default (keep APIs lean). Use `[CallerArgumentExpression]` only for the validated `value` parameter.
- GuardClauses should preferably call MustClauses so the same attribution/message is used across Guard/DataAnnotations/FluentValidation.

Example (pattern/predicate/etc):

- If `predicate` is null, the failure’s `ParamName` should be `nameof(predicate)` (and must not be the caller-argument-expression name for `value`).

### 5.5 Integrations (FluentValidation / DataAnnotations) must reuse Must messages (required)

Integrations must reuse MustClauses as the canonical source of messaging and validation semantics:

- Validate by calling `Must.Be.*` (never call `Rules`/`Utils` directly from integrations).
- Use the Must failure message as the integration error message (do not invent new message text).

### 5.6 Parameter naming rule for integrations (required)

- When an integration needs the error message to name a _property/display name_ (not the caller argument expression), call Must with `paramName: null` so the message remains a template (contains `{paramName}`), then replace `{paramName}` with the framework’s display/property name.

Rationale:

- Keeps one canonical message template.
- Lets each integration choose its own “parameter name” source (expression vs property path) without forking message text.

Notes:

- It’s acceptable to use `[CallerArgumentExpression]` for a config/dependency parameter in rare cases, but avoid it by default; it widens signatures and adds API noise.
- The primary UX win is allowing the validated `value` parameter name to reflect the caller expression.

---

## 6) How to communicate (global)

### 6.1 Be direct and engineering-focused

AI output should be concise, factual, and engineering-focused:

- Avoid flattery and “cheerleading”.
- Prefer actionable conclusions and explicit tradeoffs.
- If something is unknown, say so; do not guess.

### 6.2 Don’t invent repo facts

- Do not claim that a file/function exists unless it can be located in the repo.
- When uncertain, search first or ask for clarification.

### 6.3 Optimize for reviewable diffs

- Prefer small, surgical changes.
- Avoid unrelated reformatting.
- Keep public APIs stable unless explicitly asked to change.

### 6.4 Temporary policy: greenfield foundational changes

PineGuard is currently treated as a **greenfield** codebase for foundational work.

Implications (temporary):

- For major, cross-cutting decisions (e.g., solution-wide nullable adoption, reshaping Rule signatures), we may make **breaking API changes** without preserving backward/binary compatibility.
- During this phase, **do not add** `[Obsolete]` attributes purely to maintain compatibility; we have full control of consumers.
- Still aim for a **consistent and coherent** public surface (symmetry, naming, null semantics), but prefer the simplest correct API over compatibility scaffolding.

Note on generics:

- If a Rule is generic over `T` with `where T : struct`, prefer a **single non-nullable signature** (`T`) for the validated value.
- Do not overload with `T?` unless "null" is a distinct valid state for that domain (uncommon for value types in this architecture).
- Callers with `T?` should check `Must.Be.NotNull` first (smart-casting) or use `.Value` / `.GetValueOrDefault()`.
- **Rationale**: see `docs/ai/specs/core/project.md §3.3` CAUTION block for the overload-disambiguation reason this convention exists.

Exit criteria:

- Once the foundational change is implemented and agreed (e.g., nullable policy stabilized), this temporary policy ends.
- From that point forward, treat public APIs as **stable by default** again: avoid breaking changes unless explicitly approved, and use normal deprecation patterns only when/if needed.

---

## 7) Operational best practices (global)

### 7.1 Prefer the repo’s “work order”

When implementing a new validation/parsing feature, default to:

1. `Utils` (`Try*` parsing/normalization)
2. `Rules` (thin positive predicates calling Utils)
3. MustClauses (canonical user-facing messages)
4. GuardClauses (throwing layer using MustResult messages)
5. FluentValidation (`IRuleBuilder` extension adapting the MustClause)
6. DataAnnotations (`ValidationAttribute` adapting the same MustClause)
7. Tests (xUnit) and coverage validation

Child specs may refine this.

### 7.2 Scope discipline

- Fix the root cause in the intended layer.
- Don’t introduce “just in case” abstractions.
- Don’t expand scope to unrelated refactors.

### 7.3 Simplicity first

- Make every change as simple as possible. Minimal code, minimal files, minimal scope.
- Single-responsibility changes: one fix per commit, one feature per PR.
- No preemptive refactoring (“while I’m here, let me clean up...”).
- No gratuitous improvements — don’t add docstrings, comments, or type hints to code you didn’t change.
- Three similar lines of code is better than a premature abstraction.

### 7.4 Root-cause discipline (no laziness)

- Find root causes, not symptoms. No temporary workarounds.
- If a build fails, understand why — do not just `dotnet clean`.
- If a test flakes, fix the underlying race condition — do not add a retry loop.
- If a hook or safety check blocks you, fix the issue it was preventing — do not bypass it.
- No skipping safety checks (`--no-verify`, `--force-push`).

### 7.5 Minimal impact (no side effects)

- Changes must not introduce unintended side effects.
- Only validate at system boundaries (user input, external APIs). Trust framework guarantees internally.
- Do not add fallbacks for scenarios that cannot happen.
- Do not defensively restructure code just because a change is nearby.

### 7.6 Demand elegance (balanced)

- For non-trivial changes (multi-file, new patterns, precedent-setting): pause and ask “is there a more elegant way?”
- Challenge your own work before presenting it — would you implement the same way knowing what you know now?
- Skip elegance checks for obvious, simple fixes — a typo fix does not need a design review.

---

## 8) Patterns and conventions (global)

- Layering pattern: `Utils` → `Rules` → `MustClauses` → `GuardClauses` → integrations.
- Naming patterns:
  - Rules: `Is*`, `Has*`
  - Utils: `Try*`
  - Must: positive/intention-revealing; negations only when strict complements and still validate inputs.
  - Guard: bad-state names derived from Must vocabulary; implement via Must.

---

## 9) Keep specs DRY (commonality extraction)

If guidance appears in multiple child specs:

- If it is truly cross-cutting, move it into this root spec.
- Then remove or shorten it in child specs, replacing it with a pointer like:
  - “See `docs/ai/specs/spec.md` §X.Y.”

Maintenance workflow (required discipline):

1. When editing a child spec, scan for any rule that would apply to _all_ or _most_ other specs.
2. If found, lift it into `docs/ai/specs/spec.md`.
3. Update the child spec to reference the root section.

---

## 10) Coding Standards & Analyzer Rules (Global)

All code must be clean, engineering-focused, and **warning-free**.

**Refer to [Coding Standards & Static Analysis Rules](./coding-standard.md) for strict rules regarding:**

- Resharper Optimisations (primary constructors, redundant members, pattern matching).
- SonarQube Optimisations / compiler features (`[GeneratedRegex]` source generation).
- Nullability and the sanctioned CS0612 obsolete-member suppression pattern.

---

## 11) Directory layout

### 11.1 Directory Structure

Root files: `spec.md` (this file), `protocol.md`, `orchestration.md`, `dependencies.md`,
`safety.md`, `coding-standard.md`, `council.md`, `project.md` (base project spec).

Per-project spec directories (each carries `project.md`, `unit-test.md`, `coverage.md`):

- `docs/ai/specs/core/`
- `docs/ai/specs/must-clauses/`
- `docs/ai/specs/guard-clauses/`
- `docs/ai/specs/data-annotations/`
- `docs/ai/specs/fluent-validation/`
- `docs/ai/specs/testing/` (plus `fixture.md` and the `gold-standard.md` status index)

Other directories:

- `docs/ai/specs/language/` — vocabulary and naming-collision specs
- `docs/ai/specs/scan/` — SonarQube scan spec
- `docs/ai/specs/tools/` — tool specs (`audit-cli/`, `code-diagnostics/`, `code-inspection/`)

### 11.2 Scope identifier → spec directory map

Filenames elsewhere in the Brain use the short scope identifiers from
`docs/ai/meta/taxonomy.md` §N.4; the spec directories predate that convention and keep their
long names. Translate with this table (the only two that coincide are `core` and `testing`):

| Scope id | Spec directory |
|----------|----------------|
| `core` | `docs/ai/specs/core/` |
| `must` | `docs/ai/specs/must-clauses/` |
| `guard` | `docs/ai/specs/guard-clauses/` |
| `fluent` | `docs/ai/specs/fluent-validation/` |
| `annotation` | `docs/ai/specs/data-annotations/` |
| `testing` | `docs/ai/specs/testing/` |

---

## 12) Symmetric and non-inheriting interface support

### 12.1 Separate File Strategy for Non-Inheriting Interfaces

The architecture strictly adheres to **Standard 12.1**.

Since `IDictionary` and `IReadOnlyDictionary` do not inherit from each other, they are treated as distinct domains ("Mutable" vs "Immutable").

- **Do not merge** them into a single file or abstraction.
- **Do not blur** the boundary.
- **Risk**: Merging would potentially expose "Mutable" semantics (like `Add` or `Clear` checks) to ReadOnly types, or restrict Mutable types to a ReadOnly subset.

This applies to all layers (Core, Must, Guard, Integrations).

Exception: the Integration Tier may share a reflection base — see §12.6.

### 12.2 Semantic Parity for Symmetric Interfaces

When supporting symmetric interfaces (e.g., `IDictionary` and `IReadOnlyDictionary`), the library MUST ensure **Semantic Parity**:

- **Logic**: The validation logic must be identical. If `IDictionary` checks `Count > 0`, `IReadOnlyDictionary` must check `Count > 0`.
- **Messages**: The failure messages must be identical.
- **Testing**: The unit test datasets (Valid/Invalid cases) must be mirrored exactly.

### 12.3 Extension Method Ambiguity Resolution

To prevent compiler ambiguity when a type implements multiple interfaces supported by extension methods (e.g., `Dictionary<K,V>` implements both `IDictionary` and `IReadOnlyDictionary`):

- **Library Code**: MUST NOT rely on extension method discovery for these types. Use explicit static calls: `MustReadOnlyDictionaryClauses.HasAnyKey(...)`.
- **Test Code**: MUST cast the input object to the specific interface being tested: `Guard.Against.Empty((IReadOnlyDictionary<string, int>)dict)`.

### 12.4 Global Ambiguity Audit

When adding `IReadOnly*` or sibling interface support to the library:

- **Audit**: You MUST run a full test suite audit.
- **Remediation**: Existing tests that rely on implicit extension discovery for the original interface (e.g., `IDictionary`) may now become ambiguous. Update them to use explicit casts if they fail to compile.

### 12.5 Generic Interface Reflection Pattern

For **Integration Tier** components (DataAnnotations) that must validate generic interfaces without compile-time knowledge of `T`:

- **Pattern**: Use a `Generic{Interface}AttributeBase` that inspects the object at runtime.
- **Mechanism**:
  1.  Inspect `value.GetType().GetInterfaces()`.
  2.  Find the matching open generic (e.g., `IDictionary<,>`).
  3.  Extract generic arguments (`TKey`, `TValue`).
  4.  Use `MakeGenericMethod` to invoke the corresponding `MustClause` **via reflection**.
- **Constraint**: This is permitted **ONLY** in the Integration Tier (Standard 12.6).
- **Constraint**: `dynamic`/the DLR and a `Microsoft.CSharp` package reference are prohibited in all layers. The DLR fails to bind `MustResult<T>` generically; use `MethodInfo.Invoke` and reflect over the result.

### 12.6 Logic Tier Separation vs Integration Tier Abstraction

To balance performance and maintainability:

- **Logic Tiers (Rules, Must, Guard)**: MUST maintain separate, unrelated implementations for disjoint interfaces (e.g., `IDictionary` and `IReadOnlyDictionary`).
  - _Rationale_: These layers require O(1) performance and strict type safety.
- **Integration Tier (DataAnnotations)**: CAN use abstraction and reflection (e.g., `GenericDictionaryAttributeBase`).
  - _Rationale_: Setup costs are already high (framework reflection); reducing code duplication here is worth the minor runtime cost.
