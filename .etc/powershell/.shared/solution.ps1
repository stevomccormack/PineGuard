<#
.SYNOPSIS
    Declares the $Solution variable describing PineGuard.slnx and every project in it.

.DESCRIPTION
    Dot-sourced by index.ps1, after project.ps1.

    Every project path here is PROJECTED FROM tools/.shared/dotnet-projects.ps1, the repository's
    single project registry - nothing is hand-listed. The previous version of this file was a
    hand-maintained literal that had drifted badly: it named 5 of the 14 scopes, pointed Qodana at
    the pre-rename tools/code-inspection/qodana/ folder, and still declared a
    tools/audit-cli/solution/PineGuard.AuditCli.csproj that no longer exists (the audit CLI moved
    to TypeScript under apps/cli). Deriving the list means adding a scope to the registry is the
    only edit a new scope ever needs.

    Package versions are NOT modelled here. Directory.Packages.props sets
    ManagePackageVersionsCentrally, so a project's PackageReference carries no version of its own
    and there is nothing per-project left to declare.

.NOTES
    Shape of $Solution:
      Name, Path, RepoRoot        - the solution file and where it lives
      TargetFrameworks            - Directory.Build.props' repo-wide <TargetFrameworks>
      Scopes                      - the 14 registry scopes, each with absolute paths
      Projects / TestProjects     - flat absolute csproj lists, for build and test loops
      PackableProjects            - nuget.org package ids (excludes IsPackable=false projects)
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '../../../tools/.shared/dotnet-projects.ps1')

# -------------------------------------------------------------------------------------------------
# Solution variables (projected from the project registry)
# -------------------------------------------------------------------------------------------------

function Resolve-SolutionPath {
    <#
    .SYNOPSIS
        Turns a registry-relative path into an absolute one under the repository root.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [AllowEmptyString()]
        [string] $RelativePath
    )

    if ([string]::IsNullOrWhiteSpace($RelativePath)) {
        return $null
    }

    return [System.IO.Path]::GetFullPath((Join-Path $PineGuardRepoRoot $RelativePath))
}

$solutionScopes = foreach ($scope in (Get-PineGuardScope -All)) {
    [pscustomobject]@{
        Name           = $scope.Name
        SourceDir      = Resolve-SolutionPath -RelativePath $scope.SourceDir
        SourceProjects = @($scope.SourceCsprojs | ForEach-Object { Resolve-SolutionPath -RelativePath $_ })
        TestProject    = Resolve-SolutionPath -RelativePath $scope.TestCsproj
        QodanaConfig   = Resolve-SolutionPath -RelativePath $scope.QodanaConfig
        QodanaSolution = Resolve-SolutionPath -RelativePath "tools/code-scan/qodana/PineGuard.$($scope.Name).Qodana.slnx"
        QodanaSlug     = $scope.QodanaSlug
    }
}

$solutionScopes = @($solutionScopes)

$targetFrameworks = @()
$buildPropsPath = Join-Path $PineGuardRepoRoot 'Directory.Build.props'
if (Test-Path -LiteralPath $buildPropsPath) {
    $buildProps = [xml](Get-Content -LiteralPath $buildPropsPath -Raw)
    $declared = $buildProps.SelectSingleNode('//TargetFrameworks')
    if ($null -ne $declared) {
        $targetFrameworks = @($declared.InnerText -split ';' | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    }
}

$Solution = [pscustomobject]@{
    Name             = $Project.Name
    RepoRoot         = $PineGuardRepoRoot
    Path             = Join-Path $PineGuardRepoRoot 'PineGuard.slnx'
    TargetFrameworks = $targetFrameworks
    Scopes           = $solutionScopes
    Projects         = @($solutionScopes | ForEach-Object { $_.SourceProjects } | Where-Object { $_ })
    TestProjects     = @($solutionScopes | ForEach-Object { $_.TestProject } | Where-Object { $_ })
    PackableProjects = @(Get-PineGuardPackableProjects -RepoRoot $PineGuardRepoRoot)
    QodanaSolutions  = @(
        Resolve-SolutionPath -RelativePath 'tools/code-scan/qodana/PineGuard.All.Qodana.slnx'
    ) + @($solutionScopes | ForEach-Object { $_.QodanaSolution })
}

# -------------------------------------------------------------------------------------------------

if ($Global.Log.Enabled) {
    Write-Header "`$Solution variable:"
    $Solution | Format-List
}
