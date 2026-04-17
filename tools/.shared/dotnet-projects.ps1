<#
.SYNOPSIS
    Shared .NET project discovery helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Test-ProjectHasSources and Get-TestProjects
    into the calling script's scope.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Test-ProjectHasSources {
    <#
    .SYNOPSIS
        Returns $true if the project directory contains *.cs files (excludes bin/obj).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $ProjectPath
    )

    $projectDir = Split-Path -Parent $ProjectPath

    $csFiles = Get-ChildItem -Path $projectDir -Recurse -File -Filter '*.cs' -ErrorAction SilentlyContinue |
    Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }

    return ($null -ne $csFiles -and $csFiles.Count -gt 0)
}

function Get-TestProjects {
    <#
    .SYNOPSIS
        Finds test projects matching a filter; supports multi-filter; excludes empty projects.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot,

        [string] $TestsDirectory = (Join-Path $RepoRoot 'tests'),

        [string] $ProjectFilter = '*.UnitTests.csproj',

        [switch] $IncludeEmpty
    )

    # Support multiple glob filters delimited by ';' or ','
    $filters = @(
        ($ProjectFilter -split '[;,]') |
        ForEach-Object { $_.Trim() } |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    )

    $projects = foreach ($filter in $filters) {
        Get-ChildItem -Path $TestsDirectory -Recurse -File -Filter $filter -ErrorAction SilentlyContinue |
        Select-Object -ExpandProperty FullName
    }

    $projects = @(
        $projects |
        Sort-Object -Unique
    )

    $projects = @($projects)

    if (-not $projects -or $projects.Count -eq 0) {
        throw "No test projects found under '$TestsDirectory' matching '$ProjectFilter'."
    }

    if ($IncludeEmpty) {
        return @($projects)
    }

    $runnable = foreach ($project in $projects) {
        if (Test-ProjectHasSources -ProjectPath $project) {
            $project
            continue
        }

        $name = [IO.Path]::GetFileNameWithoutExtension($project)
        Write-Host "Skipping empty test project (no *.cs): $name" -ForegroundColor Yellow
    }

    $runnable = @($runnable)
    if (-not $runnable -or $runnable.Count -eq 0) {
        throw "No runnable test projects found under '$TestsDirectory' matching '$ProjectFilter'."
    }

    return $runnable
}
