<#
.SYNOPSIS
    Tests for Import-DotEnv (tools/.shared/dotenv.ps1).

.DESCRIPTION
    tools/.shared/env.ps1 (Sync-Env/Sync-Path/Get-JavaVersion) reads the Machine/User
    *environment*, not a .env file — it is unrelated to dotenv parsing and is not covered here
    (F-25 notes the scan spec incorrectly claims Sync-Env loads .env). tools/.shared/dotenv.ps1's
    Import-DotEnv is the actual .env-file parser, so it is the only one under test.

    Fixture: tools/.tests/fixtures/sample.env — bare KEY=value, a comment, a blank line, single-
    and double-quoted values, and whitespace around '='.
#>

BeforeAll {
    . (Join-Path $PSScriptRoot '..' '.shared' 'dotenv.ps1')
    $script:FixturePath = Join-Path $PSScriptRoot 'fixtures' 'sample.env'
}

Describe 'Import-DotEnv' {
    BeforeAll {
        $script:Parsed = Import-DotEnv -Path $script:FixturePath
    }

    It 'parses a bare KEY=value line' {
        $script:Parsed['FOO'] | Should -Be 'bar'
    }

    It 'parses a numeric bare value' {
        $script:Parsed['BAZ'] | Should -Be '123'
    }

    It 'strips single quotes and preserves internal whitespace' {
        $script:Parsed['QUOTED_SINGLE'] | Should -Be 'value with spaces'
    }

    It 'strips double quotes' {
        $script:Parsed['QUOTED_DOUBLE'] | Should -Be 'another value'
    }

    It 'tolerates whitespace around the =' {
        $script:Parsed['SPACED_EQUALS'] | Should -Be 'trimmed'
    }

    It 'ignores the comment line and does not create a spurious key' {
        $script:Parsed.Keys | Should -Not -Contain '#'
        ($script:Parsed.Keys | Where-Object { $_ -match '^\s*#' }) | Should -BeNullOrEmpty
    }

    It 'returns exactly the five keys the fixture defines (comment and blank line produce none)' {
        @($script:Parsed.Keys).Count | Should -Be 5
    }

    It 'throws when the file does not exist' {
        $missing = Join-Path $PSScriptRoot 'fixtures' 'does-not-exist.env'
        { Import-DotEnv -Path $missing } | Should -Throw
    }

    It 'throws when -Path is not supplied' {
        { Import-DotEnv -Path '' } | Should -Throw
    }
}
