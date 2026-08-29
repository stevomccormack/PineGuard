namespace PineGuard.Extensions.Options.UnitTests.Samples;

public sealed class SmtpOptions
{
    public string? Host { get; set; }

    public int Port { get; set; }

    public string? From { get; set; }

    public bool UseTls { get; set; }
}
