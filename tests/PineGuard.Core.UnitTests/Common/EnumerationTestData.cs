namespace PineGuard.Core.UnitTests.Common;

using PineGuard.Testing;

public static class EnumerationTestData
{
    public static class IntConstructor
    {
        public sealed record ValidCase(string Name, int Value, string EnumerationName);

        public sealed record InvalidCase(string Name, int Value, string? EnumerationName, ExpectedException Expected);

        private static ValidCase V(int value, string enumerationName)
            => new(Name: enumerationName, Value: value, EnumerationName: enumerationName);

        private static InvalidCase I(int value, string? enumerationName)
            => new(
                Name: $"value={value}, name='{enumerationName ?? "<null>"}'",
                Value: value,
                EnumerationName: enumerationName,
                Expected: new ExpectedException(typeof(ArgumentException), ParamName: "name"));

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V(1, "Alpha") },
            { V(2, "Bravo") },
            { V(3, "Charlie") },
            { V(4, "Delta") },
            { V(5, "Echo") },
            { V(6, "Foxtrot") },
            { V(7, "Golf") },
            { V(8, "Hotel") },
            { V(9, "India") },
            { V(10, "Juliet") },
            { V(11, "Kilo") },
            { V(12, "Lima") },
            { V(13, "Mike") },
            { V(14, "November") },
            { V(15, "Oscar") },
            { V(16, "Papa") },
            { V(17, "Quebec") },
            { V(18, "Romeo") },
            { V(19, "Sierra") },
            { V(20, "Tango") },
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I(1, null) },
            { I(2, "") },
            { I(3, " ") },
            { I(4, "\t") },
            { I(5, "\r") },
            { I(6, "\n") },
            { I(7, "\u00A0") },
            { I(8, "\u2007") },
            { I(9, "\u202F") },
            { I(10, "\u2003") },
            { I(11, "\u2009") },
            { I(12, "\u3000") },
            { I(13, "\v") },
            { I(14, "\f") },
            { I(15, "\t\t") },
            { I(16, "  ") },
            { I(17, "\n\n") },
            { I(18, "\r\n") },
            { I(19, "\t \n") },
            { I(20, " \t ") },
        };
    }

    public static class StringConstructor
    {
        public sealed record InvalidCase(string Name, string? Value, string? EnumerationName, ExpectedException Expected);

        private static InvalidCase IValue(string? value, string? enumerationName)
            => new(
                Name: $"value='{value ?? "<null>"}', name='{enumerationName ?? "<null>"}'",
                Value: value,
                EnumerationName: enumerationName,
                Expected: new ExpectedException(typeof(ArgumentException), ParamName: "value"));

        private static InvalidCase IName(string? value, string? enumerationName)
            => new(
                Name: $"value='{value ?? "<null>"}', name='{enumerationName ?? "<null>"}'",
                Value: value,
                EnumerationName: enumerationName,
                Expected: new ExpectedException(typeof(ArgumentException), ParamName: "name"));

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { IValue(null, "X") },
            { IValue(null, null) },
            { IName("ok", null) },
            { IName("ok", "") },
            { IName("ok", " ") },
            { IName("ok", "\t") },
            { IName("ok", "\r") },
            { IName("ok", "\n") },
            { IName("ok", "\u00A0") },
            { IName("ok", "\u2007") },
            { IName("ok", "\u202F") },
            { IName("ok", "\u2003") },
            { IName("ok", "\u2009") },
            { IName("ok", "\u3000") },
            { IName("ok", "\v") },
            { IName("ok", "\f") },
            { IName("ok", "  ") },
            { IName("ok", "\t\t") },
            { IName("ok", "\n\n") },
            { IName("ok", "\r\n") },
        };
    }
}
