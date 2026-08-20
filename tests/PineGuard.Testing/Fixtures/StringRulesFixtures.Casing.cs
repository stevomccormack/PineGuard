using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── Casing ──────────────────────────────────────────────────────

    public static class IsCaseStyle
    {
        public static readonly (string? value, StringCasing style) CamelCase = ("helloWorld", StringCasing.CamelCase);
        public static readonly (string? value, StringCasing style) PascalCase = ("HelloWorld", StringCasing.PascalCase);
        public static readonly (string? value, StringCasing style) SnakeCase = ("hello_world", StringCasing.SnakeCase);
        public static readonly (string? value, StringCasing style) UpperSnakeCase = ("HELLO_WORLD", StringCasing.UpperSnakeCase);
        public static readonly (string? value, StringCasing style) KebabCase = ("hello-world", StringCasing.KebabCase);
        public static readonly (string? value, StringCasing style) TrainCase = ("Hello-World", StringCasing.TrainCase);
        public static readonly (string? value, StringCasing style) DotCase = ("hello.world", StringCasing.DotCase);
        public static readonly (string? value, StringCasing style) SpaceCase = ("Hello World", StringCasing.SpaceCase);
        public static readonly (string? value, StringCasing style) InvalidStyle = ("Hello_world", StringCasing.SnakeCase);
        public static readonly (string? value, StringCasing style) NullValue = (null, StringCasing.CamelCase);
        public static readonly (string? value, StringCasing style) UnknownStyle = ("hello", (StringCasing)999);

        public static RuleScenario<(string? value, StringCasing style)>[] ValidScenarios => [new(nameof(CamelCase), CamelCase, true), new(nameof(PascalCase), PascalCase, true), new(nameof(SnakeCase), SnakeCase, true), new(nameof(UpperSnakeCase), UpperSnakeCase, true), new(nameof(KebabCase), KebabCase, true), new(nameof(TrainCase), TrainCase, true), new(nameof(DotCase), DotCase, true), new(nameof(SpaceCase), SpaceCase, true)];
        public static RuleScenario<(string? value, StringCasing style)>[] InvalidScenarios => [new(nameof(InvalidStyle), InvalidStyle, false), new(nameof(NullValue), NullValue, false), new(nameof(UnknownStyle), UnknownStyle, false)];
        public static RuleScenario<(string? value, StringCasing style)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsCamelCase
    {
        public static readonly string ValidCamelCase = "helloWorld";
        public static readonly string InvalidPascalCase = "HelloWorld";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidCamelCase), ValidCamelCase, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidPascalCase), InvalidPascalCase, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPascalCase
    {
        public static readonly string ValidPascalCase = "HelloWorld";
        public static readonly string InvalidCamelCase = "helloWorld";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidPascalCase), ValidPascalCase, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidCamelCase), InvalidCamelCase, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSnakeCase
    {
        public static readonly string ValidSnakeCase = "hello_world";
        public static readonly string InvalidPascalCase = "HelloWorld";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidSnakeCase), ValidSnakeCase, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidPascalCase), InvalidPascalCase, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsUpperSnakeCase
    {
        public static readonly string ValidUpperSnake = "HELLO_WORLD";
        public static readonly string InvalidLowerSnake = "hello_world";
        public static readonly string InvalidLowercaseWithNoUppercaseMapping = "STRAßE_TEST";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidUpperSnake), ValidUpperSnake, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidLowerSnake), InvalidLowerSnake, false), new(nameof(InvalidLowercaseWithNoUppercaseMapping), InvalidLowercaseWithNoUppercaseMapping, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsKebabCase
    {
        public static readonly string ValidKebabCase = "hello-world";
        public static readonly string InvalidPascalCase = "HelloWorld";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidKebabCase), ValidKebabCase, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidPascalCase), InvalidPascalCase, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsTrainCase
    {
        public static readonly string ValidTrainCase = "Hello-World";
        public static readonly string InvalidLowerKebab = "hello-world";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidTrainCase), ValidTrainCase, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidLowerKebab), InvalidLowerKebab, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsDotCase
    {
        public static readonly string ValidDotCase = "hello.world";
        public static readonly string InvalidPascalCase = "HelloWorld";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidDotCase), ValidDotCase, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidPascalCase), InvalidPascalCase, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSpaceCase
    {
        public static readonly string ValidSpaceCase = "hello world";
        public static readonly string InvalidSnakeCase = "hello_world";

        public static RuleScenario<string>[] ValidScenarios => [new(nameof(ValidSpaceCase), ValidSpaceCase, true)];
        public static RuleScenario<string>[] InvalidScenarios => [new(nameof(InvalidSnakeCase), InvalidSnakeCase, false)];
        public static RuleScenario<string>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsUpperInvariant
    {
        public static readonly string? Upper = "ABC";
        public static readonly string? UpperWithPunctuation = "ABC-123";
        public static readonly string? NotUpper = "AbC";
        public static readonly string? NullValue = null;
        public static readonly string? Whitespace = "  ";
        public static readonly string? LowercaseWithNoUppercaseMapping = "STRAßE";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Upper), Upper, true), new(nameof(UpperWithPunctuation), UpperWithPunctuation, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NotUpper), NotUpper, false), new(nameof(NullValue), NullValue, false), new(nameof(Whitespace), Whitespace, false), new(nameof(LowercaseWithNoUppercaseMapping), LowercaseWithNoUppercaseMapping, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLowerInvariant
    {
        public static readonly string? Lower = "abc";
        public static readonly string? LowerWithPunctuation = "abc-123";
        public static readonly string? NotLower = "aBc";
        public static readonly string? NullValue = null;
        public static readonly string? Whitespace = "  ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Lower), Lower, true), new(nameof(LowerWithPunctuation), LowerWithPunctuation, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NotLower), NotLower, false), new(nameof(NullValue), NullValue, false), new(nameof(Whitespace), Whitespace, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
