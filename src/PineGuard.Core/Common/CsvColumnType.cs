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
    /// <remarks>
    /// The accepted formats differ by target framework. On <c>net8.0</c> and <c>net10.0</c>, validation uses
    /// the general invariant-culture <c>DateOnly.TryParse(string?, IFormatProvider?, out DateOnly)</c>,
    /// which accepts a broad range of date representations (e.g. <c>"01/15/2024"</c>). On <c>netstandard2.1</c>,
    /// where <c>System.DateOnly</c> does not exist, the fallback parser accepts only the exact format
    /// <c>"yyyy-MM-dd"</c>. Consumers targeting <c>netstandard2.1</c> should not rely on the wider set of
    /// formats accepted on the other target frameworks.
    /// </remarks>
    DateOnly,

    /// <summary>
    /// A time-only column.
    /// </summary>
    /// <remarks>
    /// The accepted formats differ by target framework. On <c>net8.0</c> and <c>net10.0</c>, validation uses
    /// the general invariant-culture <c>TimeOnly.TryParse(string?, IFormatProvider?, out TimeOnly)</c>,
    /// which accepts a broad range of time representations. On <c>netstandard2.1</c>, where
    /// <c>System.TimeOnly</c> does not exist, the fallback parser accepts only the exact formats
    /// <c>"HH:mm:ss"</c>, <c>"HH:mm:ss.fff"</c>, and <c>"HH:mm"</c>. Consumers targeting
    /// <c>netstandard2.1</c> should not rely on the wider set of formats accepted on the other target
    /// frameworks.
    /// </remarks>
    TimeOnly,

    /// <summary>
    /// A date/time with offset column.
    /// </summary>
    DateTimeOffset
}
