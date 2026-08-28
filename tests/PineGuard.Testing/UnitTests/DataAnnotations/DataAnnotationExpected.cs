using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.DataAnnotations;

public sealed record DataAnnotationExpected(bool IsValid, string? Message = null, string? MemberName = null, string? Code = null) : ReturnExpected(IsValid, Message);
