namespace PineGuard.Common;

/// <summary>
/// Defines the schema for a single CSV column, including name, type, and constraints.
/// </summary>
/// <param name="Name">The column name.</param>
/// <param name="Type">The expected data type of the column.</param>
/// <param name="IsRequired">Whether the column value is required. Defaults to <see langword="true"/>.</param>
/// <param name="MaxLength">The maximum character length for the column value. Defaults to <see cref="DefaultMaxLength"/>.</param>
public readonly record struct CsvColumnSchema(
    string Name,
    CsvColumnType Type,
    bool IsRequired = true,
    int MaxLength = CsvColumnSchema.DefaultMaxLength)
{
    /// <summary>
    /// The default maximum length for a CSV column value (255 characters).
    /// </summary>
    public const int DefaultMaxLength = 255;

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvColumnSchema"/> struct using a string type name.
    /// </summary>
    /// <param name="name">The column name.</param>
    /// <param name="type">The column type as a string (e.g., <c>"string"</c>, <c>"int"</c>, <c>"guid"</c>).</param>
    /// <param name="isRequired">Whether the column value is required. Defaults to <see langword="true"/>.</param>
    /// <param name="maxLength">The maximum character length for the column value. Defaults to <see cref="DefaultMaxLength"/>.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="type"/> is <see langword="null"/>, empty, or an unsupported type name.</exception>
    public CsvColumnSchema(
        string name,
        string type,
        bool isRequired = true,
        int maxLength = DefaultMaxLength)
        : this(name, ParseColumnType(type), isRequired, maxLength)
    {
    }

    private static CsvColumnType ParseColumnType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be null/empty.", nameof(type));

        var normalized = type.Trim();

        return normalized.ToLowerInvariant() switch
        {
            "string" or "text" => CsvColumnType.String,
            "int" or "int32" => CsvColumnType.Int32,
            "long" or "int64" => CsvColumnType.Int64,
            "decimal" => CsvColumnType.Decimal,
            "float" or "single" => CsvColumnType.Single,
            "double" => CsvColumnType.Double,
            "guid" or "uuid" => CsvColumnType.Guid,
            "bool" or "boolean" => CsvColumnType.Bool,
            "date" or "dateonly" => CsvColumnType.DateOnly,
            "time" or "timeonly" => CsvColumnType.TimeOnly,
            "datetimeoffset" => CsvColumnType.DateTimeOffset,
            _ => throw new ArgumentException($"Unsupported CSV column type '{type}'.", nameof(type))
        };
    }
}
