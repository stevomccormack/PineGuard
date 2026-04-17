# PineGuard.Testing

**Test your validators the way PineGuard tests its own — 13,000+ cases strong.**

PineGuard.Testing is the xUnit toolkit that powers PineGuard's internal test suite: base test classes, case records, expected-result types, and exhaustive fixture catalogs of valid/invalid inputs for every rule category. Install it in your test project and your validator tests read the same way — and reuse the same data — as PineGuard itself.

Use this package when you're writing tests for custom Must, Guard, Fluent, or DataAnnotation validators and want rigorous, consistent coverage without rebuilding fixture data from scratch.

## Install

```bash
dotnet add package PineGuard.Testing
```

Targets `net8.0`, `net10.0`, and `netstandard2.1`. Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [xunit](https://www.nuget.org/packages/xunit), and [FluentValidation](https://www.nuget.org/packages/FluentValidation).

## Example

```csharp
using PineGuard.Testing.UnitTests.MustClauses;

// Test any of the canonical rules (Email, StrictEmail, OwaspSafe, HttpsUrl, …)
// with exhaustive fixture data that ships with the package.
public sealed class EmailMustTests : BaseMustUnitTest<string>
{
    [Theory]
    [MemberData(nameof(ValidCases))]
    public void Valid_emails_pass(MustCase<string> testCase) =>
        AssertMust(testCase, Must.Be.Email);

    public static TheoryData<MustCase<string>> ValidCases() =>
        new(EmailRulesFixtures.ValidEmails.Select(
            email => new MustCase<string>(email, new MustExpected(IsValid: true))));
}
```

## What you get

- **Base unit test classes** — `BaseMustUnitTest`, `BaseGuardUnitTest`, `BaseFluentUnitTest`, `BaseDataAnnotationUnitTest`, `BaseRuleUnitTest`
- **Case records** — `MustCase`, `GuardCase`, `FluentCase`, `DataAnnotationCase`, `RuleCase`
- **Expected records** — a uniform `IsValid` boolean across every layer
- **Fixtures** — exhaustive valid/invalid data for emails, URIs, OWASP input, network/HTTP identifiers, GUIDs, dates, times, numbers, collections, and more

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete base-class hierarchy and fixture catalog.

## License

MIT © Steve McCormack
