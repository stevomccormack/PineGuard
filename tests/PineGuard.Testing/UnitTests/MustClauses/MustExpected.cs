using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.MustClauses;

public sealed record MustExpected(bool IsValid, string? Message = null, string? ParamName = null, string? Code = null) : ReturnExpected(IsValid, Message);
