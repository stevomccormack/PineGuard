using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PineGuard.AspNetCore.UnitTests;

public static class ProblemDetailsAssert
{
    public static void Expected(ProblemDetailsExpected expected, ValidationProblemDetails actual)
    {
        Assert.Equal(expected.IsValid, actual.Errors.Count == 0);
        Assert.Equal(expected.Status ?? StatusCodes.Status400BadRequest, actual.Status);
        Assert.Equal(ProblemDetailsExtension.BadRequestType, actual.Type);

        if (expected.Title is not null)
            Assert.Equal(expected.Title, actual.Title);

        if (expected.ErrorKeys is not null)
            Assert.Equal(expected.ErrorKeys, actual.Errors.Keys);

        if (expected.Messages is not null)
            Assert.Equal(expected.Messages, actual.Errors.Values.SelectMany(messages => messages));

        AssertFailures(expected, actual);
    }

    /// <summary>
    /// Asserts the same expectations against the body as it actually goes over the wire, so a test proves
    /// the JSON a client reads and not only the object a filter built.
    /// </summary>
    /// <param name="expected">What the response is expected to say.</param>
    /// <param name="actual">The parsed response body.</param>
    public static void Expected(ProblemDetailsExpected expected, JsonElement actual)
    {
        var errors = actual.GetProperty("errors");

        Assert.Equal(expected.IsValid, !errors.EnumerateObject().Any());
        Assert.Equal(expected.Status ?? StatusCodes.Status400BadRequest, actual.GetProperty("status").GetInt32());
        Assert.Equal(ProblemDetailsExtension.BadRequestType, actual.GetProperty("type").GetString());

        if (expected.Title is not null)
            Assert.Equal(expected.Title, actual.GetProperty("title").GetString());

        if (expected.ErrorKeys is not null)
            Assert.Equal(expected.ErrorKeys, errors.EnumerateObject().Select(error => error.Name));

        if (expected.Messages is not null)
            Assert.Equal(expected.Messages, errors.EnumerateObject().SelectMany(error => error.Value.EnumerateArray().Select(message => message.GetString())));

        AssertFailures(expected, actual);
    }

    private static void AssertFailures(ProblemDetailsExpected expected, ValidationProblemDetails actual)
    {
        if (expected.Codes is null)
        {
            Assert.False(actual.Extensions.ContainsKey(ProblemDetailsExtension.FailuresExtensionKey));
            return;
        }

        var failures = Assert.IsType<List<MustFailureDetail>>(actual.Extensions[ProblemDetailsExtension.FailuresExtensionKey]);

        Assert.Equal(expected.Codes, failures.Select(failure => failure.Code));

        if (expected.ErrorKeys is not null)
            Assert.Equal(expected.ErrorKeys, failures.Select(failure => failure.PropertyPath).Distinct());
    }

    private static void AssertFailures(ProblemDetailsExpected expected, JsonElement actual)
    {
        if (expected.Codes is null)
        {
            Assert.False(actual.TryGetProperty(ProblemDetailsExtension.FailuresExtensionKey, out _));
            return;
        }

        var failures = actual.GetProperty(ProblemDetailsExtension.FailuresExtensionKey).EnumerateArray().ToList();

        Assert.Equal(expected.Codes, failures.Select(failure => failure.GetProperty("code").GetString()));

        if (expected.ErrorKeys is not null)
            Assert.Equal(expected.ErrorKeys, failures.Select(failure => failure.GetProperty("property").GetString()).Distinct());
    }
}
