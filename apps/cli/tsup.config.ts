import { defineConfig } from "tsup";

export default defineConfig({
    entry: ["src/index.ts"],
    format: ["esm"],
    target: "node22",
    platform: "node",
    clean: true,
    sourcemap: true,
    dts: false,
    shims: false,
    // src/index.ts carries the `#!/usr/bin/env node` shebang; tsup detects it
    // on the entry point and re-emits it (plus chmod +x) on the built file.
});
