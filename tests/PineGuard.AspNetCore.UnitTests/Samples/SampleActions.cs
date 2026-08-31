using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Builds the MVC filter contexts an action filter is handed — MVC only ever creates them from inside its
/// own invoker, so a test has to assemble them itself.
/// </summary>
public static class SampleActions
{
    /// <summary>
    /// Builds the context describing an action that is about to execute with <paramref name="actionArguments"/> bound.
    /// </summary>
    /// <param name="httpContext">The request the action is answering, carrying the validators' services.</param>
    /// <param name="actionArguments">The arguments model binding produced, keyed by parameter name.</param>
    public static ActionExecutingContext Executing(HttpContext httpContext, IDictionary<string, object?> actionArguments) =>
        new(new ActionContext(httpContext, new RouteData(), new ActionDescriptor()), [], actionArguments, Controller);

    /// <summary>
    /// Builds the context the rest of the pipeline hands back once <paramref name="context"/>'s action has run.
    /// </summary>
    /// <param name="context">The context the filter was invoked with.</param>
    public static ActionExecutedContext Executed(ActionExecutingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new ActionExecutedContext(context, context.Filters, context.Controller);
    }

    private static object Controller { get; } = new();
}
