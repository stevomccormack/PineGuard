---
spec:
  id: pineguard.ai.specs.coding-standards
  title: "Coding Standards & Static Analysis Rules"
  version: 1
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/**"
  - "tests/**"
---

# Coding Standards & Static Analysis Rules

This document defines the strict coding patterns required to satisfy Resharper, SonarQube, and internal PineGuard quality gates.

## Resharper Optimisations

### 1. Primary Constructors (C# 12)

- **Rule**: Use Primary Constructors for all classes/structs where the constructor parameters map directly to fields/properties.
- **Why**: Reduces boilerplate code.

```csharp
// BAD
public class User
{
    private readonly string _name;
    public User(string name)
    {
        _name = name;
    }
}

// GOOD
public class User(string name)
{
    private readonly string _name = name;
}
```

### 2. Remove Redundant Constructor

- **Rule**: Do not declare an empty constructor if it does nothing (default compiler constructor is sufficient).
- **Why**: Noise reduction.

### 3. Remove Redundant Type Arguments

- **Rule**: If the compiler can infer the type, omit the generic argument.
- **Why**: Cleaner syntax.

```csharp
// BAD
Must.Be.Default<int>(value);

// GOOD
Must.Be.Default(value);
```

### 4. Merge into Pattern

- **Rule**: Prefer pattern matching (`is` expressions) over `as` cast + null check.
- **Why**: More performant and safer.

```csharp
// BAD
var x = value as string;
if (x != null) { ... }

// GOOD
if (value is string x) { ... }
```

### 5. Suppress Nullable Warning with `!`

- **Rule**: Use the null-forgiving operator (`!`) **ONLY** when you have external knowledge that a value is not null, which the compiler cannot see (e.g., inside a check that implies validity but isn't a type guard).
- **Caveat**: Overuse defeats the purpose of NRT. Prefer refactoring to proper null checks where possible.

---

## Sonar Qube Optimisations / Compiler Features

### 1. Regex Source Generation

- **Rule**: Always use `[GeneratedRegex]` for compile-time compiled regexes.
- **Pattern**:
  1. Define the pattern string as a `public const` (allows reuse in attributes/tests).
  2. Define the `static partial` method returning `Regex`.

```csharp
public const string SignedIntegerPattern = @"^[\+\-]?\d+$";

[GeneratedRegex(SignedIntegerPattern, RegexOptions.CultureInvariant)]
public static partial Regex SignedIntegerRegex();
```

### 2. Obsolete Member Validation (Suppression Pattern)

- **Rule**: When writing **validation logic** that _must_ check obsolete enum members (e.g., explicit "must not have flag" checks), suppress `CS0612` locally.
- **Why**: The code is doing exactly what it intends (checking for the forbidden/obsolete value), so the warning is a false positive in this specific context.

```csharp
#pragma warning disable CS0612 // Type or member is obsolete
        public NotHasFlagValidator() => RuleFor(x => x.Value).NotHasFlag(FluentEnumExtensionsTestData.TestEnum.First);
#pragma warning restore CS0612 // Type or member is obsolete
```
