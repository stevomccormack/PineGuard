# Skill: Implement GuardClauses
**ID**: pineguard.skill.scaffold-guard
**Version**: 1.0

## 1. Context & Goal
Implement a new **GuardClause** method (`Guard.Against.Xxx`). A GuardClause ensures a condition is met and **throws an exception** if it fails.

## 2. Inputs
- **MustClause**: The underlying `Must.Be.Xxx` validation that this guard enforces (or its complement).
- **Forbidden State**: What are we guarding *against*? (e.g., `Null`, `Negative`, `InvalidFormat`).

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> 1.  **Thin Facade**: GuardClauses **MUST** call the corresponding `Must.Be.Xxx` clause. Do not duplicate logic.
> 2.  **Throw on Failure**: If Must returns `Failed`, throw using `GuardFailure.Throw(...)`.
> 3.  **Reuse Message**: Use `result.Message` from the Must result. Do NOT invent new messages.
> 4.  **Vocabulary**: Name the method after the *Forbidden State* (e.g., `Guard.Against.Null`, `Guard.Against.Negative`).

## 4. Execution Steps

1.  **Identify Target Must Clause**
    *   Find (or create) the `Must.Be.[GoodState]` validation.
    *   *Example*: To implement `Guard.Against.Negative`, you need `Must.Be.ZeroOrPositive`.

2.  **Locate Target File**
    *   Folder: `src/PineGuard.GuardClauses/[Domain]/` or relative root.
    *   File: `Guard[Domain]Clauses.cs`.

3.  **Implement Extension Method**
    ```csharp
    using System.Runtime.CompilerServices;
    using PineGuard.MustClauses;

    namespace PineGuard.GuardClauses;

    public static class GuardDomainClauses
    {
        public static string MyForbiddenCondition(
            this IGuardClause _,
            string? value,
            string? message = null,
            Func<Exception>? exceptionCreator = null,
            [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            // Guard.Against.Negative => Must.Be.ZeroOrPositive (complement)
            var result = Must.Be.MyGoodCondition(value, paramName);
            if (result.Failed)
                GuardFailure.Throw(result, message, exceptionCreator);

            return result.Result!;
        }
    }
    ```

    Parameter order is fixed: `value`, `message`, `exceptionCreator`, `paramName`. `paramName` is last
    because `[CallerArgumentExpression]` must not be displaced by an explicit argument.

4.  **Handling Typed Returns**
    *   Guards return the validated value for pass-through chaining — `result.Result!` on success.
    *   If the Guard acts as a parser (e.g. `Guard.Against.InvalidGuid`), the return type is the parsed
        type `TParsed` rather than the input type.

## 5. Definition of Done
- [ ] Method calls `Must.Be.*`.
- [ ] Method accepts `message` and `exceptionCreator`.
- [ ] Method throws on failure.
- [ ] Method returns the validated value (pass-through), never `void`.

## 6. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | Build passes | `dotnet build` exits 0 with no warnings |
| 2 | Throws on failure | Every guard calls `GuardFailure.Throw(...)` when `result.Failed` |
| 3 | Delegates to Must | Every guard calls `Must.Be.*`; no inline validation logic |
| 4 | Reuses Must message | Uses `result.Message` (no invented messages) |
| 5 | Supports custom exception | Signature includes `Func<Exception>? exceptionCreator = null` |
| 6 | Pass-through return | Returns the validated value (`result.Result!`); never `void` |

## 7. Reference Material
- `docs/ai/specs/guard-clauses/project.md`
- [Reference exemplars](references/README.md)
