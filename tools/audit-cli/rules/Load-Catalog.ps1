<#
.SYNOPSIS
    Load Catalog

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

[CmdletBinding()]
param(
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-PineGuardAuditRule-Catalog {
    [CmdletBinding()]
    param(
        [ValidateNotNullOrEmpty()]
        [string] $RulesRoot
    )

    $rule = {
        param(
            [Parameter(Mandatory = $true)][string] $Id,
            [Parameter(Mandatory = $true)][string] $Name,
            [Parameter(Mandatory = $true)][string] $Description,
            [Parameter(Mandatory = $true)][string] $Script,
            [Parameter(Mandatory = $true)][string] $OutputPath,
            [switch] $UsesConfiguration,
            [switch] $UsesFailOnFindings,
            [switch] $UsesAllowViolations
        )

        [pscustomobject]@{
            Id                  = $Id
            Name                = $Name
            Description         = $Description
            ScriptPath          = (Join-Path $RulesRoot $Script)
            OutputPath          = $OutputPath
            UsesConfiguration   = $UsesConfiguration.IsPresent
            UsesFailOnFindings  = $UsesFailOnFindings.IsPresent
            UsesAllowViolations = $UsesAllowViolations.IsPresent
        }
    }

    @(
        & $rule -Id 'Rule01' -Name 'Naming + Collisions' -Description 'MustClauses naming/nullability policy + overload collision detection (JSON report).' -Script 'Test-Rule01-Naming.ps1' -OutputPath 'artifacts/audit/Rule01-mustclauses-naming-and-collisions.json' -UsesAllowViolations
        & $rule -Id 'Rule02' -Name 'Rules → Must Usage' -Description 'Ensures Core Rules are used by MustClauses; flags unused Rules.' -Script 'Test-Rule02-RulesUsage.ps1' -OutputPath 'artifacts/audit/Rule02-rules-to-must-usage-scan.txt' -UsesConfiguration -UsesFailOnFindings
        & $rule -Id 'Rule03' -Name 'Must → Guard Usage' -Description 'Ensures MustClauses are used by GuardClauses; flags unused Must clauses.' -Script 'Test-Rule03-GuardUsage.ps1' -OutputPath 'artifacts/audit/Rule03-must-to-guard-usage-scan.txt' -UsesConfiguration -UsesFailOnFindings
        & $rule -Id 'Rule04' -Name 'Must → Fluent Usage' -Description 'Ensures MustClauses are used by FluentValidation; flags unused Must clauses.' -Script 'Test-Rule04-FluentUsage.ps1' -OutputPath 'artifacts/audit/Rule04-must-to-fluent-usage-scan.txt' -UsesConfiguration -UsesFailOnFindings
        & $rule -Id 'Rule05' -Name 'Must → DataAnnotations Usage' -Description 'Ensures MustClauses are used by DataAnnotations; flags unused Must clauses.' -Script 'Test-Rule05-DataUsage.ps1' -OutputPath 'artifacts/audit/Rule05-must-to-dataannotations-usage-scan.txt' -UsesConfiguration -UsesFailOnFindings
        & $rule -Id 'Rule06' -Name 'Adapters ↔ Must Parity' -Description 'Checks public concept parity for GuardClauses/FluentValidation/DataAnnotations against MustClauses using normalized concept vocabulary.' -Script 'Test-Rule06-Parity.ps1' -OutputPath 'artifacts/audit/Rule06-adapters-vs-must-parity.txt' -UsesConfiguration -UsesFailOnFindings
        & $rule -Id 'Rule07' -Name 'Nullability Policy' -Description 'Checks hybrid nullability strategy for Must/Guard primary parameters (TXT report).' -Script 'Test-Rule07-Nullability.ps1' -OutputPath 'artifacts/audit/Rule07-hybrid-nullability-policy-scan.txt' -UsesAllowViolations
        & $rule -Id 'Rule08' -Name 'Method Ordering' -Description 'Checks cross-layer method ordering parity (Rules/Must/Guard/FV/DA) (TXT report).' -Script 'Test-Rule08-Ordering.ps1' -OutputPath 'artifacts/audit/Rule08-method-ordering-parity.txt' -UsesAllowViolations
        & $rule -Id 'Rule09' -Name 'Catalog Integrity' -Description 'Validates audit rule catalog entries and wrapper scripts are consistent (paths/outputs/metadata).' -Script 'Test-Rule09-CatalogIntegrity.ps1' -OutputPath 'artifacts/audit/Rule09-catalog-integrity.txt' -UsesFailOnFindings
        & $rule -Id 'Rule10' -Name 'PS Normalization' -Description 'Checks PowerShell script normalization: single-line param declarations, CmdletBinding help header has .PARAMETER entries, and no comments inside param blocks.' -Script 'Test-Rule10-PsNormalization.ps1' -OutputPath 'artifacts/audit/Rule10-ps-normalization.txt' -UsesFailOnFindings
        & $rule -Id 'Rule50' -Name 'Unit Test File Normalization' -Description 'Ensures *Tests.cs ↔ *TestData.cs pairing (no orphans), and enforces Theory-only policy (no [Fact]) in *Tests.cs.' -Script 'Test-Rule50-UnitTestFileStructureNormalization.ps1' -OutputPath 'artifacts/audit/Rule50-unit-test-file-structure-normalization.txt' -UsesFailOnFindings
        & $rule -Id 'Rule51' -Name 'Unit Test Class Semantics' -Description 'Ensures unit test methods live in nested public static classes (grouped by operation); no top-level test methods on outer test classes.' -Script 'Test-Rule51-UnitTestClassSemanticStructure.ps1' -OutputPath 'artifacts/audit/Rule51-unit-test-class-semantic-structure.txt' -UsesFailOnFindings
        & $rule -Id 'Rule52' -Name 'Unit TestCase Records' -Description 'Enforces unit test case record conventions in *TestData.cs: ValidCase must inherit and must not derive directly from BaseCase/ValueCase unless allowlisted.' -Script 'Test-Rule52-UnitTestCaseRecordConventions.ps1' -OutputPath 'artifacts/audit/Rule52-unit-testcase-record-conventions.txt' -UsesFailOnFindings
        & $rule -Id 'Rule53' -Name 'Test Orphans' -Description 'Ensures every *Tests.cs file corresponds to a valid Source class file in the matching Source project.' -Script 'Test-Rule53-TestOrphans.ps1' -OutputPath 'artifacts/audit/Rule53-test-orphans.txt' -UsesFailOnFindings
        & $rule -Id 'Rule54' -Name 'Unit Test Tuple Conventions' -Description 'Enforces tuple conventions in *TestData.cs (heuristic): tuple element identifiers should be camelCase; discourages layering identifiers like Args/Arguments/Context unless allowlisted.' -Script 'Test-Rule54-UnitTestTupleConventions.ps1' -OutputPath 'artifacts/audit/Rule54-unit-testtuple-conventions.txt' -UsesFailOnFindings
    )
}
