using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="Task"/> property or field has completed successfully.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTaskClauses.Completed"/>. Supported on properties, fields, and parameters
/// of type <see cref="Task"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WorkflowModel
/// {
///     [TaskCompleted]
///     public Task InitTask { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TaskNotCompletedAttribute"/>
/// <seealso cref="MustTaskClauses.Completed"/>
/// <seealso href="https://pineguard.ai/docs/annotations/task">Task Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TaskCompletedAttribute() : ValidationAttributeBase(typeof(Task), MustCodes.Task.Status.NotCompleted)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var taskValue = (Task)value!;
        var result = Must.Be.Completed(taskValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Task"/> property or field has not yet completed.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTaskClauses.NotCompleted"/>. Supported on properties, fields, and parameters
/// of type <see cref="Task"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WorkflowModel
/// {
///     [TaskNotCompleted]
///     public Task PendingTask { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TaskCompletedAttribute"/>
/// <seealso cref="MustTaskClauses.NotCompleted"/>
/// <seealso href="https://pineguard.ai/docs/annotations/task">Task Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TaskNotCompletedAttribute() : ValidationAttributeBase(typeof(Task), MustCodes.Task.Status.Completed)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var taskValue = (Task)value!;
        var result = Must.Be.NotCompleted(taskValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Task"/> property or field has been canceled.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTaskClauses.Canceled"/>. Supported on properties, fields, and parameters
/// of type <see cref="Task"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WorkflowModel
/// {
///     [TaskCanceled]
///     public Task AbortedTask { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TaskNotCanceledAttribute"/>
/// <seealso cref="MustTaskClauses.Canceled"/>
/// <seealso href="https://pineguard.ai/docs/annotations/task">Task Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TaskCanceledAttribute() : ValidationAttributeBase(typeof(Task), MustCodes.Task.Status.NotCanceled)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var taskValue = (Task)value!;
        var result = Must.Be.Canceled(taskValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Task"/> property or field has not been canceled.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTaskClauses.NotCanceled"/>. Supported on properties, fields, and parameters
/// of type <see cref="Task"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WorkflowModel
/// {
///     [TaskNotCanceled]
///     public Task ActiveTask { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TaskCanceledAttribute"/>
/// <seealso cref="MustTaskClauses.NotCanceled"/>
/// <seealso href="https://pineguard.ai/docs/annotations/task">Task Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TaskNotCanceledAttribute() : ValidationAttributeBase(typeof(Task), MustCodes.Task.Status.Canceled)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var taskValue = (Task)value!;
        var result = Must.Be.NotCanceled(taskValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Task"/> property or field has faulted (completed with an exception).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTaskClauses.Faulted"/>. Supported on properties, fields, and parameters
/// of type <see cref="Task"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DiagnosticsModel
/// {
///     [TaskFaulted]
///     public Task FailedTask { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TaskNotFaultedAttribute"/>
/// <seealso cref="MustTaskClauses.Faulted"/>
/// <seealso href="https://pineguard.ai/docs/annotations/task">Task Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TaskFaultedAttribute() : ValidationAttributeBase(typeof(Task), MustCodes.Task.Status.NotFaulted)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var taskValue = (Task)value!;
        var result = Must.Be.Faulted(taskValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Task"/> property or field has not faulted.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTaskClauses.NotFaulted"/>. Supported on properties, fields, and parameters
/// of type <see cref="Task"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WorkflowModel
/// {
///     [TaskNotFaulted]
///     public Task HealthyTask { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TaskFaultedAttribute"/>
/// <seealso cref="MustTaskClauses.NotFaulted"/>
/// <seealso href="https://pineguard.ai/docs/annotations/task">Task Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TaskNotFaultedAttribute() : ValidationAttributeBase(typeof(Task), MustCodes.Task.Status.Faulted)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var taskValue = (Task)value!;
        var result = Must.Be.NotFaulted(taskValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
