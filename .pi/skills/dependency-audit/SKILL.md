---
name: dependency-audit
description: Check NuGet packages for vulnerabilities and outdated versions across the solution.
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
