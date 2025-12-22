namespace PineGuard.Rules;

public static class TaskRules
{
    public static bool IsCompleted(Task? task) => task is not null && task.IsCompleted;

    public static bool IsNotCompleted(Task? task) => !IsCompleted(task);

    public static bool IsCanceled(Task? task) => task is not null && task.IsCanceled;

    public static bool IsNotCanceled(Task? task) => !IsCanceled(task);

    public static bool IsFaulted(Task? task) => task is not null && task.IsFaulted;

    public static bool IsNotFaulted(Task? task) => !IsFaulted(task);
}
