<#
.SYNOPSIS
    Test Rule10 PS Normalization

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER FailOnFindings
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')
. (Join-Path $PSScriptRoot 'Load-Catalog.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule10' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule10." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule10' -Title 'PowerShell normalization (help header + param formatting)' -RepoRoot $repoRootResolved -OutputPath $outputPath

function Convert-ToRepoRelativePath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$Path
    )

    $rel = [System.IO.Path]::GetRelativePath($RepoRoot, $Path)
    $rel.Replace('\\', '/').Replace('\', '/')
}

function Test-IsInScopePsFile {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$RelativePath
    )

    $p = $RelativePath

    if ($p.StartsWith('artifacts/', [System.StringComparison]::OrdinalIgnoreCase)) { return $false }
    if ($p.StartsWith('logs/', [System.StringComparison]::OrdinalIgnoreCase)) { return $false }
    if ($p.StartsWith('.etc/', [System.StringComparison]::OrdinalIgnoreCase)) { return $false }

    if ($p.StartsWith('.git/', [System.StringComparison]::OrdinalIgnoreCase)) { return $false }
    if ($p.StartsWith('.vs/', [System.StringComparison]::OrdinalIgnoreCase)) { return $false }

    if ($p -match '(?i)(^|/)bin(/|$)') { return $false }
    if ($p -match '(?i)(^|/)obj(/|$)') { return $false }

    return $true
}

function Get-TopHelpBlock {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$Text
    )

    $m = [regex]::Match($Text, '(?s)^\s*<#(.*?)#>')
    if (-not $m.Success) { return $null }

    '<#' + $m.Groups[1].Value + '#>'
}

function Get-HelpParameterNames {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$HelpBlock
    )

    $names = New-Object System.Collections.Generic.List[string]

    foreach ($m in [regex]::Matches($HelpBlock, '(?im)^\s*\.PARAMETER\s+(?<n>[A-Za-z_][A-Za-z0-9_]*)\s*$')) {
        $n = $m.Groups['n'].Value
        if (-not [string]::IsNullOrWhiteSpace($n)) {
            $names.Add($n) | Out-Null
        }
    }

    @($names | Select-Object -Unique)
}

$findings = New-Object System.Collections.Generic.List[string]

function Add-Finding {
    param(
        [Parameter(Mandatory = $true)][string]$RelativePath,
        [Parameter(Mandatory = $true)][string]$Message
    )

    $findings.Add("[$RelativePath] $Message") | Out-Null
}

$files = Get-ChildItem -LiteralPath $repoRootResolved -Recurse -File -Filter '*.ps1'

foreach ($f in $files) {
    $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $f.FullName
    if (-not (Test-IsInScopePsFile -RelativePath $rel)) { continue }

    $text = Get-Content -LiteralPath $f.FullName -Raw

    $tokens = $null
    $parseErrors = $null
    $ast = [System.Management.Automation.Language.Parser]::ParseInput($text, [ref]$tokens, [ref]$parseErrors)

    if ($parseErrors -and $parseErrors.Count -gt 0) {
        Add-Finding -RelativePath $rel -Message ("Parse error(s): {0}" -f (($parseErrors | Select-Object -First 1).Message))
        continue
    }

    $hasCmdletBinding = ($text -match '(?im)^\s*\[\s*CmdletBinding\b')

    $paramBlock = $ast.ParamBlock
    if ($paramBlock -and $paramBlock.Parameters) {
        foreach ($p in $paramBlock.Parameters) {
            if ($p.Extent.StartLineNumber -ne $p.Extent.EndLineNumber) {
                Add-Finding -RelativePath $rel -Message ("Parameter '{0}' is not single-line." -f $p.Name.VariablePath.UserPath)
            }
        }
    }

    if ($hasCmdletBinding -and $paramBlock) {
        $paramExtent = $paramBlock.Extent
        $commentTokens = @(
            $tokens |
                Where-Object {
                    $_.Kind -eq 'Comment' -and
                    $_.Extent.StartOffset -ge $paramExtent.StartOffset -and
                    $_.Extent.EndOffset -le $paramExtent.EndOffset
                }
        )

        if ($commentTokens.Count -gt 0) {
            Add-Finding -RelativePath $rel -Message "Param block contains comment(s); comments are not allowed inside param(...) for CmdletBinding scripts."
        }
    }

    if ($hasCmdletBinding) {
        $help = Get-TopHelpBlock -Text $text
        if (-not $help) {
            Add-Finding -RelativePath $rel -Message 'Missing comment-based help block at top of file.'
            continue
        }

        if ($help -notmatch '(?im)^\s*\.SYNOPSIS\b') {
            Add-Finding -RelativePath $rel -Message 'Help block missing .SYNOPSIS.'
        }

        if ($help -notmatch '(?im)^\s*\.DESCRIPTION\b') {
            Add-Finding -RelativePath $rel -Message 'Help block missing .DESCRIPTION.'
        }

        $helpParamNames = Get-HelpParameterNames -HelpBlock $help

        $paramNames = @()
        if ($paramBlock -and $paramBlock.Parameters) {
            $paramNames = @($paramBlock.Parameters | ForEach-Object { $_.Name.VariablePath.UserPath })
        }

        foreach ($paramName in $paramNames) {
            if (-not ($helpParamNames -contains $paramName)) {
                Add-Finding -RelativePath $rel -Message ("Help block missing .PARAMETER {0}." -f $paramName)
            }
        }
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath

if ($findings.Count -eq 0) {
    'PS normalization: PASS' | Out-File -LiteralPath $reportPath -Encoding utf8
    Write-Host 'PS normalization: PASS' -ForegroundColor Green
    exit 0
}

$findings | Out-File -LiteralPath $reportPath -Encoding utf8
Write-Host ("PS normalization: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red

if ($FailOnFindings.IsPresent) {
    exit 1
}

exit 0
