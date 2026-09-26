<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w21
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W21 — Per-criterion10/10 readiness and release

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

The user requires **10/10 for every technical rubric criterion**. No weighted average can conceal a weaker row. This is an evidence-based engineering acceptance target within declared support scope, not mathematical proof of perfect software.

Inputs: docs/reports/technical-readiness-rubric-2026-09-25.md §§How to interpret, Rubric, R1–R7, Assessment decision; all child evidence; coverage contract; actual council verdict; current package facts. Preserve the historical rubric unchanged. Output: docs/reports/readiness-final-assessment.md.

## Tasks

| Task | Action | Output |
|---|---|---|
| 21.1 | Agree observable10/10 evidence for each of nine rows before execution | Acceptance matrix with owner, support scope, proof and failure condition |
| 21.2 | Maintain gate ledger across waves | Current revision/command/artifact/decision references |
| 21.3 | Reproduce final coverage, conformance, hostile testing, budgets, deployment, API/dependencies and docs checks | Release-candidate evidence tied to exact artifacts |
| 21.4 | Perform Astra review and required formal council stages | Real findings, responses, revisions and verdict references |
| 21.5 | Reassess each row independently with confidence/limitations | New assessment; no automatic score from plan completion |
| 21.6 | Consider separately bounded rule expansion only after prerequisite exits | Explicit decision and new manifest/conformance/coverage obligations |

## Hard exits

All validation surfaces meet100% line and branch coverage in the audited approved Coverlet denominator; all required reports exist. New structure/manifest, semantic truth, projections, security/resources, performance, deployment and consumer guidance pass their distinct gates. Duplicates, local outliers, hacks, hidden failures and speculative abstractions violate the quality constitution.

Any unmet material criterion prevents declaring every row10/10. Explicit unsupported scope may be documented honestly; it cannot hide a promised failing capability or dilute coverage. Required council records must exist, not be inferred from an agent's role.

## Controls

No broad rule expansion displaces early CI trust, taxonomy, new structure/manifest or benchmarks. The handoff's indefinite freeze is not adopted: later expansion is a bounded reviewed decision. Major sample projects remain late, with individual gates and declared release scope.

Checkpoint each wave and before release decisions. Actual release/publication/deployment remains a separately authorized action. Luna gathers evidence, Sol executes approved code, Astra adjudicates/chairs review, and the orchestrator coordinates only.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

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
