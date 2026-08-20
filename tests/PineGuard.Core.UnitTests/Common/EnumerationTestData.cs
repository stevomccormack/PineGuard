using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class EnumerationTestData
{
    public sealed class DynamicIntEnumeration(int value, string name) : Enumeration<int>(value, name);

    public sealed class DynamicStringEnumeration(string value, string name) : StringEnumeration(value, name);

    public sealed class TestColor(int value, string name) : Enumeration<int>(value, name)
    {
        public static readonly TestColor Red = new(1, "Red");
        public static readonly TestColor Green = new(2, "Green");
        public static readonly TestColor Blue = new(3, "Blue");
    }

    public sealed class TestStatus(int value, string name) : Enumeration<int>(value, name)
    {
        public static readonly TestStatus None = new(0, "None");
        public static readonly TestStatus Active = new(1, "Active");
    }

    public static class IntConstructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Alpha", 1, "Alpha"),
            new("Bravo", 2, "Bravo"),
            new("Charlie", 3, "Charlie"),
            new("Delta", 4, "Delta"),
            new("Echo", 5, "Echo"),
            new("Foxtrot", 6, "Foxtrot"),
            new("Golf", 7, "Golf"),
            new("Hotel", 8, "Hotel"),
            new("India", 9, "India"),
            new("Juliet", 10, "Juliet"),
            new("Kilo", 11, "Kilo"),
            new("Lima", 12, "Lima"),
            new("Mike", 13, "Mike"),
            new("November", 14, "November"),
            new("Oscar", 15, "Oscar"),
            new("Papa", 16, "Papa"),
            new("Quebec", 17, "Quebec"),
            new("Romeo", 18, "Romeo"),
            new("Sierra", 19, "Sierra"),
            new("Tango", 20, "Tango")
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("value=1, name='<null>'", 1, null, new ExpectedException(typeof(ArgumentNullException), "name")),
            new("value=2, name=''", 2, "", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=3, name=' '", 3, " ", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=4, name='\t'", 4, "\t", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=5, name='\r'", 5, "\r", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=6, name='\n'", 6, "\n", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=7, name='\u00A0'", 7, "\u00A0", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=8, name='\u2007'", 8, "\u2007", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=9, name='\u202F'", 9, "\u202F", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=10, name='\u2003'", 10, "\u2003", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=11, name='\u2009'", 11, "\u2009", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=12, name='\u3000'", 12, "\u3000", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=13, name='\v'", 13, "\v", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=14, name='\f'", 14, "\f", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=15, name='\t\t'", 15, "\t\t", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=16, name='  '", 16, "  ", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=17, name='\n\n'", 17, "\n\n", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=18, name='\r\n'", 18, "\r\n", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=19, name='\t \n'", 19, "\t \n", new ExpectedException(typeof(ArgumentException), "name")),
            new("value=20, name=' \t '", 20, " \t ", new ExpectedException(typeof(ArgumentException), "name"))
        ];

        public sealed record ValidCase(string Name, int InputValue, string EnumerationName)
            : ReturnCase<(int Value, string EnumerationName), string>(Name, (InputValue, EnumerationName), EnumerationName);

        public sealed record InvalidCase(string Name, int EnumerationValue, string? EnumerationName, ExpectedException ExpectedException)
            : ThrowsCase<(int EnumerationValue, string? EnumerationName)>(Name, (EnumerationValue, EnumerationName), ExpectedException);
    }

    public static class StringConstructor
    {
        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("value='<null>', name='X'", null, "X", new ExpectedException(typeof(ArgumentNullException), "value")),
            new("value='<null>', name='<null>'", null, null, new ExpectedException(typeof(ArgumentNullException), "value")),
            new("value='ok', name='<null>'", "ok", null, new ExpectedException(typeof(ArgumentNullException), "name")),
            new("value='ok', name=''", "ok", "", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name=' '", "ok", " ", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\t'", "ok", "\t", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\r'", "ok", "\r", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\n'", "ok", "\n", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\u00A0'", "ok", "\u00A0", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\u2007'", "ok", "\u2007", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\u202F'", "ok", "\u202F", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\u2003'", "ok", "\u2003", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\u2009'", "ok", "\u2009", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\u3000'", "ok", "\u3000", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\v'", "ok", "\v", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\f'", "ok", "\f", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='  '", "ok", "  ", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\t\t'", "ok", "\t\t", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\n\n'", "ok", "\n\n", new ExpectedException(typeof(ArgumentException), "name")),
            new("value='ok', name='\r\n'", "ok", "\r\n", new ExpectedException(typeof(ArgumentException), "name"))
        ];

        public sealed record InvalidCase(string Name, string? EnumerationValue, string? EnumerationName, ExpectedException ExpectedException)
            : ThrowsCase<(string? EnumerationValue, string? EnumerationName)>(Name, (EnumerationValue, EnumerationName), ExpectedException);
    }

    public static class DuplicateName
    {
        public static TheoryData<Case> Cases =>
        [
            new("Duplicate Name Ignores Case", 1, "Alpha", 2, "ALPHA", new ExpectedException(typeof(ArgumentException), "name", "already exists"))
        ];

        public sealed record Case(string Name, int FirstValue, string FirstName, int SecondValue, string SecondName, ExpectedException ExpectedException)
            : ThrowsCase<(int FirstValue, string FirstName, int SecondValue, string SecondName)>(Name, (FirstValue, FirstName, SecondValue, SecondName), ExpectedException);
    }

    public static class DuplicateValue
    {
        public static TheoryData<Case> Cases =>
        [
            new("Duplicate Value Rolls Back", 1, "Alpha", 1, "Bravo", new ExpectedException(typeof(ArgumentException), "value"))
        ];

        public sealed record Case(string Name, int FirstValue, string FirstName, int SecondValue, string SecondName, ExpectedException ExpectedException)
            : ThrowsCase<(int FirstValue, string FirstName, int SecondValue, string SecondName)>(Name, (FirstValue, FirstName, SecondValue, SecondName), ExpectedException);
    }

    public static class GetAll
    {
        public static TheoryData<Case> Cases =>
        [
            new("Returns Static Fields", 3, [TestColor.Red, TestColor.Green, TestColor.Blue])
        ];

        public sealed record Case(string Name, int ExpectedCount, IReadOnlyList<TestColor> ExpectedItems)
            : ValueCase<int>(Name, ExpectedCount);
    }

    public static class FromValue
    {
        public static TheoryData<Case> Cases =>
        [
            new("Red Match", 1, TestColor.Red),
            new("Red Match Generic", 1, TestColor.Red),
            new("Missing", 999, null)
        ];

        public sealed record Case(string Name, int Input, TestColor? Expected)
            : ReturnCase<int, TestColor?>(Name, Input, Expected);
    }

    public static class TryFromValue
    {
        public static TheoryData<Case> Cases =>
        [
            new("Match Found", 1, true, TestColor.Red),
            new("Match Missing", 999, false, null)
        ];

        public sealed record Case(string Name, int Input, bool Expected, TestColor? ExpectedOut)
            : TryCase<int, TestColor?>(Name, Input, Expected, ExpectedOut);
    }

    public static class TryFromValueNull
    {
        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null string value returns false", null, false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, TestStringColor? ExpectedOutValue)
            : TryCase<string?, TestStringColor?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryFromValueDefault
    {
        public static TheoryData<Case> Cases =>
        [
            new("default int value 0 is found when a member is registered for it", 0, true, TestStatus.None)
        ];

        public sealed record Case(string Name, int Input, bool Expected, TestStatus? ExpectedOut)
            : TryCase<int, TestStatus?>(Name, Input, Expected, ExpectedOut);
    }

    public sealed class TestStringColor(string value, string name) : StringEnumeration(value, name);


    public static class FromName
    {
        public static TheoryData<Case> Cases =>
        [
            new("Match Found Ignores Case", "gReEn", TestColor.Green)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null", null!, new ExpectedException(typeof(ArgumentNullException), "name")),
            new("Empty", "", new ExpectedException(typeof(ArgumentException), "name")),
            new("Whitespace", " ", new ExpectedException(typeof(ArgumentException), "name"))
        ];

        public sealed record Case(string Name, string Input, TestColor Expected)
            : ReturnCase<string, TestColor>(Name, Input, Expected);

        public sealed record InvalidCase(string Name, string Input, ExpectedException ExpectedException)
            : ThrowsCase<string>(Name, Input, ExpectedException);
    }

    public static class TryFromName
    {
        public static TheoryData<Case> Cases =>
        [
            new("Match Found", "red", true, TestColor.Red),
            new("Missing", "ruby", false, null),
            new("Null", null, false, null),
            new("Empty", "", false, null),
            new("Whitespace", " ", false, null)
        ];

        public sealed record Case(string Name, string? Input, bool Expected, TestColor? ExpectedOut)
            : TryCase<string?, TestColor?>(Name, Input, Expected, ExpectedOut);
    }

    public static class Equality
    {
        public static TheoryData<Case> Cases =>
        [
            new("Same Instance", TestColor.Red, TestColor.Red, true),
            new("Different Instance Same Value", new TestColor(1, "Red"), TestColor.Red, true),
            new("Different Values", TestColor.Red, TestColor.Blue, false),
            new("Null", TestColor.Red, null, false),
            new("Different Type", TestColor.Red, TestStatus.Active, false),
            new("Object Same", TestColor.Red, TestColor.Red, true),
            new("Object Null", TestColor.Red, null, false),
            new("Object Other Type", TestColor.Red, new object(), false)
        ];

        public sealed record Case(string Name, object? Left, object? Right, bool Expected)
            : ValueCase<(object? Left, object? Right)>(Name, (Left, Right));
    }

    public static class ImplicitOperatorString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Red implicit string", TestColor.Red, "Red"),
            new("Green implicit string", TestColor.Green, "Green")
        ];

        public sealed record ValidCase(string Name, TestColor Input, string ExpectedString)
            : ReturnCase<TestColor, string>(Name, Input, ExpectedString);
    }

    public static class ImplicitOperatorInt
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Red implicit int", TestColor.Red, 1),
            new("Green implicit int", TestColor.Green, 2)
        ];

        public sealed record ValidCase(string Name, TestColor Input, int ExpectedValue)
            : ReturnCase<TestColor, int>(Name, Input, ExpectedValue);
    }

    public static class CompareTo
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("lesser returns positive", TestColor.Blue, TestColor.Red, 1),
            new("same returns zero", TestColor.Red, TestColor.Red, 0),
            new("greater returns negative", TestColor.Red, TestColor.Blue, -1)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null returns 1", TestColor.Red, null, 1)
        ];

        public sealed record ValidCase(string Name, TestColor Left, TestColor? Right, int ExpectedResult)
            : ReturnCase<(TestColor Left, TestColor? Right), int>(Name, (Left, Right), ExpectedResult);
    }

    public static class CompareToOrdinalString
    {
        public static TheoryData<Case> Cases =>
        [
            new("ordinal orders 'ch' before 'h' even under a culture that collates 'ch' after 'h'", "ch", "h", true)
        ];

        public sealed record Case(string Name, string LeftValue, string RightValue, bool ExpectedLessThanZero)
            : BaseCase(Name);
    }

    public static class OperatorEquals
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("same value is true", TestColor.Red, TestColor.Red, true),
            new("different values is false", TestColor.Red, TestColor.Blue, false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null == null is true", null, null, true),
            new("null == non-null is false", null, TestColor.Red, false),
            new("non-null == null is false", TestColor.Red, null, false)
        ];

        public sealed record ValidCase(string Name, TestColor? Left, TestColor? Right, bool Expected)
            : IsCase<(TestColor? Left, TestColor? Right)>(Name, (Left, Right), Expected);
    }

    public static class OperatorComparison
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("lesser sorts before greater", TestColor.Red, TestColor.Blue, true, true, false, false),
            new("greater sorts after lesser", TestColor.Blue, TestColor.Red, false, false, true, true),
            new("same value sorts equal", TestColor.Red, TestColor.Red, false, true, false, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null sorts before non-null", null, TestColor.Red, true, true, false, false),
            new("non-null sorts after null", TestColor.Red, null, false, false, true, true),
            new("null sorts equal to null", null, null, false, true, false, true)
        ];

        public sealed record ValidCase(string Name, TestColor? Left, TestColor? Right, bool ExpectedLessThan, bool ExpectedLessThanOrEqual, bool ExpectedGreaterThan, bool ExpectedGreaterThanOrEqual)
            : ValueCase<(TestColor? Left, TestColor? Right)>(Name, (Left, Right));
    }

    public static class HashCode
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Red", TestColor.Red, TestColor.Red.Value.GetHashCode()),
            new("Green", TestColor.Green, TestColor.Green.Value.GetHashCode()),
            new("Blue", TestColor.Blue, TestColor.Blue.Value.GetHashCode())
        ];

        public sealed record ValidCase(string Name, TestColor Input, int ExpectedValue)
            : ReturnCase<TestColor, int>(Name, Input, ExpectedValue);
    }
}
