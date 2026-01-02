using System.Globalization;
using Xunit.Abstractions;

namespace PineGuard.Testing;

public abstract class UnitTestBase : IDisposable
{
    protected UnitTestBase(ITestOutputHelper? output = null)
    {
        Output = output;

        _originalCulture = CultureInfo.CurrentCulture;
        _originalUiCulture = CultureInfo.CurrentUICulture;

        SetCulture(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture);
    }

    protected ITestOutputHelper? Output { get; }

    private readonly CultureInfo _originalCulture;
    private readonly CultureInfo _originalUiCulture;

    private int _disposed;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1)
        {
            return;
        }

        if (!disposing)
        {
            return;
        }

        try
        {
            SetCulture(_originalCulture, _originalUiCulture);
        }
        finally
        {
            OnDispose();
        }
    }

    /// <summary>
    /// Override only when a test class must cleanup state (e.g., resetting static caches in SUT).
    /// Must be deterministic and not throw.
    /// </summary>
    protected virtual void OnDispose()
    {
    }

    protected IDisposable UseCulture(string cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
        {
            throw new ArgumentException("Culture name must not be null or whitespace.", nameof(cultureName));
        }

        var culture = CultureInfo.GetCultureInfo(cultureName);
        return UseCulture(culture, culture);
    }

    protected IDisposable UseCulture(CultureInfo culture, CultureInfo uiCulture)
    {
        ArgumentNullException.ThrowIfNull(culture);
        ArgumentNullException.ThrowIfNull(uiCulture);

        var previousCulture = CultureInfo.CurrentCulture;
        var previousUiCulture = CultureInfo.CurrentUICulture;

        SetCulture(culture, uiCulture);

        return new Scope(() => SetCulture(previousCulture, previousUiCulture));
    }

    protected IDisposable UseEnvironmentVariable(string key, string? value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Environment variable key must not be null or whitespace.", nameof(key));
        }

        var previous = Environment.GetEnvironmentVariable(key);
        Environment.SetEnvironmentVariable(key, value);

        return new Scope(() => Environment.SetEnvironmentVariable(key, previous));
    }

    protected Random CreateDeterministicRandom(int seed = 123456789) => new(seed);

    protected CancellationToken CreateCancelledToken()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        return cts.Token;
    }

    protected void WriteLine(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        Output?.WriteLine(message);
    }

    private static void SetCulture(CultureInfo culture, CultureInfo uiCulture)
    {
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = uiCulture;
    }

    private sealed class Scope : IDisposable
    {
        private readonly Action _onDispose;
        private int _disposed;

        public Scope(Action onDispose)
        {
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 1)
            {
                return;
            }

            _onDispose();
        }
    }
}
