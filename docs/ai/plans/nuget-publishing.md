# NuGet Publishing Plan for PineGuard

## Context

PineGuard is a mature, multi-target validation library with 5 source packages, 100% test coverage,
zero Roslyn warnings, and zero SonarQube issues. This plan covers publishing all packages to
nuget.org, adding the AuditCli as a dotnet tool, and the CI/CD release workflow.

## Package Inventory

| Package | Internal Dependencies | Purpose |
|---------|-----------------------|---------|
| `PineGuard.Core` | — | Rules engine, primitives |
| `PineGuard.MustClauses` | Core | Result-based fluent API |
| `PineGuard.GuardClauses` | Core, Must | Throw-on-failure guards |
| `PineGuard.FluentValidation` | Core, Must | FluentValidation adapters |
| `PineGuard.DataAnnotations` | Core, Must | `[Attribute]` validators |
| `PineGuard.Testing` | Core | Test fixtures for consumers writing PineGuard tests |
| `PineGuard.AuditCli` | — | dotnet global tool (separate from library packages) |

`PineGuard.Testing` already has `IsTestProject: false` and should be published so consumers can
use the shared test infrastructure when building PineGuard-based validation.

## Locked Decisions

- **All ProjectReferences become PackageReferences automatically** — NuGet handles this when all
  referenced projects are also published packages. No manual `.nuspec` needed.
- **No per-TFM packages** — one package per library containing all TFM assets inside it
  (`netstandard2.1;net8.0;net10.0`). NuGet resolves the best-matching TFM at install time.
- **MinVer for versioning** — git-tag-driven, zero manual version bumping. Tag `v1.0.0` → package
  version `1.0.0`. Pre-releases use `v1.1.0-alpha.1` tags.
- **Symbol packages** — `.snupkg` format, pushed alongside `.nupkg` to nuget.org.
- **Source Link** — Microsoft.SourceLink.GitHub enables IDE step-into-source for consumers.
- **Deterministic builds** — enabled on CI only (`ContinuousIntegrationBuild`).

## Target Framework Strategy

Current TFMs: `netstandard2.1;net8.0;net10.0`

- Keep `netstandard2.1` — covers Xamarin, Unity, Blazor WASM, .NET Core 3.x/5/6/7.
- Drop `net8.0` when it reaches EOL (November 2026).
- `net10.0` is STS (EOL May 2026) — add `net12.0` when available; consider whether to retain
  `net10.0` during the overlap window.
- `Microsoft.CSharp` conditional reference in `DataAnnotations` for `netstandard2.1` is already
  handled correctly and will flow into the published package's conditional dependency.
- `System.Text.Json` and `System.ComponentModel.Annotations` are inbox on net8.0+; NuGet will
  include them as conditional dependencies for `netstandard2.1` consumers only.

## Implementation Steps

### 1. NuGet Metadata — `Directory.Build.props`

Add shared metadata inside a condition so test/tool projects are excluded:

```xml
<PropertyGroup Condition="'$(IsPackable)' != 'false' and '$(IsTestProject)' != 'true'">
  <Authors>Steve McCormack</Authors>
  <Company>stevomccormack</Company>
  <Copyright>Copyright © 2024-2025 Steve McCormack</Copyright>
  <PackageLicenseExpression>MIT</PackageLicenseExpression>
  <PackageProjectUrl>https://github.com/stevomccormack/PineGuard</PackageProjectUrl>
  <RepositoryUrl>https://github.com/stevomccormack/PineGuard</RepositoryUrl>
  <RepositoryType>git</RepositoryType>
  <PackageIcon>icon.png</PackageIcon>
  <PackageReadmeFile>README.md</PackageReadmeFile>
  <PackageTags>validation;guard;must;fluent;data-annotations;dotnet</PackageTags>
  <PublishRepositoryUrl>true</PublishRepositoryUrl>
  <EmbedUntrackedSources>true</EmbedUntrackedSources>
  <IncludeSymbols>true</IncludeSymbols>
  <SymbolPackageFormat>snupkg</SymbolPackageFormat>
  <Deterministic>true</Deterministic>
  <ContinuousIntegrationBuild Condition="'$(GITHUB_ACTIONS)' == 'true'">true</ContinuousIntegrationBuild>
</PropertyGroup>
```

Each `.csproj` adds its own `<Description>` and optionally `<PackageTags>` override.

### 2. Versioning — MinVer

```xml
<!-- Directory.Packages.props -->
<PackageVersion Include="MinVer" Version="5.0.0" />

<!-- Directory.Build.props (inside packable condition) -->
<ItemGroup>
  <PackageReference Include="MinVer" PrivateAssets="all" />
</ItemGroup>
```

### 3. Source Link

```xml
<!-- Directory.Packages.props -->
<PackageVersion Include="Microsoft.SourceLink.GitHub" Version="8.0.0" />

<!-- Directory.Build.props (inside packable condition) -->
<ItemGroup>
  <PackageReference Include="Microsoft.SourceLink.GitHub" PrivateAssets="all" />
</ItemGroup>
```

### 4. Package Icon and Per-Package README

Each packable `.csproj` includes:

```xml
<ItemGroup>
  <None Include="..\..\icon.png" Pack="true" PackagePath="\" />
  <None Include="README.md" Pack="true" PackagePath="\" />
</ItemGroup>
```

Each package needs a short, focused `README.md` alongside its `.csproj`. The root README is too
broad for per-package display on nuget.org.

### 5. License File

Either use `<PackageLicenseExpression>MIT</PackageLicenseExpression>` (sufficient for nuget.org),
or embed the file for enterprise compliance tooling:

```xml
<PackageLicenseFile>LICENSE</PackageLicenseFile>
<!-- plus -->
<None Include="..\..\LICENSE" Pack="true" PackagePath="\" />
```

### 6. AuditCli as a dotnet tool

In `tools/audit-cli/solution/PineGuard.AuditCli.csproj`:

```xml
<PropertyGroup>
  <PackAsTool>true</PackAsTool>
  <ToolCommandName>pineguard-audit</ToolCommandName>
  <PackageId>PineGuard.AuditCli</PackageId>
  <Description>Audit CLI for PineGuard validation rule coverage analysis.</Description>
</PropertyGroup>
```

Consumers install with: `dotnet tool install PineGuard.AuditCli -g`

### 7. Package Validation in CI

Add to the build job in `ci.yml`:

```yaml
- run: dotnet pack --configuration Release --no-build --output ./artifacts
- run: dotnet tool run dotnet-package-validate --package ./artifacts/*.nupkg
```

Add to `Directory.Packages.props`:

```xml
<PackageVersion Include="Microsoft.DotNet.PackageValidation" Version="1.0.0-preview.7" />
```

### 8. Publish Workflow — `.github/workflows/publish.yml`

Triggered by publishing a GitHub Release (not a raw tag push — see Branching & Release Strategy).

```yaml
name: Publish to NuGet

on:
  release:
    types: [published]    # fires only when release moves from Draft → Published

jobs:
  publish:
    runs-on: ubuntu-latest
    permissions:
      contents: read
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0    # MinVer requires full git history

      - uses: actions/setup-dotnet@v5
        with:
          dotnet-version: '10.0.x'

      - name: Restore
        run: dotnet restore PineGuard.slnx

      - name: Build
        run: dotnet build PineGuard.slnx -c Release --no-restore

      - name: Test
        run: dotnet test PineGuard.slnx -c Release --no-build

      - name: Pack
        run: dotnet pack PineGuard.slnx -c Release --no-build --output ./artifacts

      - name: Push to NuGet
        run: |
          dotnet nuget push ./artifacts/*.nupkg \
            --api-key ${{ secrets.NUGET_API_KEY }} \
            --source https://api.nuget.org/v3/index.json \
            --skip-duplicate
        # .snupkg symbol packages are pushed automatically alongside .nupkg
```

Add `NUGET_API_KEY` as a secret in GitHub repo settings (generate on nuget.org → Account → API Keys,
scoped to push only, with a package prefix filter of `PineGuard.*`).

### 9. Pre-release via GitHub Packages (Optional)

For alpha/beta validation before pushing to nuget.org, publish to GitHub Packages first:

```yaml
- name: Push to GitHub Packages (pre-release only)
  if: contains(github.ref, '-')    # e.g. v1.0.0-alpha.1
  run: |
    dotnet nuget push ./artifacts/*.nupkg \
      --api-key ${{ secrets.GITHUB_TOKEN }} \
      --source https://nuget.pkg.github.com/stevomccormack/index.json
```

## Branching & Release Strategy

### Recommendation: Trunk-based, tags only

**No develop branch. No permanent release branches.**

| Strategy | Verdict | Reason |
|----------|---------|--------|
| `main` + annotated tags | **Recommended** | Minimal overhead; MinVer works perfectly; single source of truth |
| `main` + `develop` | Avoid | Gitflow is designed for parallel-stream teams. Double merges, no benefit for a single developer or small library team |
| Permanent `release/x.y` | Create on demand only | Only needed when `main` has moved to breaking v2 work and a v1 consumer needs a hotfix; never pre-create |

The `releases/*` trigger in `ci.yml` is the correct escape hatch — keep it, but don't create the
branch until a real backport scenario arises.

### MinVer Behaviour by Situation

MinVer requires **annotated tags** (not lightweight) and **full git history** (`fetch-depth: 0`).

| Situation | MinVer output | Package version |
|-----------|---------------|-----------------|
| Commit is tagged `v1.0.0` | `1.0.0` | Stable release |
| 3 commits after `v1.0.0` on main | `1.0.1-alpha.0.3.abcdef` | Auto pre-release |
| Tagged `v1.1.0-beta.1` | `1.1.0-beta.1` | Explicit pre-release |
| On `release/1.0` branch, tagged `v1.0.1` | `1.0.1` | Patch from release branch |

Pre-release packages between stable tags are automatic — no version file to update.

### Release Flow

```
feature/add-something
        │
        └─ PR → main (CI: build, test, coverage, format, roslyn — all must pass)
                    │
                    │  (work accumulates, all green)
                    │
                    ▼
         gh release create v1.0.0 --generate-notes
                    │
                    ▼
         publish.yml triggers → pack → push to nuget.org
```

### Publish Trigger: GitHub Release (recommended over raw tag push)

Use `on: release: types: [published]` rather than `on: push: tags: ['v*']`.

Benefits:
- You can draft and review the release before it fires the publish job.
- `gh release create v1.0.0 --generate-notes` auto-generates notes from merged PR titles.
- Release notes are attached to the tag on GitHub and serve as the changelog.
- Prevents accidental publishes from a mis-typed tag push.

```yaml
on:
  release:
    types: [published]    # fires only when you click Publish (not on Draft)
```

To create a release from the CLI:
```bash
gh release create v1.0.0 --generate-notes --title "1.0.0"
# Review the draft, then publish in the GitHub UI or:
gh release edit v1.0.0 --draft=false
```

### Branch Protection Rules (GitHub Settings)

- **`main`**: require PR, require all CI jobs to pass, no direct push, no force push.
- **Tag pattern `v*`**: restrict tag creation to maintainers only (prevents accidental publish
  triggers from contributors).

### Hotfix Flow (when it arises)

```
v1.0.0 tag on main
        │
        └─ main moves forward to v2 breaking work
        │
        │  consumer reports critical v1 bug
        │
        ▼
git checkout -b release/1.0 v1.0.0
        │
        └─ fix commit on release/1.0
        │
        ▼
gh release create v1.0.1 --target release/1.0 --generate-notes
        │
        ▼
publish.yml triggers from the v1.0.1 tag
        │
        ▼
cherry-pick fix back to main (or open a PR)
```

## Open Questions

- **`global.json`** — currently absent. Consider adding with `"rollForward": "latestMinor"` to pin
  SDK major version for reproducible local + CI builds.
- **API compatibility baseline** — on first public release, generate a baseline with
  `Microsoft.DotNet.ApiCompat`. From v2 onwards CI enforces no accidental breaking changes.
- **Package signing** — nuget.org supports Authenticode signing. Not mandatory but increases trust.
  Requires a code-signing certificate stored in GitHub secrets.
- **CHANGELOG** — decide on format (Keep a Changelog, conventional commits). Set
  `<PackageReleaseNotes>` in each release or embed `CHANGELOG.md`.
- **`CallerArgumentExpressionAttribute` polyfill** — inline polyfills in MustClauses, GuardClauses,
  FluentValidation are `internal`. Verify the `.nuspec` does not expose them. Run
  `dotnet pack` and inspect the `.nupkg` to confirm.

## Things Not Covered Here (Out of Scope)

- Chocolatey / winget packaging for AuditCli (only relevant if CLI adoption becomes a goal)
- MyGet / Azure Artifacts (only if enterprise internal feed is needed)
- NPM — not applicable; PineGuard is .NET only. NPM is the JavaScript/Node.js package manager.
  A JS/TS port would be a separate project.

## Recommended Execution Order

1. Add NuGet metadata to `Directory.Build.props`
2. Add MinVer to `Directory.Packages.props` and `Directory.Build.props`
3. Add Microsoft.SourceLink.GitHub
4. Create package icon (`icon.png`) and per-package `README.md` files
5. Add `LICENSE` file embedding
6. Run `dotnet pack` locally and inspect `.nupkg` contents to verify
7. Add `publish.yml` GitHub Actions workflow
8. Register on nuget.org, get API key scoped to `PineGuard.*`, add `NUGET_API_KEY` secret
9. Configure `PineGuard.AuditCli` as a dotnet tool
10. Tag `v0.1.0-alpha.1`, let CI publish, verify on nuget.org
11. Add package validation (`Microsoft.DotNet.PackageValidation`) to CI
12. Add `global.json` for SDK version pinning
