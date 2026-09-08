# Git hooks

`pre-commit` formats only the staged C# files with `dotnet format --include` and re-stages
them. A commit with no staged `.cs` file runs nothing. The hook is an `sh` shim (git runs hooks
through `sh`) over `pre-commit.ps1`, which holds the logic.

Enable once per clone; the setting is shared by every worktree of that clone:

```
git config core.hooksPath tools/git/hooks
```

## Line endings

`.gitattributes` sets `eol=lf` for every text file. That overrides `core.autocrlf=true`, which the
Git for Windows installer sets at machine level, so a Windows checkout gets LF, the same ending
`.editorconfig` (`end_of_line = lf`) tells `dotnet format` to write. Because the two agree, a
format pass never rewrites a line ending and git never reports hundreds of phantom modified files
afterwards. CMD batch files are the one exception and are CRLF in both places.

A checkout made before this change still holds CRLF files. Refresh it once, with a clean working
tree. `git checkout-index --force` is not enough, because it skips every file whose stat still
matches the index:

```
git rm -rq --cached . && git reset -q --hard
```

`git ls-files --eol` shows the state per file: `i/` is the index, `w/` is the working tree, and
every text file should read `i/lf w/lf`.
