---
name: changelog
description: Generate a changelog from git history between two refs (tags, commits, or dates).
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
