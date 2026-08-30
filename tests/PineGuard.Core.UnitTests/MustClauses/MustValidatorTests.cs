using PineGuard.Codes;
using PineGuard.Core.UnitTests.MustClauses.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustValidatorTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidatorTestData.Validate.Cases), MemberType = typeof(MustValidatorTestData.Validate))]
    public void Validate_CollectsFailuresInRegistrationOrder(MustValidatorTestData.Validate.Case testCase)
    {
        // Arrange
        var validator = new CreateOrderValidator();

        // Act
        var result = validator.Validate(testCase.Value!);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
        Assert.Equal(testCase.ExpectedFailures.Length, result.Failures.Count);

        for (var i = 0; i < testCase.ExpectedFailures.Length; i++)
        {
            Assert.Equal(testCase.ExpectedFailures[i].PropertyPath, result.Failures[i].PropertyPath);
            Assert.Equal(testCase.ExpectedFailures[i].Code, result.Failures[i].Code);

            if (testCase.ExpectedFailures[i].Message is { } expectedMessage)
                Assert.Equal(expectedMessage, result.Failures[i].Message);
        }
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.ValidateNull.Cases), MemberType = typeof(MustValidatorTestData.ValidateNull))]
    public void Validate_NullInstance_ReturnsSingleRootFailure(bool _)
    {
        // Arrange
        var validator = new CreateOrderValidator();

        // Act
        var result = validator.Validate(null!);

        // Assert
        Assert.False(result.Success);
        Assert.Single(result.Failures);
        Assert.Equal(string.Empty, result.Failures[0].PropertyPath);
        Assert.Equal(MustCodes.Value.State.Null, result.Failures[0].Code);
        Assert.Equal("CreateOrder must not be null.", result.Failures[0].Message);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.Validate.Cases), MemberType = typeof(MustValidatorTestData.Validate))]
    public async Task ValidateAsync_MatchesValidate(MustValidatorTestData.Validate.Case testCase)
    {
        // Arrange
        var validator = new CreateOrderValidator();

        // Act
        var syncResult = validator.Validate(testCase.Value!);
        var asyncResult = await validator.ValidateAsync(testCase.Value!);

        // Assert
        Assert.Equal(syncResult.Success, asyncResult.Success);
        Assert.Equal(syncResult.Failures.Select(f => (f.PropertyPath, f.Code, f.Message)), asyncResult.Failures.Select(f => (f.PropertyPath, f.Code, f.Message)));
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.ValidateNull.Cases), MemberType = typeof(MustValidatorTestData.ValidateNull))]
    public async Task ValidateAsync_NullInstance_ReturnsSingleRootFailure(bool _)
    {
        // Arrange
        var validator = new CreateOrderValidator();

        // Act
        var result = await validator.ValidateAsync(null!);

        // Assert
        Assert.False(result.Success);
        Assert.Single(result.Failures);
        Assert.Equal(MustCodes.Value.State.Null, result.Failures[0].Code);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.NonGenericDispatch.ValidCases), MemberType = typeof(MustValidatorTestData.NonGenericDispatch))]
    public void NonGenericValidate_DispatchesToTypedOverload(MustValidatorTestData.NonGenericDispatch.Case testCase)
    {
        // Arrange
        IMustValidator validator = new CreateOrderValidator();

        // Act
        var result = validator.Validate((object)testCase.Value);

        // Assert
        Assert.Equal(typeof(CreateOrder), validator.ValidatedType);
        Assert.True(result.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.NonGenericDispatch.InvalidCases), MemberType = typeof(MustValidatorTestData.NonGenericDispatch))]
    public void NonGenericValidate_WrongType_ThrowsArgumentException(MustValidatorTestData.NonGenericDispatch.WrongTypeCase testCase)
    {
        // Arrange
        IMustValidator validator = new CreateOrderValidator();

        // Act & Assert
        if (testCase.Value is null)
        {
            var result = validator.Validate(null);
            Assert.False(result.Success);
            Assert.Equal(MustCodes.Value.State.Null, result.Failures[0].Code);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => validator.Validate(testCase.Value));
        }
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.NonGenericDispatch.InvalidCases), MemberType = typeof(MustValidatorTestData.NonGenericDispatch))]
    public async Task NonGenericValidateAsync_WrongType_ThrowsArgumentException(MustValidatorTestData.NonGenericDispatch.WrongTypeCase testCase)
    {
        // Arrange
        IMustValidator validator = new CreateOrderValidator();

        // Act & Assert
        if (testCase.Value is null)
        {
            var result = await validator.ValidateAsync(null);
            Assert.False(result.Success);
        }
        else
        {
            await Assert.ThrowsAsync<ArgumentException>(() => validator.ValidateAsync(testCase.Value).AsTask());
        }
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.SingleEnumeration.Cases), MemberType = typeof(MustValidatorTestData.SingleEnumeration))]
    public void RuleForEach_EnumeratesCollectionExactlyOnce(bool _)
    {
        // Arrange
        var counting = new MustValidatorTestData.SingleEnumeration.CountingEnumerable<int>([1, -2, 3]);
        var validator = new InlineMustValidator<MustValidatorTestData.SingleEnumeration.ItemsHolder>();
        validator.RuleForEach(x => x.Items, item => item > 0
            ? MustResult<int>.Ok(item)
            : MustResult<int>.Fail("sample.item.not-positive", "{paramName} must be positive.", "item", item));

        // Act
        var result = validator.Validate(new MustValidatorTestData.SingleEnumeration.ItemsHolder(counting));

        // Assert
        Assert.Equal(1, counting.EnumerationCount);
        Assert.Single(result.Failures);
        Assert.Equal("Items[1]", result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.WhenUnless.Cases), MemberType = typeof(MustValidatorTestData.WhenUnless))]
    public void When_And_Unless_CombineWithAndSemantics(MustValidatorTestData.WhenUnless.Case testCase)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleFor(x => x.Weight, weight => MustResult<decimal>.Fail("sample.weight.always-fails", "{paramName} always fails.", "weight", weight))
            .When(x => x.IsPhysical)
            .Unless(x => x.Weight > 100);

        // Act
        var result = validator.Validate(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.Reusable.Cases), MemberType = typeof(MustValidatorTestData.Reusable))]
    public void Validator_IsReusableAcrossMultipleValidateCalls(bool _)
    {
        // Arrange
        var validator = new CreateOrderValidator();
        var valid = new CreateOrder("a@b.com", new DateTime(2026, 1, 1), new DateTime(2026, 1, 2), false, 0m, [new OrderLine("SKU-1", 1)]);
        var invalid = valid with { Email = "bad" };

        // Act
        var first = validator.Validate(valid);
        var second = validator.Validate(invalid);
        var third = validator.Validate(valid);

        // Assert
        Assert.True(first.Success);
        Assert.False(second.Success);
        Assert.True(third.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.RunnerConditionsAndAsync.Cases), MemberType = typeof(MustValidatorTestData.RunnerConditionsAndAsync))]
    public async Task CrossPropertyRunner_ConditionFalse_SkipsSyncAndAsync(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleFor(x => x.EndDate, (order, end) => MustResult<DateTime>.Fail("sample.always-fails", "{paramName} always fails.", "end", end))
            .When(_ => false);
        var order = new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, null);

        // Act
        var syncResult = validator.Validate(order);
        var asyncResult = await validator.ValidateAsync(order);

        // Assert
        Assert.True(syncResult.Success);
        Assert.True(asyncResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.RunnerConditionsAndAsync.Cases), MemberType = typeof(MustValidatorTestData.RunnerConditionsAndAsync))]
    public async Task CollectionRunner_ConditionFalse_SkipsSyncAndAsync(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, (OrderLine line) => MustResult<int>.Fail("sample.always-fails", "{paramName} always fails.", "line", line))
            .When(_ => false);
        var order = new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("SKU-1", 1)]);

        // Act
        var syncResult = validator.Validate(order);
        var asyncResult = await validator.ValidateAsync(order);

        // Assert
        Assert.True(syncResult.Success);
        Assert.True(asyncResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.RunnerConditionsAndAsync.Cases), MemberType = typeof(MustValidatorTestData.RunnerConditionsAndAsync))]
    public async Task CollectionCrossPropertyRunner_ConditionFalse_SkipsSyncAndAsync(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, (CreateOrder order, OrderLine line) => MustResult<int>.Fail("sample.always-fails", "{paramName} always fails.", "line", line))
            .When(_ => false);
        var order = new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("SKU-1", 1)]);

        // Act
        var syncResult = validator.Validate(order);
        var asyncResult = await validator.ValidateAsync(order);

        // Assert
        Assert.True(syncResult.Success);
        Assert.True(asyncResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.RunnerConditionsAndAsync.Cases), MemberType = typeof(MustValidatorTestData.RunnerConditionsAndAsync))]
    public async Task NestedValidatorRunner_ConditionFalseAndAsync(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<InlineMustValidatorTestData.OrderWithSingleLine>();
        validator.RuleFor(x => x.Line, new OrderLineValidator())
            .When(_ => false);
        var invalid = new InlineMustValidatorTestData.OrderWithSingleLine(new OrderLine(null, 1));

        // Act
        var conditionSkipResult = validator.Validate(invalid);

        var asyncValidator = new InlineMustValidator<InlineMustValidatorTestData.OrderWithSingleLine>();
        asyncValidator.RuleFor(x => x.Line, new OrderLineValidator());
        var asyncFailureResult = await asyncValidator.ValidateAsync(invalid);
        var asyncSuccessResult = await asyncValidator.ValidateAsync(new InlineMustValidatorTestData.OrderWithSingleLine(null));

        // Assert
        Assert.True(conditionSkipResult.Success);
        Assert.False(asyncFailureResult.Success);
        Assert.Equal("Line.Sku", asyncFailureResult.Failures[0].PropertyPath);
        Assert.True(asyncSuccessResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.RunnerConditionsAndAsync.Cases), MemberType = typeof(MustValidatorTestData.RunnerConditionsAndAsync))]
    public async Task CollectionValidatorRunner_ConditionFalseAndAsync(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, new OrderLineValidator())
            .When(_ => false);
        var invalid = new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine(null, 1)]);

        // Act
        var syncSkipResult = validator.Validate(invalid);
        var asyncSkipResult = await validator.ValidateAsync(invalid);

        var runningValidator = new InlineMustValidator<CreateOrder>();
        runningValidator.RuleForEach(x => x.Lines, new OrderLineValidator());
        var asyncResult = await runningValidator.ValidateAsync(invalid);

        // Assert
        Assert.True(syncSkipResult.Success);
        Assert.True(asyncSkipResult.Success);
        Assert.False(asyncResult.Success);
        Assert.Equal("Lines[0].Sku", asyncResult.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.ValidatorCastValueType.Cases), MemberType = typeof(MustValidatorTestData.ValidatorCastValueType))]
    public void NonGenericValidate_NullValueTypeInstance_ThrowsArgumentException(bool _)
    {
        // Arrange
        IMustValidator validator = new InlineMustValidator<int>();
        ((InlineMustValidator<int>)validator).RuleFor(x => x, v => MustResult<int>.Ok(v));

        // Act & Assert
        Assert.Throws<ArgumentException>(() => validator.Validate(null));
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.CollectionRunnerBranches.Cases), MemberType = typeof(MustValidatorTestData.CollectionRunnerBranches))]
    public void CollectionCheckRunner_SkipsNullCollection_AndReportsOnlyFailingItems(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, (OrderLine line) => line.Quantity > 0
            ? MustResult<int>.Ok(line.Quantity)
            : MustResult<int>.Fail("sample.quantity.not-positive", "{paramName} must be positive.", "quantity", line.Quantity));

        // Act
        var nullCollectionResult = validator.Validate(new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, null));
        var mixedResult = validator.Validate(new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("SKU-1", 1), new OrderLine("SKU-2", 0)]));

        // Assert
        Assert.True(nullCollectionResult.Success);
        Assert.False(mixedResult.Success);
        Assert.Single(mixedResult.Failures);
        Assert.Equal("Lines[1]", mixedResult.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.CollectionRunnerBranches.Cases), MemberType = typeof(MustValidatorTestData.CollectionRunnerBranches))]
    public void CollectionCrossPropertyRunner_SkipsNullCollection_AndReportsOnlyFailingItems(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, (CreateOrder order, OrderLine line) => line.Quantity >= order.Lines!.Count
            ? MustResult<int>.Ok(line.Quantity)
            : MustResult<int>.Fail("sample.quantity.below-minimum", "{paramName} must be at least the line count.", "quantity", line.Quantity));

        // Act
        var nullCollectionResult = validator.Validate(new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, null));
        var mixedResult = validator.Validate(new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("SKU-1", 1), new OrderLine("SKU-2", 5)]));

        // Assert
        Assert.True(nullCollectionResult.Success);
        Assert.False(mixedResult.Success);
        Assert.Single(mixedResult.Failures);
        Assert.Equal("Lines[0]", mixedResult.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.CollectionRunnerBranches.Cases), MemberType = typeof(MustValidatorTestData.CollectionRunnerBranches))]
    public async Task NestedValidatorRunner_SameValidator_SkipsSyncAndAsyncWhenConditionFalse(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<InlineMustValidatorTestData.OrderWithSingleLine>();
        validator.RuleFor(x => x.Line, new OrderLineValidator())
            .When(_ => false);
        var invalid = new InlineMustValidatorTestData.OrderWithSingleLine(new OrderLine(null, 1));

        // Act
        var syncResult = validator.Validate(invalid);
        var asyncResult = await validator.ValidateAsync(invalid);

        // Assert
        Assert.True(syncResult.Success);
        Assert.True(asyncResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.CollectionRunnerBranches.Cases), MemberType = typeof(MustValidatorTestData.CollectionRunnerBranches))]
    public async Task NestedValidatorRunner_ValidProperty_SucceedsSyncAndAsync(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<InlineMustValidatorTestData.OrderWithSingleLine>();
        validator.RuleFor(x => x.Line, new OrderLineValidator());
        var valid = new InlineMustValidatorTestData.OrderWithSingleLine(new OrderLine("SKU-1", 1));

        // Act
        var syncResult = validator.Validate(valid);
        var asyncResult = await validator.ValidateAsync(valid);

        // Assert
        Assert.True(syncResult.Success);
        Assert.True(asyncResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.CollectionRunnerBranches.Cases), MemberType = typeof(MustValidatorTestData.CollectionRunnerBranches))]
    public async Task CollectionValidatorRunner_MixedItems_AsyncReportsOnlyFailingElements(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, new OrderLineValidator());
        var order = new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("SKU-1", 1), new OrderLine(null, 1)]);

        // Act
        var result = await validator.ValidateAsync(order);

        // Assert
        Assert.False(result.Success);
        Assert.Single(result.Failures);
        Assert.Equal("Lines[1].Sku", result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.CollectionRunnerBranches.Cases), MemberType = typeof(MustValidatorTestData.CollectionRunnerBranches))]
    public async Task HandRolledValidator_ExercisesDefaultInterfaceMembers(bool _)
    {
        // Arrange
        IMustValidator validator = new MustValidatorTestData.HandRolledOrderLineValidator();
        var validLine = new OrderLine("SKU-1", 1);
        var invalidLine = new OrderLine(null, 1);

        // Act
        var validatedType = validator.ValidatedType;
        var syncSuccess = validator.Validate(validLine);
        var syncFailure = validator.Validate((object)invalidLine);
        var asyncSuccess = await validator.ValidateAsync(validLine);

        // Assert
        Assert.Equal(typeof(OrderLine), validatedType);
        Assert.True(syncSuccess.Success);
        Assert.False(syncFailure.Success);
        Assert.True(asyncSuccess.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncRuleFor.Cases), MemberType = typeof(MustValidatorTestData.AsyncRuleFor))]
    public async Task RuleForAsync_SingleArgument_AttributesFailureToThePropertyPath(MustValidatorTestData.AsyncRuleFor.Case testCase)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForAsync(x => x.Email, (email, cancellationToken) => IsEmailAvailableAsync(email, cancellationToken));

        // Act
        var result = await validator.ValidateAsync(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
        if (!testCase.ExpectedSuccess)
            Assert.Equal("Email", result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncRuleFor.Cases), MemberType = typeof(MustValidatorTestData.AsyncRuleFor))]
    public async Task RuleForAsync_CrossProperty_ReceivesTheWholeInstance(MustValidatorTestData.AsyncRuleFor.Case testCase)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForAsync(x => x.Email, (CreateOrder order, string? email, CancellationToken cancellationToken) =>
            IsEmailAvailableAsync(order.Email == email ? email : "taken@b.com", cancellationToken));

        // Act
        var result = await validator.ValidateAsync(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
        if (!testCase.ExpectedSuccess)
            Assert.Equal("Email", result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncRuleForEach.Cases), MemberType = typeof(MustValidatorTestData.AsyncRuleForEach))]
    public async Task RuleForEachAsync_SingleArgument_ReportsFailuresAtTheElementIndex(MustValidatorTestData.AsyncRuleForEach.Case testCase)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEachAsync(x => x.Lines, (OrderLine line, CancellationToken cancellationToken) => IsEmailAvailableAsync(line.Sku, cancellationToken));

        // Act
        var result = await validator.ValidateAsync(OrderWith(testCase.Value));

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
        if (testCase.ExpectedPropertyPath is { } expectedPath)
            Assert.Equal(expectedPath, result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncRuleForEach.Cases), MemberType = typeof(MustValidatorTestData.AsyncRuleForEach))]
    public async Task RuleForEachAsync_CrossProperty_ReportsFailuresAtTheElementIndex(MustValidatorTestData.AsyncRuleForEach.Case testCase)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEachAsync(x => x.Lines, (CreateOrder order, OrderLine line, CancellationToken cancellationToken) =>
            IsEmailAvailableAsync(order.Lines is null ? "taken" : line.Sku, cancellationToken));

        // Act
        var result = await validator.ValidateAsync(OrderWith(testCase.Value));

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
        if (testCase.ExpectedPropertyPath is { } expectedPath)
            Assert.Equal(expectedPath, result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncNullArguments.Cases), MemberType = typeof(MustValidatorTestData.AsyncNullArguments))]
    public void RuleForAsync_NullExpressionOrCheck_ThrowsArgumentNullException(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        Func<string?, CancellationToken, ValueTask<MustResult<string>>> check = IsEmailAvailableAsync;
        Func<CreateOrder, string?, CancellationToken, ValueTask<MustResult<string>>> crossCheck = (_, email, cancellationToken) => IsEmailAvailableAsync(email, cancellationToken);
        Func<OrderLine, CancellationToken, ValueTask<MustResult<string>>> itemCheck = (line, cancellationToken) => IsEmailAvailableAsync(line.Sku, cancellationToken);
        Func<CreateOrder, OrderLine, CancellationToken, ValueTask<MustResult<string>>> crossItemCheck = (_, line, cancellationToken) => IsEmailAvailableAsync(line.Sku, cancellationToken);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => validator.RuleForAsync(null!, check));
        Assert.Throws<ArgumentNullException>(() => validator.RuleForAsync(x => x.Email, (Func<string?, CancellationToken, ValueTask<MustResult<string>>>)null!));
        Assert.Throws<ArgumentNullException>(() => validator.RuleForAsync(null!, crossCheck));
        Assert.Throws<ArgumentNullException>(() => validator.RuleForAsync(x => x.Email, (Func<CreateOrder, string?, CancellationToken, ValueTask<MustResult<string>>>)null!));
        Assert.Throws<ArgumentNullException>(() => validator.RuleForEachAsync(null!, itemCheck));
        Assert.Throws<ArgumentNullException>(() => validator.RuleForEachAsync(x => x.Lines, (Func<OrderLine, CancellationToken, ValueTask<MustResult<string>>>)null!));
        Assert.Throws<ArgumentNullException>(() => validator.RuleForEachAsync(null!, crossItemCheck));
        Assert.Throws<ArgumentNullException>(() => validator.RuleForEachAsync(x => x.Lines, (Func<CreateOrder, OrderLine, CancellationToken, ValueTask<MustResult<string>>>)null!));
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncSynchronousUse.Cases), MemberType = typeof(MustValidatorTestData.AsyncSynchronousUse))]
    public void Validate_WithAsyncRules_ThrowsInvalidOperationException(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForAsync(x => x.Email, (email, cancellationToken) => IsEmailAvailableAsync(email, cancellationToken));
        var order = new CreateOrder("free@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => validator.Validate(order));
        Assert.Equal("CreateOrder has async rules; call ValidateAsync.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncSynchronousUse.Cases), MemberType = typeof(MustValidatorTestData.AsyncSynchronousUse))]
    public void AsyncRuleRunner_Run_ThrowsInvalidOperationException(bool _)
    {
        // Arrange
        MustAsyncRuleRunnerBase<OrderLine> runner = new MustAsyncPropertyRuleRunner<OrderLine, string?, string>(
            "Sku",
            line => line.Sku,
            IsEmailAvailableAsync);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => runner.Run(new OrderLine("SKU-1", 1)));
        Assert.Equal("OrderLine has async rules; call ValidateAsync.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.HasAsyncRulesProbe.Cases), MemberType = typeof(MustValidatorTestData.HasAsyncRulesProbe))]
    public void HasAsyncRules_ReflectsWhetherAnAsyncRuleWasRegistered(MustValidatorTestData.HasAsyncRulesProbe.Case testCase)
    {
        // Arrange
        var validator = new MustValidatorTestData.AsyncRuleProbeValidator(testCase.RegisterAsyncRule);

        // Act
        var hasAsyncRules = validator.AsyncRulesRegistered;

        // Assert
        Assert.Equal(testCase.RegisterAsyncRule, hasAsyncRules);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncOrderingAndCancellation.Cases), MemberType = typeof(MustValidatorTestData.AsyncOrderingAndCancellation))]
    public async Task ValidateAsync_RunsSyncAndAsyncRulesSequentiallyInRegistrationOrder(bool _)
    {
        // Arrange
        var executed = new List<string>();
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleFor(x => x.Email, email => Record(executed, "first-sync", MustResult<string>.Ok(email!)));
        validator.RuleForAsync(x => x.Email, (email, _) => new ValueTask<MustResult<string>>(Record(executed, "async", MustResult<string>.Ok(email!))));
        validator.RuleFor(x => x.Email, email => Record(executed, "second-sync", MustResult<string>.Ok(email!)));

        // Act
        var result = await validator.ValidateAsync(new CreateOrder("free@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, null));

        // Assert
        Assert.True(result.Success);
        Assert.Equal(["first-sync", "async", "second-sync"], executed);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncOrderingAndCancellation.Cases), MemberType = typeof(MustValidatorTestData.AsyncOrderingAndCancellation))]
    public async Task ValidateAsync_ObservesCancellationBetweenRules(bool _)
    {
        // Arrange
        using var cancellation = new CancellationTokenSource();
        var executed = new List<string>();
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForAsync(x => x.Email, (email, _) =>
        {
            executed.Add("first");
            cancellation.Cancel();
            return new ValueTask<MustResult<string>>(MustResult<string>.Ok(email!));
        });
        validator.RuleForAsync(x => x.Email, (email, _) => new ValueTask<MustResult<string>>(Record(executed, "second", MustResult<string>.Ok(email!))));
        var order = new CreateOrder("free@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, null);

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => validator.ValidateAsync(order, cancellation.Token).AsTask());
        Assert.Equal(["first"], executed);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncMode.Cases), MemberType = typeof(MustValidatorTestData.AsyncMode))]
    public async Task ValidateAsync_Mode_AggregatesOrStopsAtTheFirstFailingRule(MustValidatorTestData.AsyncMode.Case testCase)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForAsync(x => x.Email, (email, cancellationToken) => IsEmailAvailableAsync("taken@b.com", cancellationToken));
        validator.RuleFor(x => x.Weight, weight => MustResult<decimal>.Fail("sample.weight.not-positive", "{paramName} must be positive.", "weight", weight));
        validator.RuleFor(x => x.StartDate, start => MustResult<DateTime>.Fail("sample.start-date.missing", "{paramName} must be supplied.", "start", start));
        var order = new CreateOrder("taken@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, null);

        // Act
        var result = await validator.ValidateAsync(order, testCase.Mode);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(testCase.ExpectedFailureCount, result.Failures.Count);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncMode.Cases), MemberType = typeof(MustValidatorTestData.AsyncMode))]
    public async Task ValidateAsync_Mode_SucceedsWhenNoRuleFails(MustValidatorTestData.AsyncMode.Case testCase)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForAsync(x => x.Email, (email, cancellationToken) => IsEmailAvailableAsync(email, cancellationToken));

        // Act
        var result = await validator.ValidateAsync(new CreateOrder("free@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, null), testCase.Mode);
        var nullResult = await validator.ValidateAsync(null!, testCase.Mode);

        // Assert
        Assert.True(result.Success);
        Assert.False(nullResult.Success);
        Assert.Equal(MustCodes.Value.State.Null, nullResult.Failures[0].Code);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.ModeDispatch.Cases), MemberType = typeof(MustValidatorTestData.ModeDispatch))]
    public async Task ValidateAsync_Mode_DispatchesThroughEveryInterfaceForm(bool _)
    {
        // Arrange
        var invalidOrder = new CreateOrder("bad", new DateTime(2026, 1, 2), new DateTime(2026, 1, 1), false, 0m, null);
        var invalidLine = new OrderLine(null, 1);
        IMustValidator baseClassNonGeneric = new CreateOrderValidator();
        IMustValidator<OrderLine> handRolled = new MustValidatorTestData.HandRolledOrderLineValidator();
        IMustValidator handRolledNonGeneric = handRolled;
        IMustValidator nonGenericOnly = new MustValidatorTestData.HandRolledNonGenericValidator();

        // Act
        var baseClassResult = await baseClassNonGeneric.ValidateAsync(invalidOrder, MustValidationMode.StopOnFirstFailure);
        var handRolledResult = await handRolled.ValidateAsync(invalidLine, MustValidationMode.StopOnFirstFailure);
        var reimplementedResult = await handRolledNonGeneric.ValidateAsync(invalidLine, MustValidationMode.Aggregate);
        var nonGenericOnlyResult = await nonGenericOnly.ValidateAsync(invalidLine, MustValidationMode.Aggregate);

        // Assert
        Assert.Single(baseClassResult.Failures);
        Assert.False(handRolledResult.Success);
        Assert.False(reimplementedResult.Success);
        Assert.False(nonGenericOnlyResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidatorTestData.AsyncConditions.Cases), MemberType = typeof(MustValidatorTestData.AsyncConditions))]
    public async Task AsyncRules_ConditionFalse_SkipEveryAsyncRunner(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForAsync(x => x.Email, (email, cancellationToken) => IsEmailAvailableAsync("taken@b.com", cancellationToken)).When(_ => false);
        validator.RuleForAsync(x => x.Email, (CreateOrder order, string? email, CancellationToken cancellationToken) => IsEmailAvailableAsync("taken@b.com", cancellationToken)).When(_ => false);
        validator.RuleForEachAsync(x => x.Lines, (OrderLine line, CancellationToken cancellationToken) => IsEmailAvailableAsync("taken@b.com", cancellationToken)).When(_ => false);
        validator.RuleForEachAsync(x => x.Lines, (CreateOrder order, OrderLine line, CancellationToken cancellationToken) => IsEmailAvailableAsync("taken@b.com", cancellationToken)).When(_ => false);
        var order = new CreateOrder("taken@b.com", DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("TAKEN", 1)]);

        // Act
        var result = await validator.ValidateAsync(order);

        // Assert
        Assert.True(result.Success);
    }

    private static ValueTask<MustResult<string>> IsEmailAvailableAsync(string? email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var available = email is not null && !email.StartsWith("taken", StringComparison.OrdinalIgnoreCase) && !email.StartsWith("TAKEN", StringComparison.OrdinalIgnoreCase);
        return new ValueTask<MustResult<string>>(available
            ? MustResult<string>.Ok(email!, email, nameof(email))
            : MustResult<string>.Fail("sample.email.taken", "{paramName} must be available.", nameof(email), email));
    }

    private static CreateOrder OrderWith(IReadOnlyList<OrderLine>? lines) =>
        new(null, DateTime.MinValue, DateTime.MinValue, false, 0m, lines);

    private static MustResult<string> Record(List<string> executed, string name, MustResult<string> result)
    {
        executed.Add(name);
        return result;
    }
}
