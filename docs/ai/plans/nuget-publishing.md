# NuGet Publishing Plan for PineGuard

## Status

PineGuard is a mature multi-target validation library now living in a clean,
fresh public repo (MIT, nuget.org) at
[github.com/stevomccormack/PineGuard](https://github.com/stevomccormack/PineGuard).
Six ship-ready packages, 13,000+ tests green on net8.0 and net10.0, zero
Roslyn warnings, zero SonarQube issues, and a working PowerShell toolchain.

**What's not yet set up:** NuGet metadata, an icon bundled into each package,
per-package README files for nuget.org, MinVer versioning, Source Link, and a
publish workflow wired to GitHub Releases.

## Package Inventory

| Package | Dependencies | Purpose |
|---------|--------------|---------|
| `PineGuard.Core` | — | Rules engine and primitive validations |
| `PineGuard.MustClauses` | Core | Result-based composable validators |
| `PineGuard.GuardClauses` | Core, Must | Fail-fast guards with typed returns |
| `PineGuard.FluentValidation` | Core, Must, FluentValidation | AbstractValidator extensions |
| `PineGuard.DataAnnotations` | Core, Must | `[ValidationAttribute]` implementations |
| `PineGuard.Testing` | Core, FluentValidation, xunit | Shared fixtures and base classes for consumers |

`tools/audit-cli/` is internal tooling and is **not** part of the first release
cut. Ship it as a dotnet global tool in a follow-up once the library packages
are stable.

## Design Decisions (locked)

- **Pack format:** one package per library containing all three TFMs
  (`netstandard2.1;net8.0;net10.0`). NuGet resolves the best match at install.
- **Versioning:** MinVer drives versions from annotated git tags (`v1.0.0` →
  package `1.0.0`). Pre-releases use `v1.1.0-alpha.1` etc.
- **Publish trigger:** `on: release: types: [published]` — fires only when a
  GitHub Release is moved out of Draft. Prevents accidental publishes from a
  mistyped tag push and gives you a review gate.
- **Symbol packages:** `.snupkg` format, pushed alongside `.nupkg`.
- **Source Link:** `Microsoft.SourceLink.GitHub` enables IDE step-into-source.
- **Branching:** trunk-based on `main`. No `develop`. Hotfix via on-demand
  `release/x.y` branches only when a v1 consumer needs a fix after main has
  moved to v2 breaking work.

## Prerequisites (once, before any publish work)

1. **Icon file.** The brand icon is at
   `docs/brand/pineguard-logo-512px.png` (512×512). nuget.org accepts any size
   up to 1 MB but recommends 128×128 for package-page rendering; the 512px PNG
   renders cleanly at that size without a separate downscale.

   Keep the PNG in `docs/brand/` (source of truth) and reference it from each
   `.csproj` via a relative path — no need to duplicate the file at the root.

2. **Per-package README.md files.** Each `.csproj` needs a short focused README
   sitting next to it (e.g. `src/PineGuard.Core/README.md`). The root README is
   too broad for nuget.org's left-column display. Aim for ~20–40 lines per
   package: one-paragraph summary, install snippet, 3–5 canonical examples,
   link back to the root README for the full picture.

3. **nuget.org account + API key.** Create a push-only API key at
   nuget.org → Account → API Keys with:
   - Key name: `pineguard-github-actions`
   - Package owner: your nuget.org account
   - Scopes: Push + Push new packages and package versions
   - Glob pattern: `PineGuard.*`
   - Expiration: 365 days (renewable; shorter is fine for first release)

   Store as repo secret `NUGET_TOKEN` at
   github.com/stevomccormack/PineGuard → Settings → Secrets → Actions.

## Implementation

### 1. Shared package metadata — `Directory.Build.props`

Add a packable condition so test projects (which have `IsTestProject=true`
from the SDK) and non-packable assets stay out of the pack step.

```xml
<Project>
  <PropertyGroup>
    <TargetFrameworks>netstandard2.1;net8.0;net10.0</TargetFrameworks>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <NoWarn>$(NoWarn);CS8795</NoWarn>
  </PropertyGroup>

  <PropertyGroup Condition="'$(IsPackable)' != 'false' and '$(IsTestProject)' != 'true'">
    <Authors>Steve McCormack</Authors>
    <Company>stevomccormack</Company>
    <Copyright>Copyright © 2026 Steve McCormack</Copyright>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageProjectUrl>https://github.com/stevomccormack/PineGuard</PackageProjectUrl>
    <RepositoryUrl>https://github.com/stevomccormack/PineGuard</RepositoryUrl>
    <RepositoryType>git</RepositoryType>
    <PackageIcon>pineguard-logo-512px.png</PackageIcon>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageTags>validation;guard;must;fluent;data-annotations;dotnet</PackageTags>
    <PublishRepositoryUrl>true</PublishRepositoryUrl>
    <EmbedUntrackedSources>true</EmbedUntrackedSources>
    <IncludeSymbols>true</IncludeSymbols>
    <SymbolPackageFormat>snupkg</SymbolPackageFormat>
    <Deterministic>true</Deterministic>
    <ContinuousIntegrationBuild Condition="'$(GITHUB_ACTIONS)' == 'true'">true</ContinuousIntegrationBuild>
  </PropertyGroup>

  <ItemGroup Condition="'$(IsPackable)' != 'false' and '$(IsTestProject)' != 'true'">
    <PackageReference Include="MinVer" PrivateAssets="all" />
    <PackageReference Include="Microsoft.SourceLink.GitHub" PrivateAssets="all" />
  </ItemGroup>
</Project>
```

### 2. Per-`.csproj` additions

Each of the six packable projects needs:

```xml
<PropertyGroup>
  <Description>One focused sentence describing the package's role.</Description>
</PropertyGroup>

<ItemGroup>
  <None Include="..\..\docs\brand\pineguard-logo-512px.png" Pack="true" PackagePath="\" />
  <None Include="README.md" Pack="true" PackagePath="\" />
</ItemGroup>
```

Path depth for `PineGuard.Testing` (lives under `tests/`): `..\..\docs\brand\…`.
Path depth for the `src/PineGuard.X/` packages: `..\..\docs\brand\…`.

Suggested `<Description>` for each:

| Package | Description |
|---------|-------------|
| `PineGuard.Core` | Zero-dependency validation primitives: rules and utilities for strings, numbers, dates, collections, URIs, emails, network identifiers, and OWASP-safe input. |
| `PineGuard.MustClauses` | Result-based composable validators. Must.Be.Email(value) returns a MustResult<T> the caller inspects or escalates. |
| `PineGuard.GuardClauses` | Fail-fast guards with parsed typed returns. Guard.Against.NotHttpsUrl(url) returns a Uri or throws. |
| `PineGuard.FluentValidation` | Rule-builder extensions for FluentValidation's AbstractValidator pipeline. |
| `PineGuard.DataAnnotations` | `[ValidationAttribute]` implementations for DTOs, MVC binding, Blazor forms, and EF Core. |
| `PineGuard.Testing` | Shared fixtures, base test classes, and assertion helpers for consumers writing PineGuard-based validation tests. |

### 3. `PineGuard.Testing` — flip to packable

Already has `<IsTestProject>false</IsTestProject>`. The shared metadata in
`Directory.Build.props` will now apply to it since neither `IsPackable=false`
nor `IsTestProject=true`. Verify with `dotnet pack tests/PineGuard.Testing`.

### 4. Central package versions — `Directory.Packages.props`

Append:

```xml
<PackageVersion Include="MinVer" Version="5.0.0" />
<PackageVersion Include="Microsoft.SourceLink.GitHub" Version="8.0.0" />
```

### 5. MinVer configuration (optional tweak)

Defaults work for `v1.0.0`-style tags. If you want to start at `0.1.0-alpha.1`,
set the floor in `Directory.Build.props`:

```xml
<PropertyGroup>
  <MinVerTagPrefix>v</MinVerTagPrefix>
  <MinVerDefaultPreReleaseIdentifiers>alpha.0</MinVerDefaultPreReleaseIdentifiers>
</PropertyGroup>
```

### 6. Polyfill leakage check

`src/PineGuard.Core/Polyfills/CallerArgumentExpressionAttribute.cs` is linked
into Must, Guard, and Fluent projects as an `internal` type. Run `dotnet pack`
locally and inspect each `.nupkg` (rename to `.zip`, look at
`lib/{tfm}/{pkg}.dll` via `ildasm` or `dotnet-sos`) to confirm it isn't
exposed in the public API surface. Only a concern if any project references it
with `internal: false` — unlikely but worth a one-time audit.

### 7. GitHub Actions publish workflow — `.github/workflows/publish.yml`

```yaml
name: Publish to NuGet

on:
  release:
    types: [published]

jobs:
  publish:
    runs-on: ubuntu-latest
    permissions:
      contents: read
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0  # MinVer needs full git history + tags

      - uses: actions/setup-dotnet@v5
        with:
          dotnet-version: |
            8.0.x
            10.0.x

      - name: Restore
        run: dotnet restore PineGuard.slnx

      - name: Build
        run: dotnet build PineGuard.slnx -c Release --no-restore

      - name: Test
        run: dotnet test PineGuard.slnx -c Release --no-build --verbosity minimal

      - name: Pack
        run: dotnet pack PineGuard.slnx -c Release --no-build --output ./artifacts

      - name: Push to NuGet
        run: |
          dotnet nuget push ./artifacts/*.nupkg \
            --api-key ${{ secrets.NUGET_TOKEN }} \
            --source https://api.nuget.org/v3/index.json \
            --skip-duplicate
```

`.snupkg` symbol packages are pushed automatically alongside `.nupkg`.

### 8. CI workflow (separate from publish) — `.github/workflows/ci.yml`

If you don't already have one, add a PR-gate workflow:

```yaml
name: CI
on:
  pull_request: { branches: [main] }
  push:         { branches: [main] }

jobs:
  build-test:
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest]
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/checkout@v4
        with: { fetch-depth: 0 }
      - uses: actions/setup-dotnet@v5
        with: { dotnet-version: "10.0.x" }
      - run: dotnet restore PineGuard.slnx
      - run: dotnet build PineGuard.slnx -c Release --no-restore
      - run: dotnet test PineGuard.slnx -c Release --no-build
```

## Release Flow (end-to-end walkthrough)

```
feature/add-something
        │
        └─ PR → main (CI green)
                    │
                    │  (accumulate changes; CI stays green on main)
                    │
                    ▼
         gh release create v1.0.0 \
           --draft \
           --generate-notes \
           --title "1.0.0"
                    │
                    │  (review auto-generated notes, edit if needed)
                    │
                    ▼
         Click "Publish release" in GitHub UI
         (or: gh release edit v1.0.0 --draft=false)
                    │
                    ▼
         publish.yml fires automatically
           restore → build → test → pack → push → nuget.org
                    │
                    ▼
         All 6 packages appear on nuget.org within 5–10 min
         (indexing can take up to 1 hour before they show in `dotnet add package` search)
```

Annotated tag is created by `gh release create` automatically — no need for a
separate `git tag -a` step.

### First release suggestion

Cut `v0.1.0-alpha.1` first to validate the entire pipeline on a pre-release
version. Install into a scratch project, confirm icon + README + Source Link
all render on nuget.org. Then cut `v1.0.0` when ready.

## Verification

After the first successful publish, verify on each package page:
- Icon renders (top-left of the package page)
- README renders (main body, should be the per-package README, not the root)
- Source Link works: in a consumer project, `Go to Definition` on a
  PineGuard type steps into the decompiled-with-source view, not metadata-only.
- Symbol package resolved: the `.snupkg` is at
  `https://nuget.smbsrc.net/src/…` or directly queryable via
  `nuget.org/packages/PineGuard.Core` → Symbol sidebar.

Rollback path if something is wrong:
- `dotnet nuget delete PineGuard.Core 1.0.0 --api-key ...` (works within 72h)
- Or "list as unlisted" via the nuget.org web UI (permanent; new installs
  won't see it but existing consumers keep working).

## Branch Protection

At github.com/stevomccormack/PineGuard → Settings → Branches → `main`:
- Require pull request before merging
- Require all CI jobs to pass
- No direct push
- No force push

At Settings → Tags:
- Protect pattern `v*` — restrict tag creation to maintainers only. Prevents
  accidental publish triggers from contributors.

## Open Questions

- **`global.json`** — currently absent. Consider `"rollForward": "latestMinor"`
  to pin SDK major version for reproducible local + CI builds.
- **API compatibility baseline** — on `v1.0.0` generate a baseline with
  `Microsoft.DotNet.ApiCompat`. From `v1.1.0` onwards CI enforces no
  accidental breaking changes.
- **Package signing** — nuget.org supports Authenticode signing. Not
  mandatory but adds trust. Requires a code-signing cert in GitHub secrets.
- **CHANGELOG** — pick a format: Keep a Changelog, conventional commits, or
  rely purely on GitHub's auto-generated release notes (simplest). If you want
  `<PackageReleaseNotes>` in the `.nupkg` itself, consume the release body
  via a small workflow step before pack.
- **Public API baseline tool** — `PublicApiAnalyzers` can enforce that every
  public type/member is listed in `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt`.
  Worth adding once the surface is stable.

## Execution Order

Progress: **3 / 12 complete**.

- [x] **1. Per-package `README.md` files (6 files)** — `src/PineGuard.Core/README.md`, `src/PineGuard.MustClauses/README.md`, `src/PineGuard.GuardClauses/README.md`, `src/PineGuard.FluentValidation/README.md`, `src/PineGuard.DataAnnotations/README.md`, `tests/PineGuard.Testing/README.md`. Each follows a four-block shape: benefit-first masthead → canonical four-rule example (Email, StrictEmail, OwaspSafe, HttpsUrl) → architectural sweet spot (DDD for Guard; Clean Architecture for Fluent and DataAnnotations) → shared "one rule library, every call site" closer. Committed in `7e9213e`.
- [x] **2. `Directory.Build.props`** — packable metadata block added (see §1). `MinVerDefaultPreReleaseIdentifiers=alpha.0` set inside the gated PropertyGroup; `MinVerTagPrefix>v` skipped (MinVer default). Committed in `cff873d`.
- [x] **3. `Directory.Packages.props`** — `MinVer 7.0.0` and `Microsoft.SourceLink.GitHub 10.0.202` added (both latest stable as of 2026-04, newer than the 5.0.0/8.0.0 floor originally spec'd). `dotnet restore PineGuard.slnx --force` passes clean across all 12 projects. Committed in `cff873d`.
- [ ] **4. Per-`.csproj` additions** — `<Description>` plus the `<None Include="..\..\docs\brand\pineguard-logo-512px.png">` and `<None Include="README.md">` blocks (see §2).
- [ ] **5. Local `dotnet pack` verification** — run `dotnet pack PineGuard.slnx -c Release` and inspect each `.nupkg`:
    - [ ] Contains `pineguard-logo-512px.png` at root
    - [ ] Contains `README.md` at root (the per-package one, not the root README)
    - [ ] `lib/netstandard2.1/`, `lib/net8.0/`, `lib/net10.0/` all present
    - [ ] No polyfill types in public surface
- [ ] **6. GitHub Actions workflows** — `.github/workflows/ci.yml` (if not present) and `publish.yml`.
- [ ] **7. nuget.org account + secret** — register `PineGuard.*` on nuget.org; add `NUGET_TOKEN` repo secret.
- [ ] **8. Branch and tag protection** — enable on `main` and the `v*` tag pattern.
- [ ] **9. Cut `v0.1.0-alpha.1`** — via GitHub Release; verify full pipeline end-to-end.
- [ ] **10. Post-publish verification** — icon, READMEs, and Source Link render correctly on nuget.org.
- [ ] **11. Cut `v1.0.0`** — once the alpha pipeline is proven.
- [ ] **12. Follow-up: `PineGuard.AuditCli`** — ship as a dotnet global tool in a separate release cut.

## Notes from execution

Decisions and findings captured while working through the plan, worth preserving so the same ground isn't re-litigated at later steps.

- **Canonical four-rule example set.** Every layer's README leads its `## Examples` block with the same four rules: `Email`, `StrictEmail`, `OwaspSafe`, `HttpsUrl`. Chosen because they're the rules most developers recognise by name, they cover three validation axes (format, security, URL scheme), and they let a reader skim any two READMEs and see the same concept expressed in two surfaces.
- **Target-framework ordering: modern-first.** Every README and every TFM mention uses `net8.0`, `net10.0`, `netstandard2.1` in that order. Reads as "we're modern-first with legacy support" rather than the opposite.
- **`[Required]` on netstandard2.1.** Confirmed to work — lives in the `System.ComponentModel.Annotations` NuGet package which is already a transitive dependency via `PineGuard.Core`. Same for `[StringLength]`, `[MaxLength]`, `[Range]`, etc. DA README's "Chain with built-in DataAnnotations" example uses `[Required] [MaxLength(256)] [HttpsUrl]` composition.
- **`[Required]` vs PineGuard's `[NotNull]`.** Subtly different: `[Required]` rejects null *and* empty string (unless `AllowEmptyStrings = true`); `[NotNull]` rejects null only. DA README documents the distinction in a "Presence semantics" subsection so consumers pick the right one.
- **Architectural framing: "perfect fit for", not "built for".** Guard is a perfect fit for DDD; Fluent and DataAnnotations are a perfect fit for Clean Architecture. But none of them are *limited to* those patterns — the READMEs list the broader use cases first, then call out the architectural sweet spot. "Built for X" would have been too narrow.
- **Guard's differentiating moat.** Exception policy at three tiers (global, per-scope, per-call). Ardalis.GuardClauses — the dominant alternative — offers only a per-call override. Guard's README leads with this as the explicit differentiator paragraph.
- **Chain-with-built-ins sections.** Both Fluent and DataAnnotations READMEs include a "Chain with FluentValidation's built-ins" / "Chain with built-in DataAnnotations" subsection showing PineGuard rules composing with `.MaximumLength()`, `.When()`, `.WithMessage()`, `[Required]`, `[StringLength]`, `[Range]`. These pull their weight in onboarding — they show PineGuard isn't an all-or-nothing commitment.
- **Unverified against source code.** A couple of README API shapes weren't grep-verified before commit: `Must.Be.GuidV4`, `Must.Be.NotNull().AndThen(...)` composition, `GuidRules` / `StrictEmail` parity across every layer. Worth a spot-check during step 5 when inspecting the built `.nupkg`s — any example that doesn't compile against the packed assembly needs the README nudged.

## Related

- [`docs/ai/plans/multi-target-framework.md`](multi-target-framework.md) — target-framework strategy
- [`docs/ai/plans/competitive-analysis.md`](competitive-analysis.md) — positioning vs other validation libraries
- [`docs/ai/plans/v2-masterplan.md`](v2-masterplan.md) — overall v2 roadmap
