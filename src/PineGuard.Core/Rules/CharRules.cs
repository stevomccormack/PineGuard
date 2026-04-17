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
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is a Unicode letter; otherwise, <see langword="false"/>.</returns>
    public static bool IsLetter(char? c) => c is not null && char.IsLetter(c.Value);

    /// <summary>
    /// Determines whether the specified character is an ASCII decimal digit (0–9).
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is between <c>0</c> and <c>9</c>; otherwise, <see langword="false"/>.</returns>
    public static bool IsDigit(char? c) => c is >= '0' and <= '9';

    /// <summary>
    /// Determines whether the specified character is a Unicode letter or a decimal digit.
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is a letter or digit; otherwise, <see langword="false"/>.</returns>
    public static bool IsLetterOrDigit(char? c) => c is not null && char.IsLetterOrDigit(c.Value);

    /// <summary>
    /// Determines whether the specified character is within the ASCII range (0x00–0x7F).
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/> (null &lt;= 0x7F is false by nullable semantics).</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is &lt;= <see cref="AsciiMaxValue"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAscii(char? c) => c <= AsciiMaxValue;

    /// <summary>
    /// Determines whether the specified character is a printable ASCII character (0x20–0x7E).
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="c"/> is between <see cref="PrintableAsciiMinValue"/> and
    /// <see cref="PrintableAsciiMaxValue"/> inclusive; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsPrintableAscii(char? c) => c is >= PrintableAsciiMinValue and <= PrintableAsciiMaxValue;

    /// <summary>
    /// Determines whether the specified character is a Unicode whitespace character.
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is whitespace; otherwise, <see langword="false"/>.</returns>
    public static bool IsWhitespace(char? c) => c is not null && char.IsWhiteSpace(c.Value);

    /// <summary>
    /// Determines whether the specified character is a Unicode control character.
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is a control character; otherwise, <see langword="false"/>.</returns>
    public static bool IsControl(char? c) => c is not null && char.IsControl(c.Value);

    /// <summary>
    /// Determines whether the specified character is an uppercase Unicode letter.
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is uppercase; otherwise, <see langword="false"/>.</returns>
    public static bool IsUppercase(char? c) => c is not null && char.IsUpper(c.Value);

    /// <summary>
    /// Determines whether the specified character is a lowercase Unicode letter.
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="c"/> is lowercase; otherwise, <see langword="false"/>.</returns>
    public static bool IsLowercase(char? c) => c is not null && char.IsLower(c.Value);

    /// <summary>
    /// Determines whether the specified character is a hexadecimal digit (0–9, a–f, A–F).
    /// </summary>
    /// <param name="c">The character to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="c"/> is a valid hex digit; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool hex = CharRules.IsHexDigit('A'); // true
    /// bool hex = CharRules.IsHexDigit('G'); // false
    /// </code>
    /// </example>
    public static bool IsHexDigit(char? c) => c is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
}
