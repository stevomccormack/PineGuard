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
        Returns the per-scope path/identifier registry entry (or entries) for the seven named
        PineGuard scopes: Core, MustClauses, GuardClauses, DataAnnotations, FluentValidation,
        Testing, Options.

    .DESCRIPTION
        Centralises the per-scope source directory, project paths, coverage include pattern,
        path-include regex, default source prefix and Qodana config/slug that used to be
        repeated as switch/hashtable blocks across tools/code-coverage, tools/code-formatter,
        tools/code-diagnostics and tools/code-inspection scripts.

        'All' (the aggregate pseudo-scope) and 'Custom' (Test-CoverageAnalysis.ps1 only) are
        NOT registry entries — callers that need the aggregate keep handling those cases
        specially, using -All to enumerate the seven real entries in a stable order
        (Core, MustClauses, GuardClauses, DataAnnotations, FluentValidation, Options, Testing).

        Adding a new scope means adding one entry here plus one ValidateSet token per consumer
        script — ValidateSet attributes must stay literal (PowerShell requires compile-time
        constants), so the scope names are necessarily still spelled out there too.

    .PARAMETER Name
        One of the ten scope names. Returns the single matching registry entry.

    .PARAMETER All
        Returns all ten registry entries, in the stable order used to build 'All' aggregates.
    #>
    [CmdletBinding(DefaultParameterSetName = 'One')]
    param(
        [Parameter(Mandatory, ParameterSetName = 'One', Position = 0)]
        [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'ErrorOr', 'FluentResults', 'OneOf', 'Testing')]
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
        Options           = [pscustomobject]@{
            Name                     = 'Options'
            SourceDir                = 'src\PineGuard.Extensions.Options'
            SourceCsproj             = 'src\PineGuard.Extensions.Options\PineGuard.Extensions.Options.csproj'
            TestCsproj               = 'tests\PineGuard.Extensions.Options.UnitTests\PineGuard.Extensions.Options.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.Extensions.Options.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.Extensions.Options]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.Extensions\.Options[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.Extensions.Options'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.options.yaml'
            QodanaSlug               = 'options'
            IncludeEmptyTestProjects = $false
        }
        ErrorOr           = [pscustomobject]@{
            Name                     = 'ErrorOr'
            SourceDir                = 'src\PineGuard.ErrorOr'
            SourceCsproj             = 'src\PineGuard.ErrorOr\PineGuard.ErrorOr.csproj'
            TestCsproj               = 'tests\PineGuard.ErrorOr.UnitTests\PineGuard.ErrorOr.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.ErrorOr.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.ErrorOr]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.ErrorOr[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.ErrorOr'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.erroror.yaml'
            QodanaSlug               = 'erroror'
            IncludeEmptyTestProjects = $false
        }
        FluentResults     = [pscustomobject]@{
            Name                     = 'FluentResults'
            SourceDir                = 'src\PineGuard.FluentResults'
            SourceCsproj             = 'src\PineGuard.FluentResults\PineGuard.FluentResults.csproj'
            TestCsproj               = 'tests\PineGuard.FluentResults.UnitTests\PineGuard.FluentResults.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.FluentResults.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.FluentResults]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.FluentResults[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.FluentResults'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.fluentresults.yaml'
            QodanaSlug               = 'fluentresults'
            IncludeEmptyTestProjects = $false
        }
        OneOf             = [pscustomobject]@{
            Name                     = 'OneOf'
            SourceDir                = 'src\PineGuard.OneOf'
            SourceCsproj             = 'src\PineGuard.OneOf\PineGuard.OneOf.csproj'
            TestCsproj               = 'tests\PineGuard.OneOf.UnitTests\PineGuard.OneOf.UnitTests.csproj'
            DefaultProjectFilter     = 'PineGuard.OneOf.UnitTests.csproj'
            CoverageIncludePattern   = '[PineGuard.OneOf]*'
            PathIncludeRegex         = '^src[/\\]+PineGuard\.OneOf[/\\]+'
            DefaultSourcePrefix      = 'src\PineGuard.OneOf'
            QodanaConfig             = 'tools/code-inspection/qodana/config/qodana.oneof.yaml'
            QodanaSlug               = 'oneof'
            IncludeEmptyTestProjects = $false
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
