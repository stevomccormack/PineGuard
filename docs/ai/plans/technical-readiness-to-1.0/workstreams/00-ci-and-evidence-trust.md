<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w00
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W00 — CI routing and trustworthy evidence

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and current evidence

P0 makes later evidence trustworthy. v1.6 §5.2 and rubric R1–R3 identify source-derived routing gaps: shared test props and CLI/workspace/lockfile inputs are not completely represented, the CLI audit job omits Vitest/typecheck/lint, absent expected TRX can be treated as no coverage required, and layer-parity/must-codes have gate:false. These observations are not results from a reproduced hosted workflow run.

Verified inputs: `.github/workflows/ci.yml`, `tests/Directory.Build.props`, `apps/cli/package.json`, `apps/cli/src/audit/engine.ts`, `apps/cli/src/audit/rules/layer-parity.ts`, `must-codes.ts`; dated test report in the source map.

## Prerequisites and decisions

Luna must load coordination and CLI/project testing specifications. Astra must approve the expected job/artifact contract and exact routing fixture paths after inventory. D03 governs counts. D07 governs inventory provenance; do not activate semantic parity/code gates until W01/W02/W05 provide reliable contracts. Current 30-TRX evidence is one dated matrix, not a hard-coded perpetual expectation.

## Ordered tasks

| Task | Action and inputs | Required output |
|---|---|---|
| 00.1 | Inventory event/path filters, change detection, job conditions, matrix expansion, package scripts, workspace manifests and lockfiles | `docs/reports/readiness-ci-evidence.md`: exact path/job/artifact matrix and current blind spots |
| 00.2 | Define representative changed-file fixtures for shared test props, CLI source/test/config/dependency inputs, docs-only and unrelated legitimate changes | Approved exact fixture path map and expected selected jobs; no filename guessed before inventory |
| 00.3 | Implement only approved routing corrections and CLI self-check commands | Owned workflow/CLI diff; fixtures prove jobs execute or intentionally skip |
| 00.4 | Define required artifacts from selected jobs/targets and distinguish no-applicable-test scope from missing/empty/malformed artifacts | Fail-closed artifact manifest/validator in approved existing mechanism |
| 00.5 | Route validator self-tests and seed missing TRX, wrong target, corrupt TRX, absent coverage-required output and valid no-applicable cases | Saved positive/negative gate outcomes with exact commands |
| 00.6 | After pilot contracts are reviewed, seed layer/code drift, prove findings, then activate each gate independently | Gate activation record and no unexplained initial findings |

## Acceptance and evidence

C0 requires all selected test jobs to account for their expected artifacts, and every deliberate missing-evidence case to fail. Required CLI Vitest/typecheck/lint scripts run as identified from package.json; do not invent command names. Routing tests must catch each documented blind spot and accept legitimate skip cases. Audit self-tests themselves must be selected by relevant changes.

Record revision/environment, selected jobs, expected/actual artifact counts, failure reasons and hosted/local limitations. A local fixture test does not prove hosted permissions, upload/download or event behavior; reproduce the relevant hosted path before claiming end-to-end enforcement.

## Ownership, checkpoints and stops

Sol may edit only Astra-approved workflow/CLI paths; Luna supplies all file reads. Checkpoint after routing inventory, after fail-closed fixtures, and before each gate activation. Astra reviews seeded-failure evidence before a coherent exact-file commit. Stop for unexplained missing matrix entries, unsupported no-coverage exceptions or broad unrelated CI changes. W00 owns routing/artifact trust; semantic expectations remain owned by W01/W05.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [source map](../references/source-map.md). This child remains Planned; no implementation is authorized by this document.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

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

Independent outcomes: absent scope, missing/duplicate artifact, corrupt JSON, empty counts or Covered<Total throws; exact Covered=Total>0 for both metrics for every expected row passes. Inputs must use reviewed scope, not discovered successful artifacts. The synthetic governance self-test exercised this example against local fixtures; it does not prove hosted artifact routing or repository coverage. The ordinary pilot pass is not coverage evidence.
