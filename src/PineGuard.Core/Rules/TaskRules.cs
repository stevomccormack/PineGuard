namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="System.Threading.Tasks.Task"/> state validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/task">Task Rules documentation</seealso>
public static class TaskRules
{
    /// <summary>
    /// Determines whether the specified task has completed (successfully, faulted, or cancelled).
    /// </summary>
    /// <param name="task">The task to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="task"/> is not <see langword="null"/> and
    /// <see cref="System.Threading.Tasks.Task.IsCompleted"/> is <see langword="true"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool done = TaskRules.IsCompleted(myTask);
    /// </code>
    /// </example>
    public static bool IsCompleted(Task? task) => task is { IsCompleted: true };

    /// <summary>
    /// Determines whether the specified task was cancelled.
    /// </summary>
    /// <param name="task">The task to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="task"/> is not <see langword="null"/> and
    /// <see cref="System.Threading.Tasks.Task.IsCanceled"/> is <see langword="true"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsCanceled(Task? task) => task is { IsCanceled: true };

    /// <summary>
    /// Determines whether the specified task faulted (threw an unhandled exception).
    /// </summary>
    /// <param name="task">The task to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="task"/> is not <see langword="null"/> and
    /// <see cref="System.Threading.Tasks.Task.IsFaulted"/> is <see langword="true"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsFaulted(Task? task) => task is { IsFaulted: true };
}
