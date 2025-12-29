using System.Net.Mail;

namespace PineGuard.Utils;

public static class EmailUtility
{
    public static bool TryCreate(string? value, out MailAddress? email)
    {
        email = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return MailAddress.TryCreate(trimmed, out email);
    }
}
