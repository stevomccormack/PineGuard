# Council Transcript: pineguard-internal

**Date**: 2026-04-24
**Plan**: [`../pineguard-internal.md`](../pineguard-internal.md)
**Trigger**: `docs/ai/specs/council.md` §1(d) release/IP implications

## Framed Question

The user maintains `PineGuard`, a .NET validation library (5 NuGet packages: Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations). The single public GitHub repo `stevomccormack/PineGuard` contains library source AND a large AI "Brain" (100+ markdown files of agents/skills/specs/workflows/roles), tooling (PowerShell, audit CLI, coverage runners, Qodana), and adapters for Claude/Copilot/Gemini/Pi.

**Proposal**: split into two repos — `PineGuard.Internal` (PRIVATE, full tree, source of truth) and `PineGuard` (PUBLIC, minimal src/tests/solution + NuGet publishing). Mechanism: one-way automated sync via PowerShell script + GitHub Action + PAT force-pushing allowlisted paths. Alternatives (private submodule, monorepo with .gitignore) rejected.

**Known constraint**: existing public repo's git history *already contains* the IP through commit `25cba5b`; force-push doesn't delete forks, reflog, or archive.org copies. NuGet packages already published with `github.com/stevomccormack/PineGuard` as repository URL.

**Constraints**: single maintainer, low release cadence, no community PRs initially, GH_TOKEN available.

**Decision to pressure-test**: Is the two-repo + automated one-way sync the right architecture, and what is the right way to execute the cutover given the existing history?

## Anonymization Map

| Advisor | Response Letter |
|---|---|
| The Executor | A |
| The First Principles Thinker | B |
| The Expansionist | C |
| The Contrarian | D |
| The Outsider | E |

## Advisor Responses

### The Contrarian

The premise is broken before you write a single line of sync script. Your "private IP" is already public. It's in the git history of `stevomccormack/PineGuard`, it's in GitHub's reflog for 90+ days after any force-push, it's in every fork, every archive.org snapshot, every `git clone` anyone has ever done, and almost certainly in Google's and Bing's and Common Crawl's indexes of the raw GitHub blob URLs. Force-pushing a "clean" tree is security theater. If the Brain is actually sensitive, you need to treat it as compromised and rotate anything secret-adjacent (tokens, prompts tied to private infra). If it isn't sensitive enough to warrant that, then why are you paying the complexity tax of two repos?

Second: you are a single maintainer shipping a few releases per quarter. You are proposing a force-pushing PAT-driven sync pipeline, an allowlist, and a cutover plan — for a workload that is fundamentally "copy five `src/` folders to another repo four times a year." The allowlist has already failed once in design: the logo required for NuGet pack isn't on it. That's the canary. Every new asset (icon, README badge, analyzer ruleset, snippet) will silently break publish until you remember the allowlist exists. Force-push also nukes public issue/PR refs and star-linked commit SHAs.

Third: what problem does this actually solve? "Consumers shouldn't see my agents folder" is an aesthetic problem, not an architectural one. A `.gitignore`-on-publish or a `dotnet pack`-only workflow from a single repo solves 95% of it with zero sync infrastructure.

Do the cheap thing: single repo, `docs/ai/` and `tools/` stay where they are, publish from tags. If the Brain is truly secret, it already leaked — act accordingly, don't paper over it.

### The First Principles Thinker

You are solving the wrong problem.

The stated motivation is "protect IP." But your IP is already public. It sits in `stevomccormack/PineGuard` history through commit `25cba5b`. A force-push doesn't delete it — it lingers in forks, clones, the GitHub Archive Program, and anyone's local checkout. The split, executed today, protects *future* commits only. If that's acceptable, say so explicitly. If it isn't, no amount of sync scripting fixes it.

Now strip further. What is the AI Brain actually? It's prompts, agent playbooks, and tooling recipes. Is that IP, or is it scaffolding? Prompts are not a moat. Competitors cannot ship your library faster because they read your `coverage-guard.md`. The genuine IP is the validation library itself — and that one you are *deliberately publishing*. You are hiding the packaging material and shipping the product.

The second motivation — "keep the public repo clean for consumers" — is the real problem, and it's solvable without two repos. Consumers interact with NuGet, not your repo tree. They never `git clone` you. A tidy `src/` and a README is sufficient. `docs/ai/` and `tools/` in a public monorepo bother no one except your own aesthetic sense.

Reframe the question:
1. Do you need privacy, or tidiness? These have different solutions.
2. If privacy: accept that history is already leaked, and the split is forward-only hygiene — fine, but name it honestly.
3. If tidiness: a single public monorepo with a clear top-level layout costs zero engineering and zero sync infrastructure.

The one-way force-push sync is a machine built to answer a question you haven't actually asked yet. Answer the question first. The architecture follows trivially.

### The Expansionist

Split the repos. But stop thinking of this as "protecting the Brain." Think of it as **productizing** it.

The Brain is the asset. A solo maintainer producing a 13,000-test, 100%-covered, multi-layer validation library via an AI-first engineering system is not a validation library story — it is a **reproducible software factory** story. PineGuard-the-library is the proof artifact. The Brain is the product.

Upside nobody is naming:

1. **The Brain becomes its own NuGet/template pack.** `PineGuard.Engineering.Brain` as a `dotnet new` template, or a paid private GitHub template repo.
2. **The sync pipeline is itself IP.** Extract it as `PineGuard.Sync` or `BrainSync`. Second product, near-zero marginal cost.
3. **Cutover is a content moment.** Blog post, conference talk, Twitter thread, case study.
4. **Private Brain enables a paid tier later.** GitHub Sponsors "Brain Access" tier.

Execute the cutover with a public changelog entry on day one explaining the split and teasing the Brain. Scarcity plus visibility is leverage. The two-repo model is correct. Just do not under-ship it.

### The Outsider

Walking up to this cold, here's what I see as a stranger.

**"Protect IP" — what IP?** Prompt templates, markdown playbooks, and PowerShell audit scripts for orchestrating Claude/Gemini/etc. against your own codebase. That's scaffolding, not IP. Nobody is going to clone `docs/ai/agents/coverage-fluent.md` and build a competing validation library. The actual IP is the validation library, and that's the part you're keeping public.

**"Keep the public repo small and clean" — for whom?** I'm a .NET dev who just found PineGuard on NuGet. I want `src/`, `tests/`, a README that tells me which of your five packages to install, and examples. I do not care if there's a `docs/ai/` folder next to `src/`. "Clean" is a problem you have, not a problem your users have.

**What a stranger actually notices that you didn't mention:** Five package names for one validation library is confusing. That's the real discoverability problem on your repo, and splitting repos doesn't touch it.

**The cutover gotcha you're burying.** "Sync private → public on every release" means contributors can't meaningfully contribute — their PRs land in a repo that gets overwritten. You are quietly converting an open-source project into a source-available mirror.

**My read:** solving the wrong problem. A `.github/` tidy-up and a top-level `/ai-tooling/` folder gets you 90% of "clean" with 0% of the complexity.

### The Executor

Monday morning, you do exactly one thing: create the empty private repo `stevomccormack/PineGuard.Internal` on GitHub and `git remote set-url origin` in your local `PineGuard.Internal` clone. That's it. Ten minutes. Everything else flows from there.

The architecture is fine — stop pressure-testing it. Single maintainer, PAT available, no community PRs. Submodules are a tax you'd pay forever for a problem you don't have. Ship the two-repo plan.

But your sequence is wrong. You've got the risky step (force-forward public main) at #9 behind nine dependencies. Re-order for reversibility:

**Smallest shippable cut (this week):**
1. Create private repo, push Internal as-is. *(10 min. Backup acquired.)*
2. Move logo to `assets/`, patch `Directory.Build.props` and csproj. Verify `dotnet pack` locally still produces a valid `.nupkg` with the icon. *(Only change that can silently break NuGet.)*
3. Write `sync-public.ps1` as a local script first — no Action, no PAT, no CI. Run it by hand. Output to a scratch directory. Diff against the current public repo.
4. Push the script's output to a `sync-bootstrap` branch on public. Run `restore/build/test/pack` there.
5. Only then wrap it in an Action.
6. Branch protection last.

Skip the Action entirely until step 3 works by hand twice. Automating a broken sync just force-pushes garbage faster.

First step Monday: create the repo. Don't write a line of script until that exists.

## Peer Reviews

### Review 1
- **Strongest**: A (Executor) — treats this as execution, not relitigation.
- **Biggest blind spot**: C (Expansionist) — productization is a second full-time job, contradicts privacy goal.
- **All missed**: Two-way drift — consumers file issues/PRs against files that only exist in Internal. NuGet SourceLink breaks on force-push (embedded SHAs orphaned).

### Review 2
- **Strongest**: B (First Principles) — separates privacy vs tidiness cleanly. "Hiding packaging material and shipping the product" is the killer line.
- **Biggest blind spot**: C (Expansionist) — market value assumed without evidence.
- **All missed**: Sync script is a permanent second codebase (allowlist drift, PAT rotation, Action debugging) for the next 5 years.

### Review 3
- **Strongest**: B (First Principles) — "answer the question first, architecture follows."
- **Biggest blind spot**: C (Expansionist) — conflates "novel to maintainer" with "valuable to market."
- **All missed**: Reproducibility/attribution — Renovate, Dependabot, SBOM pins break on history rewrite. Stars/issues/tags survive but commit links dangle (blog posts, Stack Overflow, Source Link for published packages).

### Review 4
- **Strongest**: A (Executor) — accepts the decision and answers the "how."
- **Biggest blind spot**: C (Expansionist) — ignores stated constraints (no community PRs, low cadence).
- **All missed**: `dotnet pack` embeds repo URL and commit SHA; post-cutover `.nupkg` symbols point at commits that exist only in private repo. Source Link breaks for consumers.

### Review 5
- **Strongest**: B (First Principles) — refuses the premise cleanly.
- **Biggest blind spot**: C (Expansionist) — scaffolding-as-product monetization fanfic.
- **All missed**: Reverse direction — no protocol for accepting community PRs on public repo once sync is one-way. `.mailmap`, signed commits, NuGet `repositoryUrl`/SourceLink breakage for already-published versions.

## Chairman Verdict

### Where the Council Agrees
The IP the user thinks they are protecting is already public, and the Brain is scaffolding, not a moat. "Clean public repo" is an aesthetic goal consumers do not share.

### Where the Council Clashes
"Should you?" vs "How do you?" First Principles + Outsider (+ partially Contrarian) say solving the wrong problem. Executor says decision made, sequence for reversibility. Expansionist is the unanimous outlier — productize-the-Brain is scaffolding-as-product without evidence.

### Blind Spots the Council Caught
1. NuGet Source Link breaks for already-published packages (embedded SHAs orphaned).
2. Dangling commit references across Renovate/Dependabot/SBOM/blog posts.
3. One-way sync converts OSS → source-available; social contract change.
4. Allowlist drift is permanent overhead; logo failure is the canary.
5. PAT rotation, signed commits, `.mailmap` — ongoing hygiene unbudgeted.

### The Recommendation
Do the split. Do it smaller than planned. Name it honestly.
1. Stop calling it "protect IP." Call it forward-only hygiene and packaging discipline.
2. Accept public becomes source-available, not OSS. State in README day one.
3. Fix Source Link before cutover (deterministic builds targeting future public SHA) or accept old versions' symbols break.
4. Follow Executor's re-ordering. Private repo → logo/pack → hand-run sync → scratch branch → Action → branch protection.
5. Reject productization track. No blog post, no paid tier, no Brain-as-product.
6. Do not force-push public `main` until sync script produces byte-identical output twice against scratch.

### The One Thing to Do First
**Create the empty private repo `stevomccormack/PineGuard.Internal` on GitHub and push your current local tree. Ten minutes. Nothing else.**
