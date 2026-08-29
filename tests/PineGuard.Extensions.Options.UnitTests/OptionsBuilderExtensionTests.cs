using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PineGuard.Extensions.Options.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Extensions.Options.UnitTests;

public sealed class OptionsBuilderExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(OptionsBuilderExtensionTestData.ValidateMustRules.Cases), MemberType = typeof(OptionsBuilderExtensionTestData.ValidateMustRules))]
    public void ValidateMustRules_BehavesAsExpected(OptionsBuilderExtensionTestData.ValidateMustRules.Case tc)
    {
        // Arrange
        var (options, registerValidator) = tc.Value;
        var services = new ServiceCollection();
        services.AddOptions<SmtpOptions>().Configure(o => CopySmtp(options, o)).ValidateMustRules();
        if (registerValidator)
            services.AddSingleton<IMustValidator<SmtpOptions>, SmtpOptionsValidator>();

        // Act & Assert
        using var provider = services.BuildServiceProvider();
        AssertResult(tc.Expected, () => _ = provider.GetRequiredService<IOptions<SmtpOptions>>().Value);
    }

    [Theory]
    [MemberData(nameof(OptionsBuilderExtensionTestData.ValidateMustRules.InvalidCases), MemberType = typeof(OptionsBuilderExtensionTestData.ValidateMustRules))]
    public void ValidateMustRules_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(OptionsBuilderExtensionTestData.ValidateMustRulesInstance.Cases), MemberType = typeof(OptionsBuilderExtensionTestData.ValidateMustRulesInstance))]
    public void ValidateMustRulesInstance_BehavesAsExpected(OptionsBuilderExtensionTestData.ValidateMustRulesInstance.Case tc)
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddOptions<SmtpOptions>().Configure(o => CopySmtp(tc.Value, o)).ValidateMustRules(new SmtpOptionsValidator());

        // Act & Assert
        using var provider = services.BuildServiceProvider();
        AssertResult(tc.Expected, () => _ = provider.GetRequiredService<IOptions<SmtpOptions>>().Value);
    }

    [Theory]
    [MemberData(nameof(OptionsBuilderExtensionTestData.ValidateMustRulesInstance.InvalidCases), MemberType = typeof(OptionsBuilderExtensionTestData.ValidateMustRulesInstance))]
    public void ValidateMustRulesInstance_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(OptionsBuilderExtensionTestData.ValidateMustRulesInline.Cases), MemberType = typeof(OptionsBuilderExtensionTestData.ValidateMustRulesInline))]
    public void ValidateMustRulesInline_BehavesAsExpected(OptionsBuilderExtensionTestData.ValidateMustRulesInline.Case tc)
    {
        // Arrange
        var configureInvocationCount = 0;
        var services = new ServiceCollection();
        services.AddOptions<CacheOptions>()
            .Configure(o => o.TtlSeconds = tc.Value)
            .ValidateMustRules(v =>
            {
                configureInvocationCount++;
                v.RuleFor(o => o.TtlSeconds, ttl => Must.Be.GreaterThan(ttl, 0));
            });

        // Act & Assert
        Assert.Equal(1, configureInvocationCount);
        using var provider = services.BuildServiceProvider();
        AssertResult(tc.Expected, () => _ = provider.GetRequiredService<IOptions<CacheOptions>>().Value);
    }

    [Theory]
    [MemberData(nameof(OptionsBuilderExtensionTestData.ValidateMustRulesInline.InvalidCases), MemberType = typeof(OptionsBuilderExtensionTestData.ValidateMustRulesInline))]
    public void ValidateMustRulesInline_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(OptionsBuilderExtensionTestData.ValidateOnStart.Cases), MemberType = typeof(OptionsBuilderExtensionTestData.ValidateOnStart))]
    public async Task ValidateOnStart_BehavesAsExpected(OptionsBuilderExtensionTestData.ValidateOnStart.Case tc)
    {
        // Arrange
        using var host = new HostBuilder()
            .ConfigureAppConfiguration(configuration => configuration.AddInMemoryCollection(tc.Value))
            .ConfigureServices(services =>
            {
                services.AddSingleton<IMustValidator<SmtpOptions>, SmtpOptionsValidator>();
                services.AddOptions<SmtpOptions>().BindConfiguration("Smtp").ValidateMustRules().ValidateOnStart();
            })
            .Build();

        // Act & Assert
        await AssertStartupResult(tc.Expected, host);
    }

    private static void CopySmtp(SmtpOptions source, SmtpOptions target)
    {
        target.Host = source.Host;
        target.Port = source.Port;
        target.From = source.From;
        target.UseTls = source.UseTls;
    }

    private static void AssertResult(OptionsBuilderExtensionTestData.ResolveExpected expected, Action resolve)
    {
        if (expected.ExceptionType is null)
        {
            resolve();
            return;
        }

        var ex = Assert.Throws(expected.ExceptionType, resolve);
        AssertMessage(expected, ex);
    }

    private static async Task AssertStartupResult(OptionsBuilderExtensionTestData.ResolveExpected expected, IHost host)
    {
        if (expected.ExceptionType is null)
        {
            await host.StartAsync();
            await host.StopAsync();
            return;
        }

        var ex = await Assert.ThrowsAsync(expected.ExceptionType, () => host.StartAsync());
        AssertMessage(expected, ex);
    }

    private static void AssertMessage(OptionsBuilderExtensionTestData.ResolveExpected expected, Exception ex)
    {
        if (expected.MessageContains is not null)
            Assert.Contains(expected.MessageContains, ex.Message, StringComparison.OrdinalIgnoreCase);

        if (expected.MessageContainsAll is not null)
            foreach (var fragment in expected.MessageContainsAll)
                Assert.Contains(fragment, ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
