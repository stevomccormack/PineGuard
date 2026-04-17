using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.DataAnnotations;

public sealed record DataAnnotationExpected(bool IsValid, string? Message = null, string? MemberName = null) : ReturnExpected(IsValid, Message);
