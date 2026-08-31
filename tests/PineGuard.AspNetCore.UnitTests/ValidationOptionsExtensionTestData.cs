#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
using Microsoft.Extensions.Validation;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class ValidationOptionsExtensionTestData
{
    public static class AddMustValidatorResolver
    {
        public static TheoryData<Case> Cases =>
        [
            new("one-call-adds-the-resolver-at-the-head", Empty, new ResolverExpected(true, ResolverCount: 1, PineGuardIndex: 0)),
            new("an-existing-resolver-is-pushed-behind-it", WithExistingResolver, new ResolverExpected(true, ResolverCount: 2, PineGuardIndex: 0)),
            new("a-second-call-adds-a-second-resolver", Empty, new ResolverExpected(true, ResolverCount: 2, PineGuardIndex: 0, Calls: 2))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-options", () => ValidationOptionsExtension.AddMustValidatorResolver(null!), new ExpectedException(typeof(ArgumentNullException), "options"))
        ];

        private static ValidationOptions Empty() => new();

        private static ValidationOptions WithExistingResolver()
        {
            var options = new ValidationOptions();
            options.Resolvers.Add(new SampleValidatableInfoResolver("existing"));

            return options;
        }

        public sealed record ResolverExpected(bool IsValid, int ResolverCount, int PineGuardIndex, int Calls = 1) : ReturnExpected(IsValid);

        public sealed record Case(string Name, Func<ValidationOptions> Value, ResolverExpected Expected)
            : ReturnCase<Func<ValidationOptions>, ResolverExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
#pragma warning restore ASP0029
#endif
