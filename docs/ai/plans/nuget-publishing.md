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

1. Add per-package `README.md` files (6 files — quickest wins, no tooling yet).
2. Update `Directory.Build.props` with packable metadata block.
3. Add `MinVer` and `Microsoft.SourceLink.GitHub` to `Directory.Packages.props`.
4. Add `<Description>` and `<None Include=...>` blocks to each packable `.csproj`.
5. Run `dotnet pack PineGuard.slnx -c Release` locally; inspect each `.nupkg`:
   - Contains `pineguard-logo-512px.png` at root
   - Contains `README.md` at root (the per-package one, not root README)
   - `lib/netstandard2.1/`, `lib/net8.0/`, `lib/net10.0/` all present
   - No polyfill types in public surface
6. Add `.github/workflows/ci.yml` (if not present) and `publish.yml`.
7. Register `PineGuard.*` on nuget.org; add `NUGET_TOKEN` secret.
8. Enable branch and tag protection rules.
9. Cut `v0.1.0-alpha.1` via GitHub Release; verify full pipeline succeeds.
10. Verify icon + READMEs + Source Link on nuget.org.
11. When satisfied, cut `v1.0.0`.
12. Follow-up: `PineGuard.AuditCli` as a dotnet global tool in a separate release.

## Related

- [`docs/ai/plans/multi-target-framework.md`](multi-target-framework.md) — target-framework strategy
- [`docs/ai/plans/competitive-analysis.md`](competitive-analysis.md) — positioning vs other validation libraries
- [`docs/ai/plans/v2-masterplan.md`](v2-masterplan.md) — overall v2 roadmap
