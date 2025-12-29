# PineGuard

PineGuard is a .NET validation/guard library with reusable rules, standards helpers, and integrations.

## Projects

- `src/PineGuard.Core/` – core rules + common types (net8.0)
- `src/PineGuard.DataAnnotations/` – DataAnnotations integration
- `src/PineGuard.FluentValidation/` – FluentValidation integration
- `src/PineGuard.GuardClauses/` – guard-clause helpers
- `src/PineGuard.MustClauses/` – “Must” clause helpers
- `tests/PineGuard.UnitTests.Core/` – xUnit unit tests

## Build / test

From repo root:

```powershell
dotnet restore
dotnet build

# run unit tests
dotnet test ./tests/PineGuard.UnitTests.Core/PineGuard.UnitTests.Core.csproj
```

## ISO reference data generation

This repo includes deterministic generators for ISO reference data (countries/currencies/languages). See `.iso/README.md` for usage:

- `./.iso/GenerateIsoCountries.ps1`
- `./.iso/GenerateIsoCurrencies.ps1`
- `./.iso/GenerateIsoLanguages.ps1`

By default, scripts generate into `.iso/generated`. Use `-EnableUpdateTarget $true` to copy outputs into `src/`.

## IANA time zone data generation

This repo includes a deterministic generator for IANA tzdb time zone reference data (from `zone1970.tab`). See `.iana/tz-zone-info/README.md` for usage:

- `./.iana/GenerateIanaTimeZones.ps1`

By default, the script generates into `.iana/generated`. Use `-EnableUpdateTarget $true` to copy outputs into `src/`.
