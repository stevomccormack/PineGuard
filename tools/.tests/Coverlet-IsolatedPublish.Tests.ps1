<#
.SYNOPSIS
    Tests for the shared isolated-publish coverage helpers in tools/.shared/coverage.ps1
    (Resolve-CoverageTestFrameworks, Get-IsolatedPublishArguments, Invoke-IsolatedPublish) and the
    coverlet/New-CoverageReport.ps1 -Isolated regression they fix.

.DESCRIPTION
    Root cause (artifacts/reviews/T1-U0-fable-review.md, "Binding runner-fix decision"):
    coverlet/New-CoverageReport.ps1's -Isolated branch ran `dotnet publish $project -c
    $Configuration -o $tempDir` with no -f. Every test project inherits <TargetFrameworks>
    net8.0;net10.0 from tests/Directory.Build.props, so the SDK rejects the publish with
    NETSDK1129 whenever -Framework is left unbound, and the failure's own error text was
    discarded by a `| Out-Null`.

    Fast contexts dot-source tools/.shared/coverage.ps1 directly into this file's scope (following
    JBInspectCodeSarif.Tests.ps1) so Mock can intercept every call to the external `dotnet`
    executable the helpers under test shell out to, without invoking the real .NET SDK. Slow
    contexts (-Tag 'Slow', like DotCover-Snapshot.Tests.ps1) exercise the real `dotnet
    msbuild`/`dotnet publish`/`dotnet test` pipeline end to end, against the exact §9 invocation
    shape that failed.
#>

BeforeAll {
    . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
    $script:RepoRootCip = Get-RepoRoot -StartDirectory $PSScriptRoot
    . (Join-Path $script:RepoRootCip 'tools' '.shared' 'coverage.ps1')
    $script:TestingCsproj = Join-Path $script:RepoRootCip 'tests' 'PineGuard.Testing.UnitTests' 'PineGuard.Testing.UnitTests.csproj'
}

Describe 'Resolve-CoverageTestFrameworks' {

    Context 'bound -Framework, -Isolated' {
        BeforeAll {
            Mock dotnet {}
        }

        It 'returns exactly one pass: the bound framework' {
            $result = @(Resolve-CoverageTestFrameworks -ProjectPath $script:TestingCsproj -Framework 'net8.0' -Isolated)
            $result | Should -Be @('net8.0')
        }

        It 'never shells out to dotnet -- the framework is already known' {
            Should -Invoke -CommandName dotnet -Times 0 -Exactly
        }
    }

    Context 'bound -Framework, not isolated' {
        It 'returns exactly one pass: the bound framework' {
            $result = @(Resolve-CoverageTestFrameworks -ProjectPath $script:TestingCsproj -Framework 'net10.0')
            $result | Should -Be @('net10.0')
        }
    }

    Context 'unbound -Framework, not isolated (preserved non-isolated path)' {
        BeforeAll {
            Mock dotnet {}
        }

        It 'returns exactly one element, null or empty' {
            $result = @(Resolve-CoverageTestFrameworks -ProjectPath $script:TestingCsproj)
            $result.Count | Should -Be 1
            [string]::IsNullOrEmpty($result[0]) | Should -BeTrue
        }

        It 'never shells out to dotnet -- the SDK fans out across TFMs itself' {
            Should -Invoke -CommandName dotnet -Times 0 -Exactly
        }
    }

    Context 'unbound -Framework, -Isolated (resolves the project''s real TFMs)' -Tag 'Slow' {
        It 'returns every TFM the project actually builds' {
            $result = @(Resolve-CoverageTestFrameworks -ProjectPath $script:TestingCsproj -Isolated)
            $result | Should -Be @('net8.0', 'net10.0')
        }
    }
}

Describe 'Get-IsolatedPublishArguments' {

    It 'renders the exact publish argument array' {
        $publishArgs = Get-IsolatedPublishArguments -ProjectPath 'C:\repo\Some.csproj' -Configuration 'Debug' -Framework 'net8.0' -OutputDirectory 'C:\temp\out'
        $publishArgs | Should -Be @('publish', 'C:\repo\Some.csproj', '-c', 'Debug', '-f', 'net8.0', '-o', 'C:\temp\out')
    }

    It 'throws when -Framework is empty -- isolated publish of a multi-targeted project requires exactly one TFM' {
        { Get-IsolatedPublishArguments -ProjectPath 'C:\repo\Some.csproj' -Configuration 'Debug' -Framework '' -OutputDirectory 'C:\temp\out' } |
            Should -Throw -ExpectedMessage '*Framework*multi-target*'
    }

    It 'throws when -Framework is whitespace-only' {
        { Get-IsolatedPublishArguments -ProjectPath 'C:\repo\Some.csproj' -Configuration 'Debug' -Framework '   ' -OutputDirectory 'C:\temp\out' } |
            Should -Throw -ExpectedMessage '*Framework*multi-target*'
    }
}

Describe 'Invoke-IsolatedPublish' {

    Context 'dotnet publish fails with NETSDK1129' {
        BeforeAll {
            Mock dotnet {
                Write-Output 'error NETSDK1129: cannot publish a multi-targeted project without specifying a target framework.'
                $global:LASTEXITCODE = 1
            }
        }

        It 'throws with the exit code, the project path, the framework, and the SDK error visible' {
            # Asserted as independent substring checks, not one ordered wildcard: the binding
            # contract's message shape is "...failed (exit <n>) for '<project>' [<tfm>]. ...
            # Output tail: <sdk output>" -- exit code before project before TFM before SDK
            # output -- so a single '*project*tfm*exit*sdk*' wildcard imposes the wrong order.
            $thrown = { Invoke-IsolatedPublish -ProjectPath 'C:\repo\Some.csproj' -Configuration 'Debug' -Framework 'net8.0' -OutputDirectory 'C:\temp\out' } | Should -Throw -PassThru

            $thrown.Exception.Message | Should -Match 'exit 1'
            $thrown.Exception.Message | Should -Match ([regex]::Escape('C:\repo\Some.csproj'))
            $thrown.Exception.Message | Should -Match 'net8\.0'
            $thrown.Exception.Message | Should -Match 'NETSDK1129'
        }
    }
}

Describe 'coverlet/New-CoverageReport.ps1 -Isolated end to end' -Tag 'Slow' {
    BeforeAll {
        $script:RunCodeCoverageScript = Join-Path $script:RepoRootCip 'tools' 'code-coverage' 'Run-CodeCoverage.ps1'
        $script:TestingResultsDir = Join-Path $script:RepoRootCip 'artifacts' 'code-coverage' 'coverlet' 'testing' 'testresults' 'PineGuard.Testing.UnitTests'
        $script:TestingScopeRoot = Join-Path $script:RepoRootCip 'artifacts' 'code-coverage' 'coverlet' 'testing'
    }

    It 'does not throw for the exact §9 invocation shape that failed, narrowed to one project' {
        {
            & $script:RunCodeCoverageScript -Mode Generate -Scope Testing -Framework net8.0 -Isolated -SkipHtml -NoOpen -Clean -ProjectFilter 'PineGuard.Testing.UnitTests.csproj'
        } | Should -Not -Throw
    }

    It 'produces a coverage.cobertura.xml under the scope''s testresults folder' {
        $found = Get-ChildItem -LiteralPath $script:TestingResultsDir -Recurse -File -Filter 'coverage.cobertura.xml' -ErrorAction SilentlyContinue
        $found | Should -Not -BeNullOrEmpty
    }

    It 'leaves no iso-test-* temp directory behind' {
        $stray = Get-ChildItem -LiteralPath $script:TestingScopeRoot -Directory -Filter 'iso-test-*' -ErrorAction SilentlyContinue
        $stray | Should -BeNullOrEmpty
    }
}
