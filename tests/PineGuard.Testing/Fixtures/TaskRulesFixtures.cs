using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class TaskRulesFixtures
{
    public static class IsCompleted
    {
        public static readonly Func<Task?> Completed = () => Task.CompletedTask;
        public static readonly Func<Task?> CompletedResult = () => Task.FromResult(123);
        public static readonly Func<Task?> Canceled = () => Task.FromCanceled(new CancellationToken(canceled: true));
        public static readonly Func<Task?> Faulted = () => Task.FromException(new InvalidOperationException("boom"));
        public static readonly Func<Task?> InProgress = () => new TaskCompletionSource().Task;
        public static readonly Func<Task?> Null = () => null;

        public static RuleScenario<Func<Task?>>[] ValidScenarios =>
        [
            new(nameof(Completed),       Completed,       true),
            new(nameof(CompletedResult), CompletedResult, true),
            new(nameof(Canceled),        Canceled,        true),
            new(nameof(Faulted),         Faulted,         true)
        ];

        public static RuleScenario<Func<Task?>>[] InvalidScenarios =>
        [
            new(nameof(InProgress), InProgress, false),
            new(nameof(Null),       Null,       false)
        ];

        public static RuleScenario<Func<Task?>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsCanceled
    {
        public static readonly Func<Task?> Canceled = () => Task.FromCanceled(new CancellationToken(canceled: true));
        public static readonly Func<Task?> Completed = () => Task.CompletedTask;
        public static readonly Func<Task?> InProgress = () => new TaskCompletionSource().Task;
        public static readonly Func<Task?> Null = () => null;

        public static RuleScenario<Func<Task?>>[] ValidScenarios =>
        [
            new(nameof(Canceled), Canceled, true)
        ];

        public static RuleScenario<Func<Task?>>[] InvalidScenarios =>
        [
            new(nameof(Completed),  Completed,  false),
            new(nameof(InProgress), InProgress, false),
            new(nameof(Null),       Null,       false)
        ];

        public static RuleScenario<Func<Task?>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFaulted
    {
        public static readonly Func<Task?> Faulted = () => Task.FromException(new InvalidOperationException("boom"));
        public static readonly Func<Task?> Completed = () => Task.CompletedTask;
        public static readonly Func<Task?> InProgress = () => new TaskCompletionSource().Task;
        public static readonly Func<Task?> Null = () => null;

        public static RuleScenario<Func<Task?>>[] ValidScenarios =>
        [
            new(nameof(Faulted), Faulted, true)
        ];

        public static RuleScenario<Func<Task?>>[] InvalidScenarios =>
        [
            new(nameof(Completed),  Completed,  false),
            new(nameof(InProgress), InProgress, false),
            new(nameof(Null),       Null,       false)
        ];

        public static RuleScenario<Func<Task?>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
