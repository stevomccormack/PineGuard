<!-- metadata_header
type: plan
id: plan-pineguard-public
version: 1.0
status: Planned
parent: docs/ai/plans/pineguard-internal.md
council_verdict_ref: docs/ai/plans/pineguard-internal/council-transcript.md
council_trigger: release-ip
-->

# Plan: PineGuard (Public) Repo Cutover

> Sub-plan of [`pineguard-internal.md`](pineguard-internal.md). Read that first.

## Context

The existing public repo `stevomccormack/PineGuard` currently contains the full tree (library + Brain + tooling + adapters) sharing commits with this Internal repo up to `25cba5b`. After Phase 2 of the parent plan, the Internal side will have a working sync script that produces a clean public tree. This sub-plan describes what the public repo must become, how to cut over safely, and the ongoing operational posture.

This plan executes **only after** parent plan Phases 0–3 complete. Premature execution risks force-pushing garbage.

## Target Public Tree (post-cutover)

```
PineGuard/
├── .editorconfig
├── .gitattributes
├── .gitignore
├── .github/
│   ├── workflows/
│   │   ├── ci.yml              (slim: restore/build/test only)
│   │   └── publish.yml         (OWNs NuGet publishing)
│   ├── dependabot.yml
│   ├── PULL_REQUEST_TEMPLATE.md
│   └── CODEOWNERS              (if still valid)
├── assets/
│   └── pineguard-logo-128px.png
├── src/
│   ├── PineGuard.Core/
│   ├── PineGuard.MustClauses/
│   ├── PineGuard.GuardClauses/
│   ├── PineGuard.FluentValidation/
│   └── PineGuard.DataAnnotations/
├── tests/
│   ├── PineGuard.Core.UnitTests/
│   ├── PineGuard.MustClauses.UnitTests/
│   ├── PineGuard.GuardClauses.UnitTests/
│   ├── PineGuard.FluentValidation.UnitTests/
│   ├── PineGuard.DataAnnotations.UnitTests/
│   ├── PineGuard.Testing/
│   └── PineGuard.Testing.UnitTests/
├── Directory.Build.props
├── Directory.Packages.props
├── LICENSE
├── NuGet.config
├── PineGuard.slnx
└── README.md                    (NuGet-consumer facing + source-available disclosure)
```

## What Must NOT Appear

Hard-fail in sync script if any of these are present in the output tree:
- `docs/**`, `tools/**`
- `.agent/**`, `.amazonq/**`, `.claude/**`, `.clinerules/**`, `.cursor/**`, `.junie/**`, `.pi/**`, `.windsurf/**`
- `AGENTS.md`, `CLAUDE.md`, `GEMINI.md` (at any level, including inside `src/`, `tests/`)
- `.cursorrules`, `.windsurfrules`, `sonar-project.properties`
- `PineGuard.sln.DotSettings*`, `Folder.DotSettings.user`, `.sonarlint/**`, `.etc/**`

## Phases

### Phase 4A — Produce the scratch tree

1. In Internal, run `tools/release/sync-public.ps1 -DryRun -OutputDir ./artifacts/sync-dryrun`.
2. Manually inspect `./artifacts/sync-dryrun`. Confirm it matches the target tree above exactly.
3. Compare to existing public repo: `diff -rq <public-clone> ./artifacts/sync-dryrun | head -200`. Audit every difference.
4. Re-run with any fixes. Two consecutive clean dry-runs before proceeding.

### Phase 4B — Push to `sync-bootstrap` branch on public

1. Clone public repo to a separate scratch path (`D:\temp\PineGuard-bootstrap`), not the existing `D:\...\PineGuard`.
2. Checkout a new branch `sync-bootstrap`.
3. Copy scratch-tree contents over, commit: `chore(sync): initial split from PineGuard.Internal@<sha>`.
4. Push branch: `gh pr create --base main --head sync-bootstrap --title "Split internal/public — do not merge"`.

**Do not open the PR for merge** — it's a verification artifact.

### Phase 4C — Verify the bootstrap branch

Actions triggered by the push:
1. CI should pass: `dotnet restore && dotnet build && dotnet test` against public tree.
2. Locally, on `sync-bootstrap`: `dotnet pack -c Release --output ./artifacts`. Inspect `.nupkg` for:
   - Embedded logo (`assets/pineguard-logo-128px.png`)
   - `RepositoryUrl=https://github.com/stevomccormack/PineGuard`
   - Source Link entries pointing at `github.com/stevomccormack/PineGuard`
3. Run `dotnet nuget push --dry-run` or a local feed test to confirm the package is pushable.

**Exit criteria**: all three green. No red, no yellow, no "close enough."

### Phase 4D — README rewrite (before force-forwarding main)

The council's "social contract" point. Before public `main` changes, land a commit on `sync-bootstrap` that rewrites `README.md` to state:

```markdown
> [!NOTE]
> This repository is **source-available**, not open-source-contribution-enabled.
> The canonical source of truth lives in a private repository. This repo is
> synchronized from that source on each release. Issues and discussions are
> welcome. PRs are currently not accepted; please open an issue instead.
```

Keep the badges, branding, and install instructions. Update any links that point at `docs/`, `tools/`, or other Internal-only paths.

### Phase 4E — Cutover

**Guard**: execute only after Phases 4A–4D have completed and CI is green on `sync-bootstrap`.

1. On public: `git fetch origin`, confirm `sync-bootstrap` is ahead of `main`.
2. Temporarily disable any existing branch protection on `main` (document the window).
3. Force-forward: `git push origin sync-bootstrap:main --force-with-lease`.
4. Delete `sync-bootstrap` branch.
5. Re-enable branch protection on `main` with the new rules (Phase 4F).

**Reversibility**: `main@{1}` reflog and the old commit SHAs remain accessible for ~90 days. Keep a local clone of pre-cutover `main` in `D:\temp\PineGuard-preCutover` for 30 days as a belt-and-braces backup.

### Phase 4F — Branch protection + secret setup

1. Branch protection on `main`:
   - Require PR (but PRs only accepted from sync bot — see §PAT below).
   - Require status checks (CI) to pass.
   - Require linear history.
   - Restrict who can push (sync bot identity only).
   - Disallow force-push except by repo admin (for emergency rollback).
2. Secrets for publish:
   - `NUGET_USER` (exists in `.env`).
   - `NUGET_API_KEY` — rotate the value in `.env`, store in public repo secrets. Do NOT reuse the pre-split key.
3. Tag protection: protect `v*` tags.

### Phase 4G — First post-cutover release

1. On Internal: bump `MinVer` tag, commit, tag `v<next>`.
2. Run sync manually: `tools/release/sync-public.ps1 -Push -TargetBranch main`.
3. On public: create release from the synced tag. `publish.yml` fires, pushes to NuGet.
4. Verify package on nuget.org within 1 hour. Confirm Source Link works by stepping into the new version from a consumer project.

## Operational Posture (ongoing)

| Concern | Policy |
|---|---|
| Community issue filed | Respond. If code change needed, author it in Internal, sync to public on next release. |
| Community PR filed | Close with explanation; invite as issue. (Later: cherry-pick-back protocol if volume justifies.) |
| NuGet symbol break report | Re-verify Source Link with `dotnet nuget why`; if SHA mismatch, re-publish the affected version. |
| Dependency bump | Land in Internal; sync on next release. Dependabot on public is read-only informational. |
| Force-push rollback | Only by repo admin. Keep the 30-day local `preCutover` clone until confident. |

## Risks (from council)

1. **Source Link break for pre-cutover versions** — public repo history retains old commits post-force-push, so existing `.nupkg` SHAs still resolve. Verify once after cutover.
2. **Dangling links in blog posts / Stack Overflow / SBOMs** — no mitigation; accept. The old SHAs still exist on GitHub post-force-push; only the branch ref changes.
3. **Allowlist drift** — every new asset risks silent publish breakage. Mitigation: `sync-public.ps1` runs `dotnet pack --dry-run` against the output tree as a smoke test; fails the sync if pack fails.
4. **PAT rotation** — document rotation schedule (annually); store expiry in a calendar reminder.
5. **Sync bot identity** — recommend a dedicated GitHub machine user (`stevomccormack-bot`) with minimal scope PAT, not a maintainer PAT.

## Definition of Done

- [ ] Public `main` reflects the scratch tree output of `sync-public.ps1`.
- [ ] CI green on public `main`.
- [ ] `dotnet pack` on public `main` produces valid `.nupkg` with embedded logo and correct Source Link.
- [ ] `README.md` discloses source-available status.
- [ ] Branch protection active on public `main`.
- [ ] First post-cutover NuGet release published and verified.
- [ ] 30-day local `preCutover` clone retained.

## References

- Parent plan: [`pineguard-internal.md`](pineguard-internal.md)
- Council transcript: [`pineguard-internal/council-transcript.md`](pineguard-internal/council-transcript.md)
- Safety spec: [`../specs/safety.md`](../specs/safety.md)
