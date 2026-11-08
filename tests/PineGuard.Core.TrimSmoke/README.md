# Core trimmed consumer smoke test

This standalone executable inherits the repository and tests build policies. It is intentionally outside the solution's ordinary xUnit registry: publishing and running the linked application is required to verify trimming.

The consumer discovers enumeration members only through `GetAll`, `FromName` and `FromValue`. It never references member fields directly. It verifies inherited and declared fields, declaring-type initialization, ordinal catalogue order, case-sensitive value identity, case-insensitive names and cached catalogue identity.

Run from the repository root on Windows:

```powershell
foreach ($framework in @('net8.0', 'net10.0')) {
    dotnet publish tests/PineGuard.Core.TrimSmoke/PineGuard.Core.TrimSmoke.csproj -c Release -f $framework -r win-x64 -o artifacts/trim-smoke/$framework
    if ($LASTEXITCODE -ne 0) { throw "Trim publication failed for $framework" }
    & ./artifacts/trim-smoke/$framework/PineGuard.Core.TrimSmoke.exe
    if ($LASTEXITCODE -ne 0) { throw "Trim consumer failed for $framework" }
}
```

For another platform, select its runtime identifier and executable filename. Both target frameworks must publish without compiler or linker warnings and print `Enumeration trimmed consumer passed.` No trimming warnings are suppressed.