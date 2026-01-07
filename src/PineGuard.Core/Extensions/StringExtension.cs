using PineGuard.Utils;

namespace PineGuard.Extensions;

public static class StringExtension
{
    public static bool TitleCase(this string value)
        => StringUtility.TitleCase(value);
}
