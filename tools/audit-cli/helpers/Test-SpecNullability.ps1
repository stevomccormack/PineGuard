<#
.SYNOPSIS
    Test Spec Nullability

.DESCRIPTION
    Scans MustClauses + GuardClauses public static extension methods and enforces the
    "hybrid nullability" convention for the primary validated parameter:
      - reference types: prefer nullable (e.g., string?)
      - value types: prefer non-nullable (e.g., int, Guid)

.PARAMETER AuditRuleId
    Audit rule identifier for reporting.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.

.PARAMETER OutputPath
    Output report path.

.PARAMETER AllowViolations
    If set, does not fail the rule when violations exist.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Rule07',
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [ValidateNotNullOrEmpty()] [string]$OutputPath = 'artifacts/audit/Rule07-hybrid-nullability-policy-scan.txt',
    [switch]$AllowViolations
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$resolvedOutputPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $OutputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $resolvedOutputPath)

function ConvertTo-CSharpTypeNameNormalized {
    param(
        [Parameter(Mandatory = $true)]
        [string]$TypeName
    )

    $t = $TypeName.Trim()
    $t = $t -replace '^global::', ''
    $t = $t -replace '^System\\.', ''
    return $t
}

function Split-TopLevelComma {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Text
    )

    $parts = New-Object System.Collections.Generic.List[string]
    $current = New-Object System.Text.StringBuilder

    $angleDepth = 0
    $parenDepth = 0
    $bracketDepth = 0

    foreach ($ch in $Text.ToCharArray()) {
        switch ($ch) {
            '<' { $angleDepth++; break }
            '>' { if ($angleDepth -gt 0) { $angleDepth-- }; break }
            '(' { $parenDepth++; break }
            ')' { if ($parenDepth -gt 0) { $parenDepth-- }; break }
            '[' { $bracketDepth++; break }
            ']' { if ($bracketDepth -gt 0) { $bracketDepth-- }; break }
            ',' {
                if ($angleDepth -eq 0 -and $parenDepth -eq 0 -and $bracketDepth -eq 0) {
                    $parts.Add($current.ToString()) | Out-Null
                    $current.Clear() | Out-Null
                    break
                }
            }
        }

        $null = $current.Append($ch)
    }

    $parts.Add($current.ToString()) | Out-Null
    return $parts
}

function Try-ParseParameter {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Token,

        [ref]$IsExtension,
        [ref]$Type,
        [ref]$Name
    )

    $t = $Token.Trim()
    if ([string]::IsNullOrWhiteSpace($t)) {
        return $false
    }

    # Strip default value
    $eqIndex = $t.IndexOf('=')
    if ($eqIndex -gt 0) {
        $t = $t.Substring(0, $eqIndex).Trim()
    }

    # Strip attributes
    while ($t -match '^\\s*\\[[^\\]]+\\]\\s*') {
        $t = ($t -replace '^\\s*\\[[^\\]]+\\]\\s*', '').Trim()
    }

    $t = ($t -replace '^params\\s+', '').Trim()

    $IsExtension.Value = $false
    if ($t -match '^this\\s+') {
        $IsExtension.Value = $true
        $t = ($t -replace '^this\\s+', '').Trim()
    }

    $t = ($t -replace '^(ref|in|out)\\s+', '').Trim()

    if ($t -match '^(?<Type>[\\w\\.:\\?<>\\[\\]]+)\\s+(?<Name>@?\\w+)$') {
        $Type.Value = $matches.Type
        $Name.Value = $matches.Name
        return $true
    }

    return $false
}

# Hybrid nullability strategy (current PineGuard convention)
$valueTypes = @(
    'int', 'long', 'short', 'byte', 'uint', 'ulong', 'ushort', 'sbyte',
    'double', 'float', 'decimal',
    'bool', 'char',
    'Guid',
    'DateTime', 'DateTimeOffset', 'DateOnly', 'TimeOnly', 'TimeSpan'
)

$targetProjects = @(
    @{ Name = 'MustClauses'; Path = (Join-Path $repoRootResolved 'src/PineGuard.MustClauses') },
    @{ Name = 'GuardClauses'; Path = (Join-Path $repoRootResolved 'src/PineGuard.GuardClauses') }
)

$violations = New-Object System.Collections.Generic.List[object]

foreach ($project in $targetProjects) {
    $projectPath = $project.Path
    if (-not (Test-Path $projectPath)) {
        continue
    }

    $files = Get-ChildItem -Path $projectPath -Recurse -Filter '*.cs' |
        Where-Object { $_.FullName -notmatch '\\bin\\|\\obj\\' }

    foreach ($file in $files) {
        $lines = Get-Content -Path $file.FullName

        $i = 0
        while ($i -lt $lines.Count) {
            $line = $lines[$i]

            if ($line -notmatch '\\bpublic\\b' -or $line -notmatch '\\bstatic\\b' -or $line -notmatch '\\(') {
                $i++
                continue
            }

            # Accumulate signature until parens are balanced.
            $startLine = $i + 1
            $signatureBuilder = New-Object System.Text.StringBuilder
            $parenDepth = 0
            $started = $false

            while ($i -lt $lines.Count) {
                $segment = $lines[$i]

                # Best-effort: strip inline // comments for parsing.
                $segment = ($segment -replace '//.*$', '')

                foreach ($ch in $segment.ToCharArray()) {
                    if ($ch -eq '(') {
                        $parenDepth++
                        $started = $true
                    }
                    elseif ($ch -eq ')') {
                        if ($parenDepth -gt 0) { $parenDepth-- }
                    }
                }

                $null = $signatureBuilder.Append($segment).Append(' ')

                $i++
                if ($started -and $parenDepth -eq 0) {
                    break
                }
            }

            $signature = $signatureBuilder.ToString().Trim()

            if ($signature -notmatch 'public\\s+static\\s+(?:partial\\s+)?(?:[\\w\\.:\\?<>\\[\\]]+\\s+)+(?<MethodName>\\w+)\\s*\\((?<Args>.*)\\)') {
                continue
            }

            $methodName = $matches.MethodName
            $argsStr = $matches.Args.Trim()

            if ([string]::IsNullOrWhiteSpace($argsStr)) {
                continue
            }

            $argTokens = Split-TopLevelComma -Text $argsStr
            $parsedArgs = New-Object System.Collections.Generic.List[hashtable]

            foreach ($token in $argTokens) {
                $isExt = $false
                $type = ''
                $name = ''
                if (Try-ParseParameter -Token $token -IsExtension ([ref]$isExt) -Type ([ref]$type) -Name ([ref]$name)) {
                    $parsedArgs.Add(@{ IsExtension = $isExt; Type = $type; Name = $name }) | Out-Null
                }
            }

            if ($parsedArgs.Count -eq 0) {
                continue
            }

            # Determine primary validated argument in Must/Guard extensions.
            $primaryArgIndex = 0
            if ($parsedArgs[0].IsExtension -and ($parsedArgs[0].Type -match '(^|\\.)IMustClause$' -or $parsedArgs[0].Type -match '(^|\\.)IGuardClause$')) {
                if ($parsedArgs.Count -lt 2) {
                    continue
                }
                $primaryArgIndex = 1
            }

            $primaryTypeRaw = $parsedArgs[$primaryArgIndex].Type
            $primaryName = $parsedArgs[$primaryArgIndex].Name
            $primaryType = ConvertTo-CSharpTypeNameNormalized -TypeName $primaryTypeRaw

            $isNullableValueType = $false
            $innerNullableType = ''
            if ($primaryType.EndsWith('?')) {
                $isNullableValueType = $true
                $innerNullableType = $primaryType.TrimEnd('?')
            }
            elseif ($primaryType -match '^Nullable<(?<Inner>[\\w\\.:]+)>$') {
                $isNullableValueType = $true
                $innerNullableType = ConvertTo-CSharpTypeNameNormalized -TypeName $matches.Inner
            }

            if ($primaryType -eq 'string') {
                $violations.Add([PSCustomObject]@{
                    Project = $project.Name
                    File = $file.FullName.Substring($repoRootResolved.Length).TrimStart('\\', '/')
                    Line = $startLine
                    Method = $methodName
                    Parameter = "$primaryTypeRaw $primaryName"
                    Expected = 'string?'
                    Actual = $primaryTypeRaw
                }) | Out-Null
            }
            elseif ($primaryType -eq 'string?') {
                # ok
            }
            else {
                if ($isNullableValueType -and ($valueTypes -contains $innerNullableType)) {
                    $violations.Add([PSCustomObject]@{
                        Project = $project.Name
                        File = $file.FullName.Substring($repoRootResolved.Length).TrimStart('\\', '/')
                        Line = $startLine
                        Method = $methodName
                        Parameter = "$primaryTypeRaw $primaryName"
                        Expected = "$innerNullableType (non-nullable)"
                        Actual = $primaryTypeRaw
                    }) | Out-Null
                }
            }
        }
    }
}

Write-PineGuardAuditHeader -AuditRuleId $AuditRuleId -Title 'Hybrid nullability strategy (Must/Guard primary parameter)' -RepoRoot $repoRootResolved -OutputPath $OutputPath
Write-Host "Violations: $($violations.Count)" -ForegroundColor Yellow

$sb = New-Object System.Text.StringBuilder
$null = $sb.AppendLine("AuditRule: $AuditRuleId - Hybrid nullability strategy (Must/Guard primary parameter)")
$null = $sb.AppendLine("RepoRoot : $repoRootResolved")
$null = $sb.AppendLine("Output  : $resolvedOutputPath")
$null = $sb.AppendLine("Violations: $($violations.Count)")
$null = $sb.AppendLine('')

foreach ($v in $violations | Sort-Object Project, File, Line, Method) {
    $null = $sb.AppendLine("$($v.Project) $($v.File):$($v.Line) $($v.Method)($($v.Parameter))")
    $null = $sb.AppendLine("  Expected: $($v.Expected)")
    $null = $sb.AppendLine("  Actual  : $($v.Actual)")
    $null = $sb.AppendLine('')
}

$sb.ToString() | Out-File -FilePath $resolvedOutputPath -Encoding utf8

if ($violations.Count -gt 0 -and -not $AllowViolations.IsPresent) {
    throw "Audit failed: $($violations.Count) hybrid nullability violations found."
}
