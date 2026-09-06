import { existsSync, readFileSync, readdirSync } from "node:fs";
import { join, relative } from "node:path";

import type { Node } from "web-tree-sitter";

import { registerRule } from "../catalog.js";
import {
    findInvocations,
    findMethodDeclarations,
    parseFile as parseCsharpFile,
} from "../parsing/csharp.js";
import type { Finding, RuleContext, VocabularyConfig } from "../types.js";

/**
 * `layer-parity` — from-source rebuild of the legacy tool's `Rule06`
 * (plan `docs/ai/plans/audit-cli-rebuild.md` §4.3, §8, §2.2, P2.3).
 *
 * ## What the old rule did, and why it crashed
 *
 * `tools/audit-cli/helpers/Test-ParityAgainstMust.ps1` ran `dotnet publish`
 * (without `-f`) against each of the four adapter projects, reflected over
 * the published DLLs to collect extension-method names, and diffed the
 * normalised concept sets against `docs/ai/specs/language/vocabulary.json`.
 * `dotnet publish` on a multi-targeted project without an explicit `-f`
 * fails with NETSDK1129, so the rule has crashed on every run since
 * multi-targeting landed 2026-04-17 (plan §2.2). This rule is a from-source
 * (tree-sitter, syntax-only) rebuild: no `dotnet publish`, no build, no
 * reflection — every concept is read straight out of the four layers'
 * source trees.
 *
 * The legacy rule's *enforcement policy* also had a warning-shaped escape
 * hatch this rewrite deliberately removes: Guard/FluentValidation were
 * diffed exactly both ways against Must, but DataAnnotations' missing
 * coverage was explicitly "informational only" (only its *extra*, unknown
 * concepts were enforced — see that file's `Diff-FromMust`/
 * `$dataEnforcedDiscrepancies` comment). Per plan §4.3/§8 ("missing-layer is
 * a finding, not a warning"), this rewrite is symmetric and strict: a
 * concept implemented by any layer but missing from another in-scope layer
 * is *always* a finding, DataAnnotations included. `vocabulary.json`'s
 * `concepts`/`opposites` arrays are left unused, per D9 (plan §5) — this
 * rule reads only `stripPrefixes`, `ignoreMethods`, and `aliases`.
 *
 * ## The four layers, and how each one's concept set is read
 *
 * - **Must** (`src/PineGuard.MustClauses`) and **Guard**
 *   (`src/PineGuard.GuardClauses`): every `public static` extension method
 *   whose `this`-receiver parameter's declared type contains `IMustClause`
 *   / `IGuardClause` — the method's own name is the raw concept (e.g.
 *   `Must.Be.NullOrEmpty` / `Guard.Against.NullOrEmpty` both read as the raw
 *   concept `"NullOrEmpty"`).
 * - **Fluent** (`src/PineGuard.FluentValidation`): the same shape, but the
 *   receiver's declared type contains `IRuleBuilder` (generic, e.g.
 *   `IRuleBuilder<TModel, bool?>` — a substring match, not an exact one).
 * - **DataAnnotations** (`src/PineGuard.DataAnnotations`): attribute class
 *   names intentionally diverge from Must clause names (e.g.
 *   `NullOrEmptyAttribute` vs. a differently-named clause is not assumed),
 *   so — exactly as the legacy rule did — the raw concept is read from the
 *   `Must.Be.<Name>(...)` call *inside* each attribute's validation method,
 *   not the attribute's own type name. This is the one layer that uses a
 *   different extraction shape from the other three; see
 *   {@link LayerSpec.kind}.
 *
 * Every raw name is then normalised via {@link normalizeConcept} (identical
 * algorithm to the legacy `Normalize-ConceptName`): drop anything in
 * `ignoreMethods`, apply `aliases` (checked both before and after prefix
 * stripping, since an alias target can itself need no further stripping),
 * strip the first matching `stripPrefixes` entry.
 *
 * ## Comparison: which layers are "in scope", and what counts as a gap
 *
 * A layer only participates in a given run when its directory
 * (`src/PineGuard.<Layer>` under `ctx.rootDir`) actually exists — a fixture
 * tree is free to stand up just two or three of the four layers and this
 * rule will compare only those, rather than manufacturing "whole layer
 * absent" findings against a directory a fixture never intended to include.
 * (On a real `pineguard audit` run all four directories always exist, so
 * this only matters for tests.) Within the in-scope layers, every
 * normalised concept implemented by at least one of them is expected in
 * *all* of them; a layer that is present but does not implement a concept a
 * sibling does is exactly the "missing whole layer [of a concept's
 * implementation] is a finding, not a warning" case plan §4.3 calls for —
 * whether that gap is a single missing overload or an entire unimplemented
 * concept, the mechanism is the same per-concept comparison.
 *
 * ## Vocabulary: read from `ctx.vocabulary`, with a fixture-friendly fallback
 *
 * The real engine (`src/audit/engine.ts`'s `buildContext`) parses the real,
 * ~60-line `docs/ai/specs/language/vocabulary.json` once and exposes it as
 * `ctx.vocabulary` — this rule reads it from there and never re-reads the
 * file itself. The test harness (`test/support/runRule.ts`) never populates
 * `ctx.vocabulary` (its `{ rootDir: fixtureDir }` context is deliberately
 * minimal), so a fixture that needs non-default normalisation (an alias, a
 * strip prefix) can drop a tiny `vocabulary.json` — only the keys it
 * actually needs — directly at its own fixture root
 * (`test/fixtures/layer-parity/<kind>/vocabulary.json`); see
 * {@link resolveVocabulary}. A fixture with no such file (the common case)
 * gets {@link DEFAULT_VOCABULARY}: no prefixes stripped, no aliases, no
 * ignored methods — i.e. concept names must match verbatim across layers,
 * which is exactly what the `valid`/`invalid` fixtures rely on. This keeps
 * every fixture testable in isolation, without needing the real 1000-line
 * vocabulary or a real repo checkout.
 */

// ---------------------------------------------------------------------------
// Layer specs
// ---------------------------------------------------------------------------

interface LayerSpec {
    /** Human-readable layer name used in finding messages (e.g. `"Guard"`). */
    readonly name: string;
    /** Repo-relative (or fixture-relative) source directory for this layer. */
    readonly relativeDir: string;
    /**
     * How this layer's raw concept names are extracted from its parsed
     * source files:
     *  - `"extension"`: public static extension methods whose receiver type
     *    contains `firstParamTypeMatch`; the method name is the raw concept.
     *  - `"must-call"`: the `<Name>` in every `Must.Be.<Name>(...)`
     *    invocation found anywhere in the file (DataAnnotations only — see
     *    this module's header comment for why).
     */
    readonly kind: "extension" | "must-call";
    /** Required, and only meaningful, when `kind === "extension"`. */
    readonly firstParamTypeMatch?: string;
}

const LAYERS: readonly LayerSpec[] = [
    {
        name: "Must",
        relativeDir: "src/PineGuard.MustClauses",
        kind: "extension",
        firstParamTypeMatch: "IMustClause",
    },
    {
        name: "Guard",
        relativeDir: "src/PineGuard.GuardClauses",
        kind: "extension",
        firstParamTypeMatch: "IGuardClause",
    },
    {
        name: "Fluent",
        relativeDir: "src/PineGuard.FluentValidation",
        kind: "extension",
        firstParamTypeMatch: "IRuleBuilder",
    },
    {
        name: "DataAnnotations",
        relativeDir: "src/PineGuard.DataAnnotations",
        kind: "must-call",
    },
];

// ---------------------------------------------------------------------------
// Vocabulary loading (ctx.vocabulary, with a fixture-local fallback)
// ---------------------------------------------------------------------------

/**
 * Identity normalisation: no prefixes stripped, no aliases, nothing ignored.
 * Used whenever neither `ctx.vocabulary` nor a fixture-local
 * `vocabulary.json` is available, so fixtures that don't care about
 * normalisation (the common case) can use plain, matching concept names
 * across layers with no extra setup.
 */
const DEFAULT_VOCABULARY: VocabularyConfig = {
    version: 1,
    stripPrefixes: [],
    ignoreMethods: [],
    aliases: {},
    concepts: [],
    opposites: [],
};

function isPlainObject(value: unknown): value is Record<string, unknown> {
    return typeof value === "object" && value !== null && !Array.isArray(value);
}

/**
 * Resolves the vocabulary this run should normalise concept names against.
 * Prefers `ctx.vocabulary` (populated by the real engine from the real
 * `docs/ai/specs/language/vocabulary.json` — see `src/audit/engine.ts`).
 * Falls back to a tiny fixture-local `<rootDir>/vocabulary.json`, merged
 * over {@link DEFAULT_VOCABULARY} so a fixture only needs to specify the
 * keys its scenario actually exercises. Falls back again to
 * {@link DEFAULT_VOCABULARY} itself when neither is present.
 */
function resolveVocabulary(ctx: RuleContext): VocabularyConfig {
    if (ctx.vocabulary) {
        return ctx.vocabulary;
    }

    const fixtureVocabularyPath = join(ctx.rootDir, "vocabulary.json");
    if (!existsSync(fixtureVocabularyPath)) {
        return DEFAULT_VOCABULARY;
    }

    const parsed: unknown = JSON.parse(
        readFileSync(fixtureVocabularyPath, "utf8"),
    );
    if (!isPlainObject(parsed)) {
        throw new Error(
            `layer-parity: ${fixtureVocabularyPath} did not parse to a JSON object`,
        );
    }

    return { ...DEFAULT_VOCABULARY, ...parsed } as VocabularyConfig;
}

/**
 * Normalises one raw concept name (a method name, or the `<Name>` from a
 * `Must.Be.<Name>(...)` call) exactly as the legacy tool's
 * `Normalize-ConceptName` did: drop anything in `ignoreMethods`, apply an
 * `aliases` match, strip the first matching `stripPrefixes` entry, then
 * apply `aliases` again (an alias target can itself be a prefixed name that
 * needs no further stripping — or could, in principle, itself be aliased
 * again; this mirrors the legacy single second pass, not a fixed point).
 * Returns `null` when the name should not participate in parity at all
 * (ignored outright, or reduces to the empty string after stripping).
 */
function normalizeConcept(
    rawName: string,
    vocabulary: VocabularyConfig,
): string | null {
    if (vocabulary.ignoreMethods.includes(rawName)) {
        return null;
    }

    let name = rawName;
    const aliasedBeforeStrip = vocabulary.aliases[name];
    if (aliasedBeforeStrip !== undefined) {
        name = aliasedBeforeStrip;
    }

    for (const prefix of vocabulary.stripPrefixes) {
        if (name.startsWith(prefix)) {
            name = name.slice(prefix.length);
            break;
        }
    }

    const aliasedAfterStrip = vocabulary.aliases[name];
    if (aliasedAfterStrip !== undefined) {
        name = aliasedAfterStrip;
    }

    if (name.length === 0 || vocabulary.ignoreMethods.includes(name)) {
        return null;
    }
    return name;
}

// ---------------------------------------------------------------------------
// File collection — same trackedFiles-or-walk pattern as `must-collisions`
// ---------------------------------------------------------------------------

/** Recursively lists every `*.cs` file under `dir`, skipping `obj`/`bin` build output. */
function* walkCsFiles(dir: string): Generator<string> {
    for (const entry of readdirSync(dir, { withFileTypes: true })) {
        if (entry.name === "obj" || entry.name === "bin") {
            continue;
        }
        const full = join(dir, entry.name);
        if (entry.isDirectory()) {
            yield* walkCsFiles(full);
        } else if (entry.name.endsWith(".cs")) {
            yield full;
        }
    }
}

interface CandidateFile {
    readonly absolutePath: string;
    readonly relativePath: string;
}

/**
 * Resolves the C# files under one layer's directory: every tracked file
 * under `<layerDir>/` on a real `pineguard audit` run (`ctx.trackedFiles`
 * populated by the engine), or every `*.cs` file under
 * `<ctx.rootDir>/<layerDir>` when running against a fixture tree (the test
 * harness never populates `trackedFiles` — see `test/support/runRule.ts`'s
 * header comment).
 */
function collectLayerFiles(
    ctx: RuleContext,
    layerDir: string,
): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles
            .filter(
                (file) =>
                    file.startsWith(`${layerDir}/`) && file.endsWith(".cs"),
            )
            .map((file) => ({
                absolutePath: join(ctx.rootDir, file),
                relativePath: file,
            }));
    }

    const absoluteDir = join(ctx.rootDir, layerDir);
    if (!existsSync(absoluteDir)) {
        return [];
    }
    return [...walkCsFiles(absoluteDir)].map((absolutePath) => ({
        absolutePath,
        relativePath: relative(ctx.rootDir, absolutePath).replaceAll("\\", "/"),
    }));
}

// ---------------------------------------------------------------------------
// Raw concept extraction
// ---------------------------------------------------------------------------

function extractExtensionConcepts(
    root: Node,
    firstParamTypeMatch: string,
): string[] {
    const names: string[] = [];
    for (const method of findMethodDeclarations(root)) {
        if (
            !method.modifiers.includes("public") ||
            !method.modifiers.includes("static")
        ) {
            continue;
        }
        const receiver = method.parameters[0];
        if (!receiver?.modifiers.includes("this")) {
            continue;
        }
        if (!receiver.typeText?.includes(firstParamTypeMatch)) {
            continue;
        }
        if (method.name.length > 0) {
            names.push(method.name);
        }
    }
    return names;
}

const MUST_BE_PREFIX = "Must.Be.";

function extractMustCallConcepts(root: Node): string[] {
    return findInvocations(root, "Must.Be")
        .filter((invocation) =>
            invocation.memberPath.startsWith(MUST_BE_PREFIX),
        )
        .map((invocation) => invocation.memberPath.slice(MUST_BE_PREFIX.length))
        .filter((name) => name.length > 0);
}

// ---------------------------------------------------------------------------
// The rule
// ---------------------------------------------------------------------------

interface LayerConcepts {
    readonly layer: LayerSpec;
    /** Normalised concept -> raw name(s) that produced it, for finding messages. */
    readonly concepts: Set<string>;
}

registerRule({
    slug: "layer-parity",
    legacyId: "Rule06",
    scope: "library",
    gate: false,
    description:
        "Every normalised validation concept implemented by Must, Guard, Fluent, or DataAnnotations is implemented by all of them (concept-set parity via vocabulary.json; no build, no dotnet publish).",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const parse = ctx.parseFile ?? parseCsharpFile;
        const vocabulary = resolveVocabulary(ctx);

        const inScopeLayers = LAYERS.filter((layer) =>
            existsSync(join(ctx.rootDir, layer.relativeDir)),
        );

        const layerConcepts: LayerConcepts[] = [];
        for (const layer of inScopeLayers) {
            const files = collectLayerFiles(ctx, layer.relativeDir);
            const concepts = new Set<string>();

            for (const file of files) {
                const { root } = await parse(file.absolutePath);
                const rawNames =
                    layer.kind === "extension"
                        ? extractExtensionConcepts(
                              root,
                              layer.firstParamTypeMatch ?? "",
                          )
                        : extractMustCallConcepts(root);

                for (const rawName of rawNames) {
                    const normalized = normalizeConcept(rawName, vocabulary);
                    if (normalized !== null) {
                        concepts.add(normalized);
                    }
                }
            }

            layerConcepts.push({ layer, concepts });
        }

        const allConcepts = new Set<string>();
        for (const entry of layerConcepts) {
            for (const concept of entry.concepts) {
                allConcepts.add(concept);
            }
        }

        const findings: Finding[] = [];
        for (const concept of [...allConcepts].sort()) {
            const implementing = layerConcepts.filter((entry) =>
                entry.concepts.has(concept),
            );
            const missing = layerConcepts.filter(
                (entry) => !entry.concepts.has(concept),
            );
            if (missing.length === 0) {
                continue;
            }

            const implementingNames = implementing
                .map((entry) => entry.layer.name)
                .join(", ");

            for (const entry of missing) {
                findings.push({
                    rule: "layer-parity",
                    file: entry.layer.relativeDir,
                    message:
                        `Concept '${concept}' is implemented in ${implementingNames} ` +
                        `but missing from ${entry.layer.name} (${entry.layer.relativeDir}).`,
                    key: `layer-parity:${concept}:${entry.layer.name}`,
                });
            }
        }

        return findings;
    },
});
