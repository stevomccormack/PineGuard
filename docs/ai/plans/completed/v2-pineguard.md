<!-- metadata_header
type: plan
id: v2-pineguard
version: 1.0
status: planned
parent: v2-masterplan.md
-->

# PineGuard — Repo Plan (v2 Pruning)

> [!IMPORTANT]
> **Parent plan:** [v2-masterplan.md](v2-masterplan.md)
> This plan covers the PineGuard repo after the split: pruning standards code,
> updating Brain/tooling, and publishing v2 as MIT.

> [!NOTE]
> **Phase 2 (Prune) is COMPLETE.** All standards code (ISO, IANA, CLDR, ITU, Geo Regions,
> TimeZones) has been removed from the PineGuard repo. The checklist below is retained
> for historical reference.

---

## 1. Phase 2 — Prune Standards from PineGuard (COMPLETE)

Prune all standards-related code from the PineGuard repo.

### 1.1 What to remove

| Area | What to delete |
|------|----------------|
| **Core** | `src/PineGuard.Core/Standards/` (entire folder: Iso/, Iana/, Cldr/, Itu/, Geo/) |
| **MustClauses** | `src/PineGuard.MustClauses/Standards/` (entire folder after Phase 1.5 separation) |
| **GuardClauses** | `src/PineGuard.GuardClauses/Standards/` (entire folder) |
| **FluentValidation** | `src/PineGuard.FluentValidation/Standards/` (entire folder) |
| **DataAnnotations** | `src/PineGuard.DataAnnotations/Standards/` (entire folder) |
| **Tests** | `tests/*/Standards/` (entire subfolders in all test projects) |
| **Generators** | `tools/code-generators/iso/`, `iana/`, `cldr/`, `itu/` (keep shared/ if needed) |
| **Datasets** | Any CSV/JSON source data files for ISO/IANA/CLDR/ITU |

### 1.2 What stays (does NOT change)

- All general-purpose rules (String, Number, Date, Boolean, Guid, Enum, GeoLocation, OWASP, etc.)
- All general-purpose Must/Guard/Fluent/DA adapters
- PineGuard.Testing (published as public package)
- All Brain docs (`docs/ai/`) — updated, not removed
- All Claude Code config (`.claude/`) — updated, not removed
- All tools except standards-specific generators
- CI/CD workflows (updated scope)
- Solution files, build props, editorconfig

### 1.3 Prune gated tasks

| Task | Description | Verification |
|------|-------------|--------------|
| ~~**R1**~~ | ~~Delete `Standards/` folders from all 5 src projects~~ | Done |
| ~~**R2**~~ | ~~Remove orphaned `using PineGuard.*.Standards*;` statements~~ | Done |
| ~~**R3**~~ | ~~Remove orphaned `<seealso cref>` references to standards types~~ | Done |
| ~~**R4**~~ | ~~Delete `Standards/` folders from all test projects~~ | Done |
| ~~**R5**~~ | ~~Remove standards test fixtures and data files~~ | Done |
| ~~**R6**~~ | ~~Delete standards generators and datasets~~ | Done |
| ~~**R7**~~ | ~~Update `InternalsVisibleTo` (remove standards test projects if separate)~~ | Done |
| ~~**R8**~~ | ~~Commit checkpoint — prune complete~~ | Done |

---

## 2. Phase 3 — Brain & Tooling Updates

### 2.1 Brain docs updates (`docs/ai/`)

| File/Area | Change |
|-----------|--------|
| `specs/core/project.md` | Remove standards rule references, update rule count |
| `specs/must-clauses/project.md` | Remove Standards/ folder structure, update file count |
| `specs/guard-clauses/project.md` | Same |
| `specs/fluent-validation/project.md` | Same |
| `specs/data-annotations/project.md` | Same |
| `specs/spec.md` | Remove §4 Standards Domains Playbook or redirect to Standards repo |
| `specs/dependencies.md` | Remove standards validation checklist steps |
| `specs/testing/unit-test.md` | Remove standards test mirroring examples |
| `specs/testing/gold-standard.md` | Remove standards gold standards |
| `specs/testing/fixture.md` | Remove standards fixture conventions |
| `agents/generate-*.md` | Remove or mark as "Standards repo only" |
| `skills/scaffold-standard/` | Remove (lives in Standards repo now) |
| `rules/global.md` | Update to clarify scope (no Standards) |
| `plans/` | Archive superseded plans, update active plan references |

### 2.2 Claude Code updates (`.claude/`)

| Feature | Change |
|---------|--------|
| `.claude/commands/` | Remove standards-specific commands (generate-iso, generate-iana, etc.) |
| `.claude/skills/scaffold-standard/` | Remove (lives in Standards repo) |
| `.claude/agents/validation-builder.md` | Update scope (no standards references) |
| `.claude/rules/` | Update path globs (no `Standards/` paths) |
| `.claude/settings.json` | Update whitelisted commands |

### 2.3 Tools updates

| Tool | Change |
|------|--------|
| `tools/code-generators/` | Remove ISO/IANA/CLDR/ITU generators |
| `tools/code-coverage/` | Update scope (fewer projects/files) |
| `tools/testing/Run-Tests.ps1` | Update scope |
| `tools/audit-cli/` | Remove standards-specific parity checks (or update for new structure) |
| `tools/code-diagnostics/` | Update scope |
| `tools/sonar-scanner/` | Update scope, remove standards file references |

### 2.4 Other config updates

| File | Change |
|------|--------|
| `sonar-project.properties` | Remove standards file references |
| `.github/workflows/ci.yml` | Update if standards-specific steps exist |
| `PineGuard.slnx` | Remove standards project references (if any separate projects were created) |
| Gemini adapter (`.agent/`) | Remove generate-* workflow stubs |

### 2.5 Tooling update gated tasks

| Task | Description | Verification |
|------|-------------|--------------|
| **U1** | Update Brain specs (14 files) | No stale references to Standards/ paths |
| **U2** | Update Claude Code config | No broken command/skill references |
| **U3** | Update tools | All tool scripts run without errors |
| **U4** | Update CI/CD and sonar config | CI green |
| **U5** | Archive superseded plans | Plans index updated |
| **U6** | Final verification | `Test-StructuralIntegrity.ps1` all checks pass |

---

## 3. Phase 3 — Publish v2

- [x] Update `Directory.Build.props` with v2 NuGet metadata
- [ ] Tag `v2.0.0`
- [ ] Publish 6 updated packages to nuget.org
- [ ] Write migration guide (v1 → v2):
  - `PineGuard.Iso` etc. now come from private feed
  - Add `using PineGuard.*.Standards;` → no longer needed (types are in separate packages)
  - Consumer nuget.config update for Azure Artifacts feed

### 3.1 Migration guide outline

```markdown
## Migrating from PineGuard v1 to v2

### What changed
- Standards validators (ISO, IANA, CLDR, ITU) moved to separate packages
- PineGuard core packages are now MIT-licensed and standards-free

### If you DON'T use standards validators
- Update PineGuard packages to v2 — no other changes needed

### If you DO use standards validators
1. Add the PineGuard private feed to your nuget.config
2. Install the relevant standards packages (e.g., PineGuard.Iso)
3. Update using statements (namespaces changed)
4. Set PINEGUARD_LICENSE_KEY for CI/Release builds
```

---

## 4. Post-v2 Maintenance

### 4.1 Package count

After v2, PineGuard publishes 6 packages to nuget.org:

| Package | Dependencies |
|---------|-------------|
| `PineGuard.Core` | — |
| `PineGuard.MustClauses` | Core |
| `PineGuard.GuardClauses` | Core, Must |
| `PineGuard.FluentValidation` | Core, Must |
| `PineGuard.DataAnnotations` | Core, Must |
| `PineGuard.Testing` | Core |

### 4.2 Ongoing responsibilities

- General-purpose validation rules development
- PineGuard.Testing maintenance (shared with Standards consumers)
- Brain and tooling maintenance
- Community support via GitHub Issues/Discussions
- NuGet package publishing (public feed)

---

## References

- Parent plan: [v2-masterplan.md](v2-masterplan.md)
- Verification tool: `tools/maintenance/Test-StructuralIntegrity.ps1`

<!-- footer
last_verified: 2026-04-16
-->
