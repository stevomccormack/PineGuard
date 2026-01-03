# IANA Time Zone data generation (etc/powershell/iana)

This folder contains scripts + source files used to generate IANA time zone reference data in PineGuard.

## Source

- `iana-tz-zone-info/zone1970.tab` from the IANA Time Zone Database (tzdb)
- This file is in the public domain (see header comments inside `zone1970.tab`).

## What gets generated

- Default IANA time zone dataset (zone id, country codes, coordinates, comments)

Generated C# files are written to:

- `src/PineGuard.Core/Iana/TimeZones/DefaultIanaTimeZoneData.cs`

## How to run

Run scripts from the repository root.

Preview (generate into `etc/powershell/iana/generated` only):

```powershell
./etc/powershell/iana/GenerateIanaTimeZones.ps1
```

Update source (also copy into `src/`):

```powershell
./etc/powershell/iana/GenerateIanaTimeZones.ps1 -EnableUpdateTarget
```

## File layout

- Inputs (tzdb artifacts + template + generator): `./etc/powershell/iana/iana-tz-zone-info/`
- Preview output: `./etc/powershell/iana/generated/DefaultIanaTimeZoneData.cs`
- Source output (when enabled): `./src/PineGuard.Core/Iana/TimeZones/DefaultIanaTimeZoneData.cs`

## Notes

- The generated dataset is OS-independent. It validates tzdb identifiers and country mappings.
- Converting IANA zone IDs to `TimeZoneInfo` is OS-dependent (Windows typically uses Windows time zone IDs).
