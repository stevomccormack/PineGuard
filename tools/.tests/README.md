# tools/.tests

The Pester test suite for `tools/**` (excluding `tools/audit-cli/**`, which is reviewed
separately). This is the first automated test coverage the tooling has ever had (F-48).

## Running

```pwsh
Invoke-Pester -Path tools/.tests/ -Output Detailed
```

Requires Pester 5.x or later — written and verified against **5.7.1**. This machine also has the
Windows-bundled Pester 3.4.0 (`C:\Program Files\WindowsPowerShell\Modules\Pester\3.4.0`), which
does not understand `Describe`/`BeforeAll`/`-ForEach`/`Should -Be` the way this suite uses them;
if `Get-Module -ListAvailable Pester` shows only 3.4.0, install a current one first
(`Install-Module Pester -Scope CurrentUser -MinimumVersion 5.5.0 -SkipPublisherCheck`). If more
than one major version is installed, pin the one you want with
`Import-Module Pester -RequiredVersion <x.y.z> -Force` before invoking `Invoke-Pester`.

## What's covered

| File | Covers |
|---|---|
| `Registry-Parity.Tests.ps1` | `tools/.shared/dotnet-projects.ps1`'s scope registry against `src/*.csproj`, `tests/*.csproj`, `PineGuard.slnx`, the packable list in `tools/release/Run-NugetUnlist.ps1`, Qodana config presence, and per-project `AGENTS.md` (plan §3.4) |
| `Dotenv-Parser.Tests.ps1` | `Import-DotEnv` in `tools/.shared/dotenv.ps1`, against `fixtures/sample.env` |
| `Cobertura-Parser.Tests.ps1` | `Read-CoberturaCoverage`, `ConvertTo-Rate`, `Try-ParseConditionCoverage` in `tools/.shared/coverage.ps1`, against `fixtures/sample-coverage.cobertura.xml` |
| `RepoRoot.Tests.ps1` | `Get-RepoRoot` in `tools/.shared/path.ps1`, from several starting directories |
| `Git-Helpers.Tests.ps1` | `Assert-IndexClean` in `tools/.shared/git.ps1` — the F-16 Tier-0-safety regression fixed in T1.01, tested against real throwaway git repos under `$TestDrive` |
| `Help-Placeholder.Tests.ps1` | F-46: no script still carries the placeholder `.PARAMETER` text |
| `Bom-Absence.Tests.ps1` | F-47: no script has a UTF-8 byte-order mark |
| `Windows-Isms.Tests.ps1` | F-30: no hardcoded backslash path separators, `.exe`, `$env:TEMP`, or `SetEnvironmentVariable(...,'User')` |

## Intentionally red tests

This is a foundation-laying suite, not a "make everything green" suite. The plan's Phase 2 CI
job (T2.05) runs it with `continue-on-error: true` on purpose: several tests here encode
**target-state** invariants that later phases fix. A test staying red is the point — it turns
"nobody's tracking this" into "here's exactly what's still broken." Do not weaken an assertion
here to make it pass early; let the corresponding phase task turn it green instead.

As of the run when this suite was added (Pester 5.7.1, 261 tests, 223 passed / 38 failed):

- **`Help-Placeholder.Tests.ps1`** — 21 of 60 tests red. 21 scripts still have the placeholder
  text (F-46, exact match); Phase 5 replaces it with real documentation.
- **`Windows-Isms.Tests.ps1`** — 3 of 5 tests red. Category A (hardcoded backslash path
  separators, including `tools/.shared/dotnet-projects.ps1`'s own registry fields — SourceDir,
  SourceCsprojs, TestCsproj, DefaultSourcePrefix) has 81 hits across 14 files; category B
  (hardcoded `.exe`) has 2 hits (`dotnet-tools-reportgenerator.ps1`). T3.09 (D-5 sweep) fixes
  both. Category C (`$env:TEMP`) already passes with zero hits — T1.03 fixed the one usage that
  used to exist. Category D (`SetEnvironmentVariable(...,'User')`) has one hit
  (`sonar-scanner/Setup-SonarQube.ps1`); D-4 fixes it.
- **`Bom-Absence.Tests.ps1`** — 14 of 68 tests red (14 files carry a BOM, matching F-47 for this
  scope). Red until T2.06 lands (BOM/whitespace strip); green after.

Everything else (registry parity — 99/99, the dotenv parser — 9/9, the Cobertura parser — 10/10,
`Get-RepoRoot` — 6/6, and the git helpers — 4/4) passes today. If one of those goes red, that's a
real bug — in the test or in the code under test — not expected debt. (One already surfaced and
was fixed while authoring this suite: `Assert-IndexClean`'s exception message used
`"a " + "b" -f x, y` — since PowerShell's `-f` binds tighter than `+`, the format only ever
applied to the second half of the string, so `{0}`/`{1}` were never substituted. Fixed in
`tools/.shared/git.ps1` by parenthesizing the full string before `-f`; the safety behaviour
itself — throw, never unstage — was already correct.)
