using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.UriRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class UriAttributesTestData
{
    public static class AbsoluteUri
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsAbsoluteUri.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsAbsoluteUri.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid absolute URI.")
        });
    }

    public static class RelativeUri
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsRelativeUri.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsRelativeUri.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid relative URI.")
        });
    }

    public static class WebUrl
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUrl.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsUrl.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid URL.")
        });
    }

    public static class HttpsUrl
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsHttpsUrl.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsHttpsUrl.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            nameof(F.IsHttpsUrl.HttpUrl) => new DataAnnotationExpected(false, "Value must be a valid HTTPS URL."),
            _ => new DataAnnotationExpected(false, "Value must be a valid URL.")
        });
    }

    public static class HttpUrl
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsHttpUrl.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsHttpUrl.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            nameof(F.IsHttpUrl.HttpsUrl) => new DataAnnotationExpected(false, "Value must be a valid HTTP URL."),
            _ => new DataAnnotationExpected(false, "Value must be a valid URL.")
        });
    }

    public static class FileUri
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsFileUri.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsFileUri.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            nameof(F.IsFileUri.HttpsUrl) => new DataAnnotationExpected(false, "Value must be a valid file URI."),
            _ => new DataAnnotationExpected(false, "Value must be a valid absolute URI.")
        });
    }

    public static class FilePath
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsFilePath.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsFilePath.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid file path.")
        });
    }

    public static class NotFilePath
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsFilePath.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsFilePath.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not be a valid file path."),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class HasScheme
    {
        public static string Scheme => "https";

        public static TheoryData<DataAnnotationCase> Cases => F.HasScheme.AllScenarios
            .Except(nameof(F.HasScheme.FtpMatch))
            .ToDataAnnotationCases(inputs => inputs.value, s => s.Name switch
            {
                nameof(F.HasScheme.NullValue) => new DataAnnotationExpected(true),
                _ when s.IsValid => new DataAnnotationExpected(true),
                _ => new DataAnnotationExpected(false, "Value must have the expected scheme.")
            });
    }

    public static class NotHasScheme
    {
        public static string Scheme => "https";

        public static TheoryData<DataAnnotationCase> Cases => F.HasScheme.AllScenarios
            .Except(nameof(F.HasScheme.FtpMatch))
            .ToDataAnnotationCases(inputs => inputs.value, s => s.Name switch
            {
                nameof(F.HasScheme.NullValue) => new DataAnnotationExpected(true),
                _ when s.IsValid => new DataAnnotationExpected(false, "Value must not have the expected scheme."),
                _ => new DataAnnotationExpected(true)
            });
    }
}
