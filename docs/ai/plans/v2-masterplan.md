<!-- metadata_header
type: plan
id: v2-masterplan
version: 1.0
status: active
children:
  - v2-pineguard.md
supersedes:
  - v2-restructure.md
  - licensing.md
-->

# PineGuard v2 — Master Plan

> [!IMPORTANT]
> This is the **root plan** for the PineGuard v2 restructure.
> It defines the vision, package inventory, and phased execution for the PineGuard repo.
> Standards and Skills are managed in their own repos.
>
> | Repo | Plan | Focus |
> |------|------|-------|
> | PineGuard | [v2-pineguard.md](v2-pineguard.md) | Prune standards, publish MIT packages |

---

## 1. Vision

PineGuard publishes free, MIT-licensed validation packages to nuget.org.

```
FREE (MIT)
─────────────────────────
PineGuard
  .Core
  .MustClauses
  .GuardClauses
  .FluentValidation
  .DataAnnotations
  .Testing

6 packages (nuget.org)
```

---

## 2. Package Inventory (Final State)

### 2.1 nuget.org (MIT, public) — 6 packages

| Package | Dependencies |
|---------|-------------|
| `PineGuard.Core` | — |
| `PineGuard.MustClauses` | Core |
| `PineGuard.GuardClauses` | Core, Must |
| `PineGuard.FluentValidation` | Core, Must |
| `PineGuard.DataAnnotations` | Core, Must |
| `PineGuard.Testing` | Core |

---

## 3. Phased Execution

### Phase 0 — Preparation (no code changes)

- [ ] Update PineGuard LICENSE to MIT

### Phase 1 — Publish PineGuard v1 as MIT (current code, as-is)

- [ ] Add NuGet metadata to `Directory.Build.props`
- [ ] Add MinVer, SourceLink, deterministic builds
- [ ] Create `publish.yml` CI workflow
- [ ] Tag `v1.0.0`, publish 6 packages to nuget.org
- [ ] **Standards still bundled** — this is v1, not v2

**Why:** Get packages live NOW. The split is v2.

### Phase 2 — Prune Standards from PineGuard

> **Detailed plan:** [v2-pineguard.md](v2-pineguard.md)

- [ ] Prune standards from PineGuard repo
- [ ] Verify build + tests pass independently

### Phase 3 — Publish v2

> **Detailed plan:** [v2-pineguard.md §3](v2-pineguard.md)

- [ ] Publish PineGuard v2 (6 packages, MIT) to nuget.org
- [ ] Migration guide for v1 → v2 consumers

---

## 4. Verification Strategy

Every phase uses `tools/maintenance/Test-StructuralIntegrity.ps1` as the verification gate:

| Check | What it catches |
|-------|----------------|
| `dotnet build` | Compilation errors, broken references |
| `dotnet test` | Runtime regressions |
| Stale path references | Old paths in `.md` and `.ps1` files |
| Stale namespace references | Old namespaces in `.cs` files |
| Namespace/folder alignment | Namespace doesn't match folder structure |
| Sonar path validation | Hardcoded sonar paths pointing to moved files |

---

## 5. Resolved Questions

1. **GeoLocation**: General-purpose — stays in PineGuard (pure coordinate validation).
2. **GeoRegion**: Standard — moved to Standards repo (uses DefaultGeoRegionData/GeoNames dataset).
3. **OWASP**: General-purpose — stays in PineGuard (not a governing-body standard).
4. **HttpSecurityHeaders**: General-purpose — stays in PineGuard.
5. **Copy-then-prune**: Safer than surgical extraction — both sides start from working code.
6. **FluentValidation separated**: Avoids forcing the dependency on non-Fluent consumers.

---

## References

- Previous plans (superseded by this hierarchy):
  - [v2-restructure.md](v2-restructure.md) — original strategic plan
- Verification: `tools/maintenance/Test-StructuralIntegrity.ps1`
- Brain index: `docs/ai/README.md`

<!-- footer
last_verified: 2026-04-16
-->
