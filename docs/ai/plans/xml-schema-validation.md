# Plan — XML: Core fix and `PineGuard.Xml` schema validation

**Status**: executing (2026-09-14)
**Branch / worktree**: `feature/xml-schema` at `.claude/worktrees/xml-schema`
**Governed by**: `new-surfaces-missing-validation-cases-00-program.md` (package conventions §4, code grammar §5.4, gates §7/§9, onboarding §8)
**Supersedes**: the "XSD schema validation" deferral in `library-expansion-roadmap.md` §2.6 and the Xml bullets of its Part 6

## 0. Why now

CHESS replacement, NPP and SWIFT MX are ISO 20022: hundreds of message definitions, each identified by its
root element namespace, each with its own XSD. Schema conformance is the everyday XML check in that world,
and the value PineGuard adds over fifteen lines of `XmlReaderSettings` is doing the three things nearly
everyone gets wrong (unknown-namespace-is-only-a-warning, resolver left on, one error instead of all of them)
and feeding the result into the existing `MustValidationResult` pipeline.

Two deliverables, two commits, in order:

| Commit | Scope | Subject |
|---|---|---|
| 1 | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing | `fix(xml): collapse the duplicate document clause, parse with a forward-only reader, add root identification` |
| 2 | new `PineGuard.Xml` + scope onboarding + codes in Core | `feat(xml): add PineGuard.Xml with XSD conformance over a compiled schema set` |

## 1. Commit 1 — Core XML fix

### 1.1 Defect being fixed

`Must.Be.Xml` and `Must.Be.XmlDocument` (and their Guard, Fluent and DataAnnotations adapters) call the same
predicate, `XmlRules.IsXml`, which loads a DOM with document conformance. Both reject `<a/><b/>`, both accept
`<root/>`, both emit `xml.document.invalid`. The docs claim a fragment/document distinction that does not exist.
Under §4.6 (no back-compat before first release) the duplicate is deleted outright.

### 1.2 Changes

**`src/PineGuard.Core/Utils/XmlUtility.cs`** — replace `TryParse(string?, out XmlDocument?)` with:

```csharp
public static bool TryGetRootName(string? value, out XmlQualifiedName? rootName)
```

- Forward-only `XmlReader` over a `StringReader`; no `XmlDocument` allocation.
- `XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }` (document conformance, the default).
- Reads to the end (well-formedness needs the whole document); captures the first `XmlNodeType.Element` as
  `new XmlQualifiedName(reader.LocalName, reader.NamespaceURI)`.
- `null`/whitespace → `false`, `rootName = null`; `XmlException` → `false`, `rootName = null`.
- Remarks keep the DOCTYPE explanation but state it as a deliberate policy for untrusted input, not an apology.
  Mirrors `JsonUtility.TryGetRootKind`.

**`src/PineGuard.Core/Rules/XmlRules.cs`**

- `IsXml(string? value)` → `XmlUtility.TryGetRootName(value, out _)`; semantics unchanged.
- New `HasXmlRoot(string? value, string localName, string? namespaceUri = null)`:
  `true` iff well-formed **and** root local name equals `localName` (ordinal) **and**, when `namespaceUri` is
  not `null`, root namespace equals `namespaceUri` (ordinal). `namespaceUri == null` means any namespace;
  `""` means no namespace (what `XmlReader` reports). `localName` null/whitespace → `ArgumentException` via
  `ThrowHelper` (programmer error, matches other rules taking a required argument).
- `IsXmlContentType` unchanged.

**`src/PineGuard.Core/Codes/MustCodes.Xml.cs`** — add node `Root` with `Prefix = "xml.root"` and
`Mismatch = "xml.root.mismatch"` (`<summary><c>xml.root.mismatch</c></summary>`, same shape as the siblings).

**`src/PineGuard.MustClauses/MustXmlClauses.cs`**

- Delete `XmlDocument`.
- Add `HasXmlRoot(this IMustClause _, string? value, string localName, string? namespaceUri = null, [CallerArgumentExpression] paramName)`
  → `MustResult<string>`; `null` value → `Fail(MustCodes.Xml.Root.Mismatch, "{paramName} must not be null.", …)`
  (same null handling as `Xml`); otherwise `FromBool(XmlRules.HasXmlRoot(...), MustCodes.Xml.Root.Mismatch, template, …)`
  with template `"{paramName} must be XML with root element '{localName}'."` when `namespaceUri` is null, else
  `"{paramName} must be XML with root element '{localName}' in namespace '{namespaceUri}'."` (templates are built
  with string interpolation of the *arguments only*; `{paramName}` stays a literal token). Precedent for the
  `Has` prefix under `Must.Be`: `Must.Be.HasEmailAlias`.

**`src/PineGuard.GuardClauses/GuardXmlClauses.cs`** — delete `NotXmlDocument`; add `NotHasXmlRoot(...)` returning
`string`, same parameter order as the Must clause followed by `message`, `exceptionCreator`, `paramName`
(precedent: `Guard.Against.NotHasEmailAlias`).

**`src/PineGuard.FluentValidation/FluentXmlExtensions.cs`** — delete `XmlDocument<TModel>`; add
`HasXmlRoot<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string localName, string? namespaceUri = null, string? message = null)`
via `MustBe(val => Must.Be.HasXmlRoot(val, localName, namespaceUri, paramName: null), message, MustCodes.Xml.Root.Mismatch)`.

**`src/PineGuard.DataAnnotations/XmlAttributes.cs`** — delete `XmlDocumentStringAttribute`; correct
`XmlStringAttribute` docs (it validates a complete, single-root document — there is no fragment mode); add
`HasXmlRootAttribute(string localName, string? namespaceUri = null) : ValidationAttributeBase(typeof(string), MustCodes.Xml.Root.Mismatch)`
(the simple name `HasXmlRootAttribute` does not collide with `System.Xml.Serialization.XmlRootAttribute`).

**`tests/PineGuard.Testing/Fixtures/XmlRulesFixtures.cs`**

- `IsXml`: add scenarios `Namespaced` (`<d:Document xmlns:d="urn:test:doc"><d:Id>1</d:Id></d:Document>`, valid),
  `WithDeclaration` (`<?xml version="1.0" encoding="utf-8"?><root/>`, valid), `MultipleRoots` (`<a/><b/>`, invalid),
  `Doctype` (`<!DOCTYPE root [<!ELEMENT root ANY>]><root/>`, invalid), `Fragment` (`text only`, invalid).
- New nested `HasXmlRoot` with public constants `LocalName = "Document"`, `Namespace = "urn:test:doc"`, values
  `Matching` (`<Document xmlns="urn:test:doc"><Id>1</Id></Document>`), `MatchingNoNamespace` (`<Document/>`),
  `WrongName` (`<Envelope xmlns="urn:test:doc"/>`), `WrongNamespace` (`<Document xmlns="urn:other"/>`),
  `Malformed` (`<Document`), `Null`, `Whitespace`. Scenarios are grouped as `ValidScenarios` (Matching only, when
  asserting with namespace) and `InvalidScenarios`; a second pair `AnyNamespaceValidScenarios`
  (Matching, MatchingNoNamespace, WrongNamespace) / `AnyNamespaceInvalidScenarios` (WrongName, Malformed, Null,
  Whitespace) for the `namespaceUri == null` overload path.

**Tests** — every layer's `XmlXxxTests.cs` / `XmlXxxTestData.cs` (Core rules + utility, Must, Guard, Fluent,
DataAnnotations): delete the `XmlDocument` groups, add `HasXmlRoot` groups following the file's existing
pattern exactly (`[Theory]` + `[MemberData]`, `<Member>_BehavesAsExpected`, fixtures via `using F = …`).
`XmlUtilityTests` asserts `ok` and, for valid cases, `rootName.Name`/`rootName.Namespace`.

### 1.3 Gates for commit 1

Build zero warnings; tests green net8.0 + net10.0; `-Scope Core|MustClauses|GuardClauses|FluentValidation|DataAnnotations|Testing`
coverage 100/100 per TFM; `dotnet format --verify-no-changes` clean; `must-codes` audit clean.

## 2. Commit 2 — `PineGuard.Xml`

### 2.1 Package

| Item | Value |
|---|---|
| Package id / assembly / namespace | `PineGuard.Xml` (roadmap §2.6 reserved it; namespace = package id per §4.1) |
| Scope id / PowerShell `-Scope` / worktree token | `xml` / `Xml` / `xml-schema` |
| TFMs | inherited `netstandard2.1;net8.0;net10.0` (`System.Xml.Schema` is in-box on all three) |
| Package references | none |
| Project references | `PineGuard.Core`, `PineGuard.MustClauses`, `PineGuard.GuardClauses`, `PineGuard.DataAnnotations` (all first-party, no third-party dependency reaches consumers) |
| Not included | a FluentValidation adapter — that would force the FluentValidation dependency on every consumer; the README shows the one-line recipe through `FluentExtension.MustBe` |
| Description | `XSD conformance for XML payloads, validated through PineGuard. One compiled schema set, every violation listed by element path, the unknown-namespace trap closed by default.` |
| Tags | `$(PackageTags);xml;xsd;xml-schema;iso20022;schema-validation` |

### 2.2 Public surface (namespace `PineGuard.Xml`)

XML's own vocabulary is used on purpose: *well-formed* is syntax (`IsXml`, already in Core); *valid* is
conformance to a schema. Hence `ValidXml`, `IsValidXml`, `InvalidXml`.

| Type / member | Shape | Notes |
|---|---|---|
| `XmlSchemaSetBuilder` | `sealed class`; `AddFile(string path)`, `AddStream(Stream stream)`, `AddReader(TextReader reader)`, `AddText(string schemaXml)` — each returns `this`; `XmlSchemaSet Build()` | Every source is read through `XmlReader.Create(..., new XmlReaderSettings { DtdProcessing = Prohibit, XmlResolver = null })` and added with `XmlSchemaSet.Add((string?)null, reader)`; the set is created with `XmlResolver = null` so `xs:import`/`xs:include` never reach the network, then `Compile()`d. `XmlSchemaException` from a bad schema surfaces as-is. `Build()` may be called once; a second call throws `InvalidOperationException`. Rejected names: `XmlSchemaLoader` (compiling is the point), `SafeXmlSchemaSet` (adjective-led). |
| `XmlSchemaValidationOptions` | `sealed class`; `bool RequireKnownNamespace { get; init; } = true;` `bool TreatWarningsAsViolations { get; init; } = false;` `static XmlSchemaValidationOptions Default { get; }` | Warnings default **off** because ISO 20022 `SplmtryData` uses `xs:any processContents="lax"`, which raises warnings on perfectly valid messages; the root-namespace check is what closes the silent-pass trap, so it is explicit and on by default. |
| `XmlSchemaViolationKind` | `enum { NotWellFormed, UnknownNamespace, Error, Warning }` | Maps to codes: `xml.document.invalid`, `xml.namespace.unknown`, `xml.schema.mismatch`, `xml.schema.mismatch`. |
| `XmlSchemaViolation` | `sealed record (XmlSchemaViolationKind Kind, string Path, string Message, int LineNumber, int LinePosition)` | `Path` is the element path from the root, `/`-separated, local names only: `Document/GrpHdr/MsgId`. Attribute violations carry their element's path (the reader is positioned on the element when the event fires). |
| `XmlSchemaUtility` | `static bool TryValidate(string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options, out IReadOnlyList<XmlSchemaViolation> violations)` | Streaming: `XmlReaderSettings { ValidationType = Schema, Schemas = schemas, DtdProcessing = Prohibit, XmlResolver = null }`, default `ValidationFlags` (never `ProcessSchemaLocation`/`ProcessInlineSchema`), `ValidationEventHandler` collects. Maintains a `Stack<string>` of local names (push on non-empty `Element`, pop on `EndElement`). On the first element, if `options.RequireKnownNamespace && !schemas.Contains(reader.NamespaceURI)` → one `UnknownNamespace` violation and return `false` immediately. `null`/whitespace → `false` with an empty list. `XmlException` → `false` with one `NotWellFormed` violation at the current path. Returns `true` iff no violations were collected (warnings count only when `TreatWarningsAsViolations`). `schemas` null → `ArgumentNullException`. |
| `XmlSchemaRules` | `static bool IsValidXml(string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options = null)` | Pure predicate over `TryValidate`. The class is not named `XmlRules` because that would collide with `PineGuard.Rules.XmlRules` in any file importing both. |
| `MustXmlSchemaClauses` | `MustResult<string> ValidXml(this IMustClause _, string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options = null, [CallerArgumentExpression] paramName)` | `null` → `Fail(MustCodes.Xml.Document.Invalid, "{paramName} must not be null.")`. Otherwise `TryValidate`; on failure the code comes from the **first** violation's kind and the template is deterministic (no runtime schema-message text, which differs between .NET 8 and .NET 10): `NotWellFormed` → `"{paramName} must be XML."`; `UnknownNamespace` → `"{paramName} must be XML in a namespace covered by the schema set (found '{ns}')."`; `Error`/`Warning` → `"{paramName} must be valid against the XML schema ({n} violation(s), first at '{path}')."` with `n` the violation count. |
| `GuardXmlSchemaClauses` | `string InvalidXml(this IGuardClause _, string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options = null, string? message = null, Func<Exception>? exceptionCreator = null, [CallerArgumentExpression] paramName)` | Delegates to `Must.Be.ValidXml`, throws via `GuardFailure.Throw`. |
| `ValidXmlAttribute` | `sealed class ValidXmlAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Xml.Schema.Mismatch)` | Resolves the schema set with `validationContext.GetService(typeof(XmlSchemaSet)) as XmlSchemaSet`; when absent throws `InvalidOperationException("[ValidXml] requires an XmlSchemaSet registered as a service (services.AddSingleton(new XmlSchemaSetBuilder()....Build())).")`. Options may be resolved the same way (`XmlSchemaValidationOptions`), falling back to `Default`. |
| `XmlSchemaMustValidator` | `sealed class XmlSchemaMustValidator(XmlSchemaSet schemas, XmlSchemaValidationOptions? options = null) : IMustValidator<string>` | `Validate(string? value)` → `MustValidationResult.Ok()` or `Fail(...)` with **one `MustFailure` per violation**: `PropertyPath = violation.Path`, `Code` by kind, `Message = violation.Message` (the raw schema message is right here: it is the diagnostic, not a template), `Value = null` (never echo the whole document). `null` value → one failure `("", xml.document.invalid, "Value must not be null.", null)`. Async members wrap the sync call in a completed `ValueTask`. Non-generic `IMustValidator` members: `ValidatedType = typeof(string)`, `Validate(object?)` casts (non-string → `ArgumentException`). Named after `InlineMustValidator<T>` (`<Qualifier>MustValidator`); `XmlSchemaValidator` is taken by `System.Xml.Schema`. |

**Codes added to `src/PineGuard.Core/Codes/MustCodes.Xml.cs`** (Core owns the catalogue; the header comment
gains `MustXmlSchemaClauses.cs (PineGuard.Xml)`):

| Node | Constant | Value | Reading test |
|---|---|---|---|
| `Schema` | `Mismatch` | `xml.schema.mismatch` | "xml schema mismatch" — the document mismatches the schema (`xml.schema.invalid` would read as the schema itself being broken) |
| `Namespace` | `Unknown` | `xml.namespace.unknown` | "xml namespace unknown" — no schema in the set covers the root namespace |

### 2.3 Files

```text
+ src/PineGuard.Xml/PineGuard.Xml.csproj
+ src/PineGuard.Xml/README.md
+ src/PineGuard.Xml/AGENTS.md                      (single line: Read docs/ai/rules/xml.md before writing or editing any PineGuard.Xml code.)
+ src/PineGuard.Xml/XmlSchemaSetBuilder.cs
+ src/PineGuard.Xml/XmlSchemaValidationOptions.cs
+ src/PineGuard.Xml/XmlSchemaViolation.cs          (record + enum in one file is fine; or split XmlSchemaViolationKind.cs)
+ src/PineGuard.Xml/XmlSchemaUtility.cs
+ src/PineGuard.Xml/XmlSchemaRules.cs
+ src/PineGuard.Xml/MustXmlSchemaClauses.cs
+ src/PineGuard.Xml/GuardXmlSchemaClauses.cs
+ src/PineGuard.Xml/ValidXmlAttribute.cs
+ src/PineGuard.Xml/XmlSchemaMustValidator.cs
+ tests/PineGuard.Xml.UnitTests/PineGuard.Xml.UnitTests.csproj
+ tests/PineGuard.Xml.UnitTests/Schemas/TestSchemas.cs      (inline XSD strings: `urn:test:doc` Document{GrpHdr{MsgId (maxLength 35), CreDtTm (dateTime)}, SplmtryData? (xs:any lax)}, plus `urn:test:other`)
+ tests/PineGuard.Xml.UnitTests/XmlSchemaSetBuilderTests.cs / TestData
+ tests/PineGuard.Xml.UnitTests/XmlSchemaUtilityTests.cs / TestData
+ tests/PineGuard.Xml.UnitTests/XmlSchemaRulesTests.cs / TestData
+ tests/PineGuard.Xml.UnitTests/MustXmlSchemaClausesTests.cs / TestData
+ tests/PineGuard.Xml.UnitTests/GuardXmlSchemaClausesTests.cs / TestData
+ tests/PineGuard.Xml.UnitTests/ValidXmlAttributeTests.cs / TestData
+ tests/PineGuard.Xml.UnitTests/XmlSchemaMustValidatorTests.cs / TestData
```

Test cases that must exist: valid message; missing required element; `maxLength` breach; wrong `dateTime`
lexical; two violations at once (both listed, paths correct, order preserved); unknown root namespace with
`RequireKnownNamespace` true (one `UnknownNamespace`) and false (falls through to schema warnings); lax
`xs:any` content with `TreatWarningsAsViolations` false (valid) and true (violation); not well-formed; null;
whitespace; builder from text, reader, stream and a temp file; builder `Build()` twice throws; attribute with
and without a registered `XmlSchemaSet`; Guard custom message and custom exception paths; validator through the
non-generic `IMustValidator` surface and `ValidateAsync`.

### 2.4 Scope onboarding (Plan 00 §8, current paths)

Follow the OneOf precedent commit `a1b0da0` for shape, but the tooling has moved since; the authoritative list
is every current match of `OneOf` outside `src/`, `tests/` and `docs/` — a Haiku sweep produces it (W2-H1).
Known items: `.editorconfig` test-project brace list; `.github/workflows/ci.yml` (paths-filter `xml`, job output,
test-matrix entry `run-if: core-or-must-or-guard-or-annotation-or-xml-or-testing-or-main` and its `case` arm,
every downstream arm is unaffected because nothing depends on Xml, coverage `FILTERS` block); `.vscode/tasks.json`;
`PineGuard.slnx` via `dotnet sln add` only (a hook blocks hand edits); `tools/.shared/dotnet-projects.ps1`
registry entry (after `MediatR`, before `Analyzers`) plus one `ValidateSet` token in every consumer script;
`tools/code-scan/qodana/config/qodana.xml.yaml` and `PineGuard.Xml.Qodana.slnx` beside the existing per-scope
ones plus the `All` Qodana slnx; `tools/git/Run-Commits.ps1`; `tools/github/*Release*.ps1` and
`tools/nuget/*Unlist*.ps1` package lists; `apps/cli/src/audit/rules/must-codes.ts` extra roots (the package
carries `Must.Be.*` call sites); `apps/cli/config/baseline.json` gains
`doc-links:src/PineGuard.Xml/AGENTS.md:docs/ai/rules/xml.md` (same precedent as OneOf — the Brain cascade in
§8.4 is a follow-up, see §4). Root `README.md`: package table row, install snippet, *Supported frameworks*,
and every "fourteen-package" count becomes fifteen (Haiku inventories the exact lines: W2-H2).

### 2.5 Gates for commit 2

Everything in §1.3 plus `-Scope Xml` coverage 100/100 on both TFMs, `-Scope All -Enforce100` on both TFMs,
`test-files` and `must-codes` audits clean, Qodana config parses (`Run-Qodana.ps1 -Scope Xml -WhatIf` if the
script supports it, otherwise skip and note).

## 3. Execution — agents, tiers, sync/async

**Tiers**: Haiku = read/inventory; Sonnet 5 = code, tests, tooling edits, verification runs; Fable 5.1 =
naming/design (done in this plan) and the pre-commit API review. The orchestrator only reads state
(`git status`, `git diff --stat`, build/test exit codes), dispatches, checkpoints, and commits.

**Policies for every dispatch**

- Work only under the worktree absolute path; every `git` call is `git -C "<worktree>"`; every `tools/*.ps1`
  invocation uses the worktree's own copy (scripts resolve the repo root from their own location).
- Never `git add`/`git commit`/`git stash` — the orchestrator owns the index (parallel commits collide on
  `index.lock`; the deliverable is exactly two commits). Files on disk are the save; the orchestrator makes a
  WIP checkpoint commit after every wave and squashes to the final commit per task.
- Tool-call budget per dispatch: ~60. At ~50, stop starting new work, make sure everything written builds,
  and report exactly what is done and what is not. A cut-off report beats a lost one.
- Rebuild the touched project(s) before reporting; run the touched test project(s); report the commands and
  the pass/fail counts verbatim. The coordination hook serialises `dotnet` runs machine-wide — a wait on the
  `dotnet-ops` lock is normal, not a hang.
- No edits under `docs/` except this plan; no edits to `tools/audit-cli/**`; no hand edits to `PineGuard.slnx`.
- Named agents; the orchestrator checks in via `SendMessage` at the 15-minute mark if no report has arrived.

**Waves** (`▶` sequential gate, `∥` parallel; max concurrent agents = 10)

```text
W1  ∥  S1  Sonnet  Commit-1 Core layer: XmlUtility, XmlRules, MustCodes.Xml (Root), XmlRulesFixtures,
                   Core + Testing tests. Builds Core + Testing + Core.UnitTests; runs Core.UnitTests.
       H1  Haiku   Inventory every current `OneOf` scope-enumeration point outside src/tests/docs
                   (file:line + the exact line) → checklist for S6.
       H2  Haiku   Inventory root README.md: package table rows, install snippets, Supported frameworks,
                   every "fourteen" count, and the OneOf README as the satellite README template → notes for S9.
       H3  Haiku   Distil signatures the Xml coders need: IMustValidator / IMustValidator<T> / MustValidator<T>,
                   MustValidationResult/MustFailure factories, ValidationAttributeBase, GuardFailure.Throw,
                   BaseMustUnitTest/BaseGuardUnitTest/BaseDataAnnotationUnitTest/BaseMustValidationUnitTest
                   + their Case/Expected records → one reference note for S8/T1–T3.
    ▶  orchestrator: build Core+Testing, run Core.UnitTests, WIP checkpoint.
W1b ∥  S2 Must, S3 Guard, S4 Fluent, S5 DataAnnotations — each deletes its XmlDocument member + tests,
       adds HasXmlRoot + tests, builds and runs only its own test project. Disjoint files.
    ▶  orchestrator: full build, all five test projects, format --verify, coverage per scope per TFM
       (Sonnet V1 runs the gates and reports), WIP checkpoint → squash → COMMIT 1.
W2  ▶  S6  Sonnet  Scope onboarding from H1's checklist: csproj skeletons (src + tests), dotnet sln add,
                   .editorconfig, ci.yml, tasks.json, registry + ValidateSets, Qodana files, commit/release/
                   unlist lists, must-codes.ts, baseline.json. Builds the empty projects.
    ▶  orchestrator: build, WIP checkpoint.
W2b ∥  S8  Sonnet  PineGuard.Xml source (all nine .cs files) + MustCodes.Xml (Schema, Namespace) + AGENTS.md
                   + package README. Builds PineGuard.Xml.
       S9  Sonnet  Root README edits from H2's notes (row, install, frameworks, counts).
    ▶  orchestrator: build, WIP checkpoint.
W2c ∥  T1  Sonnet  Tests: TestSchemas + XmlSchemaSetBuilder + XmlSchemaUtility + XmlSchemaRules.
       T2  Sonnet  Tests: XmlSchemaMustValidator (incl. non-generic + async surface).
       T3  Sonnet  Tests: MustXmlSchemaClauses + GuardXmlSchemaClauses + ValidXmlAttribute.
       F1  Fable   API/doc review of src/PineGuard.Xml against §2.2 (names, null handling, remarks accuracy,
                   README claims) → written findings; orchestrator routes any fix to a Sonnet follow-up.
    ▶  orchestrator: build, run Xml.UnitTests, Sonnet V2 runs full gates (§2.5), WIP checkpoint → squash → COMMIT 2.
```

Peak concurrency: W1 = 4, W1b = 4, W2c = 4. H1–H3 are read-only and overlap W1 because S1's files are not
among what they read.

**Blocking dependencies**: W1b needs S1 (the rule and fixtures must compile). W2 needs commit 1 (the codes
file and fixtures it builds on). W2b needs S6 (csproj + solution entry). W2c needs S8 (the types under test).
S9 and F1 are independent of everything except their inputs.

## 4. Follow-ups (not in these two commits)

- Brain cascade for the `xml` scope (Plan 00 §8.4 items 22–29): `docs/ai/specs/xml/`, `docs/ai/rules/xml.md`
  (removes the baseline entry), nine agent stubs across all adapter surfaces, command rows, taxonomy id.
- Byte/`Stream` overloads for `IsXml`/`HasXmlRoot` and for `XmlSchemaUtility.TryValidate` (encoding declared in
  the document, BOM handling).
- `IsXmlName`/`IsNCName`/`IsQName`, XML-legal-character check, XPath existence (`HasXPathMatch`, roadmap Part 6).
- ISO 8601 duration lexical rule via `XmlConvert.ToTimeSpan`.
- Update `library-expansion-roadmap.md` §2.6 / Part 6 / Out-of-scope lines once this plan is merged.
- Deliberately out: XSD 1.1 (unsupported by .NET), Schematron, RELAX NG, XML-DSig, XSLT. The
  `System.Security.Cryptography.Xml` pin in `Directory.Packages.props` stays — it lifts a vulnerable transitive
  for `tools/audit-cli`, it is not a stray.

## 5. Decision log

| Date | Decision | Why |
|---|---|---|
| 2026-09-14 | Delete `XmlDocument` variants rather than give `Xml` fragment semantics | Fragment conformance accepts bare text, so a "fragment" predicate that is not a lie needs its own design; nobody asked for it |
| 2026-09-14 | DOCTYPE stays `Prohibit` | Financial messaging never carries a DOCTYPE; `Ignore` is a policy change to make deliberately, not as a side effect |
| 2026-09-14 | `namespaceUri == null` means any namespace in `HasXmlRoot` | Matches XPath `local-name()` habits; `""` still selects the no-namespace case exactly |
| 2026-09-14 | One `XmlSchemaSet`, root namespace checked with `Contains` — no registry type | `XmlSchemaSet` already keys schemas by target namespace; a registry would duplicate it |
| 2026-09-14 | Warnings are not violations by default | ISO 20022 `SplmtryData` (`xs:any` lax) warns on valid messages; the explicit root-namespace check is what closes the silent-pass trap |
| 2026-09-14 | Must-clause messages are deterministic templates; the validator carries the raw schema messages | Schema message text differs across runtimes and would make the TFM test matrix brittle; the aggregate result is where diagnostics belong |
| 2026-09-14 | No Fluent adapter in `PineGuard.Xml`; Guard + DataAnnotations included | FluentValidation is third-party and must not be forced on consumers; Guard and DataAnnotations are first-party and dependency-free |
| 2026-09-14 | `XmlSchemaMustValidator`, `XmlSchemaRules`, `HasXmlRootAttribute` | Each dodges a BCL simple-name collision (`System.Xml.Schema.XmlSchemaValidator`, `PineGuard.Rules.XmlRules`, `System.Xml.Serialization.XmlRootAttribute`) |
