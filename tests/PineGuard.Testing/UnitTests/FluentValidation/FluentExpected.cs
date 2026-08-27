using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.FluentValidation;

public sealed record FluentExpected(bool IsValid, string? Message = null, string? PropertyName = null, string? Code = null) : ReturnExpected(IsValid, Message);
