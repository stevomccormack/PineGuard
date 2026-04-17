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

function Resolve-RepoRoot {
    <#
    .SYNOPSIS
        Uses git rev-parse --show-toplevel to find the repo root.
    #>
    param(
        [string]$StartPath = (Get-Location).Path
    )

    Assert-GitAvailable

    $root = & git -C $StartPath rev-parse --show-toplevel 2>$null
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($root)) {
        throw 'Not inside a git repository.'
    }

    return $root.Trim()
}

function Invoke-Git {
    <#
    .SYNOPSIS
        Executes a git command; returns PSCustomObject with Output and ExitCode.
    #>
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string[]]$Args
    )

    Assert-GitAvailable
    $output = & git -C $RepoRoot @Args
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

    $r = Invoke-Git -RepoRoot $RepoRoot -Args @('diff', '--cached', '--name-only')
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

function Unstage-AllChanges {
    <#
    .SYNOPSIS
        Runs git restore --staged . to unstage all changes.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $r = Invoke-Git -RepoRoot $RepoRoot -Args @('restore', '--staged', '.')
    if ($r.ExitCode -ne 0) {
        throw 'Failed to unstage changes.'
    }
}

function Ensure-IndexClean {
    <#
    .SYNOPSIS
        Unstages any existing staged changes before committing.
    #>
    param([Parameter(Mandatory = $true)][string]$RepoRoot)

    $staged = Get-StagedFiles -RepoRoot $RepoRoot
    if ($staged.Count -eq 0) {
        return
    }

    Write-Host "Detected existing staged changes; unstaging them to avoid mixing commits." -ForegroundColor Yellow
    Unstage-AllChanges -RepoRoot $RepoRoot
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

    $args = @('status', '--porcelain=v1')
    if ($Paths -and $Paths.Count -gt 0) {
        $args += '--'
        $args += $Paths
    }

    $r = Invoke-Git -RepoRoot $RepoRoot -Args $args
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

    $args = @('add', '--') + $Paths
    $r = Invoke-Git -RepoRoot $RepoRoot -Args $args
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

    $r = Invoke-Git -RepoRoot $RepoRoot -Args @('diff', '--cached', '--name-status')
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

    $r = Invoke-Git -RepoRoot $RepoRoot -Args @('diff', '--cached', '--numstat')
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
        Suggests commit title based on scope (tools/, src/, tests/, etc.).
    #>
    param(
        [Parameter(Mandatory = $true)][string]$DefaultTitle,
        [Parameter(Mandatory = $true)][string[]]$NameStatusLines
    )

    $prefix = $DefaultTitle
    $suffix = 'updates'

    if ($DefaultTitle -match '^([^:]+):\s*(.+)$') {
        $prefix = $Matches[1].Trim()
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

    if (& $has 'tools/git/') { return "${prefix}: git automation scripts" }
    if (& $has 'tools/audit-cli/') { return "${prefix}: audit-cli orchestration" }
    if (& $has 'tools/code-coverage/') { return "${prefix}: coverage tooling updates" }
    if (& $has 'docs/') { return "${prefix}: documentation updates" }
    if (& $has 'src/PineGuard.Core/Rules/') { return "${prefix}: rules updates" }
    if (& $has 'src/PineGuard.Core/Utils/') { return "${prefix}: utils updates" }
    if (& $has 'tests/') { return "${prefix}: tests updates" }

    return "${prefix}: $suffix"
}

function New-AutoCommitMessage {
    <#
    .SYNOPSIS
        Generates a multi-line commit message with stats and file list.
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

    $isSmall = ($fileCount -le 2) -and (($ins + $del) -le 40)
    $title = Get-CommitTitleSuggestion -DefaultTitle $DefaultTitle -NameStatusLines $nameStatus

    $scopeText = ($StagePaths -join ', ')
    $statsLine = "Files: $fileCount (M:$modified A:$added D:$deleted); Diff: +$ins/-$del"

    $lines = @()
    $lines += $title
    $lines += ''

    if ($isSmall) {
        $lines += "Summary: Small scoped update ($statsLine)."
        $lines += "Scope: $scopeText"
    }
    else {
        $lines += "Summary: Scoped update for $($DefaultTitle.Split(':')[0].Trim())."
        $lines += $statsLine
        $lines += "Scope: $scopeText"
        $lines += ''

        $lines += 'Changes:'
        $max = [Math]::Min(12, $nameStatus.Count)
        for ($i = 0; $i -lt $max; $i++) {
            $lines += "- $($nameStatus[$i])"
        }
        if ($nameStatus.Count -gt $max) {
            $lines += "- ...and $($nameStatus.Count - $max) more"
        }
    }

    return ($lines -join [Environment]::NewLine)
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
        Ensure-IndexClean -RepoRoot $RepoRoot
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
            Write-Host "[WhatIf] Note: staged changes already exist; real commits would unstage them first." -ForegroundColor Yellow
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

        $args = @('commit')
        foreach ($p in $paragraphs) {
            $args += @('-m', $p)
        }

        $r = Invoke-Git -RepoRoot $RepoRoot -Args $args
        if ($r.ExitCode -ne 0) {
            throw 'git commit failed.'
        }

        return
    }

    $templateFile = New-CommitTemplateFile -RepoRoot $RepoRoot -Title $Title -StagePaths $StagePaths -ExtraNotes $ExtraNotes
    $r2 = Invoke-Git -RepoRoot $RepoRoot -Args @('commit', '--template', $templateFile)
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

    $r = Invoke-Git -RepoRoot $RepoRoot -Args @('rev-parse', '--abbrev-ref', 'HEAD')
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

    $r = Invoke-Git -RepoRoot $RepoRoot -Args @('rev-parse', '--abbrev-ref', '--symbolic-full-name', '@{u}')
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

    $r = Invoke-Git -RepoRoot $RepoRoot -Args @('rev-list', '--left-right', '--count', ("HEAD...{0}" -f $Upstream))
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

    $fetch = Invoke-Git -RepoRoot $RepoRoot -Args @('fetch', $Remote)
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

    $pull = Invoke-Git -RepoRoot $RepoRoot -Args @('pull', '--rebase', '--autostash')
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

    $fetch = Invoke-Git -RepoRoot $RepoRoot -Args @('fetch', $Remote)
    if ($fetch.ExitCode -ne 0) {
        throw 'git fetch failed.'
    }

    $branch = Get-CurrentBranch -RepoRoot $RepoRoot
    $upstream = Get-UpstreamRef -RepoRoot $RepoRoot

    if ($null -eq $upstream) {
        $r = Invoke-Git -RepoRoot $RepoRoot -Args @('push', '-u', $Remote, $branch)
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

    $r2 = Invoke-Git -RepoRoot $RepoRoot -Args @('push', $Remote, $branch)
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
