<#
.SYNOPSIS
    Shared directory-cleaning helper for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Clear-ToolDirectory into the calling script's scope.

    Consolidates the enumerate-then-ShouldProcess-then-Remove-Item loop, extension-wildcard
    normalization, and "Deleted: <name>" reporting that used to be separately implemented across
    tools/clean/Clear-Artifacts.ps1, Clear-Logs.ps1, and Clear-Root.ps1 (F-38). Per T1.09, those
    three scripts are no longer behaviorally identical — Root never recurses and applies a hard
    exclusion list, Artifacts always recurses unconditionally, Logs keeps an optional -Recursive
    switch — so this function does not decide recursion or exclusions on its own. It only knows
    how to walk one directory for one set of extension filters, ShouldProcess each match, and
    delete it; every behavioral difference is expressed by what each caller passes in (AllowRecurse,
    ExcludeNames), never by anything this function assumes.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Clear-ToolDirectory {
    <#
    .SYNOPSIS
        Deletes files matching one or more extension filters from a directory.

    .DESCRIPTION
        Enumerates $Path for files matching each entry in $Extensions (normalizing bare extension
        names like 'log' to '*.log'; an entry that already contains '.', or is exactly '*', passes
        through unchanged), skips anything named in $ExcludeNames, previews or deletes each match
        via $PSCmdlet.ShouldProcess, and reports "Deleted: <name>" for each file actually removed.

        This function has no opinion on whether '*' is a safe filter for the caller's directory
        (Clear-Root.ps1 refuses a bare '*' before ever calling this function, F-27) — it just
        matches whatever filters it is given.

    .PARAMETER Path
        Directory to clean. Must already exist — callers are responsible for that check.

    .PARAMETER Extensions
        Extensions to match, e.g. 'txt', 'log', '*'. Each is normalized to a Get-ChildItem -Filter
        wildcard.

    .PARAMETER ItemLabel
        Noun phrase used in the ShouldProcess action text ("Delete <ItemLabel>"), e.g.
        'Artifact File', 'Log File', 'Root File'.

    .PARAMETER AllowRecurse
        Recurse into subdirectories of Path. Omit this switch entirely at the call site — never
        pass it, not even as $false — for a script that must never be able to recurse:
        Clear-Root.ps1 (F-27) never passes it, so no parameter combination makes Root recursive.

    .PARAMETER ExcludeNames
        File names to skip even when they match an extension filter (Clear-Root.ps1's hard
        exclusion list, F-27). Empty by default.

    .EXAMPLE
        Clear-ToolDirectory -Path $artifactsDir -Extensions @('*') -ItemLabel 'Artifact File' -AllowRecurse
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)]
        [string] $Path,

        [Parameter(Mandatory)]
        [string[]] $Extensions,

        [Parameter(Mandatory)]
        [string] $ItemLabel,

        [switch] $AllowRecurse,

        [string[]] $ExcludeNames = @()
    )

    foreach ($ext in $Extensions) {
        $filter = if ($ext -eq '*') { '*' } elseif ($ext -like '*.*') { $ext } else { "*.$ext" }

        $gciParams = @{
            Path = $Path
            Filter = $filter
            File = $true
            Force = $true
        }
        if ($AllowRecurse) {
            $gciParams['Recurse'] = $true
        }

        $files = Get-ChildItem @gciParams

        foreach ($file in $files) {
            if ($ExcludeNames -contains $file.Name) {
                continue
            }

            if ($PSCmdlet.ShouldProcess($file.FullName, "Delete $ItemLabel")) {
                Remove-Item -LiteralPath $file.FullName -Force
                Write-Host "Deleted: $($file.Name)" -ForegroundColor Gray
            }
        }
    }
}
