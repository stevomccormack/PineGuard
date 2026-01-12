# PineGuard.MustClauses Agent Spec

This document is the **source-of-truth instruction set** for generating and maintaining unit tests in this repository.

It is written for:

- humans contributing tests, and
- AI sessions that must iterate quickly and deterministically.

---

## Root namespace:
namespace PineGuard.MustClauses;

## Base interfaces & classes:
`PineGuard.MustClauses.IMustClause`
`PineGuard.MustClauses.MustClause`
`PineGuard.MustClauses.Must`
`PineGuard.MustClauses.MustResult`

## Example must clause class:
public static MustResult<T> Alphabetic(this MustClause _, string? value, 
  [CallerArgumentExpression("value"]) parameterName = null, string? message = null)
  => MustResult.FromBool(StringRules.IsAlphabetic(value), 
    message ?? "{paramName} must be alphabetical", parameterName, value);
}

public static MustResult<Guid> Guid(
    this IMustClause _,
    string value,
    [CallerArgumentExpression("value")] string? parameterName = null, string message = null)
{
    var messageTemplate = message ?? "{paramName} must be a valid GUID.";
    var ok = GuidRules.IsGuid(value);
    return MustResult<Guid>.FromBool(ok, messageTemplate, parameterName, value, parsed);
}

### Example notes
- All must clauses should be handled by their corresponding rule
- All must clasuses should have guards and exceptions handled by their corresponding rule
- 

## Example usage:
var result = Must.Be.Alphabetic(input);
if (result.Success)
  return true;