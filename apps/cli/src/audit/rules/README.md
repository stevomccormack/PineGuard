# `src/audit/rules/`

One file per audit rule, `<slug>.ts`. The full "adding a new rule" recipe —
what a rule file must call, how it gets wired up, and what its tests should
look like — lives in the doc comment at the top of
[`../catalog.ts`](../catalog.ts); read that first.

Quick version: a rule file calls `registerRule({ ... })` at module load time,
then gets a matching `import "./<slug>.js";` line added to
[`index.ts`](index.ts) (the barrel `src/commands/audit.ts` imports so every
rule's registration actually runs). Tests go in
`test/rules/<slug>.test.ts` against `test/fixtures/<slug>/{valid,invalid}/`
per the VIBE convention in [`../../../test/README.md`](../../../test/README.md).

Empty until plan P2 lands the first rule
(`docs/ai/plans/audit-cli-rebuild.md` §9.2, P2.1-P2.14).
