<!-- metadata_header
type: command
id: cmd-council
version: 1.0
-->

# Command: Council

> [!NOTE]
> **Interface Definition**: explicit triggers and contracts for the LLM Council decision-support procedure.

## 1. Triggers (Slash Commands)

| Command                | Scope                | Auto-Approve | Description |
| :--------------------- | :------------------- | :----------- | :---------- |
| `/ask-council`         | Ad-hoc question      | ❌ No        | Pressure-test a decision through five advisors and a chairman. |
| `/plan-with-council`   | Plan under `docs/ai/plans/` | ❌ No | Draft a plan, council its recommendation, revise, stamp the verdict. |

Auto-approval is **off** by default because the council spawns 10 sub-agents and should be an intentional user action.

### Natural-language triggers (in chat)

Mandatory: `council this`, `run the council`, `war room this`, `pressure-test this`, `stress-test this`, `debate this`.

Strong (when combined with a real trade-off): `should I X or Y`, `which option`, `what would you do`, `is this the right move`, `validate this decision`, `get multiple perspectives`, `I'm torn between`.

Do **not** trigger on factual lookups, creation tasks, or low-stakes "should I" phrasing (see [`../specs/council.md`](../specs/council.md) §"When the Council MUST NOT Run").

## 2. Execution Logic

**Agent entrypoint**: [`../agents/ask-council.md`](../agents/ask-council.md)
**Canonical procedure**: [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md)
**Plan workflow**: [`../workflows/plan-with-council.md`](../workflows/plan-with-council.md)

## 3. Auto-Approval Rules

- **Claude**: not auto-approved. Slash command `/ask-council` is explicit; the command adapter at `.claude/commands/ask-council.md` delegates to the brain agent.
- **Copilot**: `.github/prompts/ask-council.prompt.md` requires explicit invocation.
- **Gemini**: `.agent/workflows/ask-council.md` **omits** `// turbo-all` — the user must confirm.
- **Pi**: `.pi/skills/ask-council/SKILL.md` requires explicit invocation.

## 4. When Is This the Right Command?

Prefer `/ask-council` when:
- The decision has genuine uncertainty and non-trivial cost-of-being-wrong.
- Multiple valid options exist with different trade-offs.
- The plan touches public API, protocol, release mechanics, or multi-project scope.

Prefer a direct agent (e.g. `/audit-gap`, `/scan-sonar`) when the work has one right procedure.

## References

- [`../specs/council.md`](../specs/council.md)
- [`../agents/ask-council.md`](../agents/ask-council.md)
- [`../roles/council.md`](../roles/council.md)
