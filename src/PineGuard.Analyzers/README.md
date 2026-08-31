# PineGuard.Analyzers

**Your editor already knows that `if (x is null) throw new ArgumentNullException(nameof(x));` is a guard clause. Now it can write one.**

PineGuard.Analyzers is a Roslyn analyzer package that watches for the hand-rolled argument checks every C# codebase accumulates and offers the equivalent PineGuard guard clause as a one-click fix. It is silent in any project that does not reference PineGuard, and it never reports inside PineGuard's own assemblies.

**One rule library. Every call site in your architecture.** The guard the fix writes is the same clause your options binding, your request filters and your domain constructors already use.

## Install

```bash
dotnet add package PineGuard.Analyzers
```

Ships as a development dependency: it flows into your build and your IDE, and it is never a dependency of anything you publish.

### Supported frameworks

The analyzer assemblies target `netstandard2.0` and load into any Roslyn 4.14-or-later compiler — the .NET 8 SDK and newer.

## Diagnostics

| Id | Title | Category | Severity |
|---|---|---|---|
| `PG1001` | Use `Guard.Against.Null` | Usage | Info |
| `PG1002` | Use `Guard.Against.NullOrWhiteSpace` | Usage | Info |
| `PG1003` | Use `Guard.Against.NullOrEmpty` | Usage | Info |
| `PG1004` | Use `Guard.Against.OutOfRange` | Usage | Info |
| `PG2001` | Must result is discarded | Reliability | Warning |

`PG1xxx` means *prefer a guard clause*; `PG2xxx` means *guard or validation misuse*. Every diagnostic ships with a code fix, and every fix supports fix-all across a document, project or solution.

### PG2001 — Must result is discarded

A Must clause never throws on its own; it hands back a `MustResult<T>` to inspect. Calling one as a statement therefore checks nothing, and no compiler warning says so.

```csharp
Must.Be.NotNull(name);                       // checks nothing
Must.Be.NotNull(name).ThrowIfFailed();       // fix: throw if failed
var result = Must.Be.NotNull(name);          // fix: assign the result
```

## Reporting rules

- **Silent without PineGuard.** `PG1xxx` reports only when the compilation can resolve `PineGuard.GuardClauses.Guard`, and `PG2xxx` only when it can resolve PineGuard's result types. A project that has not installed PineGuard sees nothing.
- **Never inside PineGuard.** Diagnostics are suppressed in assemblies whose name starts with `PineGuard.` — the library's own `ThrowHelper` is exactly the pattern `PG1001` targets, and it must stay.

## Links

- [PineGuard on GitHub](https://github.com/stevomccormack/PineGuard)
- [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
