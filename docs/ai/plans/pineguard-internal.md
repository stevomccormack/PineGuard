<!-- metadata_header
type: plan
id: plan-pineguard-internal
version: 2.0
status: Active
council_required: true
council_verdict_ref: docs/ai/plans/pineguard-internal/council-transcript.md
council_verdict_date: 2026-04-24
council_trigger: release-ip
sub_plans:
  - docs/ai/plans/pineguard-public.md
-->

# Plan: PineGuard Publish Split (Internal → Public)

## Framing (post-council)

This plan is **forward-only hygiene and packaging discipline**, not IP protection. The council confirmed that the existing public repo history already contains the Brain and tooling through commit `25cba5b`; no force-push can retroactively privatize that content. Everything below is about keeping *future* commits clean and separating the factory from the product.

The public PineGuard repo becomes **source-available, not OSS**, as a direct consequence of one-way sync. This must be stated in the public README on day one so the social contract is explicit.

## Goals

- **This repo (PineGuard.Internal, PRIVATE)** becomes the single source of truth for library source, Brain, tooling, AI adapters, and release machinery.
- **PineGuard (PUBLIC)** becomes a minimal consumer-facing tree: `src/`, `tests/`, solution files, a narrow CI + publish pipeline. Owns NuGet publishing.
- One-way sync Internal → Public. No reverse path until a cherry-pick-back protocol is designed (out of scope for v1).

## Non-Goals (rejected by council)

- Productization of the Brain (paid tier, template pack, blog post on cutover day, Sponsors tier). Revisit in 12+ months *only* if inbound signal appears unprompted.
- Retroactive history scrub on the public repo. Accepted as already-leaked; documented, not fought.
- Accepting community PRs on the public repo. Out of scope for v1; the README states the repo is source-available.

## Two-Plan Split

| Plan | Scope |
|---|---|
| **This plan** (`pineguard-internal.md`) | Internal-side changes: create Internal GitHub repo, relocate assets for packaging, build the sync tooling in `tools/release/`, verify end-to-end from Internal. |
| **Sub-plan** ([`pineguard-public.md`](pineguard-public.md)) | Public-side changes: target tree structure, slim CI, publish workflow with Source Link fix, README disclosure of source-available status, branch protection. |

Work the sub-plan only after Phase 2 of this plan is complete and a dry-run sync has produced a verified scratch tree.

## Phases (Internal side)

### Phase 0 — Create the private repo (Monday, 10 minutes)

> Chairman's "one thing to do first." Do this before writing any script.

1. Use `gh` with `GH_TOKEN` from `.etc/powershell/.env` to create the empty private repo `stevomccormack/PineGuard.Internal`.
2. In this local clone, `git push origin main` (origin already points at `PineGuard.Internal` per prior session).
3. Verify the push succeeded: `gh repo view stevomccormack/PineGuard.Internal`.

**Reversibility**: complete. Nothing on the public repo changes.

### Phase 1 — Packaging prep in Internal

Before any sync machinery, make sure `dotnet pack` produces a valid `.nupkg` from this repo with the logo embedded. This is the council's "logo canary" — the one change that can silently break NuGet.

1. **Relocate logo**: `docs/brand/pineguard-logo-128px.png` → `assets/pineguard-logo-128px.png`.
2. **Patch `Directory.Build.props`**: `<PackageIcon>assets/pineguard-logo-128px.png</PackageIcon>` (or adjust `<None Include>` path).
3. **Add to each `src/*/*.csproj`**: `<None Include="..\..\assets\pineguard-logo-128px.png" Pack="true" PackagePath="\" />`.
4. **Verify**: `dotnet pack PineGuard.slnx -c Release --output ./artifacts/pack-verify` — inspect one `.nupkg` (e.g. with `unzip -p *.nupkg assets/pineguard-logo-128px.png | file -`). Must return a valid PNG.
5. **Grep for residuals**: `docs/`, `tools/` references in `src/**/*.{cs,csproj,md}` and root `README.md`. Patch or remove.
6. **Decide on `AGENTS.md` files in `src/`, `tests/`**: recommend delete — they're one-line pointers to `docs/ai/rules/*.md` which `.claude/rules/` already covers.

### Phase 2 — Source Link + deterministic build fix (blocker for first public release)

Council's unanimous-blind-spot item. Without this, `.nupkg` symbols for every future published version point at commits that exist only in the private repo, breaking "Go to definition" for all consumers.

Decision tree:

- **Option A (recommended)**: publish from the public repo (as already planned). The `publish.yml` in public runs against public commits. SourceLink embeds public commit SHAs. No Internal SHA ever enters a `.nupkg`.
- **Option B**: publish from Internal, rewriting `RepositoryUrl` and SHA to point at the equivalent public commit. Requires mapping Internal commits → public commits (only possible if sync is deterministic and traceable). More fragile.

Option A is the plan. This phase ensures the public `publish.yml` we emit in Phase 4 is configured correctly — `PublishRepositoryUrl=true`, `EmbedUntrackedSources=true`, `DebugType=portable`, `Deterministic=true` (already set) — AND that the Internal `Directory.Build.props` does not leak SHA references that travel to the public tree.

Note for already-published versions (pre-cutover): those `.nupkg` symbols embed `github.com/stevomccormack/PineGuard` commits that will still resolve after cutover since the public repo retains those commits in history (force-push only rewrites recent main). Document this in the next release's changelog.

### Phase 3 — Build the sync tooling (local, no Action yet)

Council insisted: **do not automate a sync that has not run by hand twice**.

Deliverables in Internal:
- `tools/release/public-allowlist.txt` — exact paths that sync.
- `tools/release/public-denylist.txt` — explicit exclusions (safety net for directories that should never leak).
- `tools/release/sync-public.ps1` — PowerShell script:
  - Input: `-RepoRoot`, `-OutputDir`, `-DryRun`, `-TargetRemote`, `-TargetBranch`.
  - Steps: `rsync`-equivalent copy per allowlist → drop `src/**/AGENTS.md`, `tests/AGENTS.md` → swap `ci.yml` with template → fail if any `docs/ai`, `tools/`, `.claude/`, `.pi/`, `.agent/`, `AGENTS.md` paths leaked into output → optional `--push` (commits + pushes to target).
- `tools/release/templates/ci.public.yml` — slim CI for public.
- `tools/release/templates/publish.yml` — canonical publish (moved from `.github/workflows/publish.yml`).

First run: `-DryRun` only. Diff output against the current public tree. Eyeball the allowlist. Expect surprises.

Second run: same, no `-DryRun`, push to a branch `sync-bootstrap` on the public repo.

### Phase 4 — Cut over public (detailed in the sub-plan)

See [`pineguard-public.md`](pineguard-public.md). At a high level:
1. Create branch `sync-bootstrap` on public from script output.
2. Verify `dotnet restore/build/test/pack` on `sync-bootstrap`.
3. README rewrite on public to disclose source-available status.
4. Force-forward `main` → `sync-bootstrap` once verified twice.
5. Enable branch protection on public `main` (no direct pushes except sync bot).

### Phase 5 — Clean up Internal

After public is stable:
- Delete `.github/workflows/publish.yml` from Internal (moved to `tools/release/templates/publish.yml`).
- Internal `ci.yml` stays as-is (uses full tree).
- Add `.github/workflows/sync-public.yml` (Action wrapping `sync-public.ps1`) — optional. Only after Phase 4 has been executed by hand *twice* with no surprises. Single maintainer can defer this indefinitely.

## Open Decisions (from the draft plan, now resolved)

| Question | Decision | Reason |
|---|---|---|
| Automated vs manual sync | **Manual first**, Action later | Council: "do not automate a broken sync faster" |
| Curated vs squash history in public | **Squash per sync** | Honest, simple; "this is a generated mirror" |
| Keep `AGENTS.md` in `src/`, `tests/` inside Internal? | **Delete everywhere** | `.claude/rules/` already covers; YAGNI |
| Logo relocation | **Move to `assets/`** | Required for packaging on public tree |
| Accept community PRs on public? | **No, not initially** | Incompatible with one-way sync; state in README |
| Rename this repo on GitHub | **Create new `PineGuard.Internal`**; push there; do NOT rename existing `PineGuard` | Existing `PineGuard` must remain in place as the consumer-facing public repo |
| Productize Brain? | **No** | Unanimous council blind spot; scaffolding not product |

## Invariants

- `docs/`, `tools/`, `.agent/`, `.amazonq/`, `.claude/`, `.clinerules/`, `.cursor/`, `.junie/`, `.pi/`, `.windsurf/`, `CLAUDE.md`, `GEMINI.md`, `AGENTS.md` — **must never appear** in the public tree. The sync script hard-fails if any of these leak.
- First public force-push requires two successful dry-runs with byte-identical output.
- The public repo README must disclose source-available status in its first post-cutover commit.

## Definition of Done (this plan)

- [ ] `stevomccormack/PineGuard.Internal` exists on GitHub and contains the current local tree.
- [ ] Logo relocated; `dotnet pack` on Internal produces valid `.nupkg` with embedded icon.
- [ ] `tools/release/sync-public.ps1` + allowlist/denylist/templates committed.
- [ ] Two successful `-DryRun` executions producing identical scratch output.
- [ ] Sub-plan [`pineguard-public.md`](pineguard-public.md) reaches its own DoD.

## References

- Council transcript: [`pineguard-internal/council-transcript.md`](pineguard-internal/council-transcript.md)
- Sub-plan: [`pineguard-public.md`](pineguard-public.md)
- Council spec: [`../specs/council.md`](../specs/council.md)
- Safety spec: [`../specs/safety.md`](../specs/safety.md)
