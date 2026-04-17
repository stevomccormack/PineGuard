namespace PineGuard.Common;

/// <summary>
/// Specifies the data type of CSV column for schema-based validation.
/// </summary>
public enum CsvColumnType
{
    /// <summary>
    /// A string/text column.
    /// </summary>
    String = 0,

    /// <summary>
    /// A 32-bit integer column.
    /// </summary>
    Int32,

    /// <summary>
    /// A 64-bit integer column.
    /// </summary>
    Int64,

    /// <summary>
    /// A decimal column.
    /// </summary>
    Decimal,

    /// <summary>
    /// A single-precision floating-point column.
    /// </summary>
    Single,

    /// <summary>
    /// A double-precision floating-point column.
    /// </summary>
    Double,

    /// <summary>
    /// A GUID/UUID column.
    /// </summary>
    Guid,

    /// <summary>
    /// A boolean column.
    /// </summary>
    Bool,

    /// <summary>
    /// A date-only column.
    /// </summary>
    DateOnly,

    /// <summary>
    /// A time-only column.
    /// </summary>
    TimeOnly,

    /// <summary>
    /// A date/time with offset column.
    /// </summary>
    DateTimeOffset
}
