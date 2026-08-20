---
name: changelog
description: Generate a formatted markdown changelog from git history. Use whenever the user says "generate a changelog", "what changed since X", "release notes", "summarise commits", or wants a commit history formatted into Features/Bug Fixes/Maintenance sections between two refs, tags, or dates.
argument-hint: "[from-ref] [to-ref]"
disable-model-invocation: true
context: fork
allowed-tools: Read, Bash, Grep, Glob
metadata:
  author: stevomccormack
  version: 1.1.0
  category: maintenance
---
# Skill: Changelog

## Step 1: Determine Range
- If the user provides two refs (tags, SHAs, dates), use them
- Otherwise, find the most recent tag: `git describe --tags --abbrev=0`
- Default range: `<last-tag>..HEAD`

## Step 2: Gather Commits
```bash
git log <from>..<to> --pretty=format:"%h %s" --no-merges
```

## Step 3: Categorise
Group commits by prefix into sections:
- **Features** — `feat:` commits
- **Bug Fixes** — `fix:` commits
- **Tests** — `test:` / `tests:` commits
- **Documentation** — `docs:` / `Docs:` commits
- **Maintenance** — `chore:` / `refactor:` / `build:` commits
- **Other** — anything that doesn't match

## Step 4: Output
Format as markdown:

```markdown
## [version] — YYYY-MM-DD

### Features
- description (hash)

### Bug Fixes
- description (hash)
...
```

Omit empty sections. Use the commit subject (without prefix) as the description.
