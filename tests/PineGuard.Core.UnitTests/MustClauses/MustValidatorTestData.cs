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

    public static class AsyncRuleFor
    {
        public static TheoryData<Case> Cases =>
        [
            new("available email passes the async rule", new CreateOrder("free@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, null), true),
            new("taken email fails the async rule", new CreateOrder("taken@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, null), false)
        ];

        public sealed record Case(string Name, CreateOrder Order, bool ExpectedSuccess)
            : ValueCase<CreateOrder>(Name, Order);
    }

    public static class AsyncRuleForEach
    {
        public static TheoryData<Case> Cases =>
        [
            new("every line passes the async rule", [new OrderLine("SKU-1", 1), new OrderLine("SKU-2", 2)], true, null),
            new("second line fails and reports its index", [new OrderLine("SKU-1", 1), new OrderLine("TAKEN", 2)], false, "Lines[1]"),
            new("null collection is skipped", null, true, null)
        ];

        public sealed record Case(string Name, IReadOnlyList<OrderLine>? Lines, bool ExpectedSuccess, string? ExpectedPropertyPath)
            : ValueCase<IReadOnlyList<OrderLine>?>(Name, Lines);
    }

    public static class AsyncMode
    {
        public static TheoryData<Case> Cases =>
        [
            new("aggregate collects every failing rule", MustValidationMode.Aggregate, 3),
            new("stop-on-first-failure keeps only the first failing rule", MustValidationMode.StopOnFirstFailure, 1)
        ];

        public sealed record Case(string Name, MustValidationMode Mode, int ExpectedFailureCount)
            : ValueCase<MustValidationMode>(Name, Mode);
    }

    public static class AsyncNullArguments
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class AsyncSynchronousUse
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class AsyncOrderingAndCancellation
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class AsyncConditions
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class ModeDispatch
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class HasAsyncRulesProbe
    {
        public static TheoryData<Case> Cases =>
        [
            new("a validator with only sync rules declares no async rules", false),
            new("a validator with one async rule declares async rules", true)
        ];

        public sealed record Case(string Name, bool RegisterAsyncRule)
            : ValueCase<bool>(Name, RegisterAsyncRule);
    }

    /// <summary>
    /// Exposes the <see langword="protected"/> <c>HasAsyncRules</c> flag of a derived validator, which is
    /// the only way a subclass author sees it.
    /// </summary>
    public sealed class AsyncRuleProbeValidator : MustValidator<OrderLine>
    {
        public AsyncRuleProbeValidator(bool registerAsyncRule)
        {
            RuleFor(x => x.Quantity, quantity => MustResult<int>.Ok(quantity));

            if (registerAsyncRule)
                RuleForAsync(x => x.Sku, (sku, _) => new ValueTask<MustResult<string>>(MustResult<string>.Ok(sku!)));
        }

        public bool AsyncRulesRegistered => HasAsyncRules;
    }

    /// <summary>
    /// Implements the non-generic contract only, so the <see cref="IMustValidator"/> mode default
    /// interface member — the one <see cref="IMustValidator{T}"/> reimplements for typed validators — is
    /// the member that actually runs.
    /// </summary>
    public sealed class HandRolledNonGenericValidator : IMustValidator
    {
        public Type ValidatedType => typeof(OrderLine);

        public MustValidationResult Validate(object? value) =>
            value is OrderLine { Sku: not null }
                ? MustValidationResult.Ok()
                : MustValidationResult.Fail(new MustFailure("Sku", "sample.sku.blank", "Sku must not be blank.", null));

        public ValueTask<MustValidationResult> ValidateAsync(object? value, CancellationToken cancellationToken = default) =>
            new(Validate(value));
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
