# Skill: Implement Core Rule/Util
**ID**: pineguard.skill.scaffold-rule
**Version**: 1.0

## 1. Context & Goal
Implement a low-level validation primitive (`Rule`) or parsing helper (`Util`) in `PineGuard.Core`. This is the foundation for Must/Guard clauses.

## 2. Inputs
- **Type**: (Rule vs Util)
- **Domain**: (e.g., "Json", "String", "Network")
- **Logic**: What needs to be validated or parsed?

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> 1.  **Pure Logic**: Rules/Utils must contain *logic only*. No user-facing messages.
> 2.  **No Exceptions**: `Try*` methods return `false` on failure. `Is*` methods return `false`. Do not throw for invalid input (only `ArgumentNullException` for null *config* parameters is allowed).
> 3.  **Determinism**: No IO (File/Network) in Core Rules.
> 4.  **Separation** (see `docs/ai/specs/core/project.md` §4.1):
>     *   `Utils`: Parsing/Normalization (`TryParse`, `TryNormalize`) — return parsed values via `out` parameters. MustClauses prefer Utils when they need the parsed result for `MustResult<T>.Result`.
>     *   `Rules`: Predicates (`IsX`, `HasX`) — return `bool` only. Call Utils internally for parsing; do not duplicate logic. Used when callers need only a yes/no answer.
> 5.  **Strict Coding**: File-scoped namespaces, sorted usings, arrow functions where possible, `value` parameter naming.

## 4. Execution Steps

1.  **Determine Pattern (Simple vs Complex)**
    *   **Simple Domain**: Public static class directly in `Rules/`.
    *   **Complex Domain** (multiple sub-domains):
        *   Implementation: `internal static` class in `Rules/[Domain]/`.
        *   Facade: `public static partial` class (the Facade) in `Rules/[Domain]Rules.cs`.

2.  **Determine Location**
    *   Simple: `src/PineGuard.Core/Rules/[Domain]Rules.cs`.
    *   Complex: `src/PineGuard.Core/Rules/[Domain]/[Name]Rules.cs` (Internal).

2.  **Implement Util (if parsing needed)**
    ```csharp
    using System.Diagnostics.CodeAnalysis;

    namespace PineGuard.Core.Utils;

    public static class MyUtility
    {
        public static bool TryParse(string? value, [NotNullWhen(true)] out T? result)
        {
            if (value is null)
            {
                result = default;
                return false;
            }
            // Logic...
        }
    }
    ```

3.  **Implement Rule (Predicate)**
    ```csharp
    using PineGuard.Core.Utils;

    namespace PineGuard.Core.Rules;

    public static class MyRules
    {
        public static bool IsCondition(string? value) =>
            value is not null && MyUtility.TryParse(value, out var result) && CheckResult(result);
    }
    ```

## 5. Definition of Done
- [ ] Code compiles.
- [ ] No allocations in hot paths (use `ReadOnlySpan<char>` if applicable).
- [ ] Null inputs handled explicitly (usually return `false`).

## 6. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | Build passes | `dotnet build` exits 0 with no warnings |
| 2 | Zero allocations on hot paths | No `new string(...)` or LINQ in `Is*`/`Try*` bodies; span-based where applicable |
| 3 | Null-safe | Every public method returns `false`/`default` for `null` input (never throws) |
| 4 | Layer isolation | No references to MustClauses, GuardClauses, or user-facing message strings |
| 5 | Facade correctness (complex domain) | Public facade delegates every call to internal class; no logic in facade |

## 7. Examples

| User says | Actions | Result |
|-----------|---------|--------|
| "Add an IsValidMacAddress rule" | Create `NetworkRules.cs` with `IsValidMacAddress` method using span-based parsing | New Core Rule in `Rules/NetworkRules.cs` |
| "Add a TryParseLatLong utility" | Create `GeoLocationUtility.cs` with `TryParse` returning parsed struct | New Core Util in `Utils/GeoLocationUtility.cs` |

## 8. Reference Material
- `docs/ai/specs/core/project.md`
- [Reference exemplars](references/README.md)
