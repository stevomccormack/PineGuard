namespace PineGuard.Rules;

public static class TaskRules
{
    public static bool IsCompleted(Task? task) => task is { IsCompleted: true };

    public static bool IsCanceled(Task? task) => task is { IsCanceled: true };

    public static bool IsFaulted(Task? task) => task is { IsFaulted: true };
}
