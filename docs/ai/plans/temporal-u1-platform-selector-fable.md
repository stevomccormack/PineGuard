# T1-U1 platform-selector coverage ruling

**Reviewer:** Claude Fable 5.1, persisted U1 review session `7ecbfc2e-3612-45de-86b4-bf2c6cbecb5e`  
**Outcome:** Binding implementation decision; U1 remains in progress until implementation and all required gates pass.  
**Public CLI evidence:** [`T1-U1-os-branch-followup.jsonl`](../../../artifacts/reviews/T1-U1-os-branch-followup.jsonl)

## Finding

The `Testing` net8.0 coverage run measured 100.00% line coverage (6459/6459) and 99.21% branch coverage (125/126), across 446 matched classes. The only missed branch is the Windows/non-Windows conditional used to select `TestTimeZones.PlatformSampleId`. The `PineGuard.Testing.UnitTests` run passed 698/698. The Cobertura report is under `artifacts/code-coverage/coverlet/testing/testresults/PineGuard.Testing.UnitTests/759c33fd-57e2-42d5-95a4-fd07d5e254e8/coverage.cobertura.xml`. No net10.0 coverage result was available for this ruling.

The Brain requires exactly 100% line and branch coverage within the filtered scope and deterministic tests (`docs/ai/specs/testing/coverage.md`, `docs/ai/specs/testing/unit-test.md`). The plan requires the Windows registry ID on Windows and the IANA ID elsewhere, while the two named platform tests remain host integration checks (`temporal-01-foundation.md`, U1). The mapping is pure data selection; only the OS probe needs the actual host.

## Binding decision

Keep the production initializer wired to the real `OperatingSystem.IsWindows()` result, and move only the two-value mapping into an internal pure selector:

```csharp
public static readonly string PlatformSampleId = GetPlatformSampleId(OperatingSystem.IsWindows());

internal static string GetPlatformSampleId(bool isWindows) =>
    isWindows ? "Pacific Standard Time" : "America/Los_Angeles";
```

Add a paired `TheoryData` group and theory that directly exercise both selector inputs: `true` maps to `Pacific Standard Time`; `false` maps to `America/Los_Angeles`. Follow the Brain's `nameof` test-case naming convention. Do not alter the existing platform resolution/classification cases or their data. The selector test covers PineGuard's deterministic mapping; it does not simulate an OS or claim the BCL probe was exercised on both operating systems. Existing friend-assembly configuration already grants `PineGuard.Testing.UnitTests` access to internals in `tests/PineGuard.Testing/PineGuard.Testing.csproj`.

Fable rejected a single canonical IANA ID because it changes the plan's Windows-ID behavior and introduces runtime/globalization prerequisites; an OS matrix cannot satisfy the local per-TFM 100% gate because CI is Ubuntu-only and the gate does not merge coverage across operating systems. Coverage exclusions, suppressions, and gate weakening remain disallowed.

## Required follow-up

Implement only these files:

- `tests/PineGuard.Testing/Common/TestTimeZones.cs`
- `tests/PineGuard.Testing.UnitTests/Common/TestTimeZonesTestData.cs`
- `tests/PineGuard.Testing.UnitTests/Common/TestTimeZonesTests.cs`

Then run format, compiler diagnostics, the Testing tests, and isolated Testing coverage separately for net8.0 and net10.0. Confirm the Cobertura report shows 100% line and branch coverage for `TestTimeZones`; expect the two selector cases to add two tests per TFM. Do not report net10.0 as passing until that run completes. This ruling does not itself accept U1.
