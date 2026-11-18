#!/usr/bin/env python3
"""destructive-guard.py — classify a shell command for pre-bash-destructive-guard.sh.

Reads the command text on stdin and prints one line per destructive operation found:

    <rule-id>\t<title>\t<why>\t<dirty-only: 1|0>

Nothing printed means nothing destructive was found. "dirty-only" marks operations that only lose
work when the working tree has uncommitted changes; the shell wrapper checks that.

Why a parser and not a regular expression over the whole string: the old guard matched text that was
never executed — a commit message containing "-conformance" tripped the force-push rule, and a script
written to disk through a heredoc tripped the rm rule. This file looks only at what will run:

  * heredoc bodies are dropped unless an interpreter consumes them (bash, python, pwsh, ...);
  * each command between ; && || | & and newlines is judged on its own tokens;
  * quoted text is an argument, not a command, unless it is handed to an interpreter
    (bash -c, pwsh -Command, python -c, eval, cmd /c), in which case it is parsed in turn.

It is a guard against accidents, not a sandbox: a destructive command hidden in a script file that is
executed later is not seen here.
"""
import re
import sys

INTERPRETERS = {
    "bash", "sh", "zsh", "dash", "ksh", "pwsh", "powershell", "python", "python3", "py", "node",
    "perl", "ruby", "cmd", "ssh", "eval", "source", ".",
}
WRAPPERS = {"sudo", "command", "exec", "time", "nohup", "nice", "ionice", "stdbuf", "builtin"}
MAX_DEPTH = 4


def strip_heredocs(text):
    """Drop heredoc bodies that are data; keep (and return for rescanning) those fed to an interpreter."""
    lines = text.split("\n")
    out, nested = [], []
    i = 0
    opener = re.compile(r"<<(-?)\s*(['\"]?)([A-Za-z_][A-Za-z0-9_]*)\2")
    while i < len(lines):
        line = lines[i]
        found = list(opener.finditer(line))
        out.append(line)
        i += 1
        for match in found:
            strip_tabs, delimiter = match.group(1) == "-", match.group(3)
            body = []
            while i < len(lines):
                candidate = lines[i].lstrip("\t") if strip_tabs else lines[i]
                i += 1
                if candidate == delimiter:
                    break
                body.append(lines[i - 1])
            before = re.split(r"\|\||&&|[|;&]", line[: match.start()])[-1]
            words = [w for w in before.split() if not re.match(r"^[A-Za-z_][A-Za-z0-9_]*=", w)]
            while words and words[0] in WRAPPERS:
                words = words[1:]
            consumer = words[0].rsplit("/", 1)[-1].lower() if words else ""
            if consumer in INTERPRETERS:
                nested.append((consumer, "\n".join(body)))
    return "\n".join(out), nested


def split_segments(text):
    """Split into commands; each command is a list of (word, was_quoted) tuples."""
    segments, words = [], []
    word, quoted_word, has_word = [], False, False
    quote = None
    i, n = 0, len(text)

    def end_word():
        nonlocal word, quoted_word, has_word
        if has_word:
            words.append(("".join(word), quoted_word))
        word, quoted_word, has_word = [], False, False

    def end_segment():
        nonlocal words
        end_word()
        if words:
            segments.append(words)
        words = []

    while i < n:
        ch = text[i]
        if quote == "'":
            if ch == "'":
                quote = None
            else:
                word.append(ch)
            i += 1
            continue
        if quote == '"':
            if ch == '"':
                quote = None
            elif ch == "\\" and i + 1 < n:
                word.append(text[i + 1])
                i += 1
            elif ch == "$" and i + 1 < n and text[i + 1] == "(":
                # Command substitution runs even inside double quotes: judge it as its own command.
                depth, j = 1, i + 2
                while j < n and depth:
                    depth += text[j] == "("
                    depth -= text[j] == ")"
                    j += 1
                segments.extend(split_segments(text[i + 2 : j - 1]))
                i = j - 1
            else:
                word.append(ch)
            i += 1
            continue
        if ch in "'\"":
            quote, quoted_word, has_word = ch, True, True
        elif ch == "\\" and i + 1 < n:
            if text[i + 1] != "\n":
                word.append(text[i + 1])
                has_word = True
            i += 1
        elif ch == "#" and not has_word:
            while i < n and text[i] != "\n":
                i += 1
            continue
        elif ch in " \t":
            end_word()
        elif ch in ";\n|&(){}`":
            end_segment()
        elif ch == "$" and i + 1 < n and text[i + 1] == "(":
            end_segment()
            i += 1
        elif ch in "<>":
            # Redirection: the target is a file name, not a command word. Skip it.
            end_word()
            while i + 1 < n and text[i + 1] in "<>&| \t":
                i += 1
            while i + 1 < n and text[i + 1] not in " \t;\n|&)":
                i += 1
        else:
            word.append(ch)
            has_word = True
        i += 1
    end_segment()
    return segments


def short_flags(args):
    """Letters of every single-dash cluster before `--`: ['-rf', '-v'] -> 'rfv'."""
    letters = ""
    for arg in args:
        if arg == "--":
            break
        if re.match(r"^-[A-Za-z]+$", arg):
            letters += arg[1:]
    return letters


def options(args):
    """Arguments before `--`."""
    return args[: args.index("--")] if "--" in args else args


def judge_git(args, found):
    i = 0
    while i < len(args):
        arg = args[i]
        if arg in ("-C", "-c", "--git-dir", "--work-tree", "--namespace"):
            i += 2
        elif arg.startswith("-"):
            i += 1
        else:
            break
    if i >= len(args):
        return
    sub, rest = args[i], args[i + 1 :]
    opts, flags = options(rest), short_flags(rest)

    if sub == "reset" and "--hard" in opts:
        found.append(("git-reset-hard", "git reset --hard",
                      "Moves HEAD and discards uncommitted changes; commits after the target are lost.", 0))
    elif sub == "push":
        refspecs = [a for a in rest if not a.startswith("-")]
        force = any(a == "--force" or a.startswith("--force-with-lease") or a in ("--force-if-includes", "--mirror")
                    for a in opts) or "f" in flags or any(r.startswith("+") for r in refspecs)
        delete = "--delete" in opts or "d" in flags or "--prune" in opts or any(r.startswith(":") for r in refspecs)
        if force:
            found.append(("git-push-force", "git push --force",
                          "Rewrites remote history; affects every collaborator and cannot be undone.", 0))
        if delete:
            found.append(("git-push-delete", "git push that deletes a remote ref",
                          "Deletes a branch or tag on the remote.", 0))
    elif sub == "branch":
        if "D" in flags or ("--delete" in opts and "--force" in opts) or ("d" in flags and "f" in flags):
            found.append(("git-branch-force-delete", "git branch -D (force delete)",
                          "Force-deletes a branch; unmerged commits may become unreachable.", 0))
    elif sub == "clean":
        dry = "n" in flags or "--dry-run" in opts
        if ("f" in flags or "--force" in opts) and not dry:
            found.append(("git-clean", "git clean -f",
                          "Permanently deletes untracked files; they are not recoverable from git.", 0))
    elif sub == "checkout":
        discards = "f" in flags or "--force" in opts or "." in rest or ("--" in rest and rest[rest.index("--") + 1 :])
        if discards:
            found.append(("git-checkout-discard", "git checkout that discards working-tree changes",
                          "Overwrites uncommitted changes in the named paths.", 1))
    elif sub == "restore":
        staged_only = ("--staged" in opts or "S" in flags) and not ("--worktree" in opts or "W" in flags)
        if not staged_only:
            found.append(("git-restore-discard", "git restore that discards working-tree changes",
                          "Overwrites uncommitted changes in the named paths.", 1))
    elif sub == "stash" and rest and rest[0] in ("drop", "clear"):
        found.append(("git-stash-drop", "git stash %s" % rest[0], "Deletes stashed work permanently.", 0))
    elif sub in ("filter-branch", "filter-repo"):
        found.append(("git-history-rewrite", "git %s" % sub, "Rewrites repository history.", 0))
    elif sub == "reflog" and rest and rest[0] in ("expire", "delete"):
        found.append(("git-reflog-prune", "git reflog %s" % rest[0],
                      "Removes the record that makes lost commits recoverable.", 0))
    elif sub == "update-ref" and ("d" in flags or "--delete" in opts):
        found.append(("git-update-ref-delete", "git update-ref -d", "Deletes a ref directly.", 0))
    elif sub == "worktree" and rest and rest[0] == "remove" and ("f" in short_flags(rest[1:]) or "--force" in rest):
        found.append(("git-worktree-force-remove", "git worktree remove --force",
                      "Deletes a worktree with uncommitted changes.", 0))


def judge_command(words, found, depth):
    """words: list of (text, was_quoted) for one command."""
    args = [w for w, _ in words]
    while args and re.match(r"^[A-Za-z_][A-Za-z0-9_]*=", args[0]):
        args = args[1:]
    while args:
        head = args[0].rsplit("/", 1)[-1]
        if head in WRAPPERS:
            args = args[1:]
            while args and args[0].startswith("-"):
                args = args[1:]
        elif head == "env":
            args = args[1:]
            while args and (args[0].startswith("-") or re.match(r"^[A-Za-z_][A-Za-z0-9_]*=", args[0])):
                args = args[1:]
        elif head == "timeout":
            args = args[1:]
            while args and args[0].startswith("-"):
                args = args[1:]
            args = args[1:]
        elif head == "xargs":
            args = args[1:]
            while args and args[0].startswith("-"):
                takes_value = args[0] in ("-I", "-n", "-P", "-L", "-d", "-E", "-s", "-a")
                args = args[2:] if takes_value else args[1:]
        else:
            break
    if not args:
        return
    name = args[0].rsplit("/", 1)[-1].rsplit("\\", 1)[-1]
    lower, rest = name.lower(), args[1:]
    if lower.endswith(".exe"):
        lower = lower[:-4]

    # Text handed to an interpreter is a command in its own right.
    if depth < MAX_DEPTH:
        if lower in ("bash", "sh", "zsh", "dash", "ksh") and "-c" in rest and rest.index("-c") + 1 < len(rest):
            scan(rest[rest.index("-c") + 1], found, depth + 1)
        elif lower in ("pwsh", "powershell"):
            for i, arg in enumerate(rest):
                if arg.lower() in ("-command", "-c") and i + 1 < len(rest):
                    scan(" ".join(rest[i + 1 :]), found, depth + 1)
                    break
        elif lower in ("python", "python3", "py") and "-c" in rest and rest.index("-c") + 1 < len(rest):
            scan_python(rest[rest.index("-c") + 1], found)
        elif lower == "cmd" and any(a.lower() == "/c" for a in rest):
            index = [a.lower() for a in rest].index("/c")
            scan(" ".join(rest[index + 1 :]), found, depth + 1)
        elif lower == "eval":
            scan(" ".join(rest), found, depth + 1)

    if lower == "git":
        judge_git(rest, found)
    elif lower == "rm":
        opts, flags = options(rest), short_flags(rest)
        recursive = "r" in flags or "R" in flags or "--recursive" in opts
        force = "f" in flags or "--force" in opts
        if recursive and force:
            found.append(("rm-recursive-force", "rm with recursive + force",
                          "Permanently deletes directories and all their contents.", 0))
    elif lower == "find":
        deletes = "-delete" in rest or any(
            rest[i] in ("-exec", "-execdir", "-ok") and i + 1 < len(rest) and rest[i + 1].rsplit("/", 1)[-1] == "rm"
            for i in range(len(rest)))
        if deletes:
            found.append(("find-delete", "find that deletes what it matches",
                          "Deletes every matching file; a wide pattern removes far more than intended.", 0))
    elif lower in ("remove-item", "ri", "del", "erase"):
        lowered = [a.lower() for a in rest]
        recurse = any(re.match(r"^-r(e(c(u(r(se?)?)?)?)?)?$", a) for a in lowered)
        force = any(re.match(r"^-fo(r(ce?)?)?$", a) for a in lowered)
        if recurse and force:
            found.append(("pwsh-remove-item", "PowerShell Remove-Item -Recurse -Force",
                          "Recursive force-delete through PowerShell.", 0))
    elif lower in ("rd", "rmdir") and any(a.lower() == "/s" for a in rest):
        found.append(("cmd-rd-s", "rd /s or rmdir /s (Windows recursive delete)",
                      "Recursively removes a directory and all its contents.", 0))
    elif lower in ("shred", "mkfs") or lower.startswith("mkfs."):
        found.append(("disk-destroy", lower, "Destroys file or filesystem contents irrecoverably.", 0))


def scan_python(text, found):
    if re.search(r"\bshutil\s*\.\s*rmtree\b|\brmtree\s*\(", text):
        found.append(("python-rmtree", "Python shutil.rmtree", "Recursive directory removal through Python.", 0))


def scan(text, found, depth=0):
    visible, nested = strip_heredocs(text)
    for words in split_segments(visible):
        judge_command(words, found, depth)
    for consumer, body in nested:
        if consumer in ("python", "python3", "py"):
            scan_python(body, found)
        elif consumer in ("pwsh", "powershell", "bash", "sh", "zsh", "dash", "ksh", "ssh", "eval", "cmd") \
                and depth < MAX_DEPTH:
            scan(body, found, depth + 1)


def main():
    found = []
    try:
        scan(sys.stdin.read(), found)
    except RecursionError:
        found.append(("unparsable", "command too deeply nested to check", "Simplify the command.", 0))
    seen = set()
    for rule, title, why, dirty_only in found:
        if rule not in seen:
            seen.add(rule)
            print("%s\t%s\t%s\t%d" % (rule, title, why, dirty_only))


if __name__ == "__main__":
    main()
