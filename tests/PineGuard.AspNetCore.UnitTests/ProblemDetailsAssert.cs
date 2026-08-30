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
}
