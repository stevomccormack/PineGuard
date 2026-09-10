<#
.SYNOPSIS
    GitHub Actions secret and variable helpers, backed by the gh CLI.

.DESCRIPTION
    Dot-sourced by index.ps1. Four thin wrappers over `gh secret set` / `gh variable set`, at
    repository and environment level.

    Values are piped to gh on stdin rather than passed as --body, so a secret never appears in
    the process command line where another user on the machine could read it from the process
    table. gh's own output is captured and included in the thrown message when the call fails -
    the previous version discarded it, so a permissions failure surfaced only as
    "Failed to set repository secret 'X'" with no reason attached.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# Internals
# -------------------------------------------------------------------------------------------------

function Invoke-GitHubValueWrite {
    <#
    .SYNOPSIS
        Pipes a value into a gh subcommand and throws, with gh's own output, on failure.

    .PARAMETER Kind
        'secret' or 'variable' - the gh subcommand group to write through.

    .PARAMETER Description
        Human-readable subject for the thrown message, e.g. "repository secret 'NUGET_TOKEN'".
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [ValidateSet('secret', 'variable')] [string] $Kind,
        [Parameter(Mandatory)] [string] $Name,
        [Parameter(Mandatory)] [AllowEmptyString()] [string] $Value,
        [Parameter(Mandatory)] [string[]] $GhArguments,
        [Parameter(Mandatory)] [string] $Description
    )

    $output = $Value | gh $Kind set $Name @GhArguments 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw ("Failed to set {0}: {1}" -f $Description, ($output -join [Environment]::NewLine))
    }
}

# -------------------------------------------------------------------------------------------------
# Secrets
# -------------------------------------------------------------------------------------------------

function Set-GitHubRepositorySecret {
    <#
    .SYNOPSIS
        Sets a repository-level Actions secret via the gh CLI.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string] $Owner,
        [Parameter(Mandatory)] [string] $Repository,
        [Parameter(Mandatory)] [string] $Name,
        [Parameter(Mandatory)] [string] $Value
    )

    $repo = "$Owner/$Repository"
    Invoke-GitHubValueWrite -Kind 'secret' -Name $Name -Value $Value `
        -GhArguments @('--repo', $repo) `
        -Description "repository secret '$Name' on $repo"
}

function Set-GitHubEnvironmentSecret {
    <#
    .SYNOPSIS
        Sets an environment-level Actions secret via the gh CLI.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string] $Owner,
        [Parameter(Mandatory)] [string] $Repository,
        [Parameter(Mandatory)] [string] $EnvironmentName,
        [Parameter(Mandatory)] [string] $Name,
        [Parameter(Mandatory)] [string] $Value
    )

    $repo = "$Owner/$Repository"
    Invoke-GitHubValueWrite -Kind 'secret' -Name $Name -Value $Value `
        -GhArguments @('--repo', $repo, '--env', $EnvironmentName) `
        -Description "environment secret '$Name' on $repo (env: $EnvironmentName)"
}

# -------------------------------------------------------------------------------------------------
# Variables
# -------------------------------------------------------------------------------------------------

function Set-GitHubRepositoryVariable {
    <#
    .SYNOPSIS
        Sets a repository-level Actions variable via the gh CLI.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string] $Owner,
        [Parameter(Mandatory)] [string] $Repository,
        [Parameter(Mandatory)] [string] $Name,
        [Parameter(Mandatory)] [string] $Value
    )

    $repo = "$Owner/$Repository"
    Invoke-GitHubValueWrite -Kind 'variable' -Name $Name -Value $Value `
        -GhArguments @('--repo', $repo) `
        -Description "repository variable '$Name' on $repo"
}

function Set-GitHubEnvironmentVariable {
    <#
    .SYNOPSIS
        Sets an environment-level Actions variable via the gh CLI.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string] $Owner,
        [Parameter(Mandatory)] [string] $Repository,
        [Parameter(Mandatory)] [string] $EnvironmentName,
        [Parameter(Mandatory)] [string] $Name,
        [Parameter(Mandatory)] [string] $Value
    )

    $repo = "$Owner/$Repository"
    Invoke-GitHubValueWrite -Kind 'variable' -Name $Name -Value $Value `
        -GhArguments @('--repo', $repo, '--env', $EnvironmentName) `
        -Description "environment variable '$Name' on $repo (env: $EnvironmentName)"
}
