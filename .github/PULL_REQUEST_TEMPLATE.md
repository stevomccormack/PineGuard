## Summary

<!-- Brief description of what this PR does and why. -->

## Changes

-

## Checklist

- [ ] Gold standard code
- [ ] `dotnet build` succeeds with zero warnings
- [ ] All unit tests pass
- [ ] Code coverage is 100%
- [ ] `dotnet format` clean (no formatting violations)
- [ ] `audit-cli` `test-files` passes — all tests are `[Theory]` + `TheoryData` (never `[Fact]`) and every `XxxTests.cs` is paired with an `XxxTestData.cs` (`pnpm -C apps/cli exec tsx src/index.ts audit test-files`)
- [ ] Qodana reports zero problems (when `QODANA_ENABLED`)

