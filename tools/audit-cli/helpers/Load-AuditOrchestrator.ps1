<#
.SYNOPSIS
    Load Audit Orchestrator

.DESCRIPTION
    Shared orchestration logic for running PineGuard audit rules.

    This script defines Invoke-PineGuardAuditRules which is used by
    tools/audit-cli/Run-*.ps1 entrypoints.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Expand-CommaSeparatedValues {
    [CmdletBinding()]
    param([string[]]$Values)

    @(
        foreach ($v in $Values) {
            if ([string]::IsNullOrWhiteSpace($v)) { continue }
            foreach ($part in ($v -split ',')) {
                $trimmed = $part.Trim()
                if (-not [string]::IsNullOrWhiteSpace($trimmed)) {
                    $trimmed
                }
            }
        }
    )
}

function ConvertTo-PineGuardAuditRuleId {
    [CmdletBinding()]
    param([string]$Value)

    if ([string]::IsNullOrWhiteSpace($Value)) { return $Value }

    $trimmed = $Value.Trim()

    # Support both legacy small rule ids (Rule01..Rule10) and newer larger ids (Rule50..).
    if ($trimmed -match '^(?i)rule(?<n>\d{1,3})$') {
        $n = [int]$Matches['n']
        if ($n -lt 10) { return ('Rule{0:00}' -f $n) }
        return ('Rule{0}' -f $n)
    }

    if ($trimmed -match '^(?<n>\d{1,3})$') {
        $n = [int]$Matches['n']
        if ($n -lt 10) { return ('Rule{0:00}' -f $n) }
        return ('Rule{0}' -f $n)
    }

    return $trimmed
}

function Get-RuleNameSuggestions {
    [CmdletBinding()]
    param(
        [object[]]$Rules,
        [string]$Query,
        [int]$Max = 5
    )

    if ([string]::IsNullOrWhiteSpace($Query)) { return @() }
    $q = $Query.Trim()

    @(
        $Rules |
            Where-Object { $_.Name.IndexOf($q, [System.StringComparison]::OrdinalIgnoreCase) -ge 0 } |
            Select-Object -ExpandProperty Name |
            Sort-Object |
            Select-Object -First $Max
    )
}

function Write-Rule-Catalog {
    [CmdletBinding()]
    param([object[]]$Rules)

    Write-Host ''
    Write-Host 'Audit rule catalog:' -ForegroundColor Cyan
    $Rules |
        Select-Object Id, Name, Description, OutputPath |
        Format-Table -AutoSize -Wrap
    Write-Host ''
}

function Write-JsonSummary {
    [CmdletBinding()]
    param(
        [ValidateNotNullOrEmpty()]
        [string]$Path,
        [object[]]$Results,
        [string]$Configuration,
        [string]$RepoRoot,
        [bool]$Success
    )

    $dir = Split-Path -Parent $Path
    if (-not [string]::IsNullOrWhiteSpace($dir) -and -not (Test-Path -LiteralPath $dir)) {
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
    }

    $payload = [pscustomobject]@{
        tool          = 'PineGuard.AuditCli'
        timestampUtc  = (Get-Date).ToUniversalTime().ToString('o')
        repoRoot      = $RepoRoot
        configuration = $Configuration
        success       = $Success
        rules         = @(
            foreach ($r in $Results) {
                [pscustomobject]@{
                    id         = $r.Id
                    name       = $r.Name
                    status     = $r.Status
                    durationMs = [int][Math]::Round($r.Duration.TotalMilliseconds)
                    outputPath = $r.OutputPath
                    error      = $r.Error
                }
            }
        )
    }

    $payload | ConvertTo-Json -Depth 6 | Out-File -LiteralPath $Path -Encoding utf8
}

function Format-DurationShort {
    [CmdletBinding()]
    param([TimeSpan]$Duration)

    if ($Duration.TotalSeconds -lt 1) { return "$( [Math]::Round($Duration.TotalMilliseconds) )ms" }
    if ($Duration.TotalMinutes -lt 1) { return "$( [Math]::Round($Duration.TotalSeconds, 1) )s" }
    return "$( [Math]::Round($Duration.TotalMinutes, 1) )m"
}

function Invoke-PineGuardAuditRules {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][ValidateNotNullOrEmpty()] [string]$AuditCliRoot,
        [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
        [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
        [switch]$AllowViolations,
        [switch]$ListRules,
        [switch]$NoCatalog,
        [switch]$ContinueOnError,
        [switch]$ShowFailures,
        [switch]$NoSummary,
        [Alias('JsonSummary')] [string]$JsonSummaryPath,
        [Alias('Rule')] [string[]]$RuleId,
        [string[]]$RuleName
    )

    . (Join-Path $AuditCliRoot 'helpers\Load-AuditHelpers.ps1')
    . (Join-Path $AuditCliRoot 'rules\Load-Catalog.ps1')

    $repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $AuditCliRoot

    Write-Host 'Running PineGuard audit rules...' -ForegroundColor Cyan

    $rulesRoot = Join-Path $AuditCliRoot 'rules'
    $rules = Get-PineGuardAuditRule-Catalog -RulesRoot $rulesRoot

    if (($RuleId -and $RuleId.Count -gt 0) -and ($RuleName -and $RuleName.Count -gt 0)) {
        throw "Use either -Rule (IDs) or -RuleName (names), not both. Use -ListRules to see available values."
    }

    if ($RuleId -and $RuleId.Count -gt 0) {
        $requested = Expand-CommaSeparatedValues -Values $RuleId
        $requested = @($requested | ForEach-Object { ConvertTo-PineGuardAuditRuleId -Value $_ })

        if ($requested.Count -gt 0) {
            $requestedSet = [System.Collections.Generic.HashSet[string]]::new([string[]]$requested, [System.StringComparer]::OrdinalIgnoreCase)
            $knownSet = [System.Collections.Generic.HashSet[string]]::new([string[]]($rules | Select-Object -ExpandProperty Id), [System.StringComparer]::OrdinalIgnoreCase)
            $unknown = @($requested | Where-Object { -not $knownSet.Contains($_) } | Select-Object -Unique)
            if ($unknown.Count -gt 0) {
                throw "Unknown rule id(s): $($unknown -join ', '). Use -ListRules to see valid IDs. Tip: numeric values like '6' or '50' are accepted and normalize to 'Rule06' / 'Rule50'."
            }

            $rules = @($rules | Where-Object { $requestedSet.Contains($_.Id) })
            if ($rules.Count -eq 0) {
                throw 'No matching rules to run. Use -ListRules to see valid IDs.'
            }
        }
    }

    if ($RuleName -and $RuleName.Count -gt 0) {
        $requestedNames = Expand-CommaSeparatedValues -Values $RuleName
        if ($requestedNames.Count -gt 0) {
            $selected = New-Object System.Collections.Generic.List[object]

            foreach ($requestedName in $requestedNames) {
                # Match strategy: exact (preferred) -> starts-with -> contains
                $ruleCandidates = @($rules | Where-Object { $_.Name -ieq $requestedName })

                if ($ruleCandidates.Count -eq 0) {
                    $ruleCandidates = @($rules | Where-Object { $_.Name.StartsWith($requestedName, [System.StringComparison]::OrdinalIgnoreCase) })
                }

                if ($ruleCandidates.Count -eq 0) {
                    $ruleCandidates = @($rules | Where-Object { $_.Name.IndexOf($requestedName, [System.StringComparison]::OrdinalIgnoreCase) -ge 0 })
                }

                if ($ruleCandidates.Count -eq 0) {
                    $suggestions = Get-RuleNameSuggestions -Rules $rules -Query $requestedName
                    if ($suggestions.Count -gt 0) {
                        throw "Unknown rule name: '$requestedName'. Suggestions: $($suggestions -join '; '). Use -ListRules to see valid names."
                    }
                    throw "Unknown rule name: '$requestedName'. Use -ListRules to see valid names."
                }

                if ($ruleCandidates.Count -gt 1) {
                    $names = ($ruleCandidates | Sort-Object Id | ForEach-Object { "{0} ({1})" -f $_.Id, $_.Name }) -join '; '
                    throw "Ambiguous rule name: '$requestedName'. Matches: $names. Use -Rule with IDs or specify a more exact -RuleName."
                }

                $selected.Add($ruleCandidates[0]) | Out-Null
            }

            # De-dupe by Id (stable order by catalog order)
            $selectedIds = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
            $rules = @(
                foreach ($r in $rules) {
                    if ($selectedIds.Contains($r.Id)) { continue }
                    if ($selected | Where-Object { $_.Id -ieq $r.Id } | Select-Object -First 1) {
                        $selectedIds.Add($r.Id) | Out-Null
                        $r
                    }
                }
            )

            if ($rules.Count -eq 0) {
                throw 'No matching rules to run. Use -ListRules to see valid values.'
            }
        }
    }

    if ($ListRules) {
        Write-Rule-Catalog -Rules $rules
        return
    }

    if (-not $NoCatalog.IsPresent) {
        Write-Rule-Catalog -Rules $rules
    }

    $results = New-Object System.Collections.Generic.List[object]

    foreach ($rule in $rules) {
        $ruleInfo = $rule
        Write-Host ("---- {0}: {1} ----" -f $ruleInfo.Id, $ruleInfo.Name) -ForegroundColor Cyan
        $start = Get-Date
        $status = 'PASS'
        $errorMessage = ''
        $stopAfterThisRule = $false

        $pwshArgs = @(
            '-NoLogo',
            '-NonInteractive',
            '-NoProfile',
            '-ExecutionPolicy', 'Bypass',
            '-File', $ruleInfo.ScriptPath,
            '-RepoRoot', $repoRootResolved
        )

        if ($ruleInfo.UsesConfiguration) {
            $pwshArgs += @('-Configuration', $Configuration)
        }

        if ($ruleInfo.UsesFailOnFindings -and (-not $AllowViolations.IsPresent)) {
            $pwshArgs += '-FailOnFindings'
        }

        if ($ruleInfo.UsesAllowViolations -and $AllowViolations.IsPresent) {
            $pwshArgs += '-AllowViolations'
        }

        try {
            & pwsh @pwshArgs
            if ($LASTEXITCODE -ne 0) {
                $status = 'FAIL'
                $errorMessage = "ExitCode=$LASTEXITCODE"
                if (-not $ContinueOnError) {
                    $stopAfterThisRule = $true
                }
                Write-Warning ("{0} failed: {1}" -f $ruleInfo.Id, $errorMessage)
            }
        }
        catch {
            $status = 'FAIL'
            $errorMessage = $_.Exception.Message

            if (-not $ContinueOnError) {
                $stopAfterThisRule = $true
            }

            Write-Warning ("{0} failed: {1}" -f $ruleInfo.Id, $errorMessage)
        }
        finally {
            $duration = (Get-Date) - $start
            $results.Add([pscustomobject]@{
                Id          = $ruleInfo.Id
                Name        = $ruleInfo.Name
                Status      = $status
                Duration    = $duration
                OutputPath  = $ruleInfo.OutputPath
                Description = $ruleInfo.Description
                Error       = $errorMessage
            }) | Out-Null
        }

        if ($stopAfterThisRule) {
            break
        }
    }

    $failed = @($results | Where-Object { $_.Status -ne 'PASS' })
    $success = ($failed.Count -eq 0)

    if (-not [string]::IsNullOrWhiteSpace($JsonSummaryPath)) {
        Write-JsonSummary -Path $JsonSummaryPath -Results $results -Configuration $Configuration -RepoRoot $repoRootResolved -Success $success
        Write-Host ("Wrote JSON summary: {0}" -f $JsonSummaryPath) -ForegroundColor DarkGray
    }

    if (-not $NoSummary.IsPresent) {
        Write-Host ''
        Write-Host 'Audit summary:' -ForegroundColor Cyan
        foreach ($r in $results) {
            $color = if ($r.Status -eq 'PASS') { 'Green' } else { 'Red' }
            $dur = Format-DurationShort -Duration $r.Duration
            Write-Host ("[{0}] {1} ({2}) -> {3}" -f $r.Id, $r.Name, $dur, $r.OutputPath) -ForegroundColor $color
        }
    }

    if ($failed.Count -gt 0) {
        if ($ShowFailures.IsPresent) {
            Write-Host ''
            Write-Host 'Audit failures:' -ForegroundColor Red
            foreach ($f in $failed) {
                $err = if ([string]::IsNullOrWhiteSpace($f.Error)) { '(no error message)' } else { $f.Error }
                Write-Host ("[{0}] {1} -> {2}" -f $f.Id, $f.Name, $err) -ForegroundColor Red
                if (-not [string]::IsNullOrWhiteSpace($f.OutputPath)) {
                    Write-Host ("       Output: {0}" -f $f.OutputPath) -ForegroundColor DarkGray
                }
            }
        }

        Write-Host ''
        Write-Host ("Audit rules complete with failures: {0}" -f $failed.Count) -ForegroundColor Red
        exit 1
    }

    Write-Host ''
    Write-Host 'Audit rules complete.' -ForegroundColor Green
}
