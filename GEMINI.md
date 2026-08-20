# Gemini Adapter (PineGuard)

> [!IMPORTANT]
> **This file is an Adapter.**
> It maps Gemini interactions to the canonical Brain in `docs/ai/`.
> Do not add logic here. Add logic to the Brain.
> 👉 Start at **[docs/ai/README.md](docs/ai/README.md)** for the full Brain index.

All shared agent instructions — role adoption, specs, rules, skills, agents, safety, and the
Knowledge Base index — live in **[AGENTS.md](AGENTS.md)**; read it first. Everything there applies
to Gemini unchanged.

## Related surfaces

`.agent/workflows/` is the **Antigravity** adapter, not a Gemini surface — its stubs delegate to the
same Brain agents this file points at. The full surface inventory is in
👉 **[docs/ai/meta/adapter-surfaces.md](docs/ai/meta/adapter-surfaces.md)**
