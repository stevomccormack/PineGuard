# Dated baseline evidence

This is a historical evidence summary, not a new test/coverage run or a1.0 certification. The [snapshot provenance](snapshot-provenance.md) records byte-preserving copies and hashes; originals remain untouched.

| Evidence | Recorded finding | Limitation |
|---|---|---|
| [Test evidence JSON](snapshots/baseline-test-evidence.json) | Revision2aeac39b88effef5518741a9cf9b505a04f926f4; Windows Debug; SDK10.0.401 | Local dated environment, not every supported platform |
| [README verification](snapshots/readme-verification-2026-09-25.txt), Full test suite |15 projects ×2TFMs =30TRX; net8 18,862, net10 18,887; total37,749; zero failed/skipped | Framework executions repeat definitions; not unique tests; raw30TRX are not bundled here |
| Same, Analyzer reference-cache repair | Initial115 failures; empty local reference cache repaired from archive; targeted118/118 | No production source fix established |
| Same, Coverage and other checks | No fresh coverage collected | Cannot claim current100% from that run |
| Same, Release and documentation checks |41 links,29 anchors,3 Mermaid;15 packages:6 alpha.7 published,9 source-only | September25 snapshot, refresh before claims |
| [Rubric](snapshots/readiness-rubric-2026-09-25.txt) | Nine coarse independent judgments with confidence | No weights/aggregate; not objective certification |
| [README snapshot](snapshots/README-2026-09-25.txt) | Product/quality/package/positioning claims | Claims need current artifact evidence |

The [v1.6 snapshot](snapshots/baseline-v1.6.txt) remains the historical planning baseline. Explicit current user instructions override its backward-compatibility/later-benchmark sequencing and authorize early new rule structure/manifest. Preserve source-derived observations as hypotheses until reproduced.

## Rubric baseline

Semantic/failure7 (high confidence); architecture/cross-surface8 (high); assertion/adversarial7 (medium); consumer API/composition8 (medium); security/data6 (medium); performance/resource4 (high confidence in gap); API/package/deployment6 (medium); CI/tooling/release5 (high for source-derived paths); docs/maintainability7 (high).

The new target is10/10 EACH technical row. This plan changes the work/evidence structure, not the historical scores. Future reassessment must use the [acceptance matrix](acceptance-matrix.md), keep confidence/limits visible and preserve the original rubric unchanged.
