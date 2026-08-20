---
name: dependency-audit
description: Check NuGet packages for vulnerabilities, outdated versions, and deprecations across the solution. Use whenever the user says "audit packages", "check dependencies", "any vulnerabilities", "outdated NuGet packages", "check for deprecated packages", or wants a package health report.
disable-model-invocation: true
context: fork
allowed-tools: Read, Bash, Grep, Glob
metadata:
  author: stevomccormack
  version: 1.1.0
  category: maintenance
---
# Skill: Dependency Audit

## Step 1: Check Vulnerabilities
```bash
dotnet list PineGuard.slnx package --vulnerable
```

## Step 2: Check Outdated Packages
```bash
dotnet list PineGuard.slnx package --outdated
```

## Step 3: Check Deprecated Packages
```bash
dotnet list PineGuard.slnx package --deprecated
```

## Step 4: Report
Summarise findings in a table:

| Package | Current | Latest | Issue | Project(s) |
|---------|---------|--------|-------|-------------|

- Flag **Critical/High** vulnerabilities prominently
- Group outdated packages by major vs minor version bumps
- Note any deprecated packages with suggested replacements
