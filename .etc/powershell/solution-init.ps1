<#
.SYNOPSIS
    solution init

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/solution-init.ps1

. ".etc/powershell/.shared/index.ps1"

# ------------------------------------------------------------------------------------------------

$project = $Project
$solution = $Solution

# ------------------------------------------------------------------------------------------------

Write-MastHead "$($project.Name) Project: Setup Solution"
Write-Var -Name "Project Name" -Value $project.Name -NoIcon
Write-Var -Name "Project Path" -Value $project.LocalPath -NoIcon
Write-Var -Name "Solution Name" -Value $solution.Name -NoIcon
Write-Var -Name "Solution Path" -Value $solution.Path -NoIcon
Write-NewLine

# ------------------------------------------------------------------------------------------------

$allProjects = @()

if ($solution.Projects) {
    $allProjects += $solution.Projects
}

if ($solution.TestProjects) {
    $allProjects += $solution.TestProjects
}

if (-not $allProjects -or $allProjects.Count -eq 0) {
    Write-FailMessage -Title "Packages" -Message "No projects found on `$Solution.Projects / `$Solution.TestProjects"
    return
}

# ------------------------------------------------------------------------------------------------

$projectPathByName = @{}
foreach ($p in $allProjects) {
    if (-not $p) {
        continue
    }

    if (-not [string]::IsNullOrWhiteSpace($p.Name) -and -not [string]::IsNullOrWhiteSpace($p.Path)) {
        $projectPathByName[$p.Name] = $p.Path
    }
}

# ------------------------------------------------------------------------------------------------

# .NET class library projects setup
if ($solution.Projects) {
    foreach ($p in $solution.Projects) {
        if (-not $p) {
            continue
        }

        $projectName = $p.Name
        $projectDir = $p.DirPath
        $projectCsprojPath = $p.Path
        $projectDotNetVersion = $p.DotNetVersion

        # Ensure project directory exists
        if (-not (Test-EnsureDirectory -Path $projectDir)) {
            return
        }

        # Create project if it does not exist
        if (-not (Test-Path -LiteralPath $projectCsprojPath -PathType Leaf)) {
            Write-StatusMessage -Title "Project .NET Class Library" -Message "Creating .NET class library project: $projectCsprojPath"
            dotnet new classlib -f $projectDotNetVersion -n $projectName -o $projectDir
            if ($LASTEXITCODE -ne 0) {
                Write-FailMessage -Title "dotnet" -Message "Failed: dotnet new classlib (-f $projectDotNetVersion -n $projectName -o $projectDir)"
                return
            }
        }
        else {
            Write-Status "Project exists: $projectCsprojPath"
        }
    }
}

# .NET xunit test projects setup
if ($solution.TestProjects) {
    foreach ($p in $solution.TestProjects) {
        if (-not $p) {
            continue
        }

        $projectName = $p.Name
        $projectDir = $p.DirPath
        $projectCsprojPath = $p.Path
        $projectDotNetVersion = $p.DotNetVersion

        # Ensure project directory exists
        if (-not (Test-EnsureDirectory -Path $projectDir)) {
            return
        }

        # Create project if it does not exist
        if (-not (Test-Path -LiteralPath $projectCsprojPath -PathType Leaf)) {
            Write-StatusMessage -Title "Project xUnit Test Creation" -Message "Creating .NET (XUnit) test project: $projectCsprojPath"
            dotnet new xunit -f $projectDotNetVersion -n $projectName -o $projectDir
            if ($LASTEXITCODE -ne 0) {
                Write-FailMessage -Title "dotnet" -Message "Failed: dotnet new xunit (-f $projectDotNetVersion -n $projectName -o $projectDir)"
                return
            }
        }
        else {
            Write-Status "XUnit test project exists: $projectCsprojPath"
        }

    }
}

# Project references + packages setup (all projects)
if ($allProjects) {
    foreach ($p in $allProjects) {
        if (-not $p) {
            continue
        }

        $projectName = $p.Name
        $projectDir = $p.DirPath
        $projectCsprojPath = $p.Path
        $projectDotNetVersion = $p.DotNetVersion

        # Guard: project csproj exists
        if (-not (Test-Path -LiteralPath $projectCsprojPath -PathType Leaf)) {
            Write-FailMessage -Title "Project" -Message "Project csproj not found: $projectCsprojPath"
            continue
        }

        # Configure project references
        if ($p.References -and $p.References.Count -gt 0) {
            foreach ($ref in $p.References) {
                if ([string]::IsNullOrWhiteSpace($ref)) {
                    continue
                }

                $refCsprojPath = $null

                if ($projectPathByName.ContainsKey($ref)) {
                    $refCsprojPath = $projectPathByName[$ref]
                }
                else {
                    $refCsprojPath = $ref
                }

                if (-not (Test-Path -LiteralPath $refCsprojPath -PathType Leaf)) {
                    Write-FailMessage -Title "Project reference" -Message "Reference csproj not found: $refCsprojPath"
                    continue
                }

                $refProjectName = $ref
                if (-not $projectPathByName.ContainsKey($ref)) {
                    $refProjectName = [System.IO.Path]::GetFileNameWithoutExtension($refCsprojPath)
                }

                if (Test-DotNetProjectHasProjectReference -CsprojPath $projectCsprojPath -ProjectName $refProjectName) {
                    Write-StatusMessage -Title "Project References" -Message "Project reference exists '$refProjectName' in project: $projectName"
                    continue
                }

                Write-StatusMessage -Title "Project References" -Message "Adding project reference '$refCsprojPath' to project: $projectName"
                dotnet add $projectCsprojPath reference $refCsprojPath
                if ($LASTEXITCODE -ne 0) {
                    Write-FailMessage -Title "dotnet" -Message "Failed: dotnet add reference ($projectCsprojPath -> $refCsprojPath)"
                    return
                }
            }
        }
        else {
            Write-Status "No references: $projectName"
        }

        # Configure packages for projects
        if (-not $p.Packages -or $p.Packages.Count -eq 0) {
            Write-Status "No packages: $projectName"
        }
        else {
            Push-Location $projectDir

                foreach ($pkg in $p.Packages) {
                    if ([string]::IsNullOrWhiteSpace($pkg)) {
                        continue
                    }

                    $packageId = ($pkg -split "\s+")[0]
                    if ([string]::IsNullOrWhiteSpace($packageId)) {
                        continue
                    }

                    if (Test-DotNetProjectHasPackageReference -CsprojPath $projectCsprojPath -PackageId $packageId) {
                        Write-StatusMessage -Title "Project Packages" -Message "Package exists '$packageId' in project: $projectName"
                        continue
                    }

                    Write-StatusMessage -Title "Project Packages" -Message "Adding package '$packageId' to project: $projectName"
                    dotnet add $projectCsprojPath package $packageId
                    if ($LASTEXITCODE -ne 0) {
                        Write-FailMessage -Title "dotnet" -Message "Failed: dotnet add package ($projectCsprojPath -> $packageId)"
                        return
                    }
                }

            Pop-Location
        }

        Write-OkMessage -Title "Project" -Message "Setup complete for project: $projectName"
        Write-NewLine
    }
}

# ------------------------------------------------------------------------------------------------

dotnet clean $solution.Path
dotnet restore $solution.Path
dotnet build $solution.Path --no-restore

# ------------------------------------------------------------------------------------------------

dotnet test $solution.Path --no-build --no-restore

# ------------------------------------------------------------------------------------------------

Write-OkMessage -Title "Solution" -Message "Solution completed: $($solution.Name)"
Write-NewLine
