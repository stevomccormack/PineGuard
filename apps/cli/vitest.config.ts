import { defineConfig } from "vitest/config";

export default defineConfig({
    test: {
        include: ["test/**/*.test.ts"],
        environment: "node",
        // P1.1 ships no tests yet (rules land in P2, each with its own
        // test/rules/<slug>.test.ts). Without this, vitest exits 1 on an empty
        // suite; flip it back off once the first real test file lands.
        passWithNoTests: true,
    },
});
