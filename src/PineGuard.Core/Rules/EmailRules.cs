using System.Net.Mail;

namespace PineGuard.Rules;

public static class EmailRules
{
    public static bool IsEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return MailAddress.TryCreate(value.Trim(), out _);
    }
}
