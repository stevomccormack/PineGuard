# PineGuard

PineGuard is a .NET validation/guard library with reusable rules, standards helpers, and integrations.

## Quick start

PineGuard’s core API is a small “fluent root” (`Must.Be` / `Guard.Against`) plus extension methods that return a `MustResult<T>`. You can either:

- check `result.Success`, or
- call `result.ThrowIfFailed()` to raise a friendly `ArgumentException`.

Example (Must clauses):

```csharp
using PineGuard.MustClauses;

Must.Be.NotNullOrWhitespace(userName).ThrowIfFailed();

var amountResult = Must.Be.ZeroOrPositive(amount);
if (!amountResult.Success)
	return BadRequest(amountResult.Message);
```

## FluentValidation integration

PineGuard provides a small adapter so you can plug `Must.Be.*` checks into FluentValidation.

```csharp
using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

public sealed class CreateUserValidator : AbstractValidator<CreateUser>
{
	public CreateUserValidator()
	{
		RuleFor(x => x.UserName)
			.MustBe(value => Must.Be.NotNullOrWhitespace(value), message: null);
	}
}
```

## Projects

- `src/PineGuard.Core/` – rules, utilities, guard/must primitives (net8.0)
- `src/PineGuard.MustClauses/` – “Must” clause extension methods
- `src/PineGuard.FluentValidation/` – FluentValidation adapter
- `src/PineGuard.DataAnnotations/` – DataAnnotations integration (currently a placeholder project)
- `src/PineGuard.GuardClauses/` – guard-clause helpers (currently a placeholder project)

## Build / test

From repo root:

```powershell
dotnet restore
dotnet build

# run all unit tests
dotnet test

# or run a single project
dotnet test ./tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj
```

## Documentation (contributors + AI)

- Code coverage agent spec: [docs/ai/code-coverage-agent-spec.md](docs/ai/code-coverage-agent-spec.md)
- Unit test agent spec: [docs/ai/unit-tests-agent-spec.md](docs/ai/unit-tests-agent-spec.md)

## ISO reference data generation

This repo includes deterministic generators for ISO reference data (countries/currencies/languages). See `etc/powershell/iso/README.md` for usage:

- `./etc/powershell/iso/GenerateIsoCountries.ps1`
- `./etc/powershell/iso/GenerateIsoCurrencies.ps1`
- `./etc/powershell/iso/GenerateIsoLanguages.ps1`

By default, scripts generate into `etc/generated`. Use `-EnableUpdateTarget $true` to copy outputs into `src/PineGuard.Core/Externals/...`.

## IANA time zone data generation

This repo includes a deterministic generator for IANA tzdb time zone reference data (from `zone1970.tab`). See `etc/powershell/iana/README.md` for usage:

- `./etc/powershell/iana/GenerateIanaTimeZones.ps1`

By default, the script generates into `etc/generated`. Use `-EnableUpdateTarget $true` to copy outputs into `src/PineGuard.Core/Externals/...`.
