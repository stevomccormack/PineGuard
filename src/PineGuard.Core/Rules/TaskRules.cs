namespace PineGuard.Rules;

public static class TaskRules
{
    public static bool IsCompleted(Task? task) => task != null && task.IsCompleted;

    public static bool IsCanceled(Task? task) => task != null && task.IsCanceled;

    public static bool IsFaulted(Task? task) => task != null && task.IsFaulted;
}
