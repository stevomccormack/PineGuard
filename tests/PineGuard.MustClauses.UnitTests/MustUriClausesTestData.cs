using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.UriRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

#pragma warning disable CS0618

public static class MustUriClausesTestData
{
    public static class AbsoluteUri
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsAbsoluteUri.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsAbsoluteUri.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsAbsoluteUri.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid absolute URI.")
        });
    }

    public static class RelativeUri
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsRelativeUri.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsRelativeUri.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsRelativeUri.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid relative URI.")
        });
    }

    public static class Url
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsUrl.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsUrl.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsUrl.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid URL.")
        });
    }

    public static class HttpsUrl
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHttpsUrl.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsHttpsUrl.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsHttpsUrl.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            nameof(F.IsHttpsUrl.HttpUrl) => new MustExpected(false, "value must be a valid HTTPS URL."),
            _ => new MustExpected(false, "value must be a valid URL.")
        });
    }

    public static class HttpUrl
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHttpUrl.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsHttpUrl.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsHttpUrl.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            nameof(F.IsHttpUrl.HttpsUrl) => new MustExpected(false, "value must be a valid HTTP URL."),
            _ => new MustExpected(false, "value must be a valid URL.")
        });
    }

    public static class FileUri
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsFileUri.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsFileUri.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsFileUri.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            nameof(F.IsFileUri.HttpsUrl) => new MustExpected(false, "value must be a valid file URI."),
            _ => new MustExpected(false, "value must be a valid absolute URI.")
        });
    }

    public static class FilePath
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsFilePath.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsFilePath.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsFilePath.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid file path.")
        });
    }

    public static class NotFilePath
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsFilePath.InvalidScenarios
            .Except(nameof(F.IsFilePath.NullValue))
            .ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases
        {
            get
            {
                var td = new TheoryData<MustCase<string?>>();
                foreach (var s in F.IsFilePath.ValidScenarios)
                    td.Add(new MustCase<string?>(s.Name, s.Inputs, new MustExpected(false, "value must not be a valid file path.")));
                td.Add(new MustCase<string?>(nameof(F.IsFilePath.NullValue), F.IsFilePath.NullValue, new MustExpected(false, "value must not be null.", "value")));
                return td;
            }
        }
    }

    public static class HasScheme
    {
        public static TheoryData<MustCase<(string? value, string scheme)>> ValidCases => F.HasScheme.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(string? value, string scheme)>> InvalidCases
        {
            get
            {
                var data = F.HasScheme.InvalidScenarios.ToMustCases(s => s.Name switch
                {
                    nameof(F.HasScheme.NullValue) => new MustExpected(false, "value must not be null.", "value"),
                    _ => new MustExpected(false, "value must have the expected scheme.")
                });
                data.Add(new MustCase<(string? value, string scheme)>(nameof(F.HasScheme.NullScheme), F.HasScheme.NullScheme, new MustExpected(false, "scheme must not be null.", "scheme")));
                return data;
            }
        }
    }

    public static class NotHasScheme
    {
        public static TheoryData<MustCase<(string? value, string scheme)>> ValidCases
        {
            get
            {
                var td = new TheoryData<MustCase<(string? value, string scheme)>>();
                foreach (var s in F.HasScheme.InvalidScenarios)
                    if (s.Name != nameof(F.HasScheme.NullValue))
                        td.Add(new MustCase<(string? value, string scheme)>(s.Name, s.Inputs, new MustExpected(true)));
                return td;
            }
        }

        public static TheoryData<MustCase<(string? value, string scheme)>> InvalidCases
        {
            get
            {
                var td = new TheoryData<MustCase<(string? value, string scheme)>>();
                foreach (var s in F.HasScheme.ValidScenarios)
                    td.Add(new MustCase<(string? value, string scheme)>(s.Name, s.Inputs, new MustExpected(false, "value must not have the expected scheme.")));
                td.Add(new MustCase<(string? value, string scheme)>(nameof(F.HasScheme.NullValue), F.HasScheme.NullValue, new MustExpected(false, "value must not be null.", "value")));
                td.Add(new MustCase<(string? value, string scheme)>(nameof(F.HasScheme.NullScheme), F.HasScheme.NullScheme, new MustExpected(false, "scheme must not be null.", "scheme")));
                return td;
            }
        }
    }
}
