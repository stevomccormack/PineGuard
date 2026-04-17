namespace PineGuard.Testing.UnitTests.DataAnnotations;

public sealed record DataAnnotationCase(string Name, object? Value, DataAnnotationExpected Expected) :
    ReturnCase<object?, DataAnnotationExpected>(Name, Value, Expected);
