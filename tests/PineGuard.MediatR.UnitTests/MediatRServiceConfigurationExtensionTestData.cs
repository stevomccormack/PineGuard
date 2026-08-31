using PineGuard.MediatR.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MediatR.UnitTests;

public static class MediatRServiceConfigurationExtensionTestData
{
    private static readonly CreateOrder ValidOrder = new("SKU-001", 3);
    private static readonly CreateOrder BlankSkuAndZeroQuantityOrder = new("   ", 0);

    public static class AddMustValidation
    {
        public static TheoryData<MustValidationCase<(CreateOrder request, bool registerFailureFactory)>> Cases =>
        [
            new("valid-request-reaches-the-handler", (ValidOrder, false), new MustValidationExpected(true, CreateOrderHandler.Response)),
            new("valid-request-reaches-the-handler-even-with-a-factory-registered", (ValidOrder, true), new MustValidationExpected(true, CreateOrderHandler.Response)),
            new("failed-request-with-a-factory-returns-the-failure-response", (BlankSkuAndZeroQuantityOrder, true), new MustValidationExpected(false, CreateOrderFailureResponseFactory.Response)),
            new("failed-request-without-a-factory-throws-carrying-every-failure", (BlankSkuAndZeroQuantityOrder, false), new MustValidationExpected(false, null, typeof(MustValidationException), ["Sku", "Quantity"]))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null-configuration", () => MediatRServiceConfigurationExtension.AddMustValidation(null!), new ExpectedException(typeof(ArgumentNullException), "configuration"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
