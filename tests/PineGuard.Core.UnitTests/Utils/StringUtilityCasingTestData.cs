using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityCasingTestData
{
    public static class TryCreateWords
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("camel", ("helloWorld", StringCasing.CamelCase), true, ["hello", "World"]),
            new("camel digits", ("test1Thing2", StringCasing.CamelCase), true, ["test", "1", "Thing", "2"]),
            new("camel multi digits", ("abc12def", StringCasing.CamelCase), true, ["abc", "12", "def"]),
            new("pascal", ("HelloWorld", StringCasing.PascalCase), true, ["Hello", "World"]),
            new("pascal acronym boundary", ("HTTPServer", StringCasing.PascalCase), true, ["HTTP", "Server"]),
            new("snake", ("hello_world", StringCasing.SnakeCase), true, ["hello", "world"]),
            new("snake digits", ("a1_b2", StringCasing.SnakeCase), true, ["a1", "b2"]),
            new("upper snake", ("HELLO_WORLD", StringCasing.UpperSnakeCase), true, ["HELLO", "WORLD"]),
            new("upper snake digits", ("A1_B2", StringCasing.UpperSnakeCase), true, ["A1", "B2"]),
            new("kebab", ("hello-world", StringCasing.KebabCase), true, ["hello", "world"]),
            new("dot", ("hello.world", StringCasing.DotCase), true, ["hello", "world"]),
            new("train title", ("Hello-World", StringCasing.TrainCase), true, ["Hello", "World"]),
            new("train acronym", ("NASA-Data", StringCasing.TrainCase), true, ["NASA", "Data"]),
            new("train digits", ("A1-B2", StringCasing.TrainCase), true, ["A1", "B2"]),
            new("space", ("Hello World", StringCasing.SpaceCase), true, ["Hello", "World"]),
            new("space leading (trimmed)", (" Hello", StringCasing.SpaceCase), true, ["Hello"]),
            new("space trailing (trimmed)", ("Hello ", StringCasing.SpaceCase), true, ["Hello"]),
            new("space digits", ("A1 B2", StringCasing.SpaceCase), true, ["A1", "B2"])
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", (null, StringCasing.CamelCase), false, []),
            new("whitespace", ("  ", StringCasing.CamelCase), false, []),
            new("unknown style", ("hello", (StringCasing)999), false, []),
            new("camel starts upper", ("HelloWorld", StringCasing.CamelCase), false, []),
            new("pascal starts lower", ("helloWorld", StringCasing.PascalCase), false, []),
            new("camel non alnum", ("hello_world", StringCasing.CamelCase), false, []),
            new("camel starts digit", ("1hello", StringCasing.CamelCase), false, []),
            new("snake has upper", ("Hello_world", StringCasing.SnakeCase), false, []),
            new("snake leading separator", ("_hello", StringCasing.SnakeCase), false, []),
            new("snake trailing separator", ("hello_", StringCasing.SnakeCase), false, []),
            new("snake double separator", ("hello__world", StringCasing.SnakeCase), false, []),
            new("snake invalid char", ("hello_wor!ld", StringCasing.SnakeCase), false, []),
            new("upper snake has lower", ("HELLO_World", StringCasing.UpperSnakeCase), false, []),
            new("kebab has upper", ("hello-World", StringCasing.KebabCase), false, []),
            new("dot double separator", ("hello..world", StringCasing.DotCase), false, []),
            new("train not title", ("Hello-world", StringCasing.TrainCase), false, []),
            new("train lower", ("hello-World", StringCasing.TrainCase), false, []),
            new("train internal upper", ("HeLlo-World", StringCasing.TrainCase), false, []),
            new("space double", ("Hello  World", StringCasing.SpaceCase), false, []),
            new("space invalid char", ("Hello W@rld", StringCasing.SpaceCase), false, [])
        ];

        public sealed record ValidCase(string Name, (string? Value, StringCasing Style) Value, bool Expected, IReadOnlyList<string> ExpectedOutValue)
            : ReturnCase<(string? Value, StringCasing Style), bool>(Name, Value, Expected);
    }

    public static class TryToCaseFromWords
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("camel", (["hello", "world"], StringCasing.CamelCase), true, "helloWorld"),
            new("camel single word", (["Hello"], StringCasing.CamelCase), true, "hello"),
            new("pascal", (["hello", "world"], StringCasing.PascalCase), true, "HelloWorld"),
            new("snake", (["Hello", "World"], StringCasing.SnakeCase), true, "hello_world"),
            new("upper snake", (["Hello", "World"], StringCasing.UpperSnakeCase), true, "HELLO_WORLD"),
            new("kebab", (["Hello", "World"], StringCasing.KebabCase), true, "hello-world"),
            new("dot", (["Hello", "World"], StringCasing.DotCase), true, "hello.world"),
            new("train", (["hello", "world"], StringCasing.TrainCase), true, "Hello-World"),
            new("space", (["hello", "world"], StringCasing.SpaceCase), true, "Hello World"),
            new("camel single-char", (["x"], StringCasing.CamelCase), true, "x")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null words", (null, StringCasing.CamelCase), false, string.Empty),
            new("empty words", ([], StringCasing.CamelCase), false, string.Empty),
            new("whitespace word", (["hello", "  "], StringCasing.CamelCase), false, string.Empty),
            new("unknown style", (["hello"], (StringCasing)999), false, string.Empty)
        ];

        public sealed record ValidCase(string Name, (IReadOnlyList<string>? Words, StringCasing OutputStyle) Value, bool Expected, string ExpectedOutValue)
            : TryCase<(IReadOnlyList<string>? Words, StringCasing OutputStyle), string>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryToCaseFromValue
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("snake->pascal", ("hello_world", StringCasing.SnakeCase, StringCasing.PascalCase), true, "HelloWorld"),
            new("train->kebab", ("Hello-World", StringCasing.TrainCase, StringCasing.KebabCase), true, "hello-world"),
            new("camel->snake", ("helloWorld", StringCasing.CamelCase, StringCasing.SnakeCase), true, "hello_world"),
            new("dot->camel", ("hello.world", StringCasing.DotCase, StringCasing.CamelCase), true, "helloWorld")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid for input style", ("Hello_world", StringCasing.SnakeCase, StringCasing.PascalCase), false, string.Empty),
            new("null", (null, StringCasing.SnakeCase, StringCasing.PascalCase), false, string.Empty),
            new("unknown output style", ("hello_world", StringCasing.SnakeCase, (StringCasing)999), false, string.Empty)
        ];

        public sealed record ValidCase(string Name, (string? Value, StringCasing InputStyle, StringCasing OutputStyle) Value, bool Expected, string ExpectedOutValue)
            : TryCase<(string? Value, StringCasing InputStyle, StringCasing OutputStyle), string>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class ToCaseSingleStyle
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("snake", ("hello_world", StringCasing.SnakeCase), true, "hello_world"),
            new("camel", ("helloWorld", StringCasing.CamelCase), true, "helloWorld")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid for input style", ("Hello_world", StringCasing.SnakeCase), false, string.Empty),
            new("null", (null, StringCasing.SnakeCase), false, string.Empty),
            new("unknown style", ("hello_world", (StringCasing)999), false, string.Empty)
        ];

        public sealed record ValidCase(string Name, (string? Value, StringCasing Style) Value, bool Expected, string ExpectedOutValue)
            : TryCase<(string? Value, StringCasing Style), string>(Name, Value, Expected, ExpectedOutValue);
    }
}
