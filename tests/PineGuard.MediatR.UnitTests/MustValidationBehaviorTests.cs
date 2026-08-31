using PineGuard.MediatR.UnitTests.Samples;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.MediatR.UnitTests;

public sealed class MustValidationBehaviorTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationBehaviorTestData.Handle.Cases), MemberType = typeof(MustValidationBehaviorTestData.Handle))]
    public async Task Handle_BehavesAsExpected(MustValidationCase<(CreateOrder request, RecordingValidator[] validators, IMustFailureResponseFactory<Guid>[] typedFactories, IMustFailureResponseFactory[] familyFactories)> tc)
    {
        // Arrange
        var (request, validators, typedFactories, familyFactories) = tc.Value;
        var behavior = new MustValidationBehavior<CreateOrder, Guid>(validators, typedFactories, familyFactories);
        var handler = new HandlerSpy(CreateOrderHandler.Response);
        using var cts = new CancellationTokenSource();

        // Act & Assert
        await MustValidationAssert.ResponseAsync(tc.Expected, () => behavior.Handle(request, handler.HandleAsync, cts.Token));

        Assert.Equal(tc.Expected.IsValid ? 1 : 0, handler.InvocationCount);
        Assert.Equal(tc.Expected.IsValid ? cts.Token : default, handler.ReceivedCancellationToken);
        Assert.All(validators, validator => Assert.Equal(cts.Token, validator.ReceivedCancellationToken));
    }
}
