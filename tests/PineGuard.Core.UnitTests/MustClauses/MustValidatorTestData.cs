using System.Collections;
using PineGuard.Core.UnitTests.MustClauses.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustValidatorTestData
{
    private static readonly OrderLine ValidLine = new("SKU-1", 2);
    private static readonly OrderLine BlankSkuLine = new("   ", 2);

    public static class Validate
    {
        public sealed record FailureExpectation(string PropertyPath, string Code, string? Message = null);

        public sealed record Case(string Name, CreateOrder? Order, bool ExpectedSuccess, FailureExpectation[] ExpectedFailures)
            : ValueCase<CreateOrder?>(Name, Order);

        public static TheoryData<Case> Cases =>
        [
            new(
                "all valid, weight rule skipped for non-physical order",
                new CreateOrder("a@b.com", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), IsPhysical: false, Weight: 0m, [ValidLine]),
                true,
                []),

            new(
                "single-member failure attributes to property path, not lambda parameter name",
                new CreateOrder("not-an-email", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), IsPhysical: false, Weight: 0m, [ValidLine]),
                false,
                [new FailureExpectation("Email", "sample.email.invalid", "Email must be a valid email address.")]),

            new(
                "cross-property failure attributes to the property being checked",
                new CreateOrder("a@b.com", new DateTime(2026, 1, 2), new DateTime(2026, 1, 1), IsPhysical: false, Weight: 0m, [ValidLine]),
                false,
                [new FailureExpectation("EndDate", "sample.end-date.not-after")]),

            new(
                "conditional rule applies when When() condition is true",
                new CreateOrder("a@b.com", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), IsPhysical: true, Weight: 0m, [ValidLine]),
                false,
                [new FailureExpectation("Weight", "sample.weight.not-positive")]),

            new(
                "empty lines collection fails the not-empty rule; RuleForEach contributes nothing",
                new CreateOrder("a@b.com", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), IsPhysical: false, Weight: 0m, []),
                false,
                [new FailureExpectation("Lines", "sample.lines.empty")]),

            new(
                "null lines collection fails the not-empty rule; RuleForEach skips the null collection",
                new CreateOrder("a@b.com", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), IsPhysical: false, Weight: 0m, null),
                false,
                [new FailureExpectation("Lines", "sample.lines.empty")]),

            new(
                "nested validator failure re-roots under Property[i]",
                new CreateOrder("a@b.com", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), IsPhysical: false, Weight: 0m, [ValidLine, BlankSkuLine]),
                false,
                [new FailureExpectation("Lines[1].Sku", "sample.sku.blank")]),

            new(
                "every failing rule collected in registration order (aggregate mode)",
                new CreateOrder("bad", new DateTime(2026, 1, 2), new DateTime(2026, 1, 1), IsPhysical: true, Weight: 0m, [BlankSkuLine]),
                false,
                [
                    new FailureExpectation("Email", "sample.email.invalid"),
                    new FailureExpectation("EndDate", "sample.end-date.not-after"),
                    new FailureExpectation("Weight", "sample.weight.not-positive"),
                    new FailureExpectation("Lines[0].Sku", "sample.sku.blank")
                ])
        ];
    }

    public static class ValidateNull
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class WhenUnless
    {
        private static readonly CreateOrder Base = new(null, DateTime.MinValue, DateTime.MinValue, IsPhysical: true, Weight: 1m, null);

        public static TheoryData<Case> Cases =>
        [
            new("both conditions allow the rule to run", Base, false),
            new("When() condition false skips the rule", Base with { IsPhysical = false }, true),
            new("Unless() condition true skips the rule", Base with { Weight = 200m }, true)
        ];

        public sealed record Case(string Name, CreateOrder Order, bool ExpectedSuccess)
            : ValueCase<CreateOrder>(Name, Order);
    }

    public static class Reusable
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class RunnerConditionsAndAsync
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class ValidatorCastValueType
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class CollectionRunnerBranches
    {
        public static TheoryData<bool> Cases => [true];
    }

    public sealed class HandRolledOrderLineValidator : IMustValidator<OrderLine>
    {
        public MustValidationResult Validate(OrderLine value) =>
            string.IsNullOrWhiteSpace(value.Sku)
                ? MustValidationResult.Fail(new MustFailure("Sku", "sample.sku.blank", "Sku must not be blank.", value.Sku))
                : MustValidationResult.Ok();

        public ValueTask<MustValidationResult> ValidateAsync(OrderLine value, CancellationToken cancellationToken = default) =>
            new(Validate(value));
    }

    public static class NonGenericDispatch
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("valid boxed order dispatches through Validate(T)", new CreateOrder("a@b.com", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), false, 0m, [ValidLine]))
        ];

        public static TheoryData<WrongTypeCase> InvalidCases =>
        [
            new("string is not a CreateOrder", "not-an-order"),
            new("null falls back to the T-null path, not a cast failure", null)
        ];

        public sealed record Case(string Name, CreateOrder Order)
            : ValueCase<CreateOrder>(Name, Order);

        public sealed record WrongTypeCase(string Name, object? Value)
            : ValueCase<object?>(Name, Value);
    }

    public static class SingleEnumeration
    {
        public static TheoryData<bool> Cases => [true];

        public sealed class CountingEnumerable<T>(IEnumerable<T> items) : IEnumerable<T>
        {
            public int EnumerationCount { get; private set; }

            public IEnumerator<T> GetEnumerator()
            {
                EnumerationCount++;
                return items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public sealed record ItemsHolder(IEnumerable<int>? Items);
    }
}
