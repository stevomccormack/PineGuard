# PineGuard.Testing

**Test your validators the way PineGuard tests its own — 18,000+ cases strong.**

PineGuard.Testing is the xUnit toolkit that powers PineGuard's internal test suite: base test classes, case records, expected-result types, and exhaustive fixture catalogs of valid/invalid inputs for every rule category. Install it in your test project and your validator tests read the same way — and reuse the same data — as PineGuard itself.

Use this package when you're writing tests for custom Must, Guard, Fluent, or DataAnnotation validators and want rigorous, consistent coverage without rebuilding fixture data from scratch.

## Install

```bash
dotnet add package PineGuard.Testing
```

Targets `net8.0` and `net10.0`. Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [xunit](https://www.nuget.org/packages/xunit), and [FluentValidation](https://www.nuget.org/packages/FluentValidation).

## Example

```csharp
using PineGuard.Codes;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit;
using Xunit.Abstractions;
using F = PineGuard.Testing.Fixtures.EmailRulesFixtures;

public sealed class EmailTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    // The shipped fixture catalogue as theory data. ToMustCases() carries each scenario's
    // IsValid across; pass a selector when you also want to assert the message or code.
    public static TheoryData<MustCase<string?>> ValidCases => F.IsEmail.ValidScenarios.ToMustCases();

    public static TheoryData<MustCase<string?>> InvalidCases => F.IsEmail.InvalidScenarios.ToMustCases(s => s.Name switch
    {
        nameof(F.IsEmail.Null) => new MustExpected(false, "value must not be null.", "value"),
        _ => new MustExpected(false, "value must be a valid email address.", Code: MustCodes.Email.Address.Invalid)
    });

    [Theory]
    [MemberData(nameof(ValidCases))]
    [MemberData(nameof(InvalidCases))]
    public void Email_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Email(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }
}
```

## What you get

- **Base unit test classes** — `BaseMustUnitTest`, `BaseGuardUnitTest`, `BaseFluentUnitTest`, `BaseDataAnnotationUnitTest`, `BaseRuleUnitTest`
- **Case records** — `MustCase<TValue>(Name, Value, Expected)`, `GuardCase`, `FluentCase`, `DataAnnotationCase`, `RuleCase`
- **Expected records** — a uniform `IsValid` boolean across every layer; `MustExpected` also takes an optional `Message`, `ParamName` and `Code`
- **Fixtures** — exhaustive valid/invalid `RuleScenario` data for emails, URIs, OWASP input, network/HTTP identifiers, GUIDs, dates, times, numbers, collections, and more, with `ToMustCases()` to turn a scenario array into `TheoryData`
- **`FixedTimeProvider`** — freeze the clock for every temporal rule

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete base-class hierarchy and fixture catalog.

## License

MIT © Steve McCormack
