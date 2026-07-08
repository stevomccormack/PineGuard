# Contributing to PineGuard

Thanks for your interest in contributing! This document covers the practical
essentials: how to build, test, and submit changes that pass CI on the first try.

## Prerequisites

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) (the solution multi-targets
  `netstandard2.1;net8.0;net10.0`; test projects run on `net8.0` and `net10.0`)
- PowerShell 7+ (repo tooling under `tools/` is PowerShell-based)

## Build, test, format

```bash
# Build (Release, zero warnings — TreatWarningsAsErrors is on)
dotnet build PineGuard.slnx -c Release

# Run all tests (6 projects × 2 target frameworks)
dotnet test PineGuard.slnx -c Release

# Formatting must be clean before you push (CI gates on this)
dotnet format PineGuard.slnx --verify-no-changes
```

A `pre-commit` git hook runs `dotnet format` automatically and re-stages
formatted files.

## Quality gates (what CI enforces)

Every pull request runs through `.github/workflows/ci.yml`:

1. **Build** — Release build of `PineGuard.slnx`, zero warnings (`TreatWarningsAsErrors`,
   .NET analyzers at `AnalysisMode=Recommended`, XML docs enforced via CS1591).
2. **Tests** — all test projects, `[Theory]` + `TheoryData` only (no `[Fact]`;
   see `docs/ai/specs/testing/unit-test.md`).
3. **Coverage** — 100% line **and** branch coverage, gated via ReportGenerator
   (`tools/code-coverage/coverlet.runsettings`).
4. **Format** — `dotnet format --verify-no-changes`.
5. **Roslyn** — zero `CS*` warnings.

Run coverage locally with `tools/code-coverage/Run-CodeCoverage.ps1`.

## Architecture in one paragraph

PineGuard is a layered validation library: **Core** (pure boolean rules + utilities)
→ **MustClauses** (result-returning, never throws) → **GuardClauses** (throwing) →
**FluentValidation** / **DataAnnotations** (framework adapters). New validations are
implemented across *all* layers, plus tests, in that order. The canonical
engineering specs live in [`docs/ai/`](docs/ai/README.md) — start there before
changing conventions.

## Commit messages

Use [Conventional Commits](https://www.conventionalcommits.org/) subjects
(`feat:`, `fix:`, `chore:`, `build:`, `test:`, `docs:` …) with a flowing-prose
body explaining *why* for any non-trivial change.

## Pull requests

- Keep PRs focused — one logical change per PR.
- Fill in the PR template; link related issues.
- All CI gates must pass; there is no "fix it later" for coverage or warnings.
- Do not suppress analyzer or Sonar findings — fix the root cause. Analyzer
  configuration changes (`.editorconfig`, `NoWarn`) need a justification comment.

## Security issues

Please do **not** open public issues for vulnerabilities — see [SECURITY.md](SECURITY.md).

## License

By contributing you agree that your contributions are licensed under the
[MIT License](LICENSE).
