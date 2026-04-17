# Skill: Implement MustClauses
**ID**: pineguard.skill.scaffold-must
**Version**: 1.0

## 1. Context & Goal
Implement a new **MustClause** fluent validation method. A MustClause validates input, optionally parses/normalizes it, and returns a `MustResult<T>`.

## 2. Inputs
- **Domain**: (e.g., "Json", "Guid", "Email")
- **Condition**: (e.g., "BeValid", "HaveProperty")
- **Input Type**: (e.g., `string`, `ReadOnlySpan<char>`)

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> 1.  **Never Throw**: MustClauses must return `MustResult.Fail(...)` for invalid input, never throw exceptions.
> 2.  **Canonical Messages**: You OWN the user-facing message. It must include `{paramName}`.
> 3.  **Layering**: Call `PineGuard.Rules` or `PineGuard.Utils` for logic. Do not write raw parsing logic or regexes in Must.
> 4.  **Config vs Value**: If a *configuration* parameter (like a regex pattern) is null, attribute the failure to `nameof(configParam)`, not `value`.
> 5.  **Strict Coding**: File-scoped namespaces, sorted usings, arrow functions where possible, `value` parameter naming.
> 6.  **Prefer `Utility.TryXxx()` for parsed results**: When `MustResult<T>.Result` needs the parsed/normalized value, call `Utility.TryXxx(value, out var parsed)` directly — not `Rules.IsXxx()`. The Try method gives you both the boolean and the parsed value. Pass the parsed value as `result:` to `FromBool()`. Use `Rules.IsXxx()` only when no parsed output is needed (pure boolean validation). See `docs/ai/specs/core/project.md` §4.1.

## 4. Execution Steps

1.  **Identify/Create Core Logic**
    *   Check `src/PineGuard.Core/Rules` or `Utils` for the validation logic.
    *   *If missing*: Stop and implement the Rule/Util in Core first (see `implement-core-rule` skill).

2.  **Locate Target (Facade Pattern)**
    *   **Simple Domain**: `src/PineGuard.MustClauses/Must[Domain]Clauses.cs`.
    *   **Complex Domain** (multiple sub-domains):
        *   Implementation: `internal static` class in `src/PineGuard.MustClauses/[Domain]/`.
        *   Facade: `public static` class `Must[Domain]Clauses.cs` in `src/PineGuard.MustClauses/`.
        *   *Rule*: The Facade must flatten the API.

3.  **Implement Extension Method**

    When a parsed/normalized result is needed (preferred — see rule 6):
    ```csharp
    using System.Runtime.CompilerServices;
    using PineGuard.Utils;

    namespace PineGuard.MustClauses;

    public static class MustDomainClauses
    {
        public static MustResult<T> MyCondition(
            this IMustClause _,
            string? value,
            [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const string messageTemplate = "{paramName} must satisfy condition.";

            var ok = MyDomainUtility.TryParse(value, out var parsed);
            return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, result: parsed);
        }
    }
    ```

    When only a boolean check is needed (no parsed output):
    ```csharp
    using System.Runtime.CompilerServices;
    using PineGuard.Rules;

    namespace PineGuard.MustClauses;

    public static class MustDomainClauses
    {
        public static MustResult<bool> MyCondition(
            this IMustClause _,
            string? value,
            [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const string messageTemplate = "{paramName} must satisfy condition.";

            var ok = MyDomainRules.IsCondition(value);
            return MustResult<bool>.FromBool(ok, messageTemplate, paramName, value, result: ok);
        }
    }
    ```

4.  **Verify Standards**
    *   Does it return `MustResult`?
    *   Does it avoid throwing?
    *   Is `paramName` passed through?

## 5. Definition of Done
- [ ] implementation compiles.
- [ ] No `[GeneratedRegex]` or raw logic in Must class.
- [ ] Returns `MustResult.Fail` for null input (unless `NullOr...`).

## 6. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | Build passes | `dotnet build` exits 0 with no warnings |
| 2 | Never throws | No `throw` statements; all failures return `MustResult.Fail(...)` |
| 3 | Message includes paramName | Every failure message interpolates `{paramName}` |
| 4 | Delegates to Core | No regex, parsing, or raw validation logic in MustClause bodies |
| 5 | Facade flattens API (complex domain) | Public facade exposes flat API, not nested namespaces |

## 7. Examples

| User says | Actions | Result |
|-----------|---------|--------|
| "Add Must.Be.ValidMacAddress" | Create `MustNetworkClauses.cs`, call `NetworkRules.IsValidMacAddress` | New MustClause returning `MustResult<string>` |
| "Add Must.Be.ValidLatLong" | Create `MustGeoLocationClauses.cs`, call `GeoLocationUtility.TryParse` | MustClause with parsed output type |

## 8. Reference Material
- `docs/ai/specs/must-clauses/project.md`
- `docs/ai/specs/spec.md` (Root)
- [Reference exemplars](references/README.md)
