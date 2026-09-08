<#
.SYNOPSIS
    Creates one or more clean, scoped git commits in-process, driven by the PineGuard project
    registry plus a small fixed table of cross-cutting meta-scopes.

.DESCRIPTION
    Replaces the sixteen near-identical tools/git/Commit-*.ps1 scripts (F-33). Each registry
    scope (Core, MustClauses, GuardClauses, DataAnnotations, FluentValidation, Options,
    DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing —
    from tools/.shared/dotnet-projects.ps1's Get-PineGuardScope) and five meta-scopes that do not
    correspond to a single shipped project (Agent, Docs, Tools, Solution, Ci) resolve to a stage
    path list and commit through tools/.shared/git.ps1's Invoke-Commit, all inside this one pwsh
    process — no child pwsh is spawned per scope, so -Message now actually reaches every commit
    it applies to (previously it could not cross the process boundary at all).

    -Scope accepts any number of these nineteen names; validity is checked against the live
    registry plus the five hard-coded meta-scope names, so a new registry scope (e.g. a future
    fifteenth project) becomes a valid -Scope value automatically, with no literal list here to
    edit.

    README.md is staged by exactly one scope going forward: Docs (F-34). Previously it was staged by
    both the old Commit-Docs and Commit-Solution scripts, so whichever ran first "won" it; Solution's
    path list no longer includes it. .github/workflows moves out of the Agent meta-scope into the
    new Ci meta-scope, since it is CI configuration, not an assistant-adapter surface.

.PARAMETER Scope
    One or more scope names to commit: the fourteen registry scopes, or the five meta-scopes
    Agent, Docs, Tools, Solution, Ci. Case-insensitive. Ignored if -All is also given.

.PARAMETER All
    Commit every registry scope and every meta-scope (implies -IncludeTests).

.PARAMETER IncludeTests
    For registry-derived scopes, also stage the paired test project's directory. Has no effect on
    the five meta-scopes, which are not registry entries.

.PARAMETER Message
    An explicit commit message applied to every scope selected in this invocation. Reaches
    Invoke-Commit directly now that every scope commits in-process (previously this could not
    cross the per-scope child-process boundary at all).

.PARAMETER AutoMessage
    Generate a conventional-commits-style message per scope (subject + flowing prose body; see
    tools/.shared/git.ps1's New-AutoCommitMessage) instead of requiring -Message or opening an
    editor. Ignored for a scope where -Message is also supplied.

.PARAMETER Push
    Push to -Remote after all selected scopes have committed. Alone, this is a plain push that
    throws if the local branch is behind upstream (matching git's own refusal); combined with
    -Rebase it becomes a safe push (fetch, rebase-if-behind, then push).

.PARAMETER Rebase
    Fetch and rebase onto -Remote (via `git pull --rebase --autostash`) if the local branch is
    behind, both before and after committing. Renamed from the old script's -AutoRebase.
    Combined with -Push, this is the old script's -SafePush shorthand (rebase, then push) with no
    separate switch needed.

.PARAMETER Remote
    Git remote name for -Push and -Rebase. Defaults to 'origin'.

.PARAMETER WhatIf
    Preview what would be staged and committed for each selected scope without making any
    changes. -DryRun is a supported alias of the same switch (D-1d in
    docs/ai/plans/tools-review-and-standardisation.md): both spellings resolve to one
    implementation.

.EXAMPLE
    ./tools/git/Run-Commits.ps1 -Scope Core -WhatIf

    Previews the Core scope's commit without staging or committing anything.

.EXAMPLE
    ./tools/git/Run-Commits.ps1 -Scope Core,Tools -IncludeTests -AutoMessage

    Commits the Core scope (including its paired test project) and the Tools scope, each with an
    auto-generated conventional-commit message.

.EXAMPLE
    ./tools/git/Run-Commits.ps1 -All -AutoMessage -Push -Rebase

    Commits every scope with auto-generated messages, rebasing onto the remote first and last,
    then safely pushes.
#>

[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateScript({
            # This block runs at parameter-binding time, before the script body below (including its
            # own dot-source of dotnet-projects.ps1) executes — so it dot-sources its own copy here,
            # scoped to this validation only. The registry lookup is what keeps -Scope's valid values
            # in sync with tools/.shared/dotnet-projects.ps1 automatically; only the five meta-scope
            # names are a literal list, and that list is small and stable by design (§3.1's two-level
            # rule), unlike the registry scopes it is meant to sit beside.
            . (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')
            $metaScopeNames = @('Agent', 'Docs', 'Tools', 'Solution', 'Ci')
            $validScopes = @((Get-PineGuardScope -All).Name) + $metaScopeNames
            foreach ($value in $_) {
                if ($value -notin $validScopes) {
                    throw "Invalid -Scope value '$value'. Valid scopes: $($validScopes -join ', ')."
                }
            }
            return $true
        })]
    [string[]] $Scope,

    [switch] $All,
    [switch] $IncludeTests,
    [string] $Message,
    [switch] $AutoMessage,
    [switch] $Push,
    [switch] $Rebase,
    [string] $Remote = 'origin',

    [Alias('DryRun')]
    [switch] $WhatIf
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/../.shared/path.ps1"
. "$PSScriptRoot/../.shared/dotnet-projects.ps1"
. "$PSScriptRoot/../.shared/git.ps1"

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

# Meta-scopes are cross-cutting file groups that do not correspond to a single registry entry —
# each is a fixed path list, not derived from Get-PineGuardScope. Order here is just the order
# -All commits them in (broadest/most-global first, then the registry scopes in registry order);
# it has no effect on correctness since every scope commits independently.
$metaScopePaths = [ordered]@{
    Solution = @(
        'PineGuard.slnx',
        'PineGuard.sln.DotSettings',
        'Directory.Packages.props',
        '.editorconfig',
        '.gitignore',
        'NuGet.config',
        'LICENSE'
    )
    Tools = @(
        'tools'
    )
    Agent = @(
        '.agent',
        '.claude',
        '.pi',
        '.cursor/rules',
        '.cursorrules',
        '.windsurf',
        '.windsurfrules',
        '.clinerules',
        '.junie',
        '.amazonq',
        '.github/agents',
        '.github/instructions',
        '.github/prompts',
        '.github/skills',
        '.github/copilot-instructions.md',
        '.vscode/settings.json',
        '.vscode/tasks.json',
        'AGENTS.md',
        'CLAUDE.md',
        'GEMINI.md',
        'src/PineGuard.Core/AGENTS.md',
        'src/PineGuard.MustClauses/AGENTS.md',
        'src/PineGuard.GuardClauses/AGENTS.md',
        'src/PineGuard.FluentValidation/AGENTS.md',
        'src/PineGuard.DataAnnotations/AGENTS.md',
        'src/PineGuard.Extensions.Options/AGENTS.md',
        'src/PineGuard.Extensions.DependencyInjection/AGENTS.md',
        'src/PineGuard.AspNetCore/AGENTS.md',
        'src/PineGuard.ErrorOr/AGENTS.md',
        'src/PineGuard.FluentResults/AGENTS.md',
        'src/PineGuard.OneOf/AGENTS.md',
        'src/PineGuard.Analyzers/AGENTS.md',
        'src/PineGuard.MediatR/AGENTS.md',
        'tests/AGENTS.md',
        'tests/PineGuard.Testing/AGENTS.md',
        'tools/AGENTS.md',
        'tools/code-diagnostics/AGENTS.md',
        'tools/code-scan/sonarqube/AGENTS.md'
    )
    Docs = @(
        'docs',
        'README.md'
    )
    Ci = @(
        '.github/workflows'
    )
}

if ($All.IsPresent) {
    $Scope = @($metaScopePaths.Keys) + @((Get-PineGuardScope -All).Name)
    $IncludeTests = $true
}

if (-not $Scope -or $Scope.Count -eq 0) {
    throw 'No scopes selected. Use -All, or -Scope <Name[,Name...]> (e.g. -Scope Core,Tools).'
}

$effectiveIncludeTests = $IncludeTests.IsPresent -or $All.IsPresent

function Invoke-ScopedCommit {
    <#
    .SYNOPSIS
        Resolves one scope name (registry or meta) to its stage paths and commits it in-process.
    #>
    param([Parameter(Mandatory = $true)][string]$ScopeName)

    if ($metaScopePaths.Contains($ScopeName)) {
        $stagePaths = @($metaScopePaths[$ScopeName])
    }
    else {
        $entry = Get-PineGuardScope -Name $ScopeName

        # A scope's SourceCsprojs is an array so a multi-project scope (e.g. Analyzers, which
        # ships PineGuard.Analyzers and the sibling PineGuard.Analyzers.CodeFixes as one NuGet
        # package) stages every project directory it owns, not just SourceDir's single directory
        # — the same reasoning the registry itself documents for why SourceCsprojs is plural.
        $dirs = [System.Collections.Generic.List[string]]::new()
        foreach ($csproj in $entry.SourceCsprojs) {
            $dir = (Split-Path -Parent $csproj) -replace '\\', '/'
            if (-not $dirs.Contains($dir)) {
                $dirs.Add($dir)
            }
        }

        if ($effectiveIncludeTests) {
            $testDir = (Split-Path -Parent $entry.TestCsproj) -replace '\\', '/'
            if (-not $dirs.Contains($testDir)) {
                $dirs.Add($testDir)
            }
        }

        $stagePaths = $dirs.ToArray()
    }

    $title = "${ScopeName}: updates"

    Invoke-Commit -RepoRoot $repoRoot -Title $title -StagePaths $stagePaths `
        -WhatIf:$WhatIf.IsPresent -AutoMessage:$AutoMessage.IsPresent -Message $Message
}

if ($Rebase.IsPresent -and -not $WhatIf.IsPresent) {
    Invoke-AutoRebaseIfNeeded -RepoRoot $repoRoot -Remote $Remote
}

foreach ($scopeName in $Scope) {
    Invoke-ScopedCommit -ScopeName $scopeName
}

if ($Rebase.IsPresent -and -not $WhatIf.IsPresent) {
    Invoke-AutoRebaseIfNeeded -RepoRoot $repoRoot -Remote $Remote
}

if ($Push.IsPresent -and -not $WhatIf.IsPresent) {
    if ($Rebase.IsPresent) {
        Invoke-SafePush -RepoRoot $repoRoot -Remote $Remote
    }
    else {
        Invoke-Push -RepoRoot $repoRoot -Remote $Remote
    }
}
