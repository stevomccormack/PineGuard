namespace PineGuard.Rules;

/// <summary>
/// Provides pure character classification predicates and ASCII boundary constants.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/char">Char Rules documentation</seealso>
public static class CharRules
{
    /// <summary>
    /// The minimum ASCII character value (NUL, <c>0x00</c>).
    /// </summary>
    public const char AsciiMinValue = (char)0x00; // '\0'

    /// <summary>
    /// The maximum ASCII character value (DEL, <c>0x7F</c>).
    /// </summary>
    public const char AsciiMaxValue = (char)0x7F; // '\u007F'

    /// <summary>
    /// The minimum printable ASCII character value (space, <c>0x20</c>).
    /// </summary>
    public const char PrintableAsciiMinValue = (char)0x20; // ' '

    /// <summary>
    /// The maximum printable ASCII character value (tilde <c>~</c>, <c>0x7E</c>).
    /// </summary>
    public const char PrintableAsciiMaxValue = (char)0x7E; // '~'

    /// <summary>
    /// Determines whether the specified character is a Unicode letter.
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a Unicode letter; otherwise, <see langword="false"/>.</returns>
    public static bool IsLetter(char? value) => value is not null && char.IsLetter(value.Value);

    /// <summary>
    /// Determines whether the specified character is an ASCII decimal digit (0–9).
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is between <c>0</c> and <c>9</c>; otherwise, <see langword="false"/>.</returns>
    public static bool IsDigit(char? value) => value is >= '0' and <= '9';

    /// <summary>
    /// Determines whether the specified character is a Unicode letter or an ASCII decimal digit (0–9).
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a letter or digit; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Implemented as <c>IsLetter(value) || IsDigit(value)</c>, so the digit half is ASCII-only,
    /// consistent with <see cref="IsDigit"/> rather than the Unicode-wide <see cref="char.IsLetterOrDigit(char)"/>
    /// (which also accepts non-ASCII Unicode decimal digits).
    /// </remarks>
    public static bool IsLetterOrDigit(char? value) => IsLetter(value) || IsDigit(value);

    /// <summary>
    /// Determines whether the specified character is within the ASCII range (0x00–0x7F).
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/> (null &lt;= 0x7F is false by nullable semantics).</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is &lt;= <see cref="AsciiMaxValue"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAscii(char? value) => value <= AsciiMaxValue;

    /// <summary>
    /// Determines whether the specified character is a printable ASCII character (0x20–0x7E).
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is between <see cref="PrintableAsciiMinValue"/> and
    /// <see cref="PrintableAsciiMaxValue"/> inclusive; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsPrintableAscii(char? value) => value is >= PrintableAsciiMinValue and <= PrintableAsciiMaxValue;

    /// <summary>
    /// Determines whether the specified character is a Unicode whitespace character.
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is whitespace; otherwise, <see langword="false"/>.</returns>
    public static bool IsWhitespace(char? value) => value is not null && char.IsWhiteSpace(value.Value);

    /// <summary>
    /// Determines whether the specified character is a Unicode control character.
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a control character; otherwise, <see langword="false"/>.</returns>
    public static bool IsControl(char? value) => value is not null && char.IsControl(value.Value);

    /// <summary>
    /// Determines whether the specified character is an uppercase Unicode letter.
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is uppercase; otherwise, <see langword="false"/>.</returns>
    public static bool IsUppercase(char? value) => value is not null && char.IsUpper(value.Value);

    /// <summary>
    /// Determines whether the specified character is a lowercase Unicode letter.
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is lowercase; otherwise, <see langword="false"/>.</returns>
    public static bool IsLowercase(char? value) => value is not null && char.IsLower(value.Value);

    /// <summary>
    /// Determines whether the specified character is a hexadecimal digit (0–9, a–f, A–F).
    /// </summary>
    /// <param name="value">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid hex digit; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool hex = CharRules.IsHexDigit('A'); // true
    /// bool hex = CharRules.IsHexDigit('G'); // false
    /// </code>
    /// </example>
    public static bool IsHexDigit(char? value) => value is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
}
