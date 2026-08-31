---
name: testserver-integration-tests
description: Writing TestServer end-to-end cases in PineGuard.AspNetCore — where the EndToEnd group lives, and why a framework-written body must be captured before its expectations are written
metadata:
  type: project
---

# TestServer end-to-end cases

Never write expectations for a body a **framework** publishes from prose — a plan, a sample's XML
remarks, or Microsoft docs. Capture the raw JSON first (`WriteLine(body.GetRawText())` from
`BaseUnitTest`, which xunit prints on failure), then write the case against what came back, then
delete the `WriteLine`.

**Why:** on 2026-08-31 the story-4 sample's own `<remarks>` predicted the .NET 10
`Microsoft.Extensions.Validation` pipeline would key errors by *parameter name + declared path* and
publish a full problem-details body. Both were wrong: the built-in endpoint filter returns a bare
`HttpValidationProblemDetails` object (not a `ValidationProblem` result), so nothing applies the
problem-details defaults — the body is `{"title":…,"errors":{…}}` with no `status`, no `type` and no
`failures`, and the keys are the bare declared paths because the built-in walk leaves
`CurrentValidationPath` empty when handing a bound argument to a type resolver. Two round-trips were
spent on expectations copied from prose.

**How to apply:** whenever the response under assertion is written by ASP.NET Core rather than by
`ProblemDetailsExtension`. A corollary: that body cannot be described by `ProblemDetailsExpected` /
`ProblemDetailsAssert` — those require `status` and `type` — so the group defines its own expected
record. Using PineGuard's record for a body PineGuard did not write quietly implies the two shapes
match, which is the exact thing the case exists to disprove.

## Where the group goes

Rule53 maps a test file to a source class by name, so there is no `EndToEndTests` file. An
end-to-end group is a nested `EndToEnd` class inside the test data of the component that *does the
work* end to end, plus one `EndToEnd_BehavesAsExpected` method on the matching test class:
the endpoint filter, the action filter, the exception handler, and the resolver — not the
registration extension that merely installs it. Each group owns its own `ResponseExpected` record;
they are deliberately not shared.

See also [[test-data-shapes]] for the Case/Expected hierarchy these records sit in.
