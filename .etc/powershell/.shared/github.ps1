<#
.SYNOPSIS
    github

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/.shared/github.ps1

# -------------------------------------------------------------------------------------------------
# GitHub Secrets Helpers (requires gh CLI)
# -------------------------------------------------------------------------------------------------

function Set-GitHubRepositorySecret {
    <#
    .SYNOPSIS
        Sets a repository-level secret via gh CLI.
    #>
    param(
        [Parameter(Mandatory)]
        [string] $Owner,

        [Parameter(Mandatory)]
        [string] $Repository,

        [Parameter(Mandatory)]
        [string] $Name,

        [Parameter(Mandatory)]
        [string] $Value
    )

    $repo = "$Owner/$Repository"
    $Value | gh secret set $Name --repo $repo
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to set repository secret '$Name' on $repo."
    }
}

function Set-GitHubEnvironmentSecret {
    <#
    .SYNOPSIS
        Sets an environment-level secret via gh CLI.
    #>
    param(
        [Parameter(Mandatory)]
        [string] $Owner,

        [Parameter(Mandatory)]
        [string] $Repository,

        [Parameter(Mandatory)]
        [string] $EnvironmentName,

        [Parameter(Mandatory)]
        [string] $Name,

        [Parameter(Mandatory)]
        [string] $Value
    )

    $repo = "$Owner/$Repository"
    $Value | gh secret set $Name --repo $repo --env $EnvironmentName
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to set environment secret '$Name' on $repo (env: $EnvironmentName)."
    }
}

# -------------------------------------------------------------------------------------------------
# GitHub Variables Helpers (requires gh CLI)
# -------------------------------------------------------------------------------------------------

function Set-GitHubRepositoryVariable {
    <#
    .SYNOPSIS
        Sets a repository-level Actions variable via gh CLI.
    #>
    param(
        [Parameter(Mandatory)]
        [string] $Owner,

        [Parameter(Mandatory)]
        [string] $Repository,

        [Parameter(Mandatory)]
        [string] $Name,

        [Parameter(Mandatory)]
        [string] $Value
    )

    $repo = "$Owner/$Repository"
    $Value | gh variable set $Name --repo $repo
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to set repository variable '$Name' on $repo."
    }
}

function Set-GitHubEnvironmentVariable {
    <#
    .SYNOPSIS
        Sets an environment-level Actions variable via gh CLI.
    #>
    param(
        [Parameter(Mandatory)]
        [string] $Owner,

        [Parameter(Mandatory)]
        [string] $Repository,

        [Parameter(Mandatory)]
        [string] $EnvironmentName,

        [Parameter(Mandatory)]
        [string] $Name,

        [Parameter(Mandatory)]
        [string] $Value
    )

    $repo = "$Owner/$Repository"
    $Value | gh variable set $Name --repo $repo --env $EnvironmentName
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to set environment variable '$Name' on $repo (env: $EnvironmentName)."
    }
}
