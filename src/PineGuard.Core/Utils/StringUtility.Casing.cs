using PineGuard.Common;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Attempts to split the specified string into individual words based on the given casing convention.
    /// </summary>
    /// <param name="value">The string to split. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="style">The <see cref="StringCasing"/> convention used to identify word boundaries.</param>
    /// <param name="words">
    /// When this method returns <see langword="true"/>, contains the extracted words.
    /// When <see langword="false"/>, contains an empty list.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully split into words; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// StringUtility.TryCreateWords("myVariableName", StringCasing.CamelCase, out var words);
    /// // words = ["my", "Variable", "Name"]
    /// </code>
    /// </example>
    public static bool TryCreateWords(string? value, StringCasing style, out IReadOnlyList<string> words)
    {
        words = [];

        if (!TryGetTrimmed(value, out var trimmed))
            return false;

        return style switch
        {
            StringCasing.CamelCase => TryCreateWordsFromCamelOrPascal(trimmed, isPascalCase: false, out words),
            StringCasing.PascalCase => TryCreateWordsFromCamelOrPascal(trimmed, isPascalCase: true, out words),
            StringCasing.SnakeCase => TryCreateWordsFromSeparated(trimmed, separator: '_', requiredLetterCasing: RequiredLetterCasing.Lower, out words),
            StringCasing.UpperSnakeCase => TryCreateWordsFromSeparated(trimmed, separator: '_', requiredLetterCasing: RequiredLetterCasing.Upper, out words),
            StringCasing.KebabCase => TryCreateWordsFromSeparated(trimmed, separator: '-', requiredLetterCasing: RequiredLetterCasing.Lower, out words),
            StringCasing.DotCase => TryCreateWordsFromSeparated(trimmed, separator: '.', requiredLetterCasing: RequiredLetterCasing.Lower, out words),
            StringCasing.TrainCase => TryCreateWordsFromSeparated(trimmed, separator: '-', requiredLetterCasing: RequiredLetterCasing.TitleOrAcronym, out words),
            StringCasing.SpaceCase => TryCreateWordsFromSpaceCase(trimmed, out words),
            _ => false
        };
    }

    /// <summary>
    /// Attempts to verify and re-emit the specified string in the given casing convention.
    /// </summary>
    /// <param name="value">The string to convert. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="style">The <see cref="StringCasing"/> convention to use for both parsing and output.</param>
    /// <param name="cased">
    /// When this method returns <see langword="true"/>, contains the cased string.
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool ToCase(string? value, StringCasing style, out string cased)
        => TryToCase(value, inputStyle: style, outputStyle: style, out cased);

    /// <summary>
    /// Attempts to convert the specified string from one casing convention to another.
    /// </summary>
    /// <param name="value">The string to convert. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="inputStyle">The <see cref="StringCasing"/> convention of the input string.</param>
    /// <param name="outputStyle">The <see cref="StringCasing"/> convention for the output.</param>
    /// <param name="cased">
    /// When this method returns <see langword="true"/>, contains the converted string.
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool ToCase(string? value, StringCasing inputStyle, StringCasing outputStyle, out string cased)
        => TryToCase(value, inputStyle, outputStyle, out cased);

    /// <summary>
    /// Attempts to join the specified words into a string using the given casing convention.
    /// </summary>
    /// <param name="words">The words to join. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="outputStyle">The <see cref="StringCasing"/> convention for the output.</param>
    /// <param name="cased">
    /// When this method returns <see langword="true"/>, contains the joined string.
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns><see langword="true"/> if the words were successfully joined; otherwise, <see langword="false"/>.</returns>
    public static bool ToCase(IReadOnlyList<string> words, StringCasing outputStyle, out string cased)
        => TryToCase(words, outputStyle, out cased);

    /// <summary>
    /// Attempts to convert the specified string from one casing convention to another.
    /// </summary>
    /// <param name="value">The string to convert. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="inputStyle">The <see cref="StringCasing"/> convention of the input string.</param>
    /// <param name="outputStyle">The <see cref="StringCasing"/> convention for the output.</param>
    /// <param name="cased">
    /// When this method returns <see langword="true"/>, contains the converted string.
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryToCase(string? value, StringCasing inputStyle, StringCasing outputStyle, out string cased)
    {
        cased = string.Empty;

        return TryCreateWords(value, inputStyle, out var words) && TryToCase(words, outputStyle, out cased);
    }

    /// <summary>
    /// Attempts to join the specified words into a string using the given casing convention.
    /// </summary>
    /// <param name="words">The words to join. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="outputStyle">The <see cref="StringCasing"/> convention for the output.</param>
    /// <param name="cased">
    /// When this method returns <see langword="true"/>, contains the joined string.
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <returns><see langword="true"/> if the words were successfully joined; otherwise, <see langword="false"/>.</returns>
    public static bool TryToCase(IReadOnlyList<string>? words, StringCasing outputStyle, out string cased)
    {
        cased = string.Empty;

        if (words is null || words.Count == 0)
            return false;

        if (words.Any(string.IsNullOrWhiteSpace))
            return false;

        cased = outputStyle switch
        {
            StringCasing.CamelCase => ToCamelCase(words),
            StringCasing.PascalCase => ToPascalCase(words),
            StringCasing.SnakeCase => ToSeparatedCase(words, "_", WordTransform.Lower),
            StringCasing.UpperSnakeCase => ToSeparatedCase(words, "_", WordTransform.Upper),
            StringCasing.KebabCase => ToSeparatedCase(words, "-", WordTransform.Lower),
            StringCasing.DotCase => ToSeparatedCase(words, ".", WordTransform.Lower),
            StringCasing.TrainCase => ToSeparatedCase(words, "-", WordTransform.Title),
            StringCasing.SpaceCase => ToSeparatedCase(words, " ", WordTransform.Title),
            _ => string.Empty
        };

        return cased.Length != 0;
    }

    private enum RequiredLetterCasing
    {
        Lower,
        Upper,
        TitleOrAcronym
    }

    private enum WordTransform
    {
        Lower,
        Upper,
        Title
    }

    private static bool TryCreateWordsFromSpaceCase(string value, out IReadOnlyList<string> words)
    {
        words = [];

        // Enforce simple single-space separation.
        // `TryCreateWords` passes a trimmed value so this method never sees leading/trailing spaces.

        var list = new List<string>();
        var start = 0;

        for (var i = 0; i < value.Length; i++)
        {
            var ch = value[i];

            if (ch == ' ')
            {
                if (i == start)
                    return false; // empty segment

                list.Add(value[start..i]);
                start = i + 1;
                continue;
            }

            if (!char.IsLetterOrDigit(ch))
                return false;
        }

        list.Add(value[start..]);

        words = list;
        return true;
    }

    private static bool TryCreateWordsFromSeparated(
        string value,
        char separator,
        RequiredLetterCasing requiredLetterCasing,
        out IReadOnlyList<string> words)
    {
        words = [];

        if (value[0] == separator || value[^1] == separator)
            return false;

        var list = new List<string>();
        var start = 0;
        var previousWasSeparator = false;

        for (var i = 0; i < value.Length; i++)
        {
            var ch = value[i];

            if (ch == separator)
            {
                if (previousWasSeparator)
                    return false;

                var segment = value[start..i];
                if (!IsValidSeparatedWordSegment(segment, requiredLetterCasing))
                    return false;

                list.Add(segment);
                start = i + 1;
                previousWasSeparator = true;
                continue;
            }

            if (!char.IsLetterOrDigit(ch))
                return false;

            previousWasSeparator = false;
        }

        var last = value[start..];
        if (!IsValidSeparatedWordSegment(last, requiredLetterCasing))
            return false;

        list.Add(last);

        words = list;
        return true;
    }

    private static bool IsValidSeparatedWordSegment(string segment, RequiredLetterCasing requiredLetterCasing) =>
        requiredLetterCasing switch
        {
            RequiredLetterCasing.Lower => AreAllLettersLowerInvariant(segment),
            RequiredLetterCasing.Upper => AreAllLettersUpperInvariant(segment),
            _ => IsTitleCaseWord(segment) || AreAllLettersUpperInvariant(segment)
        };

    private static bool TryCreateWordsFromCamelOrPascal(string value, bool isPascalCase, out IReadOnlyList<string> words)
    {
        words = [];

        if (!char.IsLetter(value[0]))
            return false;

        if (isPascalCase)
        {
            if (!char.IsUpper(value[0]))
                return false;
        }
        else
        {
            if (!char.IsLower(value[0]))
                return false;
        }

        if (value.Any(ch => !char.IsLetterOrDigit(ch))) return false;

        var list = new List<string>();
        var start = 0;

        for (var i = 1; i < value.Length; i++)
        {
            if (!IsWordBoundary(value, i))
                continue;

            list.Add(value[start..i]);
            start = i;
        }

        list.Add(value[start..]);

        words = list;
        return true;
    }

    private static bool IsWordBoundary(string value, int index)
    {
        var prev = value[index - 1];
        var current = value[index];

        // Digit/letter boundaries always start a new word.
        if (char.IsDigit(prev) != char.IsDigit(current))
            return true;

        if (!char.IsLetter(prev))
            return false;

        // lower→upper: start new word
        if (char.IsLower(prev) && char.IsUpper(current))
            return true;

        // ABCd: split before C (the last capital before a lowercase)
        if (!char.IsUpper(prev) || !char.IsUpper(current) || index + 1 >= value.Length)
            return false;

        return char.IsLower(value[index + 1]);
    }

    private static bool AreAllLettersLowerInvariant(string value) => value.Where(char.IsLetter).All(ch => char.ToLowerInvariant(ch) == ch);

    private static bool AreAllLettersUpperInvariant(string value) => value.Where(char.IsLetter).All(ch => char.ToUpperInvariant(ch) == ch);

    private static bool IsTitleCaseWord(string value)
    {
        // Must start with an uppercase letter.
        if (!char.IsUpper(value[0]))
            return false;

        for (var i = 1; i < value.Length; i++)
        {
            var ch = value[i];

            if (!char.IsLetter(ch))
                continue;

            if (char.ToLowerInvariant(ch) != ch)
                return false;
        }

        return true;
    }

    private static string ToCamelCase(IReadOnlyList<string> words)
    {
        var first = words[0];
        var firstWord = ToTitleWord(first);

        // Make first character lower-case.
        if (firstWord.Length == 1)
            return char.ToLowerInvariant(firstWord[0]).ToString();

        firstWord = char.ToLowerInvariant(firstWord[0]) + firstWord[1..];

        if (words.Count == 1)
            return firstWord;

        var restPieces = new string[words.Count - 1];

        for (var i = 1; i < words.Count; i++)
            restPieces[i - 1] = ToTitleWord(words[i]);

        return firstWord + string.Concat(restPieces);
    }

    private static string ToPascalCase(IReadOnlyList<string> words)
        => ToSeparatedCase(words, "", WordTransform.Title);

    private static string ToSeparatedCase(IReadOnlyList<string> words, string separator, WordTransform transform)
    {
        var pieces = new string[words.Count];

        for (var i = 0; i < words.Count; i++)
        {
            var word = words[i];

            pieces[i] = transform switch
            {
                WordTransform.Lower => word.ToLowerInvariant(),
                WordTransform.Upper => word.ToUpperInvariant(),
                _ => ToTitleWord(word)
            };
        }

        return string.Join(separator, pieces);
    }

    private static string ToTitleWord(string word)
    {
        if (word.Length == 1)
            return char.ToUpperInvariant(word[0]).ToString();

        var lower = word.ToLowerInvariant();
        return char.ToUpperInvariant(lower[0]) + lower[1..];
    }
}
