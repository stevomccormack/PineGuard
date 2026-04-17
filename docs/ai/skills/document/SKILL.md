# Skill: Generate XML Documentation
**ID**: pineguard.skill.document
**Version**: 1.0

## 1. Context & Goal
Generate gold-standard XML documentation comments (`///`) for all public members in a PineGuard project. Each layer has distinct consumers and requires tailored doc patterns. The output must satisfy IntelliSense consumers, NuGet package browsing, and DocFX site generation.

## 2. Inputs
- **Project**: Which project(s) to document (e.g., `PineGuard.Core`, `PineGuard.MustClauses`, or `all`)
- **Scope**: `file`, `class`, or `project` (default: `project`)
- **Docs Base URL**: `https://pineguard.ai/docs` (for `<see href>` links)

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
> 1. **Every public member** must have `<summary>`, `<param>` (all params), `<returns>` (non-void), `<typeparam>` (generics).
> 2. **Rico Suter phrasing**: Use third-person declarative voice ("Validates...", "Determines...", "Throws...").
> 3. **Layer-aware**: Each layer has a different doc template (see §5). Do NOT use a one-size-fits-all approach.
> 4. **`<see cref>` cross-references**: Always link to the underlying method in the dependency chain.
> 5. **`<see langword>` for keywords**: Use `<see langword="null"/>`, `<see langword="true"/>`, `<see langword="false"/>` — never bare text.
> 6. **`<exception cref>`**: Required on every method that throws (GuardClauses). Include the condition.
> 7. **`<example>` blocks**: Keep to 1-3 lines. Use `<code>` inside `<example>` for multi-line.
> 8. **`<see href>`**: Use for external doc links. Always include `https://` protocol and display text.
> 9. **No noise**: Don't document what's obvious from the signature alone. Focus on the "why" and behavioral nuance.
> 10. **`<inheritdoc/>`**: Use ONLY for interface implementations where the base doc is correct and complete. Never on public API surface that NuGet consumers see directly.
> 11. **CDATA in `<code>` blocks**: When a `<code>` example contains characters that conflict with XML parsing — angle brackets from generics (`<T>`, `<string>`), ampersands (`&&`), or comparison operators (`<`, `>`) — wrap the content in `<![CDATA[...]]>`. This avoids the need for entity-encoding (`&lt;`, `&gt;`, `&amp;`) and keeps examples readable. Use `<code><![CDATA[...]]></code>`. Entity-encoding (`&lt;`, `&gt;`) is still required in **prose** tags (`<summary>`, `<returns>`, `<remarks>`) where CDATA is not appropriate.

## 4. Tag Reference (Priority Order)

| Tag | When Required | IntelliSense Visibility |
|-----|---------------|------------------------|
| `<summary>` | Every public type and member | **Always shown** in tooltip |
| `<param name="">` | Every parameter | Shown in parameter hints |
| `<typeparam name="">` | Every generic type parameter | Shown in signature help |
| `<returns>` | Every non-void method | Shown in Quick Info |
| `<exception cref="">` | Every thrown exception | Shown in Quick Info |
| `<remarks>` | Behavioral nuance, null handling, threading | Expandable in VS 2022+, inline in Rider |
| `<example>` | Key public API methods (consumer-facing layers) | Object Browser, F12 Go-to-Definition |
| `<seealso cref="">` | Cross-references to related methods | Object Browser only |
| `<see href="">` | External documentation links | Clickable link in VS 2019+, Rider |
| `<see cref="">` | Inline type/member references | Rendered as link in tooltip |

## 5. Layer-Specific Templates

### 5.1 Core Rules (`PineGuard.Core/Rules/`)

**Consumer**: Internal (MustClauses call these). Also visible to advanced users via NuGet.

```csharp
/// <summary>
/// Determines whether the specified value is [CONDITION].
/// </summary>
/// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
/// <returns><see langword="true"/> if <paramref name="value"/> is [CONDITION]; otherwise, <see langword="false"/>.</returns>
/// <remarks>
/// This rule is used internally by <see cref="MustClauses.MustBoolClauses.True"/> and related higher-layer methods.
/// </remarks>
/// <example>
/// <code>
/// bool result = BoolRules.IsTrue(myValue);
/// </code>
/// </example>
/// <seealso cref="MustClauses.MustBoolClauses.True"/>
/// <seealso href="https://pineguard.ai/docs/rules/bool">Bool Rules documentation</seealso>
public static bool IsTrue(bool? value) => value is true;
```

**Rules for Rules:**
- Summary verb: "Determines whether..." (for `Is*`) or "Checks whether..." (for `Has*`)
- Always document null behavior in `<param>` tag: "If `null`, returns `false`."
- `<returns>`: Always use the `true if X; otherwise, false` pattern
- `<remarks>`: Reference the MustClause that wraps this rule
- `<seealso>`: Link to the corresponding MustClause method
- `<code>` with generics: Use CDATA (see Rule 11 in §3):

```csharp
/// <example>
/// <code><![CDATA[
/// bool exact = ObjectRules.IsOfType<string>(obj);
/// ]]></code>
/// </example>
```

### 5.2 Core Utils (`PineGuard.Core/Utils/`)

```csharp
/// <summary>
/// Attempts to trim whitespace from the specified string value.
/// </summary>
/// <param name="value">The string to trim. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
/// <param name="trimmed">
/// When this method returns <see langword="true"/>, contains the trimmed string.
/// When <see langword="false"/>, contains <see cref="string.Empty"/>.
/// </param>
/// <returns><see langword="true"/> if <paramref name="value"/> was non-empty after trimming; otherwise, <see langword="false"/>.</returns>
/// <example>
/// <code>
/// if (StringUtility.TryGetTrimmed("  hello  ", out var trimmed))
///     Console.WriteLine(trimmed); // "hello"
/// </code>
/// </example>
/// <seealso href="https://pineguard.ai/docs/utils/string">String Utility documentation</seealso>
public static bool TryGetTrimmed(string? value, out string trimmed)
```

**Rules for Utils:**
- Summary verb: "Attempts to..." (for `Try*`) or "Converts..." (for transformations)
- `<param>` for `out` parameters: Document BOTH the success and failure values
- Never skip the out-param documentation — this is the most important tag for utils

### 5.3 MustClauses (`PineGuard.MustClauses/`)

**Consumer**: Developers using the fluent `Must.Be.*` API directly.

```csharp
/// <summary>
/// Validates that the specified value is <see langword="true"/>.
/// </summary>
/// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
/// <param name="value">The boolean value to validate.</param>
/// <param name="paramName">
/// The name of the calling parameter. Automatically captured via
/// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
/// </param>
/// <returns>
/// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.IsValid"/> is <see langword="true"/>
/// if validation passed, or <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
/// </returns>
/// <remarks>
/// Delegates to <see cref="BoolRules.IsTrue"/>. The failure message follows the pattern
/// <c>"{paramName} must be true."</c>.
/// </remarks>
/// <example>
/// <code>
/// var result = Must.Be.True(isActive);
/// if (result.Failed)
///     Console.WriteLine(result.Message); // "isActive must be true."
/// </code>
/// </example>
/// <seealso cref="BoolRules.IsTrue"/>
/// <seealso cref="GuardClauses.GuardBoolClauses.True"/>
/// <seealso href="https://pineguard.ai/docs/must/bool">Must Bool Clauses documentation</seealso>
public static MustResult<bool> True(this IMustClause _, bool value, ...)
```

**Rules for MustClauses:**
- Summary verb: "Validates that..."
- `<param name="_">`: Always document as "The `IMustClause` entry point (used via `Must.Be`)."
- `<param name="paramName">`: Always document CallerArgumentExpression — tell users NOT to pass explicitly
- `<returns>`: Always reference `MustResult<T>.IsValid` and `MustResult<T>.Message`
- `<remarks>`: State which Rule it delegates to AND the message template pattern
- `<seealso>`: Link DOWN to the Rule and UP to the GuardClause

### 5.4 GuardClauses (`PineGuard.GuardClauses/`)

**Consumer**: Developers using `Guard.Against.*` for fail-fast validation.

```csharp
/// <summary>
/// Throws if the specified value is <see langword="true"/>.
/// </summary>
/// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
/// <param name="value">The boolean value to guard against.</param>
/// <param name="message">
/// An optional custom error message. If <see langword="null"/>, uses the default message
/// from <see cref="MustBoolClauses.False"/>.
/// </param>
/// <param name="exceptionCreator">
/// An optional factory to create a custom exception. If <see langword="null"/>,
/// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
/// </param>
/// <param name="paramName">
/// The name of the calling parameter. Automatically captured via
/// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
/// </param>
/// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
/// <exception cref="ArgumentException">
/// Thrown when <paramref name="value"/> is <see langword="true"/> and no
/// <paramref name="exceptionCreator"/> is provided.
/// </exception>
/// <remarks>
/// This guard is the complement of <see cref="MustBoolClauses.False"/>:
/// <c>Guard.Against.True</c> passes when the value is <see langword="false"/>.
/// </remarks>
/// <example>
/// <code>
/// Guard.Against.True(isDeleted);           // throws if true
/// Guard.Against.True(isDeleted, "Cannot proceed with deleted items.");
/// </code>
/// </example>
/// <seealso cref="MustBoolClauses.False"/>
/// <seealso href="https://pineguard.ai/docs/guard/bool">Guard Bool Clauses documentation</seealso>
public static bool True(this IGuardClause _, bool value, ...)
```

**Rules for GuardClauses:**
- Summary verb: "Throws if..." — always lead with the exception behavior
- `<exception cref>`: **REQUIRED**. State the condition that triggers the throw
- `<param name="message">`: Document what happens when null (falls back to MustClause message)
- `<param name="exceptionCreator">`: Document the default exception type when null
- `<remarks>`: Explain the complement relationship (Guard.Against.X ↔ Must.Be.Y)
- `<returns>`: Document the passthrough value on success

### 5.5 FluentValidation (`PineGuard.FluentValidation/`)

**Consumer**: Developers chaining FluentValidation rules.

```csharp
/// <summary>
/// Validates that the property value is <see langword="true"/>.
/// </summary>
/// <typeparam name="TModel">The type of the model being validated.</typeparam>
/// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
/// <param name="message">
/// An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.
/// </param>
/// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
/// <remarks>
/// Delegates to <see cref="MustBoolClauses.True"/>. If the value is <see langword="null"/>,
/// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
/// </remarks>
/// <example>
/// <code>
/// RuleFor(x => x.IsActive).True();
/// RuleFor(x => x.IsActive).True("Must be active to proceed.");
/// </code>
/// </example>
/// <seealso cref="MustBoolClauses.True"/>
/// <seealso href="https://pineguard.ai/docs/fluent/bool">Fluent Bool Extensions documentation</seealso>
public static IRuleBuilderOptions<TModel, bool?> True<TModel>(...)
```

**Rules for FluentValidation:**
- Summary verb: "Validates that..."
- `<typeparam>`: Always document `TModel`
- `<returns>`: Always mention "for further rule chaining"
- `<remarks>`: Document null handling behavior (null passes, use `.NotNull()` separately)
- `<example>`: Show the FluentValidation chain syntax (`RuleFor(x => x.Prop).Method()`)

### 5.6 DataAnnotations (`PineGuard.DataAnnotations/`)

**Consumer**: Developers applying `[Attribute]` validators to model properties.

```csharp
/// <summary>
/// Validates that the annotated <see cref="bool"/> property or field is <see langword="true"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBoolClauses.True"/>. Supported on properties, fields, and parameters
/// of type <see cref="bool"/>.
/// </para>
/// <para>
/// For nullable booleans, ensure the value is not <see langword="null"/> before this attribute runs
/// (e.g., combine with <c>[Required]</c>).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UserModel
/// {
///     [True]
///     public bool AcceptedTerms { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FalseAttribute"/>
/// <seealso cref="MustBoolClauses.True"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bool">Bool Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TrueAttribute() : ValidationAttributeBase(typeof(bool))
```

**Rules for DataAnnotations:**
- Summary verb: "Validates that the annotated..."
- `<summary>`: Include the supported CLR type (`bool`, `string`, etc.)
- `<remarks>`: Document supported targets AND nullable handling
- `<example>`: Show attribute applied to a model property (the consumer context)
- `<seealso>`: Link to the opposite attribute AND the underlying MustClause
- NO `<param>` or `<returns>` on the class — document `ValidateValue` as `<inheritdoc/>`

## 6. Execution Steps

1. **Identify Scope**
   - Determine which project(s) to document
   - List all public source files (exclude `obj/`, `bin/`, test projects)

2. **Read Each File**
   - Read the file completely
   - Identify all public types and members
   - Note the layer and determine the correct template (§5)

3. **Generate Documentation**
   - Apply the layer-specific template
   - Fill in semantic content (not boilerplate — describe the actual behavior)
   - Add `<see cref>` cross-references following the dependency chain
   - Add `<see href>` links using the docs base URL pattern
   - Add `<example>` blocks for consumer-facing methods

4. **Apply Documentation**
   - Edit each file to add the XML doc comments
   - Preserve exact existing code (no logic changes)
   - Verify the file still compiles: `dotnet build <project>.csproj`

5. **Verify**
   - Run `dotnet build` with `GenerateDocumentationFile=true`
   - Check for CS1591 warnings (missing docs on public members)
   - Fix any remaining gaps

## 7. Cross-Reference URL Patterns

Build `<see href>` and `<seealso href>` URLs using these patterns:

| Layer | URL Pattern |
|-------|------------|
| Rules | `https://pineguard.ai/docs/rules/{domain}` |
| Utils | `https://pineguard.ai/docs/utils/{domain}` |
| MustClauses | `https://pineguard.ai/docs/must/{domain}` |
| GuardClauses | `https://pineguard.ai/docs/guard/{domain}` |
| FluentValidation | `https://pineguard.ai/docs/fluent/{domain}` |
| DataAnnotations | `https://pineguard.ai/docs/annotations/{domain}` |

**Domain** = lowercase domain name from the filename (e.g., `BoolRules.cs` → `bool`, `JsonRules.cs` → `json`).

## 8. Phrasing Reference (Rico Suter Standard)

| Member Pattern | Summary Phrase |
|----------------|---------------|
| `Is*` Rule | "Determines whether the specified value is..." |
| `Has*` Rule | "Checks whether the specified value has..." |
| `Try*` Util | "Attempts to parse/normalize/extract..." |
| `Must.Be.*` | "Validates that the specified value is/has..." |
| `Guard.Against.*` | "Throws if the specified value is/has..." |
| Fluent `.*()` | "Validates that the property value is/has..." |
| `[Attribute]` | "Validates that the annotated `TYPE` property is/has..." |
| Boolean return | "`true` if CONDITION; otherwise, `false`." |
| `MustResult<T>` return | "A `MustResult<T>` where `IsValid` is `true` if validation passed." |
| `IRuleBuilderOptions` return | "An `IRuleBuilderOptions` for further rule chaining." |
| Constructor | "Initializes a new instance of the `T` class." |
| `null` param | "If `null`, returns `false`." (Rules/Utils) |

## 9. Definition of Done
- [ ] All public types and members have `<summary>` tags
- [ ] All parameters have `<param>` tags
- [ ] All generic type params have `<typeparam>` tags
- [ ] All non-void methods have `<returns>` tags
- [ ] All throwing methods have `<exception cref>` tags
- [ ] Cross-references use `<see cref>` (internal) and `<see href>` (external)
- [ ] Examples use real PineGuard syntax (not pseudo-code)
- [ ] `dotnet build` passes with `GenerateDocumentationFile=true`
- [ ] Zero CS1591 warnings on the documented project

## 10. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | IntelliSense complete | Every tooltip shows meaningful summary + param hints |
| 2 | Cross-referenced | Every method links to its dependency chain (Rule ↔ Must ↔ Guard) |
| 3 | Consumer-appropriate | Doc tone matches the layer's consumer (internal vs developer) |
| 4 | Build clean | `dotnet build` with `GenerateDocumentationFile=true` exits 0, zero CS1591 |
| 5 | DocFX ready | All tags render correctly in DocFX static site generation |

## 11. Reference Material
- [Microsoft: Recommended XML documentation tags](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags)
- [Rico Suter: XML doc phrase best practices](https://blog.rsuter.com/best-practices-for-writing-xml-documentation-phrases-in-c/)
- [Ardalis GuardClauses source](https://github.com/ardalis/GuardClauses) (exception documentation pattern)
- [FluentValidation source](https://github.com/FluentValidation/FluentValidation) (remarks + example pattern)
- [Polly source](https://github.com/App-vNext/Polly) (clean `<see cref>` cross-referencing)
- `docs/ai/specs/spec.md` (layer ordering, cascading model)
- `docs/ai/specs/coding-standard.md` (formatting rules)
