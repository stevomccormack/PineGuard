using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests;

public static class ThrowsCaseAssert
{
    public static void Expected(Exception ex, IThrowsCase testCase)
    {
        ThrowHelper.ThrowIfNull(testCase);
        Expected(ex, testCase.ExpectedException);
    }

    private static void Expected(Exception ex, ExpectedException expected)
    {
        ThrowHelper.ThrowIfNull(ex);
        ThrowHelper.ThrowIfNull(expected);

        if (ex.GetType() != expected.Type)
        {
            throw new InvalidOperationException(
                $"Expected exception type '{expected.Type.FullName}', but got '{ex.GetType().FullName}'.");
        }

        if (expected.ParamName is not null)
        {
            if (ex is not ArgumentException argEx)
            {
                throw new InvalidOperationException(
                    $"Expected exception to be assignable to '{typeof(ArgumentException).FullName}' when asserting ParamName, but got '{ex.GetType().FullName}'.");
            }

            if (!string.Equals(expected.ParamName, argEx.ParamName, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Expected ParamName '{expected.ParamName}', but got '{argEx.ParamName}'.");
            }
        }

        // ReSharper disable once InvertIf
        if (expected.MessageContains is not null)
        {
            if (ex.Message.IndexOf(expected.MessageContains, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException(
                    $"Expected exception message to contain '{expected.MessageContains}', but it did not. Actual message: '{ex.Message}'.");
            }
        }
    }
}
