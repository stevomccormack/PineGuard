# `.iso`

This folder contains scripts and source data used to generate ISO reference data files in the codebase.

- Generated output is committed into `src/PineGuard.Core/Standards/Iso/**/DefaultIso*Data.cs`.
- Scripts should be deterministic and safe to run repeatedly.

## Planned generators

- `GenerateIsoCurrencies.ps1` (ISO 4217)
- `GenerateIsoLanguages.ps1` (ISO 639)

> Note: ISO content has licensing restrictions. Prefer using the official maintenance agency datasets (e.g., SIX) when available.
