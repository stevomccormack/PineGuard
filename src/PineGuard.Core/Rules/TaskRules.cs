namespace PineGuard.Rules;

public static class TaskRules
{
    public static bool IsCompleted(Task? task) => task is { } t && t.IsCompleted;

    public static bool IsCanceled(Task? task) => task is { } t && t.IsCanceled;

    public static bool IsFaulted(Task? task) => task is { } t && t.IsFaulted;
}
