/**
 * PineGuard Hooks Extension for PI Coding Agent
 *
 * Replaces Claude Code hooks (.claude/hooks/) with PI-native TypeScript extension.
 * Provides:
 *   1. Block solution file edits (*.sln, *.slnx)
 *   2. Enforce output directory hygiene (no files in project root)
 *   3. Post-edit reminders (dotnet format, test data spec)
 *   4. Dotnet-ops mutex lock for heavy commands (build, test, coverage, sonar)
 *
 * Brain reference: docs/ai/specs/safety.md (Tier 0/1/2 command classification)
 */

import type { ExtensionAPI } from "@mariozechner/pi-coding-agent";

// ── Known source directories and extensions ──────────────────────────────────

const ALLOWED_PREFIXES = [
  "src/", "tests/", "docs/", "tools/", ".claude/", ".pi/", ".agent/",
  ".github/", ".vscode/", "artifacts/", "logs/", "diagnostics/",
];

const ALLOWED_ROOT_EXTENSIONS = new Set([
  ".cs", ".csproj", ".sln", ".slnx", ".md", ".json", ".xml", ".yaml", ".yml",
  ".ps1", ".psm1", ".psd1", ".sh", ".editorconfig", ".gitignore",
  ".gitattributes", ".props", ".targets", ".ruleset", ".DotSettings",
]);

const ALLOWED_ROOT_FILES = new Set([
  "CLAUDE.md", "AGENTS.md", "LICENSE", "LICENSE.md", "README.md",
  "Directory.Build.props", "Directory.Packages.props", "nuget.config",
  "global.json", ".cursorrules",
]);

const HEAVY_CMD_PATTERN =
  /dotnet\s+(test|build|publish)|Run-CodeCoverage|Run-SonarScanner|sonar-scanner|sonarscanner/i;

// ── Lock state (in-memory, per session) ──────────────────────────────────────

let dotnetOpsLocked = false;

// ── Helpers ──────────────────────────────────────────────────────────────────

function normalizePath(filePath: string): string {
  return filePath.replace(/\\/g, "/");
}

function getRelativePath(filePath: string): string {
  const norm = normalizePath(filePath);
  const marker = "/PineGuard/";
  const idx = norm.indexOf(marker);
  return idx >= 0 ? norm.substring(idx + marker.length) : norm;
}

function basename(filePath: string): string {
  const norm = normalizePath(filePath);
  const parts = norm.split("/");
  return parts[parts.length - 1] || "";
}

function extension(filename: string): string {
  const dot = filename.lastIndexOf(".");
  return dot >= 0 ? filename.substring(dot) : "";
}

// ── Extension entry point ────────────────────────────────────────────────────

export default function (pi: ExtensionAPI) {

  // ── 1. Block solution file edits ────────────────────────────────────────

  pi.on("tool_call", async (event, ctx) => {
    if (event.toolName !== "write" && event.toolName !== "edit") return;

    const filePath: string = event.input?.file_path || event.input?.path || "";
    const name = basename(filePath);

    if (name.endsWith(".sln") || name.endsWith(".slnx")) {
      return {
        block: true,
        reason:
          "Direct edits to solution files (*.sln/*.slnx) are not allowed. " +
          "Use `dotnet sln` commands to modify the solution safely.",
      };
    }
  });

  // ── 2. Enforce output directory hygiene ─────────────────────────────────

  pi.on("tool_call", async (event, ctx) => {
    if (event.toolName !== "write") return;

    const filePath: string = event.input?.file_path || event.input?.path || "";
    const rel = getRelativePath(filePath);
    const name = basename(filePath);

    // Allow known source directories
    for (const prefix of ALLOWED_PREFIXES) {
      if (rel.startsWith(prefix)) return;
    }

    // Allow known root config/source files
    if (ALLOWED_ROOT_FILES.has(name)) return;
    if (ALLOWED_ROOT_EXTENSIONS.has(extension(name))) return;

    // Block with suggestion
    let suggestion = `artifacts/<category>/${name} or logs/<date>/${name}`;
    if (name.endsWith(".log") || name.endsWith(".txt")) {
      suggestion = `logs/YYYY-MM-DD/${name}`;
    } else if (name.endsWith(".trx")) {
      suggestion = `artifacts/test-results/YYYY-MM-DD/${name}`;
    } else if (name.endsWith(".html")) {
      suggestion = `artifacts/<tool>/html/${name}`;
    } else if (name.endsWith(".sarif")) {
      suggestion = `artifacts/<tool>/report/${name}`;
    }

    return {
      block: true,
      reason:
        `Files must not be created in the project root. ` +
        `Move to: ${suggestion}`,
    };
  });

  // ── 3. Post-edit reminders ──────────────────────────────────────────────

  pi.on("tool_result", async (event, ctx) => {
    if (event.toolName !== "write" && event.toolName !== "edit") return;

    const filePath: string =
      event.input?.file_path || event.input?.path || "";
    const name = basename(filePath);

    if (name.endsWith(".cs")) {
      ctx.ui.notify(
        "Reminder: Run `dotnet format` to ensure editorconfig compliance.",
        "info",
      );
    }

    if (name.endsWith("TestData.cs")) {
      ctx.ui.notify(
        "TestData spec: Expected (not ExpectedReturn), camelCase tuples, single-line cases.",
        "info",
      );
    }

    if (name.endsWith("Tests.cs")) {
      ctx.ui.notify(
        "Tests spec: BehavesAsExpected/ThrowsAsExpected naming, AAA comments, nested Op Groups.",
        "info",
      );
    }
  });

  // ── 4. Dotnet-ops mutex lock ────────────────────────────────────────────

  pi.on("tool_call", async (event, ctx) => {
    if (event.toolName !== "bash") return;

    const command: string = event.input?.command || "";
    if (!HEAVY_CMD_PATTERN.test(command)) return;

    if (dotnetOpsLocked) {
      return {
        block: true,
        reason:
          "dotnet-ops lock is held by a previous command. " +
          "Wait for it to complete before running another heavy command.",
      };
    }

    dotnetOpsLocked = true;
    ctx.ui.setStatus("pineguard", `Running: ${command.substring(0, 60)}...`);
  });

  pi.on("tool_result", async (event, ctx) => {
    if (event.toolName !== "bash") return;

    const command: string = event.input?.command || "";
    if (!HEAVY_CMD_PATTERN.test(command)) return;

    dotnetOpsLocked = false;
    ctx.ui.setStatus("pineguard", "");
  });

  // ── 5. Session cleanup ─────────────────────────────────────────────────

  pi.on("session_shutdown", async (_event, _ctx) => {
    dotnetOpsLocked = false;
  });

  // ── Startup notification ───────────────────────────────────────────────

  pi.on("session_start", async (_event, ctx) => {
    ctx.ui.notify("PineGuard hooks loaded: sln-guard, output-dirs, format-remind, dotnet-lock", "info");
  });
}
