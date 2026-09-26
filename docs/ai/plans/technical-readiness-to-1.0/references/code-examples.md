# Code examples

This reference defines the classifications, dependencies, reproduction steps, source-inspected PineGuard examples, and bounded verification evidence for the modular readiness plans.

## Example contract and execution

`Program.cs` and `Pilot.csproj` are a complete **Proposed illustration**, a package-free .NET 10 console teaching miniature in namespace `ReadinessIllustration`. They demonstrate PositiveInt, not a replacement for the planned Email/bounded numeric/string→DateOnly pilot. They do not introduce a PineGuard public API. D07 manifest, D08 failure, D10 test tools, D11 budgets, D12 support, D15 diagnostics, D17 lifecycle, D22 paths and D26 sample design remain open. The local result records are candidate shapes only. `PositiveAmount` is a sealed immutable teaching value object: its private constructor is reached through a factory delegating to the same static evaluator. The factory returns false with null on rejection; successful ordinary construction preserves the positive invariant. No reflection, serialization or production API guarantees are claimed.

One static `PositiveInt.Evaluate` contains the numeric predicate once. The descriptor, Must/Guard/object miniature, boundary, and timing loop call it. Descriptor[] is illustrative registration input with derived diagnostic views; it is not an implemented source-derived manifest or production completeness gate. Test expectations are literal rows or a BCL sign oracle over explicitly bounded integers. The mutation-only `>= 0` predicate is a deliberately wrong seed, never an alternative semantic authority. The complete source is in `Program.cs`; each excerpt below declares that dependency and is a complete method/call/configuration unit in that context. The generic invalid-input response collapses parse, size and semantic failures only for this illustration; it does not settle D05/D14/D16 operational policy. The handler interface illustrates one boundary and is not a required extra production abstraction.

After saving Pilot.csproj and Program.cs together in a scratch directory, run:

```powershell
dotnet build ./Pilot.csproj --configuration Release
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
dotnet run --project ./Pilot.csproj --configuration Release --no-build
if ($LASTEXITCODE -ne 0) { throw 'Execution failed.' }
```

Expected independent checks: null/minimum/-1/0 fail; 1/maximum pass; seeded missing and orphan manifest IDs appear; zero guard throws; zero response is 400 with one safe code; invalid regex syntax remains OperationalError when negated; disabled diagnostics produce no event; enabled event contains only fixed rule/code; invariant parsing remains stable under fr-FR and culture is restored; fixed UTC day is 2030-01-02; 1,000 concurrent positive calls pass. Console first line is `PASS: semantic, manifest, projections, seeded-domain, replay, mutation, regex, privacy, determinism, concurrency, applications`. The second line has `consumed=100000`; elapsed ticks/allocation are observations without a threshold. These checks prove only the miniature.

## Verified example results

Verified: the proposed package-free net10.0 Release miniature built with SDK 10.0.401, zero warnings/errors, and ran successfully; the synthetic governance self-test passed valid cases and ten expected rejection probes. These results verify only those examples. Current PineGuard API insertion snippets are source-inspected but not executed; real repository coverage/full suite, BenchmarkDotNet and NativeAOT were not run. No library implementation or selected architecture policy is changed.

Observed commands from the workspace root:

    dotnet build planning-review/code-examples/Pilot.csproj --framework net10.0 --configuration Release
    dotnet run --project planning-review/code-examples/Pilot.csproj --framework net10.0 --configuration Release --no-build
    pwsh -NoProfile -File planning-review/code-examples/Verify-GovernanceExamples.ps1

The build and run exited 0. Build reported 0 warnings and 0 errors. The run printed the documented PASS line and the timing output consumed=100000, elapsedTicks=40333, allocatedBytes=0; timing is an observation, not a budget or benchmark result. The governance self-test exited 0 and reported its valid-case checks plus ten expected rejection probes. It writes only local synthetic fixtures in its scratch workspace.

Final frozen source SHA-256:

| File | SHA-256 |
|---|---|
| Pilot.csproj | CBDA77BE076F6FF67D1A510C950892AB127591D8DEC1E78EB4F9A23D48B41661 |
| Program.cs | F4A54C06F3440793662B8A8E76B4374A923DD1C3155F9A664B926B890EE59858 |
| Verify-Evidence.ps1 | 0C74E329C64F471614531103362914E919EBD5A676E96285E377866E5EF2E428 |
| Verify-ApiBaseline.ps1 | 7D5F5DB50A9CEFBC35D7FA23D24683D82B06E10421E6C865B87F16E0B1E66CB0 |
| Verify-Restore.ps1 | F0F80D50BB9334CCD23FFD7E870C36B0613736045FBA1B195F1DCEE5DCEB48E1 |
| Verify-GovernanceExamples.ps1 | F89992382639318308F1F4AE395BDC858761BF87B9D34B05FDF3CED989084450 |

For reproduction, copy each complete source block below into a file with its displayed filename in the same empty scratch directory, then run the relative commands in that directory. No command depends on the original workspace path. The PowerShell governance self-test creates its own local fixtures; do not run production repository checks through it.

## Current PineGuard examples

**Current API**, numeric examples require net8.0 or net10.0, never netstandard2.1. Dependencies are the existing projects containing each adapter; preserve their existing usings. Exact cross-project namespaces have not been supplied, so these are explicit insertion units, not standalone consumers. Public API names and source locations came from Luna. The current failure types remain `MustResult<T>`, `MustFailure(PropertyPath,Code,Message,Value)` and `MustValidationResult`; Value existing in a result does not authorize logging it.

Insert this complete data property into existing `MustNumberClausesTestData.cs` using existing xUnit imports (xUnit 2.9.3):

```csharp
public static TheoryData<int?, bool> DocumentationPositiveCases => new()
{
    { null, false }, { int.MinValue, false }, { -1, false },
    { 0, false }, { 1, true }, { int.MaxValue, true }
};
```

Insert this complete method into existing `MustNumberClausesTests.cs`, whose existing imports resolve Must/xUnit:

```csharp
[Theory]
[MemberData(nameof(MustNumberClausesTestData.DocumentationPositiveCases),
    MemberType = typeof(MustNumberClausesTestData))]
public void Documentation_positive_has_explicit_expected_outcomes(int? value, bool expected)
{
    var result = Must.Be.Positive<int>(value);
    Assert.Equal(expected, result.Success);
}
```

Expected outputs are the literal data rows. Status: source-grounded API, proposed test insertion, not executed. Test SDK 18.10.1 is the source pin. Do not infer every framework's null handling from these Must expectations.

**Current API**, complete insertion method in existing Guard tests with existing imports:

```csharp
private static void Documentation_positive_guard_accepts_one()
{
    Guard.Against.ZeroOrNegative(1);
}
```

Input 1 → no exception. Status not executed. The complement is ZeroOrNegative; there is no claimed Guard.Against.Positive API.

**Current API**, complete rule-registration statement inside an existing numeric FluentValidation validator constructor, model has existing int `Value`, FluentValidation 11.12.0:

```csharp
RuleFor(x => x.Value).Positive();
```

Input Value=1 → positive predicate accepted, Value=0 → positive predicate rejected; framework null/skip/object execution is a separate test contract, not inferred here. Status source-grounded, not executed.

**Current API**, complete property declaration in existing DataAnnotations numeric model using existing imports:

```csharp
[PositiveNumber]
public int Value { get; set; }
```

Input 1/0 → positive/nonpositive; invoke the actual DataAnnotations validation mechanism in its adapter tests before claiming parity. Status declaration source-grounded, not executed.

**Current API**, complete insertion method in existing Must email tests:

```csharp
private static void Documentation_email_has_an_independent_example()
{
    Assert.True(Must.Be.Email("person@example.com").Success);
    Assert.False(Must.Be.Email("not an email").Success);
}
```

Expected valid/invalid are explicit; stable existing failure code is `MustCodes.Email.Address.Invalid`. Status not executed. Existing complement is Guard.Against.NotEmail, Fluent `.Email()`, DataAnnotations `[Email]`. Do not invent a validation-manifest API from CLI layer-parity vocabulary.

**Current API**, complete forwarding method inside an existing DateOnly test class with its existing imports:

```csharp
private static bool Documentation_past_with_explicit_policy(
    DateOnly? value, Inclusion inclusion, TimeProvider clock)
{
    return DateOnlyRules.IsInPast(value, inclusion, clock);
}
```

The caller must supply an existing verified Inclusion member and `FixedTimeProvider` fixture; the packet does not identify that enum member, so no boundary truth is fabricated. Purpose: show explicit clock/policy flow, not settle inclusive boundaries. Status exact signature source-grounded, semantic execution pending. `Program.cs` independently demonstrates UTC and raw string→DateOnly conversion, not DateOnlyRules conformance.

## Direct workstream excerpts

**Current API**, additional complete insertion units for existing `MustValidatorTests.cs`, using its existing CreateOrder model, MustValidator/xUnit imports and fixtures. The invalid input fixture must independently have Email=`not an email`; no model constructor is invented here:

```csharp
private sealed class DocumentationEmailValidator : MustValidator<CreateOrder>
{
    public DocumentationEmailValidator()
    {
        RuleFor(x => x.Email, email => Must.Be.Email(email));
    }
}
private static void Documentation_object_invalid_email(CreateOrder invalidEmailCase)
{
    var result = new DocumentationEmailValidator().Validate(invalidEmailCase);
    Assert.False(result.Success);
    Assert.Contains(result.Failures, failure => failure.PropertyPath == "Email" &&
        failure.Code == MustCodes.Email.Address.Invalid);
}
```

Purpose: current object composition delegates to canonical Email through Must; input fixture `not an email`→failure at Email with stable current code. Existing child/null/indexed behavior is not inferred. Status source-inspected by Luna, not executed.

**Current API**, complete insertion method into existing `NumberAttributesTests.cs` with existing PositiveNumberAttribute/DataAnnotations/xUnit imports:

```csharp
private static void Documentation_data_annotations_invocation()
{
    var attribute = new PositiveNumberAttribute();
    var context = new ValidationContext(new object());
    Assert.Equal(ValidationResult.Success, attribute.GetValidationResult(1, context));
    Assert.NotEqual(ValidationResult.Success, attribute.GetValidationResult(0, context));
}
```

Purpose: actually invoke current attribute mechanism; 1→Success, 0→failure. Null is deliberately a separately specified adapter case. Status source-inspected, not executed.

All miniature excerpts are **Proposed illustration** and depend on the complete `Program.cs`/`Pilot.csproj`; link back to the canonical example contract above. Complete type declarations belong in namespace ReadinessIllustration and are documentation views of the existing canonical type, not additional implementations to paste alongside it. Statement blocks belong in Main or the matching Checks method. Existing canonical imports are System.Diagnostics, System.Globalization and System.Text.RegularExpressions plus SDK implicit usings. Every named dependency is defined in Program.cs. PowerShell excerpts belong in the explicitly named complete script's context. No third-party property/fuzz/mutation/benchmark package has been selected or assigned a fabricated version.

### W00 — 00-ci-and-evidence-trust

Purpose: fail closed against independently enumerated project/TFM scope, retaining 100% line and branch targets. Complete proposed script is `Verify-Evidence.ps1`; mandatory inputs ExpectedMatrix rows `{Project,Tfm}`, ArtifactIndex rows `{Project,Tfm,Path}` and independent ExpectedRevision. Each normalized artifact has `{Project,Tfm,Revision,Lines:{Covered,Total},Branches:{Covered,Total}}`. Duplicate expected keys, reused resolved report paths and mismatched artifact identity/revision reject. Numeric finite nonnegative integral Int64-range counts are accepted without parser-specific integer assumptions. The real expected matrix/normalizer remain unselected. Identity checks do not authenticate the producer. Zero branch denominator currently rejects; legitimately branch-free scopes require a reviewed policy, not an implied exception.

```powershell
$expected = @(Get-Content -LiteralPath $ExpectedMatrix -Raw | ConvertFrom-Json)
$index = @(Get-Content -LiteralPath $ArtifactIndex -Raw | ConvertFrom-Json)
if ($expected.Count -eq 0) { throw 'Expected project/TFM scope is empty.' }
foreach ($item in $expected) {
    if (-not $item.Project -or -not $item.Tfm) { throw 'Invalid expected scope row.' }
    $matches = @($index | Where-Object { $_.Project -ceq $item.Project -and $_.Tfm -ceq $item.Tfm })
    if ($matches.Count -ne 1) { throw 'Missing or duplicate artifact.' }
    $path = [string]$matches[0].Path
    if (-not $path -or -not (Test-Path -LiteralPath $path -PathType Leaf)) { throw 'Missing artifact file.' }
    $key = ConvertTo-Json -InputObject @($item.Project, $item.Tfm) -Compress
    if (-not $keys.Add($key)) { throw 'Duplicate expected project/TFM.' }
    $resolved = (Resolve-Path -LiteralPath $path).Path
    if (-not $paths.Add($resolved)) { throw 'Artifact file reused across expected identities.' }
    $artifact = Get-Content -LiteralPath $resolved -Raw | ConvertFrom-Json
    if ($artifact.Project -cne $item.Project -or $artifact.Tfm -cne $item.Tfm -or
        $artifact.Revision -cne $ExpectedRevision) { throw 'Artifact identity/revision mismatch.' }
    foreach ($kind in @('Lines', 'Branches')) {
        $total = Get-IntegralCount $artifact.$kind.Total
        $covered = Get-IntegralCount $artifact.$kind.Covered
        if ($total -le 0 -or $covered -gt $total -or $covered -ne $total) { throw 'Empty or below 100%.' }
    }
}
```

Independent outcomes: absent scope, missing/duplicate artifact, corrupt JSON, empty counts or Covered<Total throws; exact Covered=Total>0 for both metrics for every expected row passes. Inputs must use reviewed scope, not discovered successful artifacts. Status not executed; ordinary pilot pass is not coverage evidence.

### W01 — 01-semantic-specification

```csharp
(int? Input, bool Expected)[] cases =
    [(null, false), (int.MinValue, false), (-1, false),
     (0, false), (1, true), (int.MaxValue, true)];
foreach (var row in cases)
{
    var result = PositiveInt.Evaluate(row.Input);
    Checks.Require(result.Passed == row.Expected, "explicit semantic row");
    Checks.Require(result.Code == (row.Expected ? null :
        "illustration.number.not-positive"), "explicit code");
}
```

Purpose: literal null/boundary/valid truth rows, independent stable codes. Exact inputs/outputs are in the canonical contract and method; current paired TheoryData/MemberData insertion above supplies the actual Must example. Status pilot execution pending, current tests not run.

### W02 — 02-manifest-and-completeness

```csharp
internal sealed record Descriptor(string Id, string Code, Func<int?, Verdict> Evaluate);
internal static class DerivedManifest
{
    internal static readonly Descriptor[] Rules =
        [new(PositiveInt.Id, PositiveInt.FailureCode, PositiveInt.Evaluate)];
    internal static string[] Differences(IEnumerable<string> expectedIds) =>
        expectedIds.Except(Rules.Select(x => x.Id)).Select(x => "missing:" + x)
            .Concat(Rules.Select(x => x.Id).Except(expectedIds)
                .Select(x => "orphan:" + x)).Order(StringComparer.Ordinal).ToArray();
}
```

Purpose: proposed D07 descriptor points to the single static implementation. Input expected ID `illustration.number.positive` → no differences; seeded expected `illustration.absent` → exactly missing:illustration.absent and orphan:illustration.number.positive. No current public manifest is claimed. Status pending.

### W03 — 03-static-kernel-and-lowering

```csharp
internal static class PositiveInt
{
    internal const string Id = "illustration.number.positive";
    internal const string FailureCode = "illustration.number.not-positive";
    internal static Verdict Evaluate(int? value)
    {
        var passed = value is not null && value.Value > 0;
        return new(passed, passed ? null : FailureCode);
    }
}
```

Purpose: cheap complete evaluation by one static method without per-call descriptor allocation. Inputs 1/0 → true/false. Proposed miniature; source current NumberRules uses generic INumber under net8+; no runtime rule-object hierarchy proposed. Status pending.

### W04 — 04-failure-contracts-and-projections

```csharp
internal static class Projections
{
    internal static Verdict Must(int? value) => PositiveInt.Evaluate(value);
    internal static void Guard(int? value)
    {
        if (!PositiveInt.Evaluate(value).Passed)
            throw new ArgumentOutOfRangeException(nameof(value));
    }
    internal static string[] ObjectErrors(int? value) =>
        PositiveInt.Evaluate(value) is { Passed: false, Code: { } code }
            ? ["Value:" + code] : [];
}
```

Purpose: candidate D08 failure shape, safe object path and complement guard, all calling canonical authority. Input 1 → Must pass/no guard exception; input 0 → Must fail/guard exception/object `Value:illustration.number.not-positive`. Current Must/Guard/FV/DA excerpts above show verified names separately; they are not proved by this miniature. Existing MustFailure/MustValidationResult remain current object contract; protected RuleFor/RuleForEach and internal MustChildValidator are not invented public adapters. Status pending.

### W05 — 05-conformance-and-parity

```csharp
Checks.Require(Projections.Must(1).Passed, "Must positive");
Checks.Require(!Projections.Must(0).Passed, "Must zero");
Projections.Guard(1);
var threw = false;
try { Projections.Guard(0); }
catch (ArgumentOutOfRangeException) { threw = true; }
Checks.Require(threw, "Guard zero throws");
Checks.Require(Projections.ObjectErrors(0).SequenceEqual(
    new[] { "Value:illustration.number.not-positive" }), "object path/code");
```

Purpose: independent literal expectations for candidate projections. Add actual adapter cases in their existing paired Tests/TestData files, including null/indexed children/sync-async semantics; do not assume identical framework null handling. Same 1/0 expectations as W04; candidate status pending, actual frameworks unverified.

### W06 — 06-property-testing

```csharp
var random = new Random(731);
for (var i = 0; i < 1000; i++)
{
    var input = random.Next(-10000, 10001);
    var expected = Math.Sign(input) == 1;
    Checks.Require(PositiveInt.Evaluate(input).Passed == expected, "seeded sign oracle");
}
```

Purpose: repeatable seed731, 1,000 ints in [-10000,10000], independent Math.Sign oracle. Every comparison must agree. This bounded generated test is not a claim about a selected property-testing framework or all numeric types. D10 remains open. Status pending.

### W07 — 07-fuzz-testing

```csharp
(string Input, int Status)[] corpus =
    [("1", 200), ("0", 400), ("-1", 400), ("", 400),
     ("2147483648", 400), (new string('9', 129), 400)];
foreach (var row in corpus)
    Checks.Require(Boundary.Invoke(row.Input).Status == row.Status, "bounded replay row");
```

Purpose: bounded hostile replay corpus: `1`→200; `0`, `-1`, empty, int overflow and 129 digits→400. This is a replay harness, not coverage-guided fuzzing. Persist seed/corpus/tool evidence separately when a tool is selected. Status pending.

### W08 — 08-differential-testing

```csharp
var random = new Random(731);
for (var i = 0; i < 1000; i++)
{
    var input = random.Next(-10000, 10001);
    var expected = Math.Sign(input) == 1;
    Checks.Require(PositiveInt.Evaluate(input).Passed == expected, "independent sign oracle");
}
```

Purpose: BCL Math.Sign is an independent reference over the identical bounded int domain. Expected positive sign only; nullable boundary expectations live in literal W01 rows. Do not compare email syntax or dates against a reference with different semantics. Status pending.

### W09 — 09-targeted-mutation

```csharp
static bool SeededBoundaryMutant(int? value) => value is not null && value.Value >= 0;
Checks.Require(SeededBoundaryMutant(0), "mutant accepts zero");
Checks.Require(!PositiveInt.Evaluate(0).Passed, "explicit zero expectation kills mutant");
```

Purpose: explicit zero expectation kills seeded `>=0` boundary mutant; zero must fail canonical and pass mutant. This does not measure a selected mutation engine or score. Tool selection remains D10. Status pending.

### W10 — 10-performance-and-allocation

```csharp
const int iterations = 100000;
var timer = new Stopwatch();
var warmupConsumed = 0;
for (var i = 0; i < 1000; i++)
    warmupConsumed += PositiveInt.Evaluate(1).Passed ? 1 : 0;
Checks.Require(warmupConsumed == 1000, "warmup result consumed");
var consumed = 0;
timer.Start();
var before = GC.GetAllocatedBytesForCurrentThread();
for (var i = 0; i < iterations; i++)
    consumed += PositiveInt.Evaluate(1).Passed ? 1 : 0;
var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
timer.Stop();
Checks.Require(consumed == iterations, "timing result consumed");
Console.WriteLine($"TIMING illustration: consumed={consumed}; elapsedTicks={timer.ElapsedTicks}; allocatedBytes={allocated}");
```

Purpose: complete package-free timing probe consumes 100,000 static results after 1,000 consumed warmup calls. Stopwatch construction and warmup are outside the allocation-measurement window; no validator construction/reuse claim. Expected consumed=100000, elapsed/allocation observed, no threshold selected. Timer/counter overhead, JIT tiering and host noise remain uncontrolled; this is a smoke measurement, not BenchmarkDotNet, a regression budget or a publishable benchmark. BDN version/config remain unverified; D10/D11 open. Status pending.

### W11 — 11-trimming-and-native-consumers

Purpose: compile and execute the complete proposed minimal consumer above, then separate native proof. Windows host commands below explicitly choose win-x64; host architecture/toolchain availability must be checked by Luna. No success claimed until publish and binary execution both succeed.

Complete consumer project file `Pilot.csproj` (Program.cs is the canonical complete consumer source):

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
</Project>
```

```powershell
dotnet publish ./Pilot.csproj -c Release -r win-x64 --self-contained true -p:PublishAot=true -o ./native-out
if ($LASTEXITCODE -ne 0) { throw 'Native publish failed.' }
& ./native-out/Pilot.exe
if ($LASTEXITCODE -ne 0) { throw 'Native execution failed.' }
```

Expected same PASS checks and consumed=100000. Actual PineGuard adapter/native/trimming/reflection consumers require separate target-specific evidence. Status not run; D12 support/D26 sample design open.

### W12 — 12-public-api-and-contract-consistency

Purpose: review a new public API baseline then reject drift. Complete proposed `Verify-ApiBaseline.ps1` requires independently generated API text, not a handwritten production baseline or an assumed generator.

```powershell
$ErrorActionPreference = 'Stop'
foreach ($path in @($Baseline, $GeneratedApi)) {
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw "Missing API artifact: $path" }
    if ((Get-Item -LiteralPath $path).Length -eq 0) { throw "Empty API artifact: $path" }
}
$before = (Get-Content -LiteralPath $Baseline -Raw).Replace("`r`n", "`n")
$after = (Get-Content -LiteralPath $GeneratedApi -Raw).Replace("`r`n", "`n")
if ([string]::IsNullOrWhiteSpace($before) -or [string]::IsNullOrWhiteSpace($after)) { throw 'Whitespace-only API artifact.' }
if ($before -cne $after) { throw 'Public API drift requires review.' }
Write-Output 'PASS: generated API equals reviewed baseline.'
```

Input equal nonempty normalized texts→pass; missing/empty/different→throw. Existing target-specific APIs must produce separate reviewed artifacts. No current API generation tool selected; status not run.

### W13 — 13-dependencies-and-supply-chain

```powershell
$resolved = (Resolve-Path -LiteralPath $Project).Path
$directory = Split-Path -Parent $resolved
$lock = Join-Path $directory 'packages.lock.json'
if (-not (Test-Path -LiteralPath $lock -PathType Leaf)) { throw 'A reviewed lock file is required for this proposed gate.' }
dotnet restore $resolved --locked-mode
if ($LASTEXITCODE -ne 0) { throw 'Locked restore failed.' }
Write-Output 'PASS: locked restore.'
```

Purpose: complete proposed lock gate requires a reviewed packages.lock.json then runs locked restore, failing if absent/drift. The package-free pilot currently has no reviewed lock and must fail this prerequisite; do not imply repository lock files exist. Actual pins supplied by Luna: xUnit2.9.3/FV11.12.0/ErrorOr2.1.1/TestSDK18.10.1; CLI Node>=22/TS6.0.3/Vitest4.2.2. Status not run.

### W14 — 14-threat-model-and-security-claims

```csharp
Checks.Require(Boundary.Invoke(new string('9', 129)).Status == 400, "size rejection");
Checks.Require(Boundary.Invoke("2147483648").Status == 400, "overflow rejection");
var verdict = PositiveInt.Evaluate(-987654321);
Checks.Require(Diagnostics.Event(false, verdict) is null, "diagnostics opt-in");
Checks.Require(Diagnostics.Event(true, verdict) ==
    "rule=illustration.number.positive;code=illustration.number.not-positive", "exact allowlist");
```

Purpose: claim-specific tests only: oversize raw numeric input rejected before parsing and diagnostic output has exact allowlist. Overflow/129 digits→400; attempted -987654321 never appears in event. No general security claim or adversarial regex-timeout proof follows. Status pending.

### W15 — 15-regex-governance

```csharp
internal static class PatternProbe
{
    internal static RegexOutcome Evaluate(string input, string pattern, bool negate)
    {
        try
        {
            var matched = Regex.IsMatch(input, pattern, RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(20));
            return matched != negate ? RegexOutcome.Match : RegexOutcome.NoMatch;
        }
        catch (ArgumentException) { return RegexOutcome.OperationalError; }
        catch (RegexMatchTimeoutException) { return RegexOutcome.OperationalError; }
    }
}
```

Purpose: candidate D05 typed operational state keeps invalid pattern failure from becoming negated success. `[` on x→OperationalError under both negation settings; known match/nonmatch are explicit. Timeout catch uses same operational state, but deterministic timeout injection is not supplied, so timeout path is unverified. Candidate20ms is only miniature input; existing Email generated helper200ms must not be generalized to all StringRules. Status pending.

### W16 — 16-resource-limits

```csharp
internal static class Boundary
{
    internal const int InputLimit = 128;
    internal static Response Invoke(string raw) =>
        TryCreate(raw, out _) ? new(200, []) : new(400, ["illustration.invalid-input"]);
    internal static bool TryCreate(string raw, out PositiveAmount? amount)
    {
        amount = null;
        if (raw.Length > InputLimit || !int.TryParse(raw, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var number)) return false;
        return PositiveAmount.TryCreate(number, out amount);
    }
}
```

Purpose: provisional D11/D26 128-character raw-input budget, rejection before parsing. 129digits→400; this is not selected production size/error policy, and per-call allocations remain observable rather than budgeted. Status pending.

### W17 — 17-optional-diagnostics

```csharp
internal static class Diagnostics
{
    internal static string? Event(bool enabled, Verdict verdict) =>
        enabled && !verdict.Passed ? "rule=" + PositiveInt.Id + ";code=" + verdict.Code : null;
}
```

Purpose: candidate D15 opt-in defaults off, enabled output contains only fixed rule/code. Failed -987654321→null when off, exact safe event when on. No attempted value, free-form input, property value or sensitive parameter name in output. Current MustResult.Value is not logging consent. Status pending.

### W18 — 18-culture-time-and-determinism

```csharp
var saved = CultureInfo.CurrentCulture;
try
{
    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
    Checks.Require(Boundary.Invoke("1").Status == 200, "invariant parse");
    Checks.Require(DateOnly.FromDateTime(new FixedClock().GetUtcNow().UtcDateTime) ==
        new DateOnly(2030, 1, 2), "fixed UTC day");
    Checks.Require(DateOnly.TryParseExact("2030-01-02", "yyyy-MM-dd", CultureInfo.InvariantCulture,
        DateTimeStyles.None, out var date) && date == new DateOnly(2030, 1, 2), "date conversion");
}
finally { CultureInfo.CurrentCulture = saved; }
Checks.Require(ReferenceEquals(CultureInfo.CurrentCulture, saved), "culture restored");
```

Purpose: finally restores ambient culture, fixed TimeProvider provides UTC date, ISO string converts independently. fr-FR+`1`→200; fixed clock→2030-01-02; `2030-01-02`→same DateOnly; saved culture restored even on throw. Use existing FixedTimeProvider fixture for actual DateOnlyRules cases. Status pending.

### W19 — 19-validator-lifecycle-and-concurrency

```csharp
var successCount = 0;
Parallel.For(0, 1000, _ =>
{
    if (PositiveInt.Evaluate(1).Passed) Interlocked.Increment(ref successCount);
});
Checks.Require(successCount == 1000, "concurrent stateless calls");
```

Purpose: 1,000 concurrent calls of this stateless miniature→1,000 successes. This proves neither mutable current MustValidator sharing nor a selected validator reuse contract; D17 remains blocked. Framework lifecycle needs actual chosen-contract tests. Status pending.

### W20 — 20-samples-documentation-and-support

```csharp
var invalid = Boundary.Invoke("0");
Checks.Require(invalid.Status == 400 && invalid.ErrorCodes.SequenceEqual(
    new[] { "illustration.invalid-input" }), "API response");
Checks.Require(Boundary.TryCreate("1", out var amount) && amount is { Value: 1 }, "parse then invariant");
Checks.Require(!Boundary.TryCreate("0", out var invalidAmount) && invalidAmount is null, "domain rejection");
Checks.Require(!PositiveAmount.TryCreate(0, out var directInvalid) && directInvalid is null, "factory rejection");
IAmountHandler handler = new AmountHandler();
Checks.Require(handler.Handle("1").Status == 200 && handler.Handle("0").Status == 400, "handler boundary");
```

Purpose: executable teaching consumer, explicit response and construction boundary; not a support-matrix promise. Inputs1/0→200/400 with safe error code. Actual supported SDK/TFM/OS evidence, normal run, trim and native execution remain separate D12/D26 artifacts. Status pending.

### W20A — API-validation app subplan

```csharp
internal static class Boundary
{
    internal const int InputLimit = 128;
    internal static Response Invoke(string raw) =>
        TryCreate(raw, out _) ? new(200, []) : new(400, ["illustration.invalid-input"]);
    internal static bool TryCreate(string raw, out PositiveAmount? amount)
    {
        amount = null;
        if (raw.Length > InputLimit || !int.TryParse(raw, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var number)) return false;
        return PositiveAmount.TryCreate(number, out amount);
    }
}
```

Purpose: invoke candidate boundary then assert transport-neutral response. Input0→400+exact one code; no HTTP framework/endpoint API is claimed. Complete sample response record and method in Program.cs. Status pending.

### W20B — DDD-validation app subplan

```csharp
internal sealed class PositiveAmount
{
    internal int Value { get; }
    private PositiveAmount(int value) => Value = value;
    internal static bool TryCreate(int value, out PositiveAmount? amount)
    {
        amount = null;
        if (!PositiveInt.Evaluate(value).Passed) return false;
        amount = new PositiveAmount(value);
        return true;
    }
}
```

Purpose: raw input→invariant int conversion→factory→single authority→candidate amount. `1` creates an immutable amount with Value1; zero returns false with null. The sealed class/private constructor prevents ordinary construction bypass in this teaching sample; there is no claim about reflection or serialization. Choosing production constructor/failure shape stays D08/D26. Email and string→DateOnly roadmap examples use verified current APIs above rather than silently inventing semantic policy. Status pending.

### W20C — Clean-architecture-validation app subplan

```csharp
internal interface IAmountHandler
{
    Response Handle(string raw);
}
internal sealed class AmountHandler : IAmountHandler
{
    public Response Handle(string raw) => Boundary.Invoke(raw);
}
```

Purpose: minimal candidate handler/interface adapter calls boundary; no additional rule runtime hierarchy or DI/layer allocation is implied. Inputs1/0→200/400. Existing PineGuard public adapters still require actual consumer evidence. Status pending.

### W21 — 21-readiness-release-and-expansion

```powershell
foreach ($item in $expected) {
    $matches = @($index | Where-Object { $_.Project -ceq $item.Project -and $_.Tfm -ceq $item.Tfm })
    if ($matches.Count -ne 1) { throw 'Missing or duplicate required readiness artifact.' }
    $path = [string]$matches[0].Path
    if (-not $path -or -not (Test-Path -LiteralPath $path -PathType Leaf)) { throw 'Missing artifact file.' }
    $key = ConvertTo-Json -InputObject @($item.Project, $item.Tfm) -Compress
    if (-not $keys.Add($key)) { throw 'Duplicate expected project/TFM.' }
    $resolved = (Resolve-Path -LiteralPath $path).Path
    if (-not $paths.Add($resolved)) { throw 'Artifact file reused across expected identities.' }
    $artifact = Get-Content -LiteralPath $resolved -Raw | ConvertFrom-Json
    if ($artifact.Project -cne $item.Project -or $artifact.Tfm -cne $item.Tfm -or
        $artifact.Revision -cne $ExpectedRevision) { throw 'Artifact identity/revision mismatch.' }
    foreach ($kind in @('Lines', 'Branches')) {
        $total = Get-IntegralCount $artifact.$kind.Total
        $covered = Get-IntegralCount $artifact.$kind.Covered
        if ($total -le 0 -or $covered -gt $total -or $covered -ne $total) { throw 'Empty or below 100%.' }
    }
}
```

Purpose: same fail-closed artifact gate before readiness, with W00 missing/corrupt/empty/under100% rejection cases. A successful miniature console run cannot substitute for required project/TFM/coverage/bench/native/security artifacts. Normalized artifact schema is proposed; no release approval decision is made. Status not run.

### W22 — 22-naming-taxonomy-and-abstractions

Purpose: concrete source mapping plus D07 candidate descriptor ID; D22 paths remain open. Current `IsPositive`→`NumberRules.IsPositive`→`Must.Be.Positive`→complement `Guard.Against.ZeroOrNegative`→`MustCodes.Number.Sign.NotPositive`; FV `.Positive()` and DA `[PositiveNumber]` are source-backed adapters. The candidate teaching descriptor is deliberately named illustration.number.positive and is not the selected manifest taxonomy.

```csharp
var descriptor = DerivedManifest.Rules.Single();
Checks.Require(descriptor.Id == "illustration.number.positive" &&
    descriptor.Code == "illustration.number.not-positive", "candidate name mapping");
```

Input descriptor→exact candidate literals; no CLI vocabulary file becomes an executable validation manifest. Status pending.

## Complete portable source files

### Pilot.csproj

````xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
</Project>
````

### Program.cs

````csharp
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ReadinessIllustration;

// Proposed teaching miniature only: no PineGuard namespace or new public API.
internal static class Program
{
    private static int Main()
    {
        Checks.Semantics();
        Checks.Manifest();
        Checks.Projections();
        Checks.SeededDomain();
        Checks.Replay();
        Checks.Mutation();
        Checks.RegexErrors();
        Checks.Privacy();
        Checks.CultureAndTime();
        Checks.ConcurrentReuse();
        Checks.Applications();
        Console.WriteLine("PASS: semantic, manifest, projections, seeded-domain, replay, mutation, regex, privacy, determinism, concurrency, applications");
        Timing.Run();
        return 0;
    }
}

internal readonly record struct Verdict(bool Passed, string? Code);

internal static class PositiveInt
{
    internal const string Id = "illustration.number.positive";
    internal const string FailureCode = "illustration.number.not-positive";

    // The sole executable definition of the proposed miniature's predicate.
    internal static Verdict Evaluate(int? value)
    {
        var passed = value is not null && value.Value > 0;
        return new(passed, passed ? null : FailureCode);
    }
}

internal sealed record Descriptor(string Id, string Code, Func<int?, Verdict> Evaluate);

internal static class DerivedManifest
{
    internal static readonly Descriptor[] Rules =
    [new(PositiveInt.Id, PositiveInt.FailureCode, PositiveInt.Evaluate)];

    // A caller must independently supply expected IDs; deriving expectations from
    // Rules would make the completeness test unable to discover missing entries.
    internal static string[] Differences(IEnumerable<string> expectedIds) =>
        expectedIds.Except(Rules.Select(x => x.Id))
            .Select(x => "missing:" + x)
            .Concat(Rules.Select(x => x.Id).Except(expectedIds)
                .Select(x => "orphan:" + x)).Order(StringComparer.Ordinal).ToArray();
}

internal static class Projections
{
    internal static Verdict Must(int? value) => PositiveInt.Evaluate(value);

    internal static void Guard(int? value)
    {
        if (!PositiveInt.Evaluate(value).Passed)
            throw new ArgumentOutOfRangeException(nameof(value));
    }

    internal static string[] ObjectErrors(int? value) =>
        PositiveInt.Evaluate(value) is { Passed: false, Code: { } code }
            ? ["Value:" + code] : [];
}

internal readonly record struct Response(int Status, string[] ErrorCodes);
internal sealed class PositiveAmount
{
    internal int Value { get; }

    private PositiveAmount(int value) => Value = value;

    internal static bool TryCreate(int value, out PositiveAmount? amount)
    {
        amount = null;
        if (!PositiveInt.Evaluate(value).Passed) return false;
        amount = new PositiveAmount(value);
        return true;
    }
}

internal static class Boundary
{
    // 128 is a provisional illustration budget (D11/D26), not a selected limit.
    internal const int InputLimit = 128;

    internal static Response Invoke(string raw) =>
        TryCreate(raw, out _) ? new(200, []) : new(400, ["illustration.invalid-input"]);

    internal static bool TryCreate(string raw, out PositiveAmount? amount)
    {
        amount = null;
        if (raw.Length > InputLimit || !int.TryParse(raw, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var number))
            return false;
        return PositiveAmount.TryCreate(number, out amount);
    }
}

internal interface IAmountHandler
{
    Response Handle(string raw);
}

internal sealed class AmountHandler : IAmountHandler
{
    public Response Handle(string raw) => Boundary.Invoke(raw);
}

internal enum RegexOutcome { Match, NoMatch, OperationalError }

internal static class PatternProbe
{
    // Candidate status model for D05; it does not settle PineGuard negation policy.
    internal static RegexOutcome Evaluate(string input, string pattern, bool negate)
    {
        try
        {
            var matched = Regex.IsMatch(input, pattern, RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(20));
            return matched != negate ? RegexOutcome.Match : RegexOutcome.NoMatch;
        }
        catch (ArgumentException) { return RegexOutcome.OperationalError; }
        catch (RegexMatchTimeoutException) { return RegexOutcome.OperationalError; }
    }
}

internal static class Diagnostics
{
    internal static string? Event(bool enabled, Verdict verdict) =>
        enabled && !verdict.Passed ? "rule=" + PositiveInt.Id + ";code=" + verdict.Code : null;
}

internal sealed class FixedClock : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => new(2030, 1, 2, 0, 0, 0, TimeSpan.Zero);
}

internal static class Checks
{
    internal static void Require(bool actual, string name)
    {
        if (!actual) throw new InvalidOperationException("Check failed: " + name);
    }

    internal static void Semantics()
    {
        (int? Input, bool Expected)[] cases =
            [(null, false), (int.MinValue, false), (-1, false), (0, false), (1, true), (int.MaxValue, true)];
        foreach (var row in cases)
        {
            var result = PositiveInt.Evaluate(row.Input);
            Require(result.Passed == row.Expected, "explicit semantic row");
            Require(result.Code == (row.Expected ? null : "illustration.number.not-positive"), "explicit code");
        }
    }

    internal static void Manifest()
    {
        Require(DerivedManifest.Differences(["illustration.number.positive"]).Length == 0, "manifest baseline");
        Require(DerivedManifest.Differences(["illustration.absent"])
            .SequenceEqual(new[] { "missing:illustration.absent", "orphan:illustration.number.positive" }), "seeded manifest drift");
        Require(DerivedManifest.Rules.Single().Evaluate(1).Passed, "executable descriptor");
    }

    internal static void Projections()
    {
        Require(ReadinessIllustration.Projections.Must(1).Passed, "Must positive");
        Require(!ReadinessIllustration.Projections.Must(0).Passed, "Must zero");
        ReadinessIllustration.Projections.Guard(1);
        var threw = false;
        try { ReadinessIllustration.Projections.Guard(0); }
        catch (ArgumentOutOfRangeException) { threw = true; }
        Require(threw, "Guard zero throws");
        Require(ReadinessIllustration.Projections.ObjectErrors(0)
            .SequenceEqual(new[] { "Value:illustration.number.not-positive" }), "object path/code");
    }

    internal static void SeededDomain()
    {
        var random = new Random(731);
        for (var i = 0; i < 1000; i++)
        {
            var input = random.Next(-10000, 10001);
            // Independent reference in exactly this finite integer domain.
            var expected = Math.Sign(input) == 1;
            Require(PositiveInt.Evaluate(input).Passed == expected, "seeded sign oracle");
        }
    }

    internal static void Replay()
    {
        (string Input, int Status)[] corpus =
            [("1", 200), ("0", 400), ("-1", 400), ("", 400), ("2147483648", 400), (new string('9', 129), 400)];
        foreach (var row in corpus)
            Require(Boundary.Invoke(row.Input).Status == row.Status, "bounded replay row");
    }

    internal static void Mutation()
    {
        static bool SeededBoundaryMutant(int? value) => value is not null && value.Value >= 0;
        Require(SeededBoundaryMutant(0), "mutant accepts zero");
        Require(!PositiveInt.Evaluate(0).Passed, "explicit zero expectation kills mutant");
    }

    internal static void RegexErrors()
    {
        Require(PatternProbe.Evaluate("x", "[", false) == RegexOutcome.OperationalError, "invalid pattern");
        Require(PatternProbe.Evaluate("x", "[", true) == RegexOutcome.OperationalError, "negation cannot convert error");
        Require(PatternProbe.Evaluate("abc", "^abc$", false) == RegexOutcome.Match, "known match");
        Require(PatternProbe.Evaluate("abc", "^abc$", true) == RegexOutcome.NoMatch, "known negation");
    }

    internal static void Privacy()
    {
        var verdict = PositiveInt.Evaluate(-987654321);
        Require(Diagnostics.Event(false, verdict) is null, "diagnostics opt-in");
        Require(Diagnostics.Event(true, verdict) ==
            "rule=illustration.number.positive;code=illustration.number.not-positive", "diagnostics exact allowlist");
    }

    internal static void CultureAndTime()
    {
        var saved = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            Require(Boundary.Invoke("1").Status == 200, "invariant parse");
            Require(DateOnly.FromDateTime(new FixedClock().GetUtcNow().UtcDateTime) ==
                new DateOnly(2030, 1, 2), "fixed UTC day");
            Require(DateOnly.TryParseExact("2030-01-02", "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date) && date == new DateOnly(2030, 1, 2), "date conversion");
        }
        finally { CultureInfo.CurrentCulture = saved; }
        Require(ReferenceEquals(CultureInfo.CurrentCulture, saved), "culture restored");
    }

    internal static void ConcurrentReuse()
    {
        // Only this stateless miniature's contract; no claim about existing validators.
        var successCount = 0;
        Parallel.For(0, 1000, _ =>
        {
            if (PositiveInt.Evaluate(1).Passed) Interlocked.Increment(ref successCount);
        });
        Require(successCount == 1000, "concurrent stateless calls");
    }

    internal static void Applications()
    {
        var invalid = Boundary.Invoke("0");
        Require(invalid.Status == 400 && invalid.ErrorCodes.SequenceEqual(new[] { "illustration.invalid-input" }), "API response");
        Require(Boundary.TryCreate("1", out var amount) && amount is { Value: 1 }, "parse then invariant");
        Require(!Boundary.TryCreate("0", out var invalidAmount) && invalidAmount is null, "domain rejection");
        Require(!PositiveAmount.TryCreate(0, out var directInvalid) && directInvalid is null, "factory rejection");
        IAmountHandler handler = new AmountHandler();
        Require(handler.Handle("1").Status == 200 && handler.Handle("0").Status == 400, "handler boundary");
    }
}

internal static class Timing
{
    internal static void Run()
    {
        const int iterations = 100000;
        var timer = new Stopwatch();
        var warmupConsumed = 0;
        for (var i = 0; i < 1000; i++)
            warmupConsumed += PositiveInt.Evaluate(1).Passed ? 1 : 0;
        Checks.Require(warmupConsumed == 1000, "warmup result consumed");
        var consumed = 0;
        timer.Start();
        var before = GC.GetAllocatedBytesForCurrentThread();
        // Repeated static evaluation; no validator construction or shared-object reuse claim.
        for (var i = 0; i < iterations; i++)
            consumed += PositiveInt.Evaluate(1).Passed ? 1 : 0;
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        timer.Stop();
        Checks.Require(consumed == iterations, "timing result consumed");
        Console.WriteLine($"TIMING illustration: consumed={consumed}; elapsedTicks={timer.ElapsedTicks}; allocatedBytes={allocated}");
    }
}
````

### Verify-Evidence.ps1

````powershell
param(
    [Parameter(Mandatory)][string]$ExpectedMatrix,
    [Parameter(Mandatory)][string]$ArtifactIndex,
    [Parameter(Mandatory)][string]$ExpectedRevision
)
$ErrorActionPreference = 'Stop'
function Get-IntegralCount($Value) {
    $numeric = $Value -is [byte] -or $Value -is [sbyte] -or $Value -is [short] -or
        $Value -is [ushort] -or $Value -is [int] -or $Value -is [uint] -or
        $Value -is [long] -or $Value -is [ulong] -or $Value -is [float] -or
        $Value -is [double] -or $Value -is [decimal]
    if (-not $numeric) { throw 'Count must be numeric.' }
    try { $count = [decimal]$Value } catch { throw 'Invalid finite count.' }
    if ($count -lt 0 -or $count -gt [long]::MaxValue -or [decimal]::Truncate($count) -ne $count) {
        throw 'Count must be a nonnegative integral Int64-range value.'
    }
    return [long]$count
}
if ([string]::IsNullOrWhiteSpace($ExpectedRevision)) { throw 'Expected revision is required.' }
# Proposed normalized evidence contract: scope/revision supplied independently.
$expected = @(Get-Content -LiteralPath $ExpectedMatrix -Raw | ConvertFrom-Json)
$index = @(Get-Content -LiteralPath $ArtifactIndex -Raw | ConvertFrom-Json)
if ($expected.Count -eq 0) { throw 'Expected project/TFM scope is empty.' }
$keys = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$paths = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
foreach ($item in $expected) {
    if ($item.Project -isnot [string] -or $item.Tfm -isnot [string] -or
        [string]::IsNullOrWhiteSpace($item.Project) -or [string]::IsNullOrWhiteSpace($item.Tfm)) { throw 'Invalid scope identity.' }
    $key = ConvertTo-Json -InputObject @($item.Project, $item.Tfm) -Compress
    if (-not $keys.Add($key)) { throw 'Duplicate expected project/TFM.' }
    $matches = @($index | Where-Object { $_.Project -ceq $item.Project -and $_.Tfm -ceq $item.Tfm })
    if ($matches.Count -ne 1) { throw "Missing or duplicate artifact: $($item.Project)/$($item.Tfm)" }
    $path = [string]$matches[0].Path
    if (-not $path -or -not (Test-Path -LiteralPath $path -PathType Leaf)) { throw 'Missing artifact file.' }
    $resolved = (Resolve-Path -LiteralPath $path).Path
    if (-not $paths.Add($resolved)) { throw 'Artifact file reused across expected identities.' }
    $artifact = Get-Content -LiteralPath $resolved -Raw | ConvertFrom-Json
    if ($artifact.Project -cne $item.Project -or $artifact.Tfm -cne $item.Tfm -or
        $artifact.Revision -cne $ExpectedRevision) { throw 'Artifact identity/revision mismatch.' }
    foreach ($kind in @('Lines', 'Branches')) {
        $total = Get-IntegralCount $artifact.$kind.Total
        $covered = Get-IntegralCount $artifact.$kind.Covered
        if ($total -le 0 -or $covered -gt $total -or $covered -ne $total) {
            throw "$kind coverage is empty or below 100%."
        }
    }
}
Write-Output 'PASS: every expected project/TFM artifact has 100% line and branch coverage.'
````

### Verify-ApiBaseline.ps1

````powershell
param(
    [Parameter(Mandatory)][string]$Baseline,
    [Parameter(Mandatory)][string]$GeneratedApi
)
$ErrorActionPreference = 'Stop'
# Requires independently generated public API text; this is not an API generator.
foreach ($path in @($Baseline, $GeneratedApi)) {
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw "Missing API artifact: $path" }
    if ((Get-Item -LiteralPath $path).Length -eq 0) { throw "Empty API artifact: $path" }
}
$before = (Get-Content -LiteralPath $Baseline -Raw).Replace("`r`n", "`n")
$after = (Get-Content -LiteralPath $GeneratedApi -Raw).Replace("`r`n", "`n")
if ([string]::IsNullOrWhiteSpace($before) -or [string]::IsNullOrWhiteSpace($after)) { throw 'Whitespace-only API artifact.' }
if ($before -cne $after) { throw 'Public API drift requires review.' }
Write-Output 'PASS: generated API equals reviewed baseline.'
````

### Verify-Restore.ps1

````powershell
param([Parameter(Mandatory)][string]$Project)
$ErrorActionPreference = 'Stop'
$resolved = (Resolve-Path -LiteralPath $Project).Path
$directory = Split-Path -Parent $resolved
$lock = Join-Path $directory 'packages.lock.json'
if (-not (Test-Path -LiteralPath $lock -PathType Leaf)) { throw 'A reviewed lock file is required for this proposed gate.' }
dotnet restore $resolved --locked-mode
if ($LASTEXITCODE -ne 0) { throw 'Locked restore failed.' }
Write-Output 'PASS: locked restore.'
````

### Verify-GovernanceExamples.ps1

````powershell
param([string]$FixtureDirectory = (Join-Path $PSScriptRoot 'governance-fixtures'))
$ErrorActionPreference = 'Stop'
New-Item -ItemType Directory -Path $FixtureDirectory -Force | Out-Null
$matrix = Join-Path $FixtureDirectory 'matrix.json'
$index = Join-Path $FixtureDirectory 'index.json'
$report = Join-Path $FixtureDirectory 'report.json'
$baseline = Join-Path $FixtureDirectory 'baseline.txt'
$generated = Join-Path $FixtureDirectory 'generated.txt'
function Write-Json($Path, $Value) { ConvertTo-Json -InputObject $Value -Depth 8 | Set-Content -LiteralPath $Path -Encoding UTF8 }
function New-Report {
    return [pscustomobject]@{ Project='Pilot'; Tfm='net10.0'; Revision='illustration-r1'; Lines=@{Covered=3;Total=3}; Branches=@{Covered=2;Total=2} }
}
function Reset-Evidence {
    Write-Json $matrix @(@{Project='Pilot';Tfm='net10.0'})
    Write-Json $index @(@{Project='Pilot';Tfm='net10.0';Path=$report})
    Write-Json $report (New-Report)
}
function Invoke-Evidence { & (Join-Path $PSScriptRoot 'Verify-Evidence.ps1') -ExpectedMatrix $matrix -ArtifactIndex $index -ExpectedRevision 'illustration-r1' }
function Invoke-Api { & (Join-Path $PSScriptRoot 'Verify-ApiBaseline.ps1') -Baseline $baseline -GeneratedApi $generated }
function Assert-Rejected([scriptblock]$Action, [string]$Name) {
    $rejected = $false
    try { & $Action | Out-Null } catch { $rejected = $true }
    if (-not $rejected) { throw "Expected rejection: $Name" }
}
Reset-Evidence
Invoke-Evidence
$bad = New-Report; $bad.Project = 'Other'; Write-Json $report $bad
Assert-Rejected { Invoke-Evidence } 'wrong identity'
Reset-Evidence
$bad = New-Report; $bad.Revision = 'wrong'; Write-Json $report $bad
Assert-Rejected { Invoke-Evidence } 'wrong revision'
Reset-Evidence
$bad = New-Report; $bad.Lines.Covered = 2; Write-Json $report $bad
Assert-Rejected { Invoke-Evidence } 'under 100 percent'
Reset-Evidence
$bad = New-Report; $bad.Branches.Total = '2'; Write-Json $report $bad
Assert-Rejected { Invoke-Evidence } 'invalid count type'
Reset-Evidence
Write-Json $matrix @(@{Project='Pilot';Tfm='net10.0'}, @{Project='Pilot';Tfm='net10.0'})
Assert-Rejected { Invoke-Evidence } 'duplicate expected identity'
Reset-Evidence
Write-Json $index @(@{Project='Pilot';Tfm='net10.0';Path=(Join-Path $FixtureDirectory 'absent.json')})
Assert-Rejected { Invoke-Evidence } 'missing artifact'
'public class Reviewed {}' | Set-Content -LiteralPath $baseline -Encoding UTF8
'public class Reviewed {}' | Set-Content -LiteralPath $generated -Encoding UTF8
Invoke-Api
' ' | Set-Content -LiteralPath $generated -Encoding UTF8
Assert-Rejected { Invoke-Api } 'whitespace API'
'public class Changed {}' | Set-Content -LiteralPath $generated -Encoding UTF8
Assert-Rejected { Invoke-Api } 'API drift'
Assert-Rejected { & (Join-Path $PSScriptRoot 'Verify-ApiBaseline.ps1') -Baseline $baseline -GeneratedApi (Join-Path $FixtureDirectory 'absent-api.txt') } 'missing API'
$project = Join-Path $FixtureDirectory 'NoLock.csproj'
'<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>' | Set-Content -LiteralPath $project -Encoding UTF8
Assert-Rejected { & (Join-Path $PSScriptRoot 'Verify-Restore.ps1') -Project $project } 'missing lock before network'
Write-Output 'PASS: synthetic governance success and rejection fixtures.'
````
