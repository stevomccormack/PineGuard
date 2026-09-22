---
name: xml-audit
description: Review heuristics learned from the PineGuard.Xml (XSD conformance) first-release review, Sep 2026 — XmlReader event timing, warning-flag gating, resolver gaps, polyfill precedent
metadata:
  type: project
---

# PineGuard.Xml review heuristics (2026-09-14)

Patterns to check first on any XmlReader/XmlSchemaSet-based code in this repo:

- **Validation events fire inside `Read()`**, before the caller's loop sees the node. Any loop-maintained
  state (path stacks, "current element") lags by one node at element-start events (missing/invalid
  attributes, empty simple elements, unexpected children). End-element events see correct state.
  Attribute-*value* events fire with the core reader on the Attribute node, so the owner element name is
  not reachable from the reader at all — only the `XmlSchemaValidator` push API gives full control.
- **Warnings need `XmlSchemaValidationFlags.ReportValidationWarnings`**; the default flags
  (`ProcessIdentityConstraints | AllowXmlAttributes`) never deliver `XmlSeverityType.Warning`, so any
  "treat warnings as X" option is dead unless the flag is set.
- **A root-namespace check done after `Read()` runs after the root's own events** — clear or insert
  ahead of collected violations when the contract says "exactly one".
- **`XmlReader.Create(string uri, settings)` opens the URI with an internal default resolver even when
  `settings.XmlResolver` is null** — "resolver null everywhere" claims need the file opened via
  `File.OpenRead` + the Stream overload.
- **Polyfill precedent** (verified 2026-09-14): Core keeps `IsExternalInit`/`CallerArgumentExpressionAttribute`
  internal and grants IVT to MustClauses + GuardClauses (no local copies); DataAnnotations carries a local
  `IsExternalInit` only. Plan 00 §8.1 item 4 mandates IVT for packages applying `[CallerArgumentExpression]`
  on netstandard2.1. Local `#if !NET8_0_OR_GREATER` internal copies are safe but drift from the plan.
- **New package `AGENTS.md` → `docs/ai/rules/<scope>.md`** does not exist until the Brain cascade lands;
  the `doc-links` audit needs a `baseline.json` exemption (OneOf precedent at the `doc-links:` list).

**Why:** the first Xml review found the warnings option dead, the path contract unmet for attribute and
empty-element errors, and the root-namespace ordering fragile — all invisible in a green build.
**How to apply:** run these checks before reading the XML docs/README claims; the docs were accurate to
the *intended* design, not the code.
