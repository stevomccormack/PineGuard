import { existsSync, readFileSync, readdirSync } from "node:fs";
import { join } from "node:path";

import { registerRule } from "../catalog.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `surface-parity` — port of the legacy PowerShell tool's `Rule12`
 * (`tools/audit-cli/rules/Test-Rule12-AdapterParity.ps1`), plan
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3, §8, §2.4, P2.9.
 *
 * This is a **hard CI gate at cutover** (`gate: true`) — parity here is the
 * only machine check that every Brain agent is actually reachable from every
 * adapter surface, and (per plan §2.2) the legacy rule is otherwise
 * trustworthy: "PASS, 84 agents x 4 surfaces — good rule, well-scoped".
 *
 * ## What this rule parses, and from where
 *
 * Everything is derived from `docs/ai/meta/adapter-surfaces.md` — the single
 * normative inventory of adapter surfaces (see that file's own `[!IMPORTANT]`
 * banner) — never hardcoded, so this rule keeps working as surfaces are
 * added, renamed, or retired:
 *
 * - **§2 "Full adapters"** — the table of surfaces checked for command
 *   parity (`Surface | Tool | Command dir | Skills | Other`). The real doc
 *   lists four (`.claude/`, `.agent/`, `.pi/`, `.github/`) — matching the
 *   legacy rule's "84 agents x 4 surfaces" scale — but the count is read
 *   from the table, never hardcoded as "4".
 * - **§1 "Root boot files"** plus each surface's own "Other" column — used to
 *   *derive* which surfaces additionally carry a "palette" file (a file
 *   whose prose must list every non-exempt agent by path, e.g. `CLAUDE.md`'s
 *   command palette). A surface gets a derived palette when either (a) its
 *   Tool name matches a §1 boot-file row's Tool (`.claude` / "Claude Code" ->
 *   `CLAUDE.md`), or (b) its "Other" column names an `AGENTS.md`-shaped file
 *   (`.pi`'s Other column lists `AGENTS.md` -> `.pi/AGENTS.md`). This
 *   reproduces the legacy rule's hardcoded two-entry `$palettes` array
 *   without hardcoding it here.
 * - **§4 "Parity policy"** — the declared-exceptions table. Every agent must
 *   appear on every full adapter surface *except* what this table declares.
 *
 * ## The two fixes over the legacy rule (plan §2.4, §8)
 *
 * The legacy rule is "good... well-scoped" but has two soft spots the plan
 * calls out by name:
 *
 * 1. **"fails silently if the exceptions section is malformed"** — the
 *    legacy PowerShell simply gets empty `$exemptSurfaces`/`$agentExemptions`
 *    hashtables if §4 doesn't parse (missing heading, empty table, a row
 *    missing its Surfaces column, ...) and carries on as if zero exceptions
 *    existed — never surfacing that the declared policy itself is broken.
 *    This rule instead returns a real {@link Finding} the moment §4 is
 *    missing, empty, or contains a row that cannot be resolved to a known
 *    surface/tool or classified into one of the two recognised shapes below
 *    (`parseParityPolicy`'s `exceptions-section-missing` /
 *    `exceptions-table-empty` / `exceptions-row-malformed` /
 *    `exceptions-row-unresolved-surface` / `exceptions-row-unclassified` /
 *    `exceptions-row-family-list-missing` finding keys).
 * 2. **"implements the Copilot-subset policy as a blanket skip of `.github/`"**
 *    — the legacy rule's exception-row classifier only recognises rows whose
 *    Exception cell backtick-quotes literal, resolvable agent names; the
 *    real Copilot-subset row backtick-quotes `.github/prompts/` instead (a
 *    path, not an agent), so it falls through to "zero agents named" and the
 *    legacy code's fallback is to blanket-exempt every surface the row
 *    names — skipping `.github/` entirely, agent by agent, forever, no
 *    matter what actually lands in `.github/prompts/`. This rule instead
 *    recognises that row's actual shape — "carries one representative per
 *    command family (coverage, test, fix-coverage, ...) rather than every
 *    agent" — and implements the real policy: for each declared **family**
 *    (a keyword such as `coverage` or `scan`; an agent "belongs to" a family
 *    when the family's own dash-segments appear, contiguously, anywhere in
 *    the agent's slug — e.g. `coverage` matches `coverage-core` at segment 0
 *    and `council` matches `ask-council` at segment 1, while the more
 *    specific two-segment `fix-coverage` wins over the one-segment
 *    `coverage` for `fix-coverage-all`, per {@link assignFamily}'s
 *    longest-match rule), the surface must carry **exactly one**
 *    representative — zero is a missing-representative finding, two or more
 *    is an ambiguous-representative finding, and a present, resolvable agent
 *    that belongs to no declared family at all is its own "unexpected"
 *    finding (`checkFamilyPolicy`'s `copilot-family-missing` /
 *    `copilot-family-ambiguous` / `copilot-family-unexpected` finding keys).
 *    This mechanism is not special-cased to `.github` by name — any surface
 *    whose declared exception matches the same "one representative per ...
 *    family (...)" shape gets the same real check, in case a future surface
 *    adopts the same subset policy.
 *
 * Every other exception row (the real doc's "Release family" row, which
 * backtick-quotes real, resolvable agent names) is handled exactly as the
 * legacy rule does: the named agents are required *only* on the named
 * surfaces and exempt everywhere else (`agentExemptions`,
 * {@link isAgentExempt}).
 */

const ADAPTER_SURFACES_DOC = "docs/ai/meta/adapter-surfaces.md";
const AGENTS_DIR = "docs/ai/agents";
const WORKFLOWS_DIR = "docs/ai/workflows";

const NON_AGENT_BASENAMES = /^(README|INDEX|AGENTS)$/i;

function isHeadingLine(line: string): boolean {
    return /^#{2,}\s/.test(line);
}

const isFullAdaptersHeading = (line: string): boolean => /^##\s*2\./.test(line);
const isRootBootFilesHeading = (line: string): boolean =>
    /^##\s*1\./.test(line);
/**
 * Matched primarily by section number (`## 4.`, mirroring the legacy rule
 * exactly), with a text fallback (`parity policy` anywhere in the heading)
 * so a future renumbering of `adapter-surfaces.md` doesn't silently make
 * this rule blind to the section — it would instead find it by title.
 */
const isParityPolicyHeading = (line: string): boolean =>
    /^##\s*4\./.test(line) || /parity policy/i.test(line);

/** One row of a markdown pipe-table, already split and trimmed into cells. */
type TableRow = string[];

/**
 * Walks `markdown` line by line, toggling "inside the target section"
 * whenever a `##`+ heading is seen (per `isTargetHeading`), and collects
 * every pipe-table data row (header/separator rows excluded) seen while
 * inside that section. Mirrors the legacy rule's `Get-TableRow` exactly,
 * including its "any heading toggles the section, matching headings turn it
 * back on" behaviour, so a subsection (e.g. `### 2.1`) correctly ends the
 * parent section's table.
 */
function extractTableRows(
    markdown: string,
    isTargetHeading: (headingLine: string) => boolean,
): TableRow[] {
    const rows: TableRow[] = [];
    let inSection = false;

    for (const rawLine of markdown.split(/\r?\n/)) {
        if (isHeadingLine(rawLine)) {
            inSection = isTargetHeading(rawLine);
            continue;
        }

        if (!inSection) {
            continue;
        }

        const line = rawLine.trim();
        if (!line.startsWith("|")) {
            continue;
        }
        if (/^\|[\s:|-]+\|$/.test(line)) {
            continue;
        }

        const inner = line.replace(/^\|/, "").replace(/\|$/, "");
        const cells = inner.split("|").map((cell) => cell.trim());
        if (cells.length < 2) {
            continue;
        }

        rows.push(cells);
    }

    return rows;
}

/** `true` if a heading matching `isTargetHeading` exists anywhere in `markdown`. */
function sectionHeadingExists(
    markdown: string,
    isTargetHeading: (headingLine: string) => boolean,
): boolean {
    return markdown
        .split(/\r?\n/)
        .some((line) => isHeadingLine(line) && isTargetHeading(line));
}

/** Every backtick-quoted token in `text`, trimmed, in source order. */
function backtickTokens(text: string): string[] {
    const tokens: string[] = [];
    const pattern = /`([^`]+)`/g;
    let match: RegExpExecArray | null;
    while ((match = pattern.exec(text)) !== null) {
        const token = match[1];
        if (token !== undefined) {
            tokens.push(token.trim());
        }
    }
    return tokens;
}

function stripTrailingSlash(value: string): string {
    return value.replace(/\/+$/, "");
}

function escapeRegExp(value: string): string {
    return value.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

/** One row of §2 "Full adapters": a surface checked for command parity. */
interface SurfaceInfo {
    readonly surface: string;
    readonly tool: string;
    readonly commandDir: string;
    readonly otherTokens: readonly string[];
}

/** Parses §2 "Full adapters" — the surfaces this rule checks for command parity. */
function parseFullAdapters(markdown: string): SurfaceInfo[] {
    const surfaces: SurfaceInfo[] = [];

    for (const cells of extractTableRows(markdown, isFullAdaptersHeading)) {
        if (/^surface$/i.test(cells[0] ?? "")) {
            continue;
        }

        const surfaceToken = backtickTokens(cells[0] ?? "")[0];
        if (!surfaceToken) {
            continue;
        }

        const commandDirToken =
            cells.length >= 3 ? backtickTokens(cells[2] ?? "")[0] : undefined;
        if (!commandDirToken) {
            continue;
        }

        const surface = stripTrailingSlash(surfaceToken);
        const tool = cells.length >= 2 ? (cells[1] ?? "") : "";
        const otherTokens =
            cells.length >= 5 ? backtickTokens(cells[4] ?? "") : [];

        surfaces.push({
            surface,
            tool,
            commandDir: `${surface}/${stripTrailingSlash(commandDirToken)}`,
            otherTokens,
        });
    }

    return surfaces;
}

/** One row of §1 "Root boot files". */
interface BootFile {
    readonly file: string;
    readonly tool: string;
}

function parseRootBootFiles(markdown: string): BootFile[] {
    const files: BootFile[] = [];

    for (const cells of extractTableRows(markdown, isRootBootFilesHeading)) {
        if (/^file$/i.test(cells[0] ?? "")) {
            continue;
        }

        const file = backtickTokens(cells[0] ?? "")[0];
        if (!file) {
            continue;
        }

        files.push({ file, tool: cells.length >= 2 ? (cells[1] ?? "") : "" });
    }

    return files;
}

/** A surface's derived "palette" file: prose that must list every non-exempt agent by path. */
interface PaletteTarget {
    readonly surface: string;
    readonly path: string;
}

/**
 * Derives each surface's palette file from the doc itself (see this file's
 * header comment) instead of hardcoding the legacy rule's two-entry array.
 */
function derivePalettes(
    surfaces: readonly SurfaceInfo[],
    bootFiles: readonly BootFile[],
): PaletteTarget[] {
    const palettes: PaletteTarget[] = [];

    for (const surface of surfaces) {
        const bootMatch = bootFiles.find(
            (bootFile) =>
                bootFile.tool.trim().toLowerCase() ===
                surface.tool.trim().toLowerCase(),
        );
        if (bootMatch) {
            palettes.push({ surface: surface.surface, path: bootMatch.file });
            continue;
        }

        const agentsToken = surface.otherTokens.find((token) =>
            /agents\.md$/i.test(stripTrailingSlash(token)),
        );
        if (agentsToken) {
            palettes.push({
                surface: surface.surface,
                path: `${surface.surface}/${stripTrailingSlash(agentsToken)}`,
            });
        }
    }

    return palettes;
}

function findingFor(file: string, message: string, key: string): Finding {
    return { rule: "surface-parity", file, message, key };
}

/** Every surface a §4 exception row's Surfaces cell names, by backtick token or tool-name mention. */
function resolveNamedSurfaces(
    surfacesText: string,
    surfaces: readonly SurfaceInfo[],
): Set<string> {
    const tokens = backtickTokens(surfacesText).map((token) =>
        stripTrailingSlash(token).toLowerCase(),
    );
    const named = new Set<string>();

    for (const surface of surfaces) {
        if (tokens.includes(surface.surface.toLowerCase())) {
            named.add(surface.surface);
            continue;
        }
        if (
            surface.tool.length > 0 &&
            new RegExp(`\\b${escapeRegExp(surface.tool)}\\b`, "i").test(
                surfacesText,
            )
        ) {
            named.add(surface.surface);
        }
    }

    return named;
}

interface ParityPolicyResult {
    /** Agent -> the surfaces it IS required on (exempt everywhere else). */
    readonly agentExemptions: ReadonlyMap<string, ReadonlySet<string>>;
    /** Surface -> its declared "one representative per family" family list. */
    readonly familyPolicyBySurface: ReadonlyMap<string, readonly string[]>;
    readonly findings: readonly Finding[];
}

const ONE_REP_PER_FAMILY = /one representative per\b.*\bfamil(?:y|ies)\b/i;

/**
 * Parses §4 "Parity policy" into agent-specific exemptions and any
 * recognised "one representative per family" subset policies, failing loudly
 * (a real {@link Finding}, never a silent empty result) on a missing,
 * empty, or unparsable/unclassifiable exceptions section — the direct fix
 * for the legacy rule's silent-pass gap (plan §2.4, §8; see this file's
 * header comment).
 */
function parseParityPolicy(
    markdown: string,
    surfaces: readonly SurfaceInfo[],
    agentNames: ReadonlySet<string>,
): ParityPolicyResult {
    const findings: Finding[] = [];

    if (!sectionHeadingExists(markdown, isParityPolicyHeading)) {
        findings.push(
            findingFor(
                ADAPTER_SURFACES_DOC,
                "Missing parity-policy / exceptions section (expected a '## 4. Parity policy' heading with a declared-exceptions table). Without it this rule cannot tell which agents or surfaces are deliberately exempt, so it fails loudly instead of silently treating every exception as absent.",
                "surface-parity:exceptions-section-missing",
            ),
        );
        return {
            agentExemptions: new Map(),
            familyPolicyBySurface: new Map(),
            findings,
        };
    }

    const rows = extractTableRows(markdown, isParityPolicyHeading).filter(
        (cells) => !/^exception$/i.test(cells[0] ?? ""),
    );

    if (rows.length === 0) {
        findings.push(
            findingFor(
                ADAPTER_SURFACES_DOC,
                "Parity-policy section is present but its exceptions table has no data rows (or the table is malformed and unparsable) — expected at least the declared Release-family and Copilot-subset exceptions this rule enforces.",
                "surface-parity:exceptions-table-empty",
            ),
        );
        return {
            agentExemptions: new Map(),
            familyPolicyBySurface: new Map(),
            findings,
        };
    }

    const agentExemptions = new Map<string, Set<string>>();
    const familyPolicyBySurface = new Map<string, string[]>();

    rows.forEach((cells, index) => {
        const rowNumber = index + 1;

        if (cells.length < 2) {
            findings.push(
                findingFor(
                    ADAPTER_SURFACES_DOC,
                    `Parity-policy exception row ${String(rowNumber)} is malformed: expected at least 2 columns (Exception | Surfaces), found ${String(cells.length)}.`,
                    `surface-parity:exceptions-row-malformed:${String(rowNumber)}`,
                ),
            );
            return;
        }

        const exceptionText = cells[0] ?? "";
        const surfacesText = cells[1] ?? "";
        const namedSurfaces = resolveNamedSurfaces(surfacesText, surfaces);

        if (namedSurfaces.size === 0) {
            findings.push(
                findingFor(
                    ADAPTER_SURFACES_DOC,
                    `Parity-policy exception row ${String(rowNumber)} ("${exceptionText}") names an unrecognised surface/tool in its Surfaces column ("${surfacesText}") — cannot apply this exception.`,
                    `surface-parity:exceptions-row-unresolved-surface:${String(rowNumber)}`,
                ),
            );
            return;
        }

        const exemptAgentTokens = backtickTokens(exceptionText).filter(
            (token) => agentNames.has(token),
        );

        if (exemptAgentTokens.length > 0) {
            for (const agent of exemptAgentTokens) {
                const requiredOn =
                    agentExemptions.get(agent) ?? new Set<string>();
                for (const surface of namedSurfaces) {
                    requiredOn.add(surface);
                }
                agentExemptions.set(agent, requiredOn);
            }
            return;
        }

        if (ONE_REP_PER_FAMILY.test(exceptionText)) {
            const familyListMatch = /\(([^)]*)\)/.exec(exceptionText);
            const families = (familyListMatch?.[1] ?? "")
                .split(",")
                .map((family) => family.trim())
                .filter((family) => family.length > 0);

            if (families.length === 0) {
                findings.push(
                    findingFor(
                        ADAPTER_SURFACES_DOC,
                        `Parity-policy exception row ${String(rowNumber)} ("${exceptionText}") describes a "one representative per family" subset policy but does not declare a parenthetical family list (expected e.g. "(coverage, test, ...)") — cannot verify the subset without knowing the families, so failing loudly instead of silently skipping the surface.`,
                        `surface-parity:exceptions-row-family-list-missing:${String(rowNumber)}`,
                    ),
                );
                return;
            }

            for (const surface of namedSurfaces) {
                familyPolicyBySurface.set(surface, families);
            }
            return;
        }

        findings.push(
            findingFor(
                ADAPTER_SURFACES_DOC,
                `Parity-policy exception row ${String(rowNumber)} ("${exceptionText}") does not resolve to specific known agent names and is not a recognised "one representative per family" subset policy — it cannot be classified, so it is reported instead of silently exempting or skipping "${surfacesText}".`,
                `surface-parity:exceptions-row-unclassified:${String(rowNumber)}`,
            ),
        );
    });

    return { agentExemptions, familyPolicyBySurface, findings };
}

/**
 * `true` when `family`'s own dash-segments appear, contiguously, anywhere in
 * `slug`'s dash-segments (`assignFamily`'s matching primitive).
 */
function familySegmentLength(
    slugSegments: readonly string[],
    family: string,
): number {
    const familySegments = family
        .split("-")
        .filter((segment) => segment.length > 0);
    if (familySegments.length === 0) {
        return 0;
    }

    for (
        let start = 0;
        start <= slugSegments.length - familySegments.length;
        start += 1
    ) {
        const isMatch = familySegments.every(
            (segment, offset) =>
                (slugSegments[start + offset] ?? "").toLowerCase() ===
                segment.toLowerCase(),
        );
        if (isMatch) {
            return familySegments.length;
        }
    }

    return 0;
}

/**
 * Assigns `slug` (an agent name, e.g. `fix-coverage-all` or `ask-council`) to
 * the most specific matching family in `families`. A family need not be a
 * prefix — `council` matches `ask-council` at segment 1 — and when more than
 * one family matches (`coverage` and `fix-coverage` both match
 * `fix-coverage-all`), the longer (more specific) family wins. Returns
 * `undefined` when no declared family matches at all.
 */
function assignFamily(
    slug: string,
    families: readonly string[],
): string | undefined {
    const segments = slug.split("-");
    let best: { family: string; length: number } | undefined;

    for (const family of families) {
        const length = familySegmentLength(segments, family);
        if (length > 0 && (!best || length > best.length)) {
            best = { family, length };
        }
    }

    return best?.family;
}

/** A markdown file present in a surface's command directory, split into stem + real filename. */
interface PresentFile {
    readonly stem: string;
    readonly filename: string;
}

/** Every `*.md` file directly under `absDir` (non-recursive, mirrors the legacy `Get-ChildItem -File -Filter '*.md'`). */
function listMarkdownFiles(absDir: string): PresentFile[] {
    try {
        return readdirSync(absDir, { withFileTypes: true })
            .filter(
                (entry) =>
                    entry.isFile() && entry.name.toLowerCase().endsWith(".md"),
            )
            .map((entry) => {
                const dot = entry.name.indexOf(".");
                const stem = dot >= 0 ? entry.name.slice(0, dot) : entry.name;
                return { stem, filename: entry.name };
            });
    } catch {
        return [];
    }
}

/**
 * The real "one representative per family" Copilot-subset check (plan §2.4,
 * §8) — replaces the legacy rule's blanket skip of the whole surface. Flags
 * a family with zero present representatives, a family with two or more
 * (ambiguous — which one is canonical?), and a present, resolvable agent
 * that belongs to no declared family at all.
 */
function checkFamilyPolicy(
    surface: SurfaceInfo,
    present: readonly PresentFile[],
    families: readonly string[],
    agentNames: ReadonlySet<string>,
    findings: Finding[],
): void {
    const membersByFamily = new Map<string, string[]>();
    const unassigned: string[] = [];

    for (const { stem } of present) {
        if (!agentNames.has(stem)) {
            continue; // not a known agent at all — the generic orphan check below reports it
        }

        const family = assignFamily(stem, families);
        if (family) {
            const members = membersByFamily.get(family) ?? [];
            members.push(stem);
            membersByFamily.set(family, members);
        } else {
            unassigned.push(stem);
        }
    }

    for (const family of families) {
        const members = [...(membersByFamily.get(family) ?? [])].sort();

        if (members.length === 0) {
            findings.push(
                findingFor(
                    surface.commandDir,
                    `Copilot-subset policy: no representative present for command family '${family}' under ${surface.commandDir} — the declared subset policy (adapter-surfaces.md §4) expects exactly one.`,
                    `surface-parity:${surface.commandDir}:copilot-family-missing:${family}`,
                ),
            );
        } else if (members.length > 1) {
            findings.push(
                findingFor(
                    surface.commandDir,
                    `Copilot-subset policy: ${String(members.length)} representatives present for command family '${family}' under ${surface.commandDir} (${members.join(", ")}) — exactly one is expected.`,
                    `surface-parity:${surface.commandDir}:copilot-family-ambiguous:${family}`,
                ),
            );
        }
    }

    for (const stem of [...unassigned].sort()) {
        findings.push(
            findingFor(
                surface.commandDir,
                `Copilot-subset policy: '${stem}' is present under ${surface.commandDir} but does not belong to any declared command family (${families.join(", ")}) — add its family to adapter-surfaces.md §4 or remove the file.`,
                `surface-parity:${surface.commandDir}:copilot-family-unexpected:${stem}`,
            ),
        );
    }
}

registerRule({
    slug: "surface-parity",
    legacyId: "Rule12",
    scope: "docs",
    gate: true,
    description:
        "Every Brain agent is represented consistently across each full adapter surface and palette declared in docs/ai/meta/adapter-surfaces.md, honouring its declared parity exceptions (agent-specific exemptions and the Copilot one-representative-per-family subset policy).",
    run(ctx: RuleContext): Finding[] {
        const docAbsPath = join(ctx.rootDir, ADAPTER_SURFACES_DOC);
        if (!existsSync(docAbsPath)) {
            return [
                findingFor(
                    ADAPTER_SURFACES_DOC,
                    `Adapter surface inventory not found at ${ADAPTER_SURFACES_DOC}.`,
                    "surface-parity:doc-missing",
                ),
            ];
        }

        const markdown = readFileSync(docAbsPath, "utf8");
        const surfaces = parseFullAdapters(markdown);
        if (surfaces.length === 0) {
            return [
                findingFor(
                    ADAPTER_SURFACES_DOC,
                    `No full adapter surfaces parsed from ${ADAPTER_SURFACES_DOC} (section 2) — cannot check command parity.`,
                    "surface-parity:no-surfaces-parsed",
                ),
            ];
        }

        const agentsAbsDir = join(ctx.rootDir, AGENTS_DIR);
        if (!existsSync(agentsAbsDir)) {
            return [
                findingFor(
                    AGENTS_DIR,
                    `Brain agent directory not found: ${AGENTS_DIR}.`,
                    "surface-parity:agents-dir-missing",
                ),
            ];
        }

        const agentNames = new Set(
            listMarkdownFiles(agentsAbsDir)
                .map((file) => file.stem)
                .filter((stem) => !NON_AGENT_BASENAMES.test(stem)),
        );
        const workflowNames = new Set(
            listMarkdownFiles(join(ctx.rootDir, WORKFLOWS_DIR)).map(
                (file) => file.stem,
            ),
        );
        const knownNames = new Set<string>([...agentNames, ...workflowNames]);

        const bootFiles = parseRootBootFiles(markdown);
        const palettes = derivePalettes(surfaces, bootFiles);

        const {
            agentExemptions,
            familyPolicyBySurface,
            findings: policyFindings,
        } = parseParityPolicy(markdown, surfaces, agentNames);

        const findings: Finding[] = [...policyFindings];

        const isAgentExempt = (agent: string, surface: string): boolean => {
            const requiredOn = agentExemptions.get(agent);
            if (!requiredOn) {
                return false;
            }
            return !requiredOn.has(surface);
        };

        for (const surface of surfaces) {
            const commandDirAbs = join(ctx.rootDir, surface.commandDir);
            if (!existsSync(commandDirAbs)) {
                findings.push(
                    findingFor(
                        surface.commandDir,
                        "Command directory declared in the adapter surface inventory does not exist.",
                        `surface-parity:${surface.commandDir}:dir-missing`,
                    ),
                );
                continue;
            }

            const present = listMarkdownFiles(commandDirAbs);
            const presentByStem = new Map(
                present.map((file) => [file.stem, file.filename]),
            );

            const familyPolicy = familyPolicyBySurface.get(surface.surface);
            if (familyPolicy) {
                checkFamilyPolicy(
                    surface,
                    present,
                    familyPolicy,
                    agentNames,
                    findings,
                );
            } else {
                for (const agent of agentNames) {
                    if (isAgentExempt(agent, surface.surface)) {
                        continue;
                    }
                    if (presentByStem.has(agent)) {
                        continue;
                    }
                    findings.push(
                        findingFor(
                            surface.commandDir,
                            `Missing adapter for agent '${agent}'.`,
                            `surface-parity:${surface.commandDir}:missing:${agent}`,
                        ),
                    );
                }
            }

            for (const [stem, filename] of presentByStem) {
                if (knownNames.has(stem)) {
                    continue;
                }
                findings.push(
                    findingFor(
                        surface.commandDir,
                        `Orphan adapter '${filename}' has no docs/ai/agents or docs/ai/workflows counterpart.`,
                        `surface-parity:${surface.commandDir}:orphan:${stem}`,
                    ),
                );
            }
        }

        for (const palette of palettes) {
            const paletteAbs = join(ctx.rootDir, palette.path);
            if (!existsSync(paletteAbs)) {
                findings.push(
                    findingFor(
                        palette.path,
                        "Palette file not found.",
                        `surface-parity:${palette.path}:missing`,
                    ),
                );
                continue;
            }

            const paletteText = readFileSync(paletteAbs, "utf8");
            for (const agent of agentNames) {
                if (isAgentExempt(agent, palette.surface)) {
                    continue;
                }
                if (paletteText.includes(`${AGENTS_DIR}/${agent}.md`)) {
                    continue;
                }
                findings.push(
                    findingFor(
                        palette.path,
                        `Palette does not list agent '${agent}'.`,
                        `surface-parity:${palette.path}:missing-agent:${agent}`,
                    ),
                );
            }
        }

        findings.sort((a, b) =>
            a.file === b.file
                ? a.message.localeCompare(b.message)
                : a.file.localeCompare(b.file),
        );

        return findings;
    },
});
