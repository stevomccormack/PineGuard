<#
.SYNOPSIS
    Initialise this repository's local git history and push it to its GitHub remote.

.DESCRIPTION
    A one-time bootstrap for a fresh working copy that has files but no .git directory. It runs
    `git init` on $Project.MainBranch, makes the initial commit, wires up origin, and pushes.

    This is the one place in the repository where `git add .` is legitimate: there is no history
    to stage against, so there are no scopes to name. Every other commit path goes through
    tools/git/Run-Commits.ps1, which stages declared scopes by name.

.NOTES
    Three bugs are fixed relative to the previous version:

      - The remote guard was inverted. It aborted when the GitHub repository EXISTED, but the
        script's only push target is that repository, so it could never reach the push it was
        written to perform. The guard now requires the remote to exist, and -CreateRemote will
        create it via the gh CLI.
      - `git branch --set-upstream-to=origin/<main>` ran BEFORE the first push, when
        origin/<main> did not yet exist, so it always failed. `git push -u` sets the upstream.
      - The commit message was a hard-coded changelog for a state the repository left long ago
        ("Created .gitiignore"). It is now a parameter with a plain default.

.PARAMETER CommitMessage
    Message for the initial commit. Default: 'chore: initial commit'.

.PARAMETER CreateRemote
    Create the GitHub repository via the gh CLI when it does not already exist, instead of
    failing. Requires gh to be installed and authenticated.

.PARAMETER Private
    Create the remote repository as private. Only meaningful with -CreateRemote.

.PARAMETER WhatIf
    Print the plan without initialising, committing, or pushing anything.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Initialize-GitRepository.ps1 -WhatIf

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Initialize-GitRepository.ps1 -CreateRemote -Private
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [ValidateNotNullOrEmpty()]
    [string] $CommitMessage = 'chore: initial commit',

    [switch] $CreateRemote,

    [switch] $Private
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: Git Repository Initialisation"
Write-Var -Name 'Project Name' -Value $Project.Name -NoIcon
Write-Var -Name 'Project Path' -Value $Project.LocalPath -NoIcon
Write-Var -Name 'Remote' -Value $Project.GitUrl -NoIcon
Write-Var -Name 'Branch' -Value $Project.MainBranch -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------
# Guards

if (-not (Test-Command -Name 'git')) {
    Write-FailMessage -Title 'Git' -Message "'git' was not found on PATH."
    exit 1
}

if (Test-GitRepositoryIsInitialized -Path $Project.LocalPath) {
    Write-FailMessage -Title 'Git Init' -Message "Already a git repository: $($Project.LocalPath). Nothing to initialise."
    exit 1
}

$remoteExists = Test-GitRepositoryExists -RepositoryUrl $Project.GitUrl

if (-not $remoteExists -and -not $CreateRemote) {
    Write-FailMessage `
        -Title 'Git Init' `
        -Message "Remote $($Project.GitUrl) does not exist, and it is this script's push target. Pass -CreateRemote to create it via the gh CLI, or create it on GitHub first."
    exit 1
}

if ($CreateRemote -and $remoteExists) {
    Write-Status "-CreateRemote ignored: $($Project.GitUrl) already exists."
}

if ($CreateRemote -and -not $remoteExists -and -not (Test-Command -Name 'gh')) {
    Write-FailMessage -Title 'GitHub CLI' -Message "-CreateRemote needs 'gh' on PATH. Install via: winget install GitHub.cli"
    exit 1
}

# -------------------------------------------------------------------------------------------------

if (-not $PSCmdlet.ShouldProcess($Project.LocalPath, 'Initialise git repository and push')) {
    Write-Status 'WhatIf: would run git init, stage every file, commit, add origin, and push.'
    exit 0
}

Write-Status 'Initialising and configuring the git repository:'
Write-NewLine

function Invoke-Git {
    <#
    .SYNOPSIS
        Runs git against the project path and throws, with git's own output, on failure.
    #>
    param([Parameter(Mandatory, ValueFromRemainingArguments)] [string[]] $Arguments)

    $output = git -C $Project.LocalPath @Arguments 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw ("git {0} failed: {1}" -f ($Arguments -join ' '), ($output -join [Environment]::NewLine))
    }
    return $output
}

Invoke-Git init --initial-branch=$($Project.MainBranch) | Out-Null

# The one legitimate `git add .` in this repository - see .DESCRIPTION.
Invoke-Git add . | Out-Null
Invoke-Git commit -m $CommitMessage | Out-Null

if (-not $remoteExists) {
    $visibility = if ($Private) { '--private' } else { '--public' }
    Write-Status "Creating remote repository ($visibility)..."

    $created = gh repo create "$($Project.Owner)/$($Project.Repository)" $visibility --description $Project.Description 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-FailMessage -Title 'gh repo create' -Message ($created -join [Environment]::NewLine)
        exit 1
    }
}

Invoke-Git remote add origin $Project.GitUrl | Out-Null

# `git push -u` records the upstream itself; setting it beforehand cannot work, because
# origin/<branch> does not exist until this push creates it.
Invoke-Git push -u origin $Project.MainBranch | Out-Null

# -------------------------------------------------------------------------------------------------

Write-OkMessage -Title 'Git Init' -Message "Pushed $($Project.MainBranch) to $($Project.WebUrl)"
