using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.UriRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

#pragma warning disable CS0618

public static class FluentUriExtensionsTestData
{
    public static class AbsoluteUri
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsAbsoluteUri.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsAbsoluteUri.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid absolute URI.", Code: MustCodes.Uri.Form.NotAbsolute)
        });
    }

    public static class RelativeUri
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsRelativeUri.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsRelativeUri.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid relative URI.")
        });
    }

    public static class WebUrl
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsUrl.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsUrl.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid URL.")
        });
    }

    public static class Url
    {
        public static TheoryData<FluentCase<string?>> Cases => WebUrl.Cases;
    }

    public static class HttpsUrl
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHttpsUrl.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpsUrl.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.IsHttpsUrl.HttpUrl) => new FluentExpected(false, "Value must be a valid HTTPS URL."),
            _ => new FluentExpected(false, "Value must be a valid URL.")
        });
    }

    public static class HttpUrl
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHttpUrl.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpUrl.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.IsHttpUrl.HttpsUrl) => new FluentExpected(false, "Value must be a valid HTTP URL."),
            _ => new FluentExpected(false, "Value must be a valid URL.")
        });
    }

    public static class FileUri
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsFileUri.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFileUri.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            nameof(F.IsFileUri.HttpsUrl) => new FluentExpected(false, "Value must be a valid file URI."),
            _ => new FluentExpected(false, "Value must be a valid absolute URI.")
        });
    }

    public static class FilePath
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsFilePath.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFilePath.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid file path.")
        });
    }

    public static class NotFilePath
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = new TheoryData<FluentCase<string?>>();
                foreach (var s in F.IsFilePath.ValidScenarios)
                    td.Add(new FluentCase<string?>(s.Name, s.Inputs, new FluentExpected(false, "Value must not be a valid file path.")));
                foreach (var s in F.IsFilePath.InvalidScenarios)
                {
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (s.Name == nameof(F.IsFilePath.NullValue))
                        td.Add(new FluentCase<string?>(s.Name, s.Inputs, new FluentExpected(false, "Value must not be null.")));
                    else
                        td.Add(new FluentCase<string?>(s.Name, s.Inputs, new FluentExpected(true)));
                }
                return td;
            }
        }
    }

    public static class HasScheme
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = new TheoryData<FluentCase<string?>>();
                foreach (var s in F.HasScheme.AllScenarios.Except(nameof(F.HasScheme.FtpMatch)))
                {
                    var expected = s.Name switch
                    {
                        nameof(F.HasScheme.NullValue) => new FluentExpected(false, "Value must not be null."),
                        _ when s.IsValid => new FluentExpected(true),
                        _ => new FluentExpected(false, "Value must have the expected scheme.")
                    };
                    td.Add(new FluentCase<string?>(s.Name, s.Inputs.value, expected));
                }
                return td;
            }
        }

        public static string Scheme => "https";
    }

    public static class NotHasScheme
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = new TheoryData<FluentCase<string?>>();
                foreach (var s in F.HasScheme.AllScenarios.Except(nameof(F.HasScheme.FtpMatch)))
                {
                    var expected = s.Name switch
                    {
                        nameof(F.HasScheme.NullValue) => new FluentExpected(false, "Value must not be null."),
                        _ when s.IsValid => new FluentExpected(false, "Value must not have the expected scheme."),
                        _ => new FluentExpected(true)
                    };
                    td.Add(new FluentCase<string?>(s.Name, s.Inputs.value, expected));
                }
                return td;
            }
        }

        public static string Scheme => "https";
    }
}
