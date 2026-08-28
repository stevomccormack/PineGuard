<#
.SYNOPSIS
    Shared .NET project discovery helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Test-ProjectHasSources, Get-TestProjects and
    Get-PineGuardScope into the calling script's scope.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-PineGuardScope {
    <#
    .SYNOPSIS
        Returns the per-scope path/identifier registry entry (or entries) for the six named
        PineGuard scopes: Core, MustClauses, GuardClauses, DataAnnotations, FluentValidation, Testing.

    .DESCRIPTION
        Centralises the per-scope source directory, project paths, coverage include pattern,
        path-include regex, default source prefix and Qodana config/slug that used to be
        repeated as switch/hashtable blocks across tools/code-coverage, tools/code-formatter,
        tools/code-diagnostics and tools/code-inspection scripts.

        'All' (the aggregate pseudo-scope) and 'Custom' (Test-CoverageAnalysis.ps1 only) are
        NOT registry entries — callers that need the aggregate keep handling those cases
        specially, using -All to enumerate the six real entries in a stable order
        (Core, MustClauses, GuardClauses, DataAnnotations, FluentValidation, Testing).

        Adding a new scope (e.g. Phase 2's Options) means adding one entry here plus one
        ValidateSet token per consumer script — ValidateSet attributes must stay literal
        (PowerShell requires compile-time constants), so the six/seven scope names are
        necessarily still spelled out there too.

    .PARAMETER Name
        One of the six scope names. Returns the single matching registry entry.

    .PARAMETER All
        Returns all six registry entries, in the stable order used to build 'All' aggregates.
    #>
    [CmdletBinding(DefaultParameterSetName = 'One')]
    param(
        [Parameter(Mandatory, ParameterSetName = 'One', Position = 0)]
        [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Testing')]
        [string] $Name,

        [Parameter(Mandatory, ParameterSetName = 'All')]
        [switch] $All
    )

    # Ordered so that -All enumerates scopes in the same order every consumer script already
    # used when building its own hardcoded 'All' aggregate (Core, MustClauses, GuardClauses,
    # DataAnnotations, FluentValidation, Testing).
    $registry = [ordered]@{
        Core              = [pscustomobject]@{
            Name                     = 'Core'
            SourceDir                = 'src\PineGuard.Core'
            SourceCsproj             = 'src\PineGuard.Core\PineGuard.Core.csproj'
            TestCsproj               = 'tests\PineGuard.Core.UnitTests\PineGuard.Core.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.Core.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.Core]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.Core[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.Core'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.core.yaml'
            QodanaSlug               = 'core'
            IncludeEmptyTestProjects = $false
        }
        MustClauses       = [pscustomobject]@{
            Name                     = 'MustClauses'
            SourceDir                = 'src\PineGuard.MustClauses'
            SourceCsproj             = 'src\PineGuard.MustClauses\PineGuard.MustClauses.csproj'
            TestCsproj               = 'tests\PineGuard.MustClauses.UnitTests\PineGuard.MustClauses.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.MustClauses.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.MustClauses]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.MustClauses[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.MustClauses'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.must-clauses.yaml'
            QodanaSlug               = 'must-clauses'
            IncludeEmptyTestProjects = $false
        }
        GuardClauses      = [pscustomobject]@{
            Name                     = 'GuardClauses'
            SourceDir                = 'src\PineGuard.GuardClauses'
            SourceCsproj             = 'src\PineGuard.GuardClauses\PineGuard.GuardClauses.csproj'
            TestCsproj               = 'tests\PineGuard.GuardClauses.UnitTests\PineGuard.GuardClauses.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.GuardClauses.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.GuardClauses]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.GuardClauses[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.GuardClauses'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.guard-clauses.yaml'
            QodanaSlug               = 'guard-clauses'
            IncludeEmptyTestProjects = $false
        }
        DataAnnotations   = [pscustomobject]@{
            Name                     = 'DataAnnotations'
            SourceDir                = 'src\PineGuard.DataAnnotations'
            SourceCsproj             = 'src\PineGuard.DataAnnotations\PineGuard.DataAnnotations.csproj'
            TestCsproj               = 'tests\PineGuard.DataAnnotations.UnitTests\PineGuard.DataAnnotations.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.DataAnnotations.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.DataAnnotations]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.DataAnnotations[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.DataAnnotations'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.data-annotations.yaml'
            QodanaSlug               = 'data-annotations'
            IncludeEmptyTestProjects = $true
        }
        FluentValidation  = [pscustomobject]@{
            Name                     = 'FluentValidation'
            SourceDir                = 'src\PineGuard.FluentValidation'
            SourceCsproj             = 'src\PineGuard.FluentValidation\PineGuard.FluentValidation.csproj'
            TestCsproj               = 'tests\PineGuard.FluentValidation.UnitTests\PineGuard.FluentValidation.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.FluentValidation.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.FluentValidation]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.FluentValidation[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.FluentValidation'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.fluent-validation.yaml'
            QodanaSlug               = 'fluent-validation'
            IncludeEmptyTestProjects = $true
        }
        Testing           = [pscustomobject]@{
            Name                     = 'Testing'
            SourceDir                = 'tests\PineGuard.Testing'
            SourceCsproj             = 'tests\PineGuard.Testing\PineGuard.Testing.csproj'
            TestCsproj               = 'tests\PineGuard.Testing.UnitTests\PineGuard.Testing.UnitTests.csproj'
            # NOTE: unlike the other five scopes, Testing's own coverage scripts default the
            # test-project filter to the wildcard, not to its own test csproj filename — verified
            # against Run-CodeCoverage.ps1 and Gen-CoverageReport.ps1 before this refactor.
            DefaultProjectFilter     = '*.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.Testing]*'
            PathIncludeRegex         = '^tests[/\\]+PineGuard\.Testing[/\\]+'
            DefaultSourcePrefix      = 'tests\PineGuard.Testing'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.testing.yaml'
            QodanaSlug               = 'testing'
            IncludeEmptyTestProjects = $false
        }
    }

    if ($All) {
        return @($registry.Values)
    }

    return $registry[$Name]
}

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
