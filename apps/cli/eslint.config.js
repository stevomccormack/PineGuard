// Minimal flat config (ESLint 10 + typescript-eslint 8). Widened per-rule as
// P2 rule agents land; see docs/ai/plans/audit-cli-rebuild.md P1.1.
import js from "@eslint/js";
import globals from "globals";
import tseslint from "typescript-eslint";

export default tseslint.config(
    {
        ignores: ["dist/**", "node_modules/**"],
    },
    js.configs.recommended,
    ...tseslint.configs.recommended,
    {
        languageOptions: {
            globals: globals.node,
        },
    },
);
