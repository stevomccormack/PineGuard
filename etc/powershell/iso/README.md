# ISO data generation (etc/powershell/iso)

This folder contains the scripts + source CSVs used to generate ISO reference data in PineGuard.

## What gets generated

- Countries (ISO 3166-1)
- Currencies (ISO 4217)
- Languages (ISO 639-1)

Generated C# files are written to:

- `src/PineGuard.Core/Iso/**/DefaultIso*Data.cs`

## How to run

Run scripts from the repository root.

Preview (generate into `etc/powershell/iso/generated` only):

```powershell
./etc/powershell/iso/GenerateIsoCountries.ps1
./etc/powershell/iso/GenerateIsoCurrencies.ps1
./etc/powershell/iso/GenerateIsoLanguages.ps1
```

Update source (also copy into `src/`):

```powershell
./etc/powershell/iso/GenerateIsoCountries.ps1 -EnableUpdateTarget
./etc/powershell/iso/GenerateIsoCurrencies.ps1 -EnableUpdateTarget
./etc/powershell/iso/GenerateIsoLanguages.ps1 -EnableUpdateTarget
```

## Code casing

- ISO 3166-1 country codes: UPPERCASE (e.g., `US`, `GBR`)
- ISO 4217 currency codes: UPPERCASE (e.g., `USD`, `EUR`)
- ISO 639-1 language codes: lowercase (e.g., `en`, `eng`)

## Notes

- Scripts are intended to be deterministic and safe to rerun.
- ISO content can have licensing restrictions; prefer official/authorized datasets when available.
