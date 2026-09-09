<#
.SYNOPSIS
    Shared Git helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Git staging, commit, push, and rebase helpers
    into the calling script's scope.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Assert-GitAvailable {
    <#
    .SYNOPSIS
        Validates git is on PATH.
    #>
    if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
        throw 'git is not available on PATH.'
    }
}

function Invoke-Git {
    <#
    .SYNOPSIS
        Executes a git command; returns PSCustomObject with Output and ExitCode.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string[]]$GitArgs
    )

    Assert-GitAvailable
    $output = & git -C $RepoRoot @GitArgs
    $exitCode = $LASTEXITCODE
    return [pscustomobject]@{
        Output = $output
        ExitCode = $exitCode
    }
}

function Get-StagedFiles {
    <#
    .SYNOPSIS
        Gets staged file names via git diff --cached --name-only.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('diff', '--cached', '--name-only')
    if ($r.ExitCode -ne 0) {
        throw 'Failed to check staged changes.'
    }

    $lines = @()
    if ($null -ne $r.Output) {
        $lines = @($r.Output)
    }

    $lines = @(
        $lines |
            Where-Object { $null -ne $_ } |
            ForEach-Object { $_.ToString().Trim() } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )

    Write-Output -NoEnumerate ([string[]]$lines)
}

function Assert-IndexClean {
    <#
    .SYNOPSIS
        Throws if the git index already has staged changes.

    .DESCRIPTION
        `git restore --staged .` is a Tier 0 NEVER command (docs/ai/specs/safety.md §2.1) because
        it can silently discard the user's own staged work. This function never unstages
        anything; it only reports the problem and stops, so the caller (a human, or an agent
        acting on human instruction) can decide what to do with the pre-existing staged changes.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $staged = Get-StagedFiles -RepoRoot $RepoRoot
    if ($staged.Count -eq 0) {
        return
    }

    throw (("The git index already has {0} staged file(s): {1}. Commit or unstage them yourself " +
        "before running this script; it will not unstage changes for you.") -f $staged.Count, ($staged -join ', '))
}

function Get-StatusPorcelain {
    <#
    .SYNOPSIS
        Gets git status with optional path filtering.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $false)][string[]]$Paths
    )

    $statusArgs = @('status', '--porcelain=v1')
    if ($Paths -and $Paths.Count -gt 0) {
        $statusArgs += '--'
        $statusArgs += $Paths
    }

    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs $statusArgs
    if ($r.ExitCode -ne 0) {
        throw 'Failed to read git status.'
    }

    $lines = @()
    if ($null -ne $r.Output) {
        $lines = @($r.Output)
    }

    $lines = @(
        $lines |
            Where-Object { $null -ne $_ } |
            ForEach-Object { $_.ToString().TrimEnd() } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )

    Write-Output -NoEnumerate ([string[]]$lines)
}

function Add-Paths {
    <#
    .SYNOPSIS
        Stages paths via git add --.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string[]]$Paths
    )

    $addArgs = @('add', '--') + $Paths
    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs $addArgs
    if ($r.ExitCode -ne 0) {
        throw 'git add failed.'
    }
}

function Get-StagedNameStatus {
    <#
    .SYNOPSIS
        Gets staged file status (M/A/D) via git diff --cached --name-status.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('diff', '--cached', '--name-status')
    if ($r.ExitCode -ne 0) {
        throw 'Failed to compute staged diff.'
    }

    $lines = @()
    if ($null -ne $r.Output) {
        $lines = @($r.Output)
    }

    $lines = @(
        $lines |
            Where-Object { $null -ne $_ } |
            ForEach-Object { $_.ToString().TrimEnd() } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )

    Write-Output -NoEnumerate ([string[]]$lines)
}

function Get-StagedNumStat {
    <#
    .SYNOPSIS
        Gets staged line counts via git diff --cached --numstat.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('diff', '--cached', '--numstat')
    if ($r.ExitCode -ne 0) {
        throw 'Failed to compute staged numstat.'
    }

    $lines = @()
    if ($null -ne $r.Output) {
        $lines = @($r.Output)
    }

    $lines = @(
        $lines |
            Where-Object { $null -ne $_ } |
            ForEach-Object { $_.ToString().TrimEnd() } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )

    Write-Output -NoEnumerate ([string[]]$lines)
}

function Get-CommitTitleSuggestion {
    <#
    .SYNOPSIS
        Refines a scope's generic "updates" suffix into a more specific phrase, using
        path-fragment heuristics over the staged files.

    .DESCRIPTION
        Returns a short phrase (e.g. "rules updates", "git automation scripts") describing what
        changed, for use as the summary half of the conventional-commit subject line that
        New-AutoCommitMessage builds. Falls back to the scope's own suffix (usually "updates")
        when nothing more specific matches.

        Pre-T3.02 this function returned a full "Prefix: Suffix" title string; it now returns
        only the suffix phrase, because the subject line's "<type>(<scope>):" prefix is built
        separately (F-33).
    #>
    param(
        [Parameter(Mandatory = $true)][string]$DefaultTitle,
        [Parameter(Mandatory = $true)][string[]]$NameStatusLines
    )

    $suffix = 'updates'
    if ($DefaultTitle -match '^([^:]+):\s*(.+)$') {
        $suffix = $Matches[2].Trim()
    }

    $paths = @(
        $NameStatusLines |
            ForEach-Object { ($_ -split "\t")[-1].Trim() } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )

    $has = {
        param([string]$fragment)
        return (@($paths | Where-Object { $_ -like "*$fragment*" })).Count -gt 0
    }

    if (& $has 'tools/git/') { return 'git automation scripts' }
    if (& $has 'tools/audit-cli/') { return 'audit-cli orchestration' }
    if (& $has 'tools/code-coverage/') { return 'coverage tooling updates' }
    if (& $has 'docs/') { return 'documentation updates' }
    if (& $has 'src/PineGuard.Core/Rules/') { return 'rules updates' }
    if (& $has 'src/PineGuard.Core/Utils/') { return 'utils updates' }
    if (& $has 'tests/') { return 'tests updates' }

    return $suffix
}

function Get-CommitTypeSuggestion {
    <#
    .SYNOPSIS
        Infers a conventional-commits type (docs/test/chore) from the staged file paths.

    .DESCRIPTION
        A mechanical generator cannot know true intent (a behaviour change vs. a fix vs. a
        refactor), so this only distinguishes what it CAN tell from paths alone: 'docs' when
        every changed path is doc-like (under docs/, or any *.md file), 'test' when every changed
        path is under tests/, and 'chore' as the safe default for everything else (including any
        mix of the two, and all src/ or tools/ changes).
    #>
    param([Parameter(Mandatory = $true)][string[]]$Paths)

    if ($Paths.Count -eq 0) {
        return 'chore'
    }

    $nonDoc = @($Paths | Where-Object { $_ -notmatch '(^|/)docs/' -and $_ -notmatch '\.md$' })
    if ($nonDoc.Count -eq 0) {
        return 'docs'
    }

    $nonTest = @($Paths | Where-Object { $_ -notmatch '^tests/' })
    if ($nonTest.Count -eq 0) {
        return 'test'
    }

    return 'chore'
}

function ConvertTo-KebabCase {
    <#
    .SYNOPSIS
        Converts a PascalCase scope name (e.g. "MustClauses") to kebab-case ("must-clauses").

    .DESCRIPTION
        A plain word-boundary conversion: a hyphen is inserted wherever a lowercase letter or
        digit is followed by an uppercase letter, then the whole string is lowercased. This is
        mechanical, not a lookup against the hand-curated Qodana slug table (F-13), so a name
        with a trailing single capital that isn't a true word boundary (e.g. "MediatR") splits
        as "mediat-r" — an accepted quirk of a generic heuristic, not a bug to chase here.
    #>
    param([Parameter(Mandatory = $true)][string]$Value)

    $withHyphens = [regex]::Replace($Value, '(?<=[a-z0-9])(?=[A-Z])', '-')
    return $withHyphens.ToLowerInvariant()
}

function New-AutoCommitMessage {
    <#
    .SYNOPSIS
        Generates a conventional-commits-style message: a "<type>(<scope>): <summary>" subject
        line, a blank line, then one flowing prose paragraph describing what changed (F-33).

    .DESCRIPTION
        This is a BEST-EFFORT MECHANICAL summary standing in for a human-written -Message. It can
        describe *what* changed — which files, how many, how many lines — because that is all
        `git diff --cached` can tell it. It has no notion of *why* the change was made. The
        owner's standing convention (a conventional-commits subject followed by a flowing prose
        body; see project memory feedback_commit-messages.md) is achievable here only for the
        "what" half of that convention. Prefer -Message over -AutoMessage whenever the change is
        meaningful enough to be worth explaining — this generator exists for the mechanical,
        low-stakes commits where writing a message by hand would not teach a reader anything a
        stat line can't already show.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$DefaultTitle,
        [Parameter(Mandatory = $true)][string[]]$StagePaths
    )

    $nameStatus = Get-StagedNameStatus -RepoRoot $RepoRoot
    $numStat = Get-StagedNumStat -RepoRoot $RepoRoot

    $fileCount = $nameStatus.Count
    $added = 0
    $deleted = 0
    $modified = 0
    foreach ($l in $nameStatus) {
        $code = ($l -split "\s+")[0]
        if ($code -eq 'A') { $added++ }
        elseif ($code -eq 'D') { $deleted++ }
        else { $modified++ }
    }

    $ins = 0
    $del = 0
    foreach ($l in $numStat) {
        $parts = $l -split "\t"
        if ($parts.Length -lt 2) { continue }
        if ($parts[0] -match '^\d+$') { $ins += [int]$parts[0] }
        if ($parts[1] -match '^\d+$') { $del += [int]$parts[1] }
    }

    $changedPaths = @(
        $nameStatus |
            ForEach-Object { ($_ -split "\t")[-1].Trim() } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )

    $scopeName = $DefaultTitle
    if ($DefaultTitle -match '^([^:]+):') {
        $scopeName = $Matches[1].Trim()
    }

    $type = Get-CommitTypeSuggestion -Paths $changedPaths
    $scopeSlug = ConvertTo-KebabCase -Value $scopeName
    $summary = Get-CommitTitleSuggestion -DefaultTitle $DefaultTitle -NameStatusLines $nameStatus

    $subject = '{0}({1}): {2}' -f $type, $scopeSlug, $summary

    $fileWord = if ($fileCount -eq 1) { 'file' } else { 'files' }
    $scopeText = ($StagePaths -join ', ')
    $body = 'Updates {0} {1} under {2} ({3} modified, {4} added, {5} deleted; +{6}/-{7} lines).' -f `
        $fileCount, $fileWord, $scopeText, $modified, $added, $deleted, $ins, $del

    return ($subject, '', $body) -join [Environment]::NewLine
}

function New-CommitTemplateFile {
    <#
    .SYNOPSIS
        Creates a timestamped commit template file in artifacts/git/commit-templates.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$Title,
        [Parameter(Mandatory = $true)][string[]]$StagePaths,
        [Parameter(Mandatory = $false)][string]$ExtraNotes = ''
    )

    $outDir = Join-Path $RepoRoot 'artifacts/git/commit-templates'
    New-Item -ItemType Directory -Force -Path $outDir | Out-Null

    $stamp = (Get-Date).ToString('yyyyMMdd-HHmmss')
    $path = Join-Path $outDir ("commit-template-{0}.txt" -f $stamp)

    $changes = Get-StagedNameStatus -RepoRoot $RepoRoot
    $changesText = if ($changes.Count -gt 0) { ($changes -join [Environment]::NewLine) } else { '(no staged changes?)' }

    $stagePathText = ($StagePaths -join ', ')

    $template = @()
    $template += "# {0}" -f $Title
    $template += ''
    $template += '# Summary'
    $template += '# - Explain what changed (multi-line).'
    $template += '# - Avoid one-liners; include rationale and scope.'
    $template += ''
    $template += '# Why'
    $template += '# - Why this change is needed.'
    $template += ''
    $template += '# Scope'
    $template += ("# - Staged paths: {0}" -f $stagePathText)
    $template += ''
    $template += '# Changes (staged)'
    $template += $changesText.Split([Environment]::NewLine) | ForEach-Object { "#   " + $_ }

    if (-not [string]::IsNullOrWhiteSpace($ExtraNotes)) {
        $template += ''
        $template += '# Notes'
        $template += $ExtraNotes.Split([Environment]::NewLine) | ForEach-Object { "#   " + $_ }
    }

    $template | Out-File -LiteralPath $path -Encoding utf8
    return $path
}

function Invoke-Commit {
    <#
    .SYNOPSIS
        Stages paths, generates message/template, invokes git commit.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$Title,
        [Parameter(Mandatory = $true)][string[]]$StagePaths,
        [switch]$WhatIf,
        [switch]$AutoMessage,
        [string]$Message,
        [string]$ExtraNotes = ''
    )

    if (-not $WhatIf.IsPresent) {
        Assert-IndexClean -RepoRoot $RepoRoot
    }

    $statusBefore = Get-StatusPorcelain -RepoRoot $RepoRoot -Paths $StagePaths
    if ($statusBefore.Count -eq 0) {
        Write-Host ("No changes for: {0}" -f $Title) -ForegroundColor DarkGray
        return
    }

    if ($WhatIf.IsPresent) {
        $staged = @()
        try { $staged = Get-StagedFiles -RepoRoot $RepoRoot } catch { $staged = @() }
        if ($staged.Count -gt 0) {
            Write-Host "[WhatIf] Note: staged changes already exist; a real commit would fail here (commit or unstage them first)." -ForegroundColor Yellow
        }
        Write-Host ("[WhatIf] Would stage: {0}" -f ($StagePaths -join ', ')) -ForegroundColor Yellow
        Write-Host ("[WhatIf] Would commit: {0}" -f $Title) -ForegroundColor Yellow
        return
    }

    Add-Paths -RepoRoot $RepoRoot -Paths $StagePaths

    $staged = Get-StagedNameStatus -RepoRoot $RepoRoot
    if ($staged.Count -eq 0) {
        Write-Host ("Nothing staged for: {0}" -f $Title) -ForegroundColor DarkGray
        return
    }

    if ($AutoMessage.IsPresent -and [string]::IsNullOrWhiteSpace($Message)) {
        $Message = New-AutoCommitMessage -RepoRoot $RepoRoot -DefaultTitle $Title -StagePaths $StagePaths
    }

    if (-not [string]::IsNullOrWhiteSpace($Message)) {
        $paragraphs = @(
            $Message -split "(\r?\n){2,}" |
                ForEach-Object { $_.Trim() } |
                Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
        )

        $commitArgs = @('commit')
        foreach ($p in $paragraphs) {
            $commitArgs += @('-m', $p)
        }

        $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs $commitArgs
        if ($r.ExitCode -ne 0) {
            throw 'git commit failed.'
        }

        return
    }

    $templateFile = New-CommitTemplateFile -RepoRoot $RepoRoot -Title $Title -StagePaths $StagePaths -ExtraNotes $ExtraNotes
    $r2 = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('commit', '--template', $templateFile)
    if ($r2.ExitCode -ne 0) {
        throw 'git commit failed.'
    }
}

function Get-CurrentBranch {
    <#
    .SYNOPSIS
        Returns current branch name.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('rev-parse', '--abbrev-ref', 'HEAD')
    if ($r.ExitCode -ne 0) {
        throw 'Failed to get current branch.'
    }

    return ($r.Output | Select-Object -First 1).Trim()
}

function Get-UpstreamRef {
    <#
    .SYNOPSIS
        Returns upstream ref or $null.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('rev-parse', '--abbrev-ref', '--symbolic-full-name', '@{u}')
    if ($r.ExitCode -ne 0) {
        return $null
    }

    $up = ($r.Output | Select-Object -First 1)
    if ([string]::IsNullOrWhiteSpace($up)) { return $null }
    return $up.Trim()
}

function Get-AheadBehind {
    <#
    .SYNOPSIS
        Computes commits ahead/behind upstream.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$Upstream
    )

    $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('rev-list', '--left-right', '--count', ("HEAD...{0}" -f $Upstream))
    if ($r.ExitCode -ne 0) {
        throw 'Failed to compute ahead/behind.'
    }

    $line = ($r.Output | Select-Object -First 1)
    $parts = $line -split '\s+'
    if ($parts.Length -lt 2) {
        throw 'Unexpected ahead/behind output.'
    }

    return [pscustomobject]@{
        Ahead = [int]$parts[0]
        Behind = [int]$parts[1]
    }
}

function Invoke-AutoRebaseIfNeeded {
    <#
    .SYNOPSIS
        Fetches, checks if behind, runs git pull --rebase --autostash.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [string]$Remote = 'origin'
    )

    $fetch = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('fetch', $Remote)
    if ($fetch.ExitCode -ne 0) {
        throw 'git fetch failed.'
    }

    $upstream = Get-UpstreamRef -RepoRoot $RepoRoot
    if ($null -eq $upstream) {
        Write-Host 'No upstream branch is configured; skipping rebase check.' -ForegroundColor DarkGray
        return
    }

    $ab = Get-AheadBehind -RepoRoot $RepoRoot -Upstream $upstream
    if ($ab.Behind -le 0) {
        Write-Host 'Remote is not ahead; no rebase needed.' -ForegroundColor DarkGray
        return
    }

    Write-Host ("Remote is ahead by {0} commit(s); rebasing..." -f $ab.Behind) -ForegroundColor Cyan

    $pull = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('pull', '--rebase', '--autostash')
    if ($pull.ExitCode -ne 0) {
        Write-Host ''
        Write-Host 'Auto rebase failed (likely conflicts).' -ForegroundColor Red
        Write-Host 'Resolve conflicts, then run:' -ForegroundColor Red
        Write-Host '  git rebase --continue' -ForegroundColor Red
        Write-Host 'Or abort:' -ForegroundColor Red
        Write-Host '  git rebase --abort' -ForegroundColor Red
        throw 'Auto rebase failed.'
    }
}

function Invoke-Push {
    <#
    .SYNOPSIS
        Fetches, checks ahead/behind, runs git push or git push -u.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [string]$Remote = 'origin'
    )

    $fetch = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('fetch', $Remote)
    if ($fetch.ExitCode -ne 0) {
        throw 'git fetch failed.'
    }

    $branch = Get-CurrentBranch -RepoRoot $RepoRoot
    $upstream = Get-UpstreamRef -RepoRoot $RepoRoot

    if ($null -eq $upstream) {
        $r = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('push', '-u', $Remote, $branch)
        if ($r.ExitCode -ne 0) {
            throw 'git push failed.'
        }
        return
    }

    $ab = Get-AheadBehind -RepoRoot $RepoRoot -Upstream $upstream
    if ($ab.Behind -gt 0) {
        throw ("Local branch is behind upstream by {0} commit(s). Run with -AutoRebase / -SafePush first." -f $ab.Behind)
    }

    if ($ab.Ahead -le 0) {
        Write-Host 'Nothing to push (local is not ahead of upstream).' -ForegroundColor DarkGray
        return
    }

    $r2 = Invoke-Git -RepoRoot $RepoRoot -GitArgs @('push', $Remote, $branch)
    if ($r2.ExitCode -ne 0) {
        throw 'git push failed.'
    }
}

function Invoke-SafePush {
    <#
    .SYNOPSIS
        Calls AutoRebase then Push.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [string]$Remote = 'origin'
    )

    $upstream = Get-UpstreamRef -RepoRoot $RepoRoot
    if ($null -eq $upstream) {
        Write-Host 'No upstream configured; pushing with -u.' -ForegroundColor Cyan
        Invoke-Push -RepoRoot $RepoRoot -Remote $Remote
        return
    }

    Invoke-AutoRebaseIfNeeded -RepoRoot $RepoRoot -Remote $Remote
    Invoke-Push -RepoRoot $RepoRoot -Remote $Remote
}
