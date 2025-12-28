using System.Net.Mail;

namespace PineGuard.Utils;

public static class EmailUtility
{
    public static bool TryCreate(string? value, out MailAddress? email)
    {
        email = null;

        if (!StringUtility.TryGetNonEmptyTrimmed(value, out var trimmed))
            return false;

        return MailAddress.TryCreate(trimmed, out email);
    }
}
