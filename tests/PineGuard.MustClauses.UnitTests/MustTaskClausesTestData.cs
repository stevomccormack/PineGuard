using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.TaskRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustTaskClausesTestData
{
    public static class Completed
    {
        public static TheoryData<MustCase<Func<Task?>>> ValidCases => F.IsCompleted.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<Func<Task?>>> InvalidCases =>
            F.IsCompleted.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "task must be completed.", Code: MustCodes.Task.Status.NotCompleted));
    }

    public static class NotCompleted
    {
        public static TheoryData<MustCase<Func<Task?>>> ValidCases =>
            F.IsCompleted.InvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<Func<Task?>>> InvalidCases =>
            F.IsCompleted.ValidScenarios.ToMustCases(_ => new MustExpected(false, "task must not be completed.", Code: MustCodes.Task.Status.Completed));
    }

    public static class Canceled
    {
        public static TheoryData<MustCase<Func<Task?>>> ValidCases => F.IsCanceled.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<Func<Task?>>> InvalidCases =>
            F.IsCanceled.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "task must be canceled.", Code: MustCodes.Task.Status.NotCanceled));
    }

    public static class NotCanceled
    {
        public static TheoryData<MustCase<Func<Task?>>> ValidCases =>
            F.IsCanceled.InvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<Func<Task?>>> InvalidCases =>
            F.IsCanceled.ValidScenarios.ToMustCases(_ => new MustExpected(false, "task must not be canceled.", Code: MustCodes.Task.Status.Canceled));
    }

    public static class Faulted
    {
        public static TheoryData<MustCase<Func<Task?>>> ValidCases => F.IsFaulted.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<Func<Task?>>> InvalidCases =>
            F.IsFaulted.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "task must be faulted.", Code: MustCodes.Task.Status.NotFaulted));
    }

    public static class NotFaulted
    {
        public static TheoryData<MustCase<Func<Task?>>> ValidCases =>
            F.IsFaulted.InvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<Func<Task?>>> InvalidCases =>
            F.IsFaulted.ValidScenarios.ToMustCases(_ => new MustExpected(false, "task must not be faulted.", Code: MustCodes.Task.Status.Faulted));
    }
}
