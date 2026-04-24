<#
.SYNOPSIS
    solution

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/.shared/solution.ps1

# -------------------------------------------------------------------------------------------------
# Solution Variables
# -------------------------------------------------------------------------------------------------

$Solution = [pscustomobject]@{
    Name        = $Project.Name
    LocalPath   = $Project.LocalPath
    Path        = [System.IO.Path]::Combine($Project.LocalPath, 'PineGuard.slnx')
    DotNetSdk   = '10.0'
    Projects    = @(
        [pscustomobject]@{
            Name         = 'PineGuard.Core'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.Core')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.Core', 'PineGuard.Core.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @()
        }
        [pscustomobject]@{
            Name         = 'PineGuard.DataAnnotations'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.DataAnnotations')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.DataAnnotations', 'PineGuard.DataAnnotations.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @(
                'PineGuard.Core'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.FluentValidation'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.FluentValidation')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.FluentValidation', 'PineGuard.FluentValidation.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @(
                'FluentValidation'
            )
            References   = @(
                'PineGuard.Core'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.GuardClauses'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.GuardClauses')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.GuardClauses', 'PineGuard.GuardClauses.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @(
                'PineGuard.Core'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.MustClauses'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.MustClauses')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'src', 'PineGuard.MustClauses', 'PineGuard.MustClauses.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @(
                'PineGuard.Core'
            )
        }
    )
    TestProjects    = @(
        [pscustomobject]@{
            Name         = 'PineGuard.Testing'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.Testing')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.Testing', 'PineGuard.Testing.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @(
                'xunit.abstractions'
            )
            References   = @()
        }
        [pscustomobject]@{
            Name         = 'PineGuard.Core.UnitTests'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.Core.UnitTests')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.Core.UnitTests', 'PineGuard.Core.UnitTests.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @(
                'PineGuard.Core'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.DataAnnotations.UnitTests'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.DataAnnotations.UnitTests')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.DataAnnotations.UnitTests', 'PineGuard.DataAnnotations.UnitTests.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @(
                'PineGuard.Core',
                'PineGuard.DataAnnotations'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.FluentValidation.UnitTests'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.FluentValidation.UnitTests')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.FluentValidation.UnitTests', 'PineGuard.FluentValidation.UnitTests.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @(
                'FluentValidation'
            )
            References   = @(
                'PineGuard.Core',
                'PineGuard.FluentValidation'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.GuardClauses.UnitTests'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.GuardClauses.UnitTests')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.GuardClauses.UnitTests', 'PineGuard.GuardClauses.UnitTests.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @(
                'PineGuard.Core',
                'PineGuard.GuardClauses'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.MustClauses.UnitTests'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.MustClauses.UnitTests')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tests', 'PineGuard.MustClauses.UnitTests', 'PineGuard.MustClauses.UnitTests.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @()
            References   = @(
                'PineGuard.Core',                
                'PineGuard.MustClauses'
            )
        }
    )
    ToolProjects    = @(
        [pscustomobject]@{
            Name         = 'PineGuard.AuditCli'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'audit-cli', 'solution')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'audit-cli', 'solution', 'PineGuard.AuditCli.csproj')
            DotNetVersion = 'net10.0'
            Packages     = @(
                'Microsoft.Build.Locator',
                'Microsoft.CodeAnalysis.CSharp.Workspaces',
                'Microsoft.CodeAnalysis.Workspaces.MSBuild'
            )
            References   = @()
        }
        [pscustomobject]@{
            Name         = 'PineGuard.AuditCli.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'audit-cli')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'audit-cli', 'PineGuard.AuditCli.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.AuditCli'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.All.Qodana.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana', 'PineGuard.All.Qodana.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.Core',
                'PineGuard.Core.UnitTests',
                'PineGuard.MustClauses',
                'PineGuard.MustClauses.UnitTests',
                'PineGuard.GuardClauses',
                'PineGuard.GuardClauses.UnitTests',
                'PineGuard.FluentValidation',
                'PineGuard.FluentValidation.UnitTests',
                'PineGuard.DataAnnotations',
                'PineGuard.DataAnnotations.UnitTests',
                'PineGuard.Testing',
                'PineGuard.Testing.UnitTests'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.Core.Qodana.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana', 'PineGuard.Core.Qodana.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.Core',
                'PineGuard.Core.UnitTests'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.DataAnnotations.Qodana.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana', 'PineGuard.DataAnnotations.Qodana.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.DataAnnotations',
                'PineGuard.DataAnnotations.UnitTests'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.FluentValidation.Qodana.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana', 'PineGuard.FluentValidation.Qodana.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.FluentValidation',
                'PineGuard.FluentValidation.UnitTests'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.GuardClauses.Qodana.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana', 'PineGuard.GuardClauses.Qodana.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.GuardClauses',
                'PineGuard.GuardClauses.UnitTests'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.MustClauses.Qodana.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana', 'PineGuard.MustClauses.Qodana.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.MustClauses',
                'PineGuard.MustClauses.UnitTests'
            )
        }
        [pscustomobject]@{
            Name         = 'PineGuard.Testing.Qodana.slnx'
            DirPath      = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana')
            Path         = [System.IO.Path]::Combine($Project.LocalPath, 'tools', 'code-inspection', 'qodana', 'PineGuard.Testing.Qodana.slnx')
            DotNetVersion = ''
            Packages     = @()
            References   = @(
                'PineGuard.Testing',
                'PineGuard.Testing.UnitTests'
            )
        }
    )
}

# -------------------------------------------------------------------------------------------------

if ($Global.Log.Enabled) {
    Write-Header "`$Project` Variable:"
    # Write-ObjectPathTree -Object $Projects -RootPath '$Projects'   

    $Solution | Format-List
}
