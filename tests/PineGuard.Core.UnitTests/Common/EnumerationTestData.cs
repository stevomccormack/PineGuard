namespace PineGuard.Core.UnitTests.Common;

using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

public static class EnumerationTestData
{
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
            new("Tango", 20, "Tango"),
        ];

        public static TheoryData<ValidCase> EdgeCases => [];

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
            new("value=20, name=' \t '", 20, " \t ", new ExpectedException(typeof(ArgumentException), "name")),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, int InputValue, string EnumerationName)
            : ReturnCase<(int Value, string EnumerationName), string>(Name, (InputValue, EnumerationName), EnumerationName);

        public sealed record InvalidCase(string Name, int EnumerationValue, string? EnumerationName, ExpectedException ExpectedException)
            : ThrowsCase<(int EnumerationValue, string? EnumerationName)>(Name, (EnumerationValue, EnumerationName), ExpectedException);

        #endregion
    }

    public static class StringConstructor
    {
        public static TheoryData<ValidCase> ValidCases => [];

        public static TheoryData<ValidCase> EdgeCases => [];

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
            new("value='ok', name='\r\n'", "ok", "\r\n", new ExpectedException(typeof(ArgumentException), "name")),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, string InputValue, string EnumerationName)
            : ReturnCase<(string Value, string EnumerationName), string>(Name, (InputValue, EnumerationName), EnumerationName);

        public sealed record InvalidCase(string Name, string? EnumerationValue, string? EnumerationName, ExpectedException ExpectedException)
            : ThrowsCase<(string? EnumerationValue, string? EnumerationName)>(Name, (EnumerationValue, EnumerationName), ExpectedException);

        #endregion
    }
}
