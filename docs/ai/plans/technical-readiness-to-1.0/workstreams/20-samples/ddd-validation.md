<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w20b
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W20B — Major DDD validation sample, scheduled last

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and entry gate

Demonstrate separation of input validation, conversion and domain invariants using the stable new PineGuard contract. Start only after early structure/manifest/benchmarks and core evidence gates. Domain examples must justify their abstractions; DDD terminology is not permission to create unnecessary layers.

Inputs: W01 input/output semantics, W03 structure, W04 failure contract, W12 public API, W19 lifecycle and W20 documentation standards. Astra first selects the concrete bounded domain, aggregate/value construction responsibilities, invariant timing and error boundary. No generated value-object framework or competitor adapter is assumed.

## Tasks

1. Luna inventories existing domain examples/precedents and supplies exact owned paths. Produce `docs/reports/readiness-sample-ddd.md` with target scenario and dependencies.
2. Astra specifies raw input versus parsed/normalized output, construction validity, aggregate mutation invariants and where exceptions versus result failures belong.
3. Sol implements the smallest coherent domain model and application use cases using canonical rules; prevent duplicate conditions across constructors/factories/handlers.
4. Test construction, invalid transitions, repeated invocation and aggregate invariants with independently specified outcomes. Distinguish request-wide failures from domain impossibility.
5. Exercise serialization/default construction or alternative entry points only if selected in scope; explicitly state untested bypass boundaries.
6. Verify package consumption and runnable examples,100% validation coverage, global naming/formatting and documentation.
7. Astra reviews domain clarity and lack of speculative abstractions; commit exact owned files.

## Acceptance and boundaries

A reader can identify which layer owns each invariant and how valid values are constructed. Normalization is explicit and never silently overwrites raw input. The sample uses approved APIs without local wrappers that merely disguise inconsistencies. All validation branches satisfy the coverage contract.

No persistence/event sourcing/CQRS framework or value-object generator is added unless Astra's scoped scenario requires it. The Vogen comparison supplies design questions, not a dependency mandate.

## Estimate and controls

Engineering-equivalent20–40 hours; active model/tool work6–15 hours; review3–5 hours, low confidence until domain scope. Split by construction, invariant transitions, execution evidence and documentation. Apply [execution protocol](../../references/execution-protocol.md) and [quality constitution](../../references/quality-constitution.md).

Stop for an unresolved core result/lifecycle contract or a domain rule duplicated in multiple layers. Fix the shared cause before extending the sample. Acceptance is separate from API and Clean Architecture projects.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../../references/code-examples.md).

```csharp
internal sealed class PositiveAmount
{
    internal int Value { get; }
    private PositiveAmount(int value) => Value = value;
    internal static bool TryCreate(int value, out PositiveAmount? amount)
    {
        amount = null;
        if (!PositiveInt.Evaluate(value).Passed) return false;
        amount = new PositiveAmount(value);
        return true;
    }
}
```

Purpose: raw input→invariant int conversion→factory→single authority→candidate amount. `1` creates an immutable amount with Value1; zero returns false with null. The sealed class/private constructor prevents ordinary construction bypass in this teaching sample; there is no claim about reflection or serialization. Choosing production constructor/failure shape stays D08/D26. Email and string→DateOnly roadmap examples use verified current APIs above rather than silently inventing semantic policy. Status pending.
