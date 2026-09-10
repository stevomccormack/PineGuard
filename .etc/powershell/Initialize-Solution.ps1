<#
.SYNOPSIS
    Verify every project the registry declares exists, then restore, build and test the solution.

.DESCRIPTION
    The workstation bring-up check for a fresh clone. It walks the fourteen scopes in the project
    registry, confirms each scope's source and test projects are present on disk, and then runs
    restore, build and test over PineGuard.slnx.

    With -CreateMissing it scaffolds anything absent - `dotnet new classlib` for a source project,
    `dotnet new xunit` for a test project - which is only useful when adding a scope, since a
    normal clone has all of them.

.NOTES
    The previous version carried its own hand-written project list, which had drifted to five of
    the fourteen scopes and still pointed at tools/audit-cli/solution and the pre-rename
    tools/code-inspection/qodana folder. It now reads $Solution, projected from
    tools/.shared/dotnet-projects.ps1, so the registry is the only place a scope is declared.

    It also added NuGet packages per project with `dotnet add package`. That has been dropped:
    Directory.Packages.props sets ManagePackageVersionsCentrally, so a PackageReference carries no
    version of its own and a per-project package list is no longer a meaningful thing to declare.
    Project-to-project references are committed in the csproj files and are not scaffolded either.

.PARAMETER CreateMissing
    Scaffold any registry project that is not on disk, instead of only reporting it.

.PARAMETER SkipBuild
    Verify the projects, then stop before restore and build.

.PARAMETER SkipTests
    Restore and build, but do not run the test suite.

.PARAMETER Configuration
    Build configuration. Default: Debug.

.PARAMETER WhatIf
    Report what would be scaffolded, restored, built and tested, without running any of it.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Initialize-Solution.ps1 -SkipBuild
    Verifies the fourteen scopes are all present on disk.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Initialize-Solution.ps1

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Initialize-Solution.ps1 -CreateMissing -Configuration Release
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [switch] $CreateMissing,

    [switch] $SkipBuild,

    [switch] $SkipTests,

    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug'
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: Solution Bring-Up"
Write-Var -Name 'Solution' -Value $Solution.Path -NoIcon
Write-Var -Name 'Scopes' -Value $Solution.Scopes.Count -NoIcon
Write-Var -Name 'Target frameworks' -Value ($Solution.TargetFrameworks -join '; ') -NoIcon
Write-Var -Name 'Configuration' -Value $Configuration -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------

if (-not (Test-Command -Name 'dotnet')) {
    Write-FailMessage -Title 'dotnet' -Message "'dotnet' was not found on PATH. Install the .NET SDK from https://dot.net"
    exit 1
}

if (-not (Test-Path -LiteralPath $Solution.Path)) {
    Write-FailMessage -Title 'Solution' -Message "Not found: $($Solution.Path)"
    exit 1
}

# -------------------------------------------------------------------------------------------------
# Verify (and optionally scaffold) every registry project
# -------------------------------------------------------------------------------------------------

function Initialize-RegistryProject {
    <#
    .SYNOPSIS
        Reports whether one registry project exists, scaffolding it when -CreateMissing is set.

    .PARAMETER Template
        'classlib' for a source project, 'xunit' for a test project.
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)] [string] $ScopeName,
        [Parameter(Mandatory)] [string] $ProjectPath,
        [Parameter(Mandatory)] [ValidateSet('classlib', 'xunit')] [string] $Template
    )

    if (Test-Path -LiteralPath $ProjectPath -PathType Leaf) {
        return $true
    }

    $projectName = [System.IO.Path]::GetFileNameWithoutExtension($ProjectPath)

    if (-not $CreateMissing) {
        Write-FailMessage -Title $ScopeName -Message "Missing: $ProjectPath (pass -CreateMissing to scaffold it)"
        return $false
    }

    if (-not $PSCmdlet.ShouldProcess($ProjectPath, "dotnet new $Template")) {
        Write-Status "WhatIf: would scaffold $projectName as a $Template project."
        return $true
    }

    $projectDirectory = Split-Path -Parent $ProjectPath
    $null = New-Item -ItemType Directory -Path $projectDirectory -Force

    Write-StatusMessage -Title $ScopeName -Message "Creating $Template project: $ProjectPath"
    dotnet new $Template -n $projectName -o $projectDirectory
    if ($LASTEXITCODE -ne 0) {
        Write-FailMessage -Title 'dotnet new' -Message "Failed for $projectName (exit code $LASTEXITCODE)."
        return $false
    }

    return $true
}

Write-Status 'Verifying registry projects...'
Write-NewLine

$missing = @()

foreach ($scope in $Solution.Scopes) {
    foreach ($sourceProject in $scope.SourceProjects) {
        if (-not (Initialize-RegistryProject -ScopeName $scope.Name -ProjectPath $sourceProject -Template 'classlib')) {
            $missing += $sourceProject
        }
    }

    if (-not [string]::IsNullOrWhiteSpace($scope.TestProject)) {
        if (-not (Initialize-RegistryProject -ScopeName $scope.Name -ProjectPath $scope.TestProject -Template 'xunit')) {
            $missing += $scope.TestProject
        }
    }
}

if ($missing.Count -gt 0) {
    Write-NewLine
    Write-FailMessage -Title 'Projects' -Message "$($missing.Count) registry project(s) are missing. Nothing was built."
    exit 1
}

Write-OkMessage -Title 'Projects' -Message "All $($Solution.Projects.Count) source and $($Solution.TestProjects.Count) test project(s) present."
Write-NewLine

if ($SkipBuild) {
    Write-OkMessage -Title 'Solution' -Message 'Verification complete (-SkipBuild).'
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Restore, build, test
# -------------------------------------------------------------------------------------------------

function Invoke-DotnetStage {
    <#
    .SYNOPSIS
        Runs one dotnet stage against the solution and exits on failure.
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)] [string] $Stage,
        [Parameter(Mandatory)] [string[]] $Arguments
    )

    if (-not $PSCmdlet.ShouldProcess($Solution.Path, "dotnet $Stage")) {
        Write-Status "WhatIf: would run 'dotnet $($Arguments -join ' ')'."
        return
    }

    Write-Status "dotnet $Stage..."
    dotnet @Arguments

    if ($LASTEXITCODE -ne 0) {
        Write-FailMessage -Title "dotnet $Stage" -Message "Failed (exit code $LASTEXITCODE)."
        exit $LASTEXITCODE
    }

    Write-OkMessage -Title "dotnet $Stage" -Message 'Succeeded.'
    Write-NewLine
}

Invoke-DotnetStage -Stage 'restore' -Arguments @('restore', $Solution.Path)
Invoke-DotnetStage -Stage 'build' -Arguments @('build', $Solution.Path, '-c', $Configuration, '--no-restore')

if (-not $SkipTests) {
    Invoke-DotnetStage -Stage 'test' -Arguments @('test', $Solution.Path, '-c', $Configuration, '--no-build', '--no-restore')
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage -Title 'Solution' -Message "Bring-up complete for $($Solution.Name)."
