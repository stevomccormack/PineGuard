---
applyTo: "tests/**"
---

Read [global rules](../../docs/ai/rules/global.md) and [unit-tests rules](../../docs/ai/rules/testing.md) before editing test code.
All tests are `[Theory]` + `TheoryData` — never `[Fact]` — and every `XxxTests.cs` has a paired `XxxTestData.cs`; CI blocks merges on audit-cli Rule50.
