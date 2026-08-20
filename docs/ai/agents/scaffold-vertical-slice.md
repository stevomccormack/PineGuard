<!-- metadata_header
type: agent
id: agent-scaffold-vertical-slice
version: 1.0
-->

# Agent: Implement a feature vertical slice (Core -> Must -> Guard -> Adapters -> Tests)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: owner ([../roles/owner.md](../roles/owner.md))

## Context

A vertical slice adds one validation concept to every layer of the stack. The procedure is not restated here:

- [`../specs/spec.md`](../specs/spec.md) §3 *Feature Implementation Checklist* is normative and mandates a strict `Utils -> Rules -> Must -> Guard -> Integrations` order.
- [`../skills/new-validation/SKILL.md`](../skills/new-validation/SKILL.md) is the procedure this agent follows — it carries the per-layer spec/skill routing table.

## Steps

1. **Load the root specs** listed in the skill's §3 before writing any code.
2. **Implement each layer in order** using the skill's §4 routing table. Never implement an upper layer before the layer it delegates to exists, and never duplicate the predicate outside `PineGuard.Core`.
3. **Write the tests** for every layer touched — see *Tests are not optional* below.
4. **Verify** using the repo toolchain, not bare `dotnet` invocations — see *Verification* below.
5. **Summarise** every file created or modified, grouped by layer.

## Tests are not optional

Every layer touched by the slice gets tests in its paired `*.UnitTests` project — `PineGuard.Core.UnitTests`, `PineGuard.MustClauses.UnitTests`, `PineGuard.GuardClauses.UnitTests`, `PineGuard.FluentValidation.UnitTests`, `PineGuard.DataAnnotations.UnitTests`. There is no "if requested" tier:

- `tools/code-coverage/Run-CodeCoverage.ps1` auto-sets `-Enforce100` for every per-layer scope, and [`../specs/testing/coverage.md`](../specs/testing/coverage.md) requires exactly 100% line **and** branch. Untested code fails the run.
- All tests are `[Theory]` + `TheoryData` (never `[Fact]`) with a paired `*TestData.cs` file — enforced by audit Rule50, which CI gates ([`.github/workflows/ci.yml`](../../../.github/workflows/ci.yml)).

## Verification

- [ ] **Build**: solution builds clean.
- [ ] **Test**: run [`/test-all`](test-all.md).
- [ ] **Format**: run [`/format-all`](format-all.md).
- [ ] **Coverage**: run `/coverage-<layer>` for each layer touched and confirm 100% line + branch.
- [ ] **Audit**: run [`/audit-cli`](audit-cli.md) — at minimum the Rule50 CI gate, plus Rule06 (parity) and Rule08 (ordering), since a slice adds methods to every layer.
- [ ] **Inspect**: run JetBrains Qodana — see [`../workflows/scan-qodana.md`](../workflows/scan-qodana.md).
