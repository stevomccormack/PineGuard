using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate the state of <see cref="Task"/> instances.
/// </summary>
/// <seealso cref="TaskRules"/>
/// <seealso href="https://pineguard.ai/docs/must/task">Task Must Clauses documentation</seealso>
public static class MustTaskClauses
{
    /// <summary>
    /// Validates that the specified task has completed (successfully, faulted, or canceled).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="task">The <see cref="Task"/> to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="task"/> has completed, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TaskRules.IsCompleted"/>. The failure message follows the pattern
    /// <c>"{paramName} must be completed."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Completed(downloadTask);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TaskRules.IsCompleted"/>
    /// <seealso href="https://pineguard.ai/docs/must/task">Task Must Clauses documentation</seealso>
    public static MustResult<Task> Completed(this IMustClause _,
        Task? task,
        [CallerArgumentExpression(nameof(task))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be completed.";

        var ok = TaskRules.IsCompleted(task);
        return MustResult<Task>.FromBool(ok, messageTemplate, paramName, task, result: task!);
    }

    /// <summary>
    /// Validates that the specified task has not yet completed.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="task">The <see cref="Task"/> to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="task"/> has not completed, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TaskRules.IsCompleted"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be completed."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotCompleted(pendingTask);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TaskRules.IsCompleted"/>
    /// <seealso href="https://pineguard.ai/docs/must/task">Task Must Clauses documentation</seealso>
    public static MustResult<Task> NotCompleted(this IMustClause _,
        Task? task,
        [CallerArgumentExpression(nameof(task))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be completed.";

        var ok = !TaskRules.IsCompleted(task);
        return MustResult<Task>.FromBool(ok, messageTemplate, paramName, task, result: task!);
    }

    /// <summary>
    /// Validates that the specified task has been canceled.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="task">The <see cref="Task"/> to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="task"/> is in a canceled state, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TaskRules.IsCanceled"/>. The failure message follows the pattern
    /// <c>"{paramName} must be canceled."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Canceled(backgroundTask);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TaskRules.IsCanceled"/>
    /// <seealso href="https://pineguard.ai/docs/must/task">Task Must Clauses documentation</seealso>
    public static MustResult<Task> Canceled(this IMustClause _,
        Task? task,
        [CallerArgumentExpression(nameof(task))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be canceled.";

        var ok = TaskRules.IsCanceled(task);
        return MustResult<Task>.FromBool(ok, messageTemplate, paramName, task, result: task!);
    }

    /// <summary>
    /// Validates that the specified task has not been canceled.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="task">The <see cref="Task"/> to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="task"/> is not in a canceled state, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TaskRules.IsCanceled"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be canceled."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotCanceled(importTask);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TaskRules.IsCanceled"/>
    /// <seealso href="https://pineguard.ai/docs/must/task">Task Must Clauses documentation</seealso>
    public static MustResult<Task> NotCanceled(this IMustClause _,
        Task? task,
        [CallerArgumentExpression(nameof(task))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be canceled.";

        var ok = !TaskRules.IsCanceled(task);
        return MustResult<Task>.FromBool(ok, messageTemplate, paramName, task, result: task!);
    }

    /// <summary>
    /// Validates that the specified task has faulted (thrown an unhandled exception).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="task">The <see cref="Task"/> to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="task"/> is in a faulted state, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TaskRules.IsFaulted"/>. The failure message follows the pattern
    /// <c>"{paramName} must be faulted."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Faulted(failedTask);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TaskRules.IsFaulted"/>
    /// <seealso href="https://pineguard.ai/docs/must/task">Task Must Clauses documentation</seealso>
    public static MustResult<Task> Faulted(this IMustClause _,
        Task? task,
        [CallerArgumentExpression(nameof(task))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be faulted.";

        var ok = TaskRules.IsFaulted(task);
        return MustResult<Task>.FromBool(ok, messageTemplate, paramName, task, result: task!);
    }

    /// <summary>
    /// Validates that the specified task has not faulted.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="task">The <see cref="Task"/> to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="task"/> is not in a faulted state, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TaskRules.IsFaulted"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be faulted."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotFaulted(exportTask);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TaskRules.IsFaulted"/>
    /// <seealso href="https://pineguard.ai/docs/must/task">Task Must Clauses documentation</seealso>
    public static MustResult<Task> NotFaulted(this IMustClause _,
        Task? task,
        [CallerArgumentExpression(nameof(task))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be faulted.";

        var ok = !TaskRules.IsFaulted(task);
        return MustResult<Task>.FromBool(ok, messageTemplate, paramName, task, result: task!);
    }
}
