import path from "node:path";
import { fileURLToPath } from "node:url";

import { describe, expect, it } from "vitest";

import {
    findAttributes,
    findInvocations,
    findMethodDeclarations,
    findRecordDeclarations,
    findTupleTypes,
    parseFile,
} from "../../../src/audit/parsing/csharp.js";

/**
 * Proves the P1.2 parser spike end-to-end (plan `docs/ai/plans/audit-cli-rebuild.md`
 * §4.5, §8, §9.2 P1.2) against three real files from this repo — not fixtures.
 * This is the "does the wasm grammar actually load and answer the queries the
 * rules will need" check; per-rule pass/fail fixture tests are P1.4/P2's job.
 */

// This file lives at apps/cli/test/audit/parsing/csharp.test.ts; walk up five
// directories (parsing -> audit -> test -> cli -> apps) to the repo/worktree
// root, so the three real files resolve regardless of the invoking cwd.
const repoRoot = path.resolve(
    path.dirname(fileURLToPath(import.meta.url)),
    "../../../../..",
);

const mustStringClausesPath = path.join(
    repoRoot,
    "src/PineGuard.MustClauses/MustStringClauses.cs",
);
const guardStringClausesPath = path.join(
    repoRoot,
    "src/PineGuard.GuardClauses/GuardStringClauses.cs",
);
const mustStringClausesTestDataPath = path.join(
    repoRoot,
    "tests/PineGuard.MustClauses.UnitTests/MustStringClausesTestData.cs",
);

describe("parseFile", () => {
    it("parses a real Must clauses file into a compilation_unit root", async () => {
        const parsed = await parseFile(mustStringClausesPath);
        expect(parsed.root.type).toBe("compilation_unit");
        expect(parsed.root.hasError).toBe(false);
    });

    it("reuses the cached Tree on a second parse of the same file", async () => {
        const first = await parseFile(mustStringClausesPath);
        const second = await parseFile(mustStringClausesPath);
        // `Tree` is the cached object identity; `Node` is a fresh lightweight
        // wrapper per access (even for the same underlying tree node), so
        // compare roots structurally (`Node#equals`) rather than by `toBe`.
        expect(second.tree).toBe(first.tree);
        expect(second.root.equals(first.root)).toBe(true);
        expect(second.root.id).toBe(first.root.id);
    });
});

describe("findMethodDeclarations — MustStringClauses.cs", () => {
    it("finds every public static Must method with its modifiers and parameters", async () => {
        const { root } = await parseFile(mustStringClausesPath);
        const methods = findMethodDeclarations(root);

        // The file declares ~55 `public static MustResult<string> Xxx(...)`
        // extension methods; a generous lower bound proves real coverage
        // without pinning to an exact, fixture-fragile count.
        expect(methods.length).toBeGreaterThanOrEqual(30);

        const nullOrEmpty = methods.find((m) => m.name === "NullOrEmpty");
        expect(nullOrEmpty).toBeDefined();
        expect(nullOrEmpty?.modifiers).toEqual(
            expect.arrayContaining(["public", "static"]),
        );
        expect(nullOrEmpty?.parameters.length).toBeGreaterThanOrEqual(2);
        expect(nullOrEmpty?.parameters[0]?.name).toBe("_");
        expect(nullOrEmpty?.parameters[0]?.modifiers).toContain("this");

        // Every method's synthetic paramName parameter carries [CallerArgumentExpression].
        const paramNameParams = methods
            .flatMap((m) => m.parameters)
            .filter((p) => p.name === "paramName");
        expect(paramNameParams.length).toBeGreaterThan(0);
        expect(
            paramNameParams.every((p) =>
                p.attributes.some((a) => a.name === "CallerArgumentExpression"),
            ),
        ).toBe(true);
    });

    it("finds [CallerArgumentExpression] attributes anywhere under the tree", async () => {
        const { root } = await parseFile(mustStringClausesPath);
        const callerArgAttrs = findAttributes(root, "CallerArgumentExpression");
        expect(callerArgAttrs.length).toBeGreaterThanOrEqual(30);
        expect(callerArgAttrs[0]?.argumentsText).toMatch(
            /^\(nameof\(value\)\)$/,
        );
    });
});

describe("findInvocations — GuardStringClauses.cs", () => {
    it("finds Must.Be.<Name> member-access invocations the Guard layer delegates to", async () => {
        const { root } = await parseFile(guardStringClausesPath);
        const mustBeCalls = findInvocations(root, "Must.Be");

        expect(mustBeCalls.length).toBeGreaterThanOrEqual(40);
        expect(
            mustBeCalls.every((call) => call.memberPath.startsWith("Must.Be.")),
        ).toBe(true);

        const nullOrEmptyCall = mustBeCalls.find(
            (call) => call.memberPath === "Must.Be.NullOrEmpty",
        );
        expect(nullOrEmptyCall).toBeDefined();
        expect(nullOrEmptyCall?.argumentsText).toBe("(value, paramName)");
    });

    it("returns [] for a member path with no matching call sites", async () => {
        const { root } = await parseFile(guardStringClausesPath);
        expect(findInvocations(root, "NotARealClass.NotARealMethod")).toEqual(
            [],
        );
    });
});

describe("findRecordDeclarations + findTupleTypes — MustStringClausesTestData.cs", () => {
    it("finds ValidCase/EdgeCase records with their primary-constructor parameters and base clause", async () => {
        const { root } = await parseFile(mustStringClausesTestDataPath);
        const records = findRecordDeclarations(root);

        expect(records.length).toBeGreaterThanOrEqual(20);
        expect(
            records.every((r) => r.modifiers.includes("record") === false),
        ).toBe(true); // sanity: "record" is a keyword, not a modifier child

        const withPrimaryCtor = records.filter((r) => r.parameters !== null);
        expect(withPrimaryCtor.length).toBeGreaterThanOrEqual(20);

        const isCaseRecord = withPrimaryCtor.find((r) =>
            r.baseListText?.includes("IsCase"),
        );
        expect(isCaseRecord).toBeDefined();
        expect(isCaseRecord?.modifiers).toEqual(
            expect.arrayContaining(["public", "sealed"]),
        );
        expect(
            isCaseRecord?.parameters?.some((p) => p.name === "Expected"),
        ).toBe(true);
    });

    it("finds tuple_type nodes and distinguishes them from record parameter lists", async () => {
        const { root } = await parseFile(mustStringClausesTestDataPath);
        const tupleTypes = findTupleTypes(root);

        // ExactLength/LengthBetween each declare a tuple parameter type and
        // reuse it in the base-record type argument (IsCase<(...)>), so real
        // tuple_type nodes vastly outnumber the ~2 distinct tuple shapes.
        expect(tupleTypes.length).toBeGreaterThanOrEqual(10);

        const lengthTuple = tupleTypes.find((t) =>
            t.elements.some((el) => el.name === "Length"),
        );
        expect(lengthTuple).toBeDefined();
        expect(lengthTuple?.elements).toEqual(
            expect.arrayContaining([
                expect.objectContaining({ name: "Value" }),
                expect.objectContaining({ name: "Length", typeText: "int" }),
            ]),
        );

        // Tuple element names are captured (not just types), which is what
        // lets a rule distinguish a tuple_type from a record's own
        // parameter_list — the two look similar in source but are unrelated
        // grammar node shapes.
        expect(
            findRecordDeclarations(root).some(
                (r) => r.node.type === "record_declaration",
            ),
        ).toBe(true);
    });

    it("finds [GeneratedRegex] attributes on partial methods in the same file", async () => {
        const { root } = await parseFile(mustStringClausesTestDataPath);
        const generatedRegexAttrs = findAttributes(root, "GeneratedRegex");
        expect(generatedRegexAttrs.length).toBeGreaterThanOrEqual(1);
    });
});
