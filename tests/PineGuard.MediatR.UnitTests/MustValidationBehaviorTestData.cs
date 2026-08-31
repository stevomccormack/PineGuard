using PineGuard.MediatR.UnitTests.Samples;
using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests;

public static class MustValidationBehaviorTestData
{
    private static readonly CreateOrder ValidOrder = new("SKU-001", 3);
    private static readonly CreateOrder BlankSkuOrder = new("   ", 3);
    private static readonly CreateOrder ZeroQuantityOrder = new("SKU-001", 0);
    private static readonly CreateOrder BlankSkuAndZeroQuantityOrder = new("   ", 0);

    public static class Handle
    {
        public static TheoryData<MustValidationCase<(CreateOrder request, RecordingValidator[] validators, IMustFailureResponseFactory<Guid>[] typedFactories, IMustFailureResponseFactory[] familyFactories)>> Cases =>
        [
            new("no-validators-passes-straight-through-to-the-handler", (ValidOrder, [], [], []), new MustValidationExpected(true, CreateOrderHandler.Response)),
            new("one-passing-validator-reaches-the-handler", (ValidOrder, [new CreateOrderValidator()], [], []), new MustValidationExpected(true, CreateOrderHandler.Response)),
            new("two-passing-validators-reach-the-handler", (ValidOrder, [new CreateOrderValidator(), new CreateOrderQuotaValidator()], [], []), new MustValidationExpected(true, CreateOrderHandler.Response)),
            new("a-typed-factory-answers-a-failed-request-and-the-handler-never-runs", (BlankSkuOrder, [new CreateOrderValidator()], [new CreateOrderFailureResponseFactory()], []), new MustValidationExpected(false, CreateOrderFailureResponseFactory.Response)),
            new("a-typed-factory-wins-over-a-family-factory", (BlankSkuOrder, [new CreateOrderValidator()], [new CreateOrderFailureResponseFactory()], [new GuidFailureResponseFactory()]), new MustValidationExpected(false, CreateOrderFailureResponseFactory.Response)),
            new("a-family-factory-answers-when-no-typed-factory-is-registered", (BlankSkuOrder, [new CreateOrderValidator()], [], [new GuidFailureResponseFactory()]), new MustValidationExpected(false, GuidFailureResponseFactory.Response)),
            new("a-family-factory-that-declines-defers-to-the-next-one", (BlankSkuOrder, [new CreateOrderValidator()], [], [new UriFailureResponseFactory(), new GuidFailureResponseFactory()]), new MustValidationExpected(false, GuidFailureResponseFactory.Response)),
            new("a-typed-factory-is-ignored-when-validation-passes", (ValidOrder, [new CreateOrderValidator()], [new CreateOrderFailureResponseFactory()], [new GuidFailureResponseFactory()]), new MustValidationExpected(true, CreateOrderHandler.Response)),
            new("no-factory-throws-carrying-the-failure", (BlankSkuOrder, [new CreateOrderValidator()], [], []), new MustValidationExpected(false, null, typeof(MustValidationException), ["Sku"])),
            new("every-family-factory-declining-throws", (BlankSkuOrder, [new CreateOrderValidator()], [], [new UriFailureResponseFactory()]), new MustValidationExpected(false, null, typeof(MustValidationException), ["Sku"])),
            new("a-later-validator-failing-throws-carrying-its-failure", (ZeroQuantityOrder, [new CreateOrderValidator(), new CreateOrderQuotaValidator()], [], []), new MustValidationExpected(false, null, typeof(MustValidationException), ["Quantity"])),
            new("two-failing-validators-throw-carrying-both-failures-in-registration-order", (BlankSkuAndZeroQuantityOrder, [new CreateOrderValidator(), new CreateOrderQuotaValidator()], [], []), new MustValidationExpected(false, null, typeof(MustValidationException), ["Sku", "Quantity"])),
            new("registration-order-decides-the-failure-order", (BlankSkuAndZeroQuantityOrder, [new CreateOrderQuotaValidator(), new CreateOrderValidator()], [], []), new MustValidationExpected(false, null, typeof(MustValidationException), ["Quantity", "Sku"]))
        ];
    }
}
