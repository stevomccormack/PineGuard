# Multi-Target Framework Support for PineGuard

## Context

PineGuard currently targets `net10.0` only. To maximize enterprise adoption while keeping a
single source tree, we're adding support for `net6.0`, `net8.0`, and `netstandard2.1` using
conditional compilation and small compatibility shims where the platform truly differs.

`netstandard2.1` stays as the widest-compatibility asset. `DateOnly`/`TimeOnly` validators will
be intentionally omitted from `netstandard2.1` because those BCL types are not available there
(Option B — progressive API surface). `net6.0+` assets will expose the full modern date/time
surface. FluentValidation will use a single compatible version across all targets (Option A —
single version), pinned to `11.11.0`.

**Target TFMs:** `netstandard2.1;net6.0;net8.0;net10.0`

## Locked Decisions

- **Single codebase:** one source tree, no per-framework forks.
- **Compatibility floor:** `netstandard2.1` remains in the package for widest consumer reach.
- **Progressive API surface:** `DateOnly` / `TimeOnly` APIs are available on `net6.0+` only.
- **No broad date/time backport:** do **not** invent or polyfill `DateOnly` / `TimeOnly` into
  `netstandard2.1`.
- **Minimal compatibility shims only:** hand-rolled `IsExternalInit`,
  `CallerArgumentExpressionAttribute`, and internal `ThrowHelper` support; no PolySharp.
- **Modern performance APIs stay internal:** `FrozenDictionary` / `FrozenSet` are optimizations
  with standard collection fallbacks on lower TFMs.
- **Single FluentValidation line:** use `FluentValidation` `11.11.0` repo-wide.

## Support Policy

- **`netstandard2.1`** — widest compatibility asset; reduced API surface by design.
- **`net6.0`** — compatibility asset for consumers that have native `DateOnly` / `TimeOnly`.
- **`net8.0`** — first-class LTS asset with modern optimizations.
- **`net10.0`** — latest/current optimized asset.

---

## Compatibility Matrix (Research Summary)

| API / Feature | netstandard2.1 | net6.0 | net8.0 | net10.0 |
|---|:-:|:-:|:-:|:-:|
| FrozenDictionary | NO | NO | YES | YES |
| DateOnly / TimeOnly (native BCL types) | NO | YES | YES | YES |
| ArgumentNullException.ThrowIfNull | NO | YES | YES | YES |
| StringSplitOptions.TrimEntries | NO | YES | YES | YES |
| readonly record struct | Needs polyfill | YES | YES | YES |
| FluentValidation 11.11.0 | YES | YES | YES | YES |
| ImplicitUsings (SDK) | Limited | YES | YES | YES |

**Public API safety:** FrozenDictionary is internal-only (providers expose `IReadOnlyCollection<T>`).
`CsvColumnSchema` is the only public `readonly record struct` — needs `IsExternalInit` polyfill.

**API parity note:** full API parity across all TFMs is not a goal. The `netstandard2.1` asset is
intentionally smaller because it cannot expose APIs that depend on `DateOnly` / `TimeOnly`.

---

## Phase 1: Project Infrastructure

### 1a. Create `Directory.Build.props` (solution root)

Centralize shared properties currently duplicated across all csproj files:

```xml
<Project>
  <PropertyGroup>
    <TargetFrameworks>netstandard2.1;net6.0;net8.0;net10.0</TargetFrameworks>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
</Project>
```

### 1b. Update all src `.csproj` files

- Remove `<TargetFramework>net10.0</TargetFramework>` (inherited from Directory.Build.props)
- Remove duplicated `<Nullable>`, `<ImplicitUsings>`, `<LangVersion>` (inherited)
- Keep project-specific settings (InternalsVisibleTo, PackageReferences, ProjectReferences)

**Files (5):**
- `src/PineGuard.Core/PineGuard.Core.csproj`
- `src/PineGuard.MustClauses/PineGuard.MustClauses.csproj`
- `src/PineGuard.GuardClauses/PineGuard.GuardClauses.csproj`
- `src/PineGuard.DataAnnotations/PineGuard.DataAnnotations.csproj`
- `src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj`

### 1c. Update test `.csproj` files

Test projects target `net6.0;net8.0;net10.0` (NOT netstandard2.1 — tests are executables).
Override `TargetFrameworks` in test csproj files or use a `tests/Directory.Build.props`.

**Files (7):**
- `tests/PineGuard.Testing/PineGuard.Testing.csproj`
- `tests/PineGuard.Testing.UnitTests/PineGuard.Testing.UnitTests.csproj`
- `tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj`
- `tests/PineGuard.MustClauses.UnitTests/PineGuard.MustClauses.UnitTests.csproj`
- `tests/PineGuard.GuardClauses.UnitTests/PineGuard.GuardClauses.UnitTests.csproj`
- `tests/PineGuard.DataAnnotations.UnitTests/PineGuard.DataAnnotations.UnitTests.csproj`
- `tests/PineGuard.FluentValidation.UnitTests/PineGuard.FluentValidation.UnitTests.csproj`

### 1d. Update `Directory.Packages.props`

- Change `FluentValidation` from `12.1.1` → `11.11.0`

---

## Phase 2: Polyfills (for netstandard2.1)

Minimal hand-rolled compatibility shims only (no broad API backports, no PolySharp).
Compiler-feature polyfills must exist in each compiling assembly, so we use `<Compile Link>` to
share a single source file.

### 2a. Polyfill source files in `src/PineGuard.Core/Polyfills/`

**File 1:** `IsExternalInit.cs` — needed for `readonly record struct` on netstandard2.1

```csharp
#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices;

internal static class IsExternalInit;
#endif
```

**File 2:** `CallerArgumentExpressionAttribute.cs` — needed for ThrowHelper on netstandard2.1

```csharp
#if !NET6_0_OR_GREATER
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
{
    public string ParameterName { get; } = parameterName;
}
#endif
```

### 2b. Link polyfills into other projects that need them

Each src csproj that declares records or uses ThrowHelper adds:

```xml
<ItemGroup Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net5.0'))">
  <Compile Include="..\PineGuard.Core\Polyfills\IsExternalInit.cs" Link="Polyfills\IsExternalInit.cs" />
</ItemGroup>
```

Projects needing polyfills: Core (owns files), FluentValidation (uses ThrowIfNull pattern).
MustClauses, GuardClauses, DataAnnotations — verify during implementation.

### 2c. `ThrowHelper` for `ArgumentNullException.ThrowIfNull`

Can't polyfill a static method on a BCL type. Instead, add an internal helper in Core
and update the 36 Core call sites. FluentValidation has only 2-4 call sites — use inline
`#if NET6_0_OR_GREATER` / `#else` guards there.

**File:** `src/PineGuard.Core/Internal/ThrowHelper.cs`

```csharp
namespace PineGuard.Core.Internal;

internal static class ThrowHelper
{
    /// <summary>Throws if <paramref name="argument"/> is null.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNull(
        [NotNull] object? argument,
        [CallerArgumentExpression("argument")] string? paramName = null)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null) throw new ArgumentNullException(paramName);
#endif
    }
}
```

**Migration:** `ArgumentNullException.ThrowIfNull(x)` → `ThrowHelper.ThrowIfNull(x)` in Core (36 sites).
FluentValidation (2-4 sites): inline `#if NET6_0_OR_GREATER` / `#else` guards.

**Non-goal:** do not try to polyfill `DateOnly`, `TimeOnly`, `FrozenDictionary`, or other modern
BCL surface area into `netstandard2.1`. Use conditional compilation and intentional API omission
instead.

---

## Phase 3: Conditional Compilation — FrozenDictionary

Guard with `#if NET8_0_OR_GREATER`, fallback to `Dictionary<K,V>` / `HashSet<T>`.

Any source files using `FrozenDictionary` or `FrozenSet` need conditional compilation guards.

**Pattern:**
```csharp
#if NET8_0_OR_GREATER
    using System.Collections.Frozen;
#endif

    private static readonly
#if NET8_0_OR_GREATER
        FrozenDictionary<string, TValue>
#else
        Dictionary<string, TValue>
#endif
        LookupByKey = items
#if NET8_0_OR_GREATER
            .ToFrozenDictionary(x => x.Key);
#else
            .ToDictionary(x => x.Key);
#endif
```

---

## Phase 4: Conditional Compilation — DateOnly / TimeOnly

Entire files wrapped in `#if NET6_0_OR_GREATER` … `#endif`. These files are cleanly
separated (one type per file or partial file) so file-level guards work.

### Core (18 files)
- `Common/DateOnlyRange.cs`
- `Common/TimeOnlyRange.cs`
- `Rules/DateOnlyRules.cs`
- `Rules/DateOnlyRangeRules.cs`
- `Rules/TimeOnlyRules.cs`
- `Rules/TimeOnlyRangeRules.cs`
- `Rules/StringRules.DateOnly.cs`
- `Rules/StringRules.TimeOnly.cs`
- `Utils/TimeOnlyUtility.cs`
- `Utils/StringUtility.DateOnly.cs`
- `Utils/StringUtility.TimeOnly.cs`
- `Utils/StringUtility.DateOnlyRange.cs`
- `Utils/StringUtility.TimeOnlyRange.cs`
- `Common/CsvColumnType.cs` (partial — DateOnly/TimeOnly enum members only)
- `Common/CsvColumnSchema.cs` (verify — may reference DateOnly in schema)

### MustClauses (7 files)
- `MustDateOnlyClauses.cs`
- `MustDateOnlyRangeClauses.cs`
- `MustTimeOnlyClauses.cs`
- `MustTimeOnlyRangeClauses.cs`
- `MustStringDateOnlyClauses.cs`
- `MustStringTimeOnlyClauses.cs`

### GuardClauses (7 files)
- `GuardDateOnlyClauses.cs`
- `GuardDateOnlyRangeClauses.cs`
- `GuardTimeOnlyClauses.cs`
- `GuardTimeOnlyRangeClauses.cs`
- `GuardStringDateOnlyClauses.cs`
- `GuardStringTimeOnlyClauses.cs`

### DataAnnotations (4 files)
- `DateOnlyAttributes.cs`
- `TimeOnlyAttributes.cs`
- `StringDateOnlyAttributes.cs`
- `StringTimeOnlyAttributes.cs`

### FluentValidation (6 files)
- `FluentDateOnlyExtensions.cs`
- `FluentDateOnlyRangeExtensions.cs`
- `FluentTimeOnlyExtensions.cs`
- `FluentTimeOnlyRangeExtensions.cs`
- `FluentStringDateOnlyExtensions.cs`
- `FluentStringTimeOnlyExtensions.cs`

**Total: ~39 source files** (exact count after verification of partial/mixed files)

---

## Phase 5: Conditional Compilation — Minor APIs

### 5a. `StringSplitOptions.TrimEntries` (2 call sites)

**File:** `src/PineGuard.Core/Utils/NetworkUtility.cs`

```csharp
#if NET5_0_OR_GREATER
    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
#else
    StringSplitOptions.RemoveEmptyEntries
#endif
```

Add `.Trim()` to the split results on the `#else` path to preserve behavior.

---

## Phase 6: Test Projects

### 6a. DateOnly/TimeOnly test files

Same `#if NET6_0_OR_GREATER` wrapping for test files and test data/fixtures:

- All `*DateOnly*Tests.cs`, `*TimeOnly*Tests.cs`, `*DateOnly*TestData.cs`, `*TimeOnly*TestData.cs`
- Fixture files: `DateOnlyRulesFixtures.cs`, `TimeOnlyRulesFixtures.cs`, etc.
- `StringRulesFixtures.DateOnly.cs`, `StringRulesFixtures.TimeOnly.cs`

### 6b. PineGuard.Testing base classes

Verify no .NET 8+ APIs in base test infrastructure (`BaseUnitTest`, etc.).
Research confirmed: no .NET 8+ specific APIs in PineGuard.Testing beyond fixtures.

---

## Phase 7: Build Scripts

### 7a. `tools/testing/Run-Tests.ps1`
- Add optional `-Framework` parameter
- Pass `--framework <tfm>` to `dotnet test` when specified

### 7b. `tools/code-coverage/xplat/Gen-CoverageReport.ps1`
- No changes needed — `dotnet test` without `--framework` runs all TFMs automatically

---

## Phase 8: Verification

1. `dotnet build` — all TFMs, zero warnings
2. `dotnet test` — runs on net6.0, net8.0, net10.0
3. Verify netstandard2.1 builds successfully (library only, no test execution)
4. Verify NuGet pack produces correct lib/ folders per TFM
5. Spot-check: consumer resolving the `netstandard2.1` asset does **not** see `DateOnly` / `TimeOnly` APIs
6. Spot-check: consumer on `net6.0+` sees the modern date/time validation surface
7. Spot-check: consumer on `net8.0+` sees full API surface with FrozenDictionary perf

---

## Open Questions to Resolve During Implementation

1. **CsvColumnType enum** — Does it have DateOnly/TimeOnly members that need conditional guards?
2. **SqlDateTime files** — Verify if they reference DateOnly or just DateTime/DateTimeOffset
4. **ImplicitUsings on netstandard2.1** — Test if SDK handles this or if we need GlobalUsings.cs fallback

---

## Implementation Order

Execute phases in order, but land the work in **green slices**. Temporary local breakage during an
in-flight slice is acceptable; commits and PRs should end on a buildable state.

**Recommended slice order:**

1. **Slice 1:** Phase 1 + Phase 2 (`Directory.Build.props`, multi-target csproj updates,
   FluentValidation version, compatibility shims, `ThrowHelper` groundwork)
2. **Slice 2:** Phase 3 + Phase 5 (FrozenDictionary and smaller BCL fallbacks)
3. **Slice 3:** Phase 4 + Phase 6 (DateOnly/TimeOnly source partitioning and test partitioning)
4. **Slice 4:** Phase 7 + Phase 8 (scripts, packaging verification, final docs alignment)
