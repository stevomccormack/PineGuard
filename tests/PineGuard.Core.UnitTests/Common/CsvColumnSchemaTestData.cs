using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class CsvColumnSchemaTestData
{
    public static class Init
    {
        public static TheoryData<Case> Cases =>
        [
            new("With Params", ("other", CsvColumnType.Int32, false, 42))
        ];

        public sealed record Case(string Name, (string Name, CsvColumnType Type, bool IsRequired, int MaxLength) Value)
            : ValueCase<(string Name, CsvColumnType Type, bool IsRequired, int MaxLength)>(Name, Value);
    }

    public static class WithExpression
    {
        public static TheoryData<Case> Cases =>
        [
            new("Mutate all fields", ("original", CsvColumnType.String, true, 255), ("mutated", CsvColumnType.Guid, false, 100))
        ];

        public sealed record Case(string Name, (string Name, CsvColumnType Type, bool IsRequired, int MaxLength) Value, (string Name, CsvColumnType Type, bool IsRequired, int MaxLength) Mutated)
            : ValueCase<(string Name, CsvColumnType Type, bool IsRequired, int MaxLength)>(Name, Value);
    }

    public static class CtorStringType
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("string", ("name", "string"), CsvColumnType.String),
            new("text", ("name", "text"), CsvColumnType.String),
            new("int", ("name", "int"), CsvColumnType.Int32),
            new("int32", ("name", "int32"), CsvColumnType.Int32),
            new("int64", ("name", "int64"), CsvColumnType.Int64),
            new("long", ("name", "long"), CsvColumnType.Int64),
            new("decimal", ("name", "decimal"), CsvColumnType.Decimal),
            new("float", ("name", "float"), CsvColumnType.Single),
            new("single", ("name", "single"), CsvColumnType.Single),
            new("double", ("name", "double"), CsvColumnType.Double),
            new("guid", ("name", "guid"), CsvColumnType.Guid),
            new("uuid", ("name", "uuid"), CsvColumnType.Guid),
            new("bool", ("name", "bool"), CsvColumnType.Bool),
            new("boolean", ("name", "boolean"), CsvColumnType.Bool),
            new("date", ("name", "date"), CsvColumnType.DateOnly),
            new("dateonly", ("name", "dateonly"), CsvColumnType.DateOnly),
            new("time", ("name", "time"), CsvColumnType.TimeOnly),
            new("timeonly", ("name", "timeonly"), CsvColumnType.TimeOnly),
            new("datetimeoffset", ("name", "datetimeoffset"), CsvColumnType.DateTimeOffset),
            new("trim+case", ("name", "  InT32  "), CsvColumnType.Int32)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("type null", ("name", null), new ExpectedException(typeof(ArgumentException), "type")),
            new("type empty", ("name", ""), new ExpectedException(typeof(ArgumentException), "type")),
            new("type whitespace", ("name", "  "), new ExpectedException(typeof(ArgumentException), "type")),
            new("type unsupported", ("name", "wat"), new ExpectedException(typeof(ArgumentException), "type"))
        ];

        public sealed record ValidCase(string Name, (string Name, string Type) Value, CsvColumnType Expected)
            : ReturnCase<(string Name, string Type), CsvColumnType>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (string Name, string? Type) Value, ExpectedException ExpectedException)
            : ThrowsCase<(string Name, string? Type)>(Name, Value, ExpectedException);
    }
}
