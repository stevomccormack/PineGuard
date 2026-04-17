# Audit CLI

Static analysis, parity checking, and specification enforcement for PineGuard.

For the normative specification (rule patterns, vocabulary, parity scope), see:

- `docs/ai/specs/tools/audit-cli/spec.md`

## Directory Structure

```
tools/audit-cli/
├── PineGuard.AuditCli.slnx         # Solution file for the .NET auditor
├── Run-All.ps1                      # Master orchestrator (preferred entrypoint)
├── Run-AuditLibraryRules.ps1        # Library rules subset (Rule01-Rule10)
├── Run-AuditTestingRules.ps1        # Testing rules subset (Rule50-Rule54)
├── Run-AuditRules.ps1               # Legacy compatibility shim
├── rules/
│   ├── Load-Catalog.ps1             # Single source of truth for rule metadata
│   ├── Test-Rule01-Naming.ps1       # through Test-Rule10 (library rules)
│   └── Test-Rule50-*.ps1            # through Test-Rule54 (testing rules)
├── helpers/
│   ├── Load-AuditHelpers.ps1        # Shared helpers
│   ├── Load-AuditOrchestrator.ps1   # Orchestrator logic
│   ├── Load-TestAuditExceptions.ps1 # Loads test audit exception allowlist
│   ├── Test-SpecNaming.ps1          # Naming validation
│   ├── Test-SpecNullability.ps1     # Nullability validation
│   ├── Test-SpecOrdering.ps1        # Method ordering parity
│   ├── Test-CoverageLatest.ps1      # Latest coverage analysis
│   ├── Test-ParityAgainstMust.ps1   # Must parity validation
│   ├── Test-ParityFluentGuard.ps1   # Fluent/Guard parity validation
│   ├── Test-TestDataTuples.ps1      # Test data tuple validation
│   ├── Find-UnusedMustData.ps1      # Unused MustClauses in DataAnnotations
│   ├── Find-UnusedMustFluent.ps1    # Unused MustClauses in FluentValidation
│   ├── Find-UnusedMustGuard.ps1     # Unused MustClauses in GuardClauses
│   └── Find-UnusedRules.ps1         # Unused Rules finder
├── solution/                        # .NET auditor console app
│   ├── PineGuard.AuditCli.csproj
│   ├── Program.cs
│   ├── MethodOrderingAudit.cs
│   └── README.md
└── utils/                           # Utility scripts
    ├── Run-Util01AnalyzeLatestCoverageDataAnnotations.ps1
    ├── Run-Util02TestDataTupleScan.ps1
    └── Sync-MarkdownPs1Refs.ps1
```

## Rule Catalog

The rule catalog is defined in `rules/Load-Catalog.ps1` (single source of truth).

### Library Rules

| Rule | Name | What It Checks |
|------|------|----------------|
| Rule01 | Naming | Method/class naming conventions |
| Rule02 | RulesUsage | Core Rules usage patterns |
| Rule03 | GuardUsage | GuardClauses usage patterns |
| Rule04 | FluentUsage | FluentValidation usage patterns |
| Rule05 | DataUsage | DataAnnotations usage patterns |
| Rule06 | Parity | Adapter ↔ MustClauses concept parity |
| Rule07 | Nullability | Parameter nullability compliance |
| Rule08 | Ordering | Method ordering parity across layers |
| Rule09 | CatalogIntegrity | Rule catalog self-consistency |
| Rule10 | PsNormalization | PowerShell parse-safety compliance |

### Testing Rules

| Rule | Name | What It Checks |
|------|------|----------------|
| Rule50 | UnitTestFileStructure | *Tests.cs ↔ *TestData.cs pairing + Theory-only |
| Rule51 | UnitTestClassSemanticStructure | Nested static operation groups |
| Rule52 | UnitTestCaseRecordConventions | ValidCase record inheritance |
| Rule53 | TestOrphans | *Tests.cs must map to a source class |
| Rule54 | UnitTestTupleConventions | camelCase tuple element names |

## Usage

Run from the repository root.

### Run all rules

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1"
```

### Run subsets

```powershell
# Library rules only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditLibraryRules.ps1"

# Testing rules only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditTestingRules.ps1"
```

### Filter by rule

```powershell
# By rule ID
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -RuleId Rule06,Rule08

# By rule name
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -RuleName Parity,Ordering
```

### Diagnostic options

```powershell
# List available rules (no execution)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -ListRules

# Continue on failure + show details
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -ContinueOnError -ShowFailures

# Suppress catalog header
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -NoCatalog

# Suppress summary
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -NoSummary

# Export JSON summary
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -JsonSummaryPath artifacts/audit/audit-summary.json
```

## Parameters (Run-All.ps1)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Configuration` | string | `Release` | Build configuration |
| `-RepoRoot` | string | auto-detected | Path to repository root |
| `-AllowViolations` | switch | `$false` | Don't fail on violations |
| `-ListRules` | switch | `$false` | List rules and exit |
| `-NoCatalog` | switch | `$false` | Suppress catalog header |
| `-ContinueOnError` | switch | `$false` | Continue past failures |
| `-ShowFailures` | switch | `$false` | Verbose error details |
| `-NoSummary` | switch | `$false` | Suppress summary output |
| `-JsonSummaryPath` | string | — | Path for JSON summary export |
| `-RuleId` | string | — | Comma-separated rule IDs (e.g., `Rule06,Rule08`) |
| `-RuleName` | string | — | Comma-separated rule names (e.g., `Parity,Ordering`) |

## Artifacts

All audit output goes to `artifacts/audit/`. Format is JSON or structured text. Filenames follow `RuleXX-description.<ext>`.

## Test Audit Exceptions

Allowlisted exceptions for testing rules: `tools/audit-cli/test-audit-exceptions.json`
