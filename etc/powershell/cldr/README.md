# CLDR data generation (etc/powershell/cldr)

This folder contains scripts + source files used to generate CLDR reference data for PineGuard.

## Windows time zone mappings

PineGuard uses CLDR's Windows time zone mapping data to relate Windows time zone IDs to IANA tzdb IDs.

### Source

- CLDR release: https://unicode.org/Public/cldr/
- For CLDR 48, download `cldr-common-48.zip` and extract:
  - `common/supplemental/windowsZones.xml`

Place the extracted file at:

- `./etc/powershell/cldr/cldr-windows-zones/windowsZones.xml`

### What gets generated

- Default Windows -> IANA time zone mapping dataset.

Generated C# files are written to:

- `src/PineGuard.Core/Externals/Cldr/TimeZones/DefaultCldrWindowsTimeZoneData.cs`

### How to run

Run scripts from the repository root.

Preview (generate into `etc/generated` only):

```powershell
./etc/powershell/cldr/GenerateCldrWindowsTimeZones.ps1
```

Update source (also copy into `src/`):

```powershell
./etc/powershell/cldr/GenerateCldrWindowsTimeZones.ps1 -EnableUpdateTarget $true
```
