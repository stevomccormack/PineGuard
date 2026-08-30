using Microsoft.Extensions.Localization;

namespace PineGuard.AspNetCore.UnitTests.Samples;

public sealed class FakeStringLocalizerFactory(IReadOnlyDictionary<string, string> resources) : IStringLocalizerFactory
{
    public Type? RequestedResourceSource { get; private set; }

    public IStringLocalizer Create(Type resourceSource)
    {
        RequestedResourceSource = resourceSource;
        return new FakeStringLocalizer(resources);
    }

    public IStringLocalizer Create(string baseName, string location) => new FakeStringLocalizer(resources);

    private sealed class FakeStringLocalizer(IReadOnlyDictionary<string, string> resources) : IStringLocalizer
    {
        public LocalizedString this[string name] =>
            resources.TryGetValue(name, out var value)
                ? new LocalizedString(name, value, resourceNotFound: false)
                : new LocalizedString(name, name, resourceNotFound: true);

        public LocalizedString this[string name, params object[] arguments] => this[name];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            resources.Select(resource => new LocalizedString(resource.Key, resource.Value));
    }
}
