using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Reads back what a component actually wrote to the response, so a test asserts the bytes a client
/// receives rather than the object the component built.
/// </summary>
public static class SampleResponses
{
    /// <summary>
    /// Builds a request whose response body can be read back — <see cref="DefaultHttpContext"/> writes to
    /// <see cref="Stream.Null"/> otherwise.
    /// </summary>
    /// <param name="requestServices">The services the component under test resolves its collaborators from.</param>
    public static DefaultHttpContext Recording(IServiceProvider requestServices)
    {
        var httpContext = new DefaultHttpContext { RequestServices = requestServices };
        httpContext.Response.Body = new MemoryStream();

        return httpContext;
    }

    /// <summary>
    /// Parses the body written to <paramref name="httpContext"/>.
    /// </summary>
    /// <param name="httpContext">The request whose response body is read.</param>
    public static async Task<JsonElement> ReadJsonAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        httpContext.Response.Body.Position = 0;

        using var document = await JsonDocument.ParseAsync(httpContext.Response.Body);

        return document.RootElement.Clone();
    }

    /// <summary>
    /// Parses the body of a response an end-to-end test received.
    /// </summary>
    /// <param name="response">The response to read.</param>
    public static async Task<JsonElement> ReadJsonAsync(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);

        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

        return document.RootElement.Clone();
    }
}
