namespace PineGuard.FluentResults.UnitTests;

internal static class MustErrorAssert
{
    public static void Expected(IReadOnlyList<(string code, string message, string propertyPath)> expected, IReadOnlyList<global::FluentResults.IError> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        for (var i = 0; i < expected.Count; i++)
            Expected(expected[i], Assert.IsType<MustError>(actual[i]));
    }

    public static void Expected((string code, string message, string propertyPath) expected, MustError actual)
    {
        Assert.Equal(expected.code, actual.Code);
        Assert.Equal(expected.message, actual.Message);
        Assert.Equal(expected.propertyPath, actual.PropertyPath);
        Assert.Equal(expected.code, actual.Metadata[MustError.CodeMetadataKey]);
        Assert.Equal(expected.propertyPath, actual.Metadata[MustError.PropertyPathMetadataKey]);
    }
}
