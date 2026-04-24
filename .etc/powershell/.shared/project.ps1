<#
.SYNOPSIS
    project

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/.shared/project.ps1

# -------------------------------------------------------------------------------------------------
# Project Variables
# -------------------------------------------------------------------------------------------------

$Project = New-GitHubProject `
    -Owner 'stevomccormack' `
    -Repository 'PineGuard' `
    -Name 'PineGuard' `
    -Description '' `
    -LocalPath (Join-Path 'D:\Steve McCormack\GitHub\@stevomccormack' 'PineGuard') `
    -UserName 'Steve McCormack' `
    -UserEmail 'hello@iamstevo.co' `
    -MainBranch 'main' `
    -FastForward $false `
    -UseRebase $true
        
# -------------------------------------------------------------------------------------------------

if ($Global.Log.Enabled) {
    Write-Header "`$Project` Variable:"
    # Write-ObjectPathTree -Object $Projects -RootPath '$Projects'   

    $Project | Format-List
}