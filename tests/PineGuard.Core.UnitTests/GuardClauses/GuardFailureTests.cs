using PineGuard.GuardClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

[Collection(GuardPolicyCollection.Name)]
public sealed class GuardFailureTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(GuardFailureTestData.Throw.Cases), MemberType = typeof(GuardFailureTestData.Throw))]
    public void Throw_BehavesAsExpected(GuardFailureTestData.Throw.Case testCase)
    {
        // Arrange
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;

        try
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.ReplaceDefaultExceptions;
            GuardExceptionPolicy.ExceptionReplacer = testCase.ConfigureReplacer
                ? ex => new InvalidOperationException("wrapped", ex)
                : null;

            Func<Exception>? creator = null;
            if (testCase.UseExceptionCreator)
            {
                creator = () => testCase.ExpectedExceptionType == typeof(ApplicationException)
                    ? new ApplicationException("boom")
                    : null!;
            }

            void Act() => GuardFailure.Throw("bad", "value", testCase.Value, creator);

            // Act
            var ex = Assert.Throws(testCase.ExpectedExceptionType, Act);

            // Assert
            if (testCase.ExpectedMessage is not null)
            {
                if (testCase.ExpectedExceptionType == typeof(InvalidOperationException))
                {
                    Assert.Equal(testCase.ExpectedMessage, ex.Message);
                    Assert.IsType<ArgumentException>(ex.InnerException);
                }
                else
                {
                    switch (testCase.ExpectedMessage)
                    {
                        // For ArgumentExceptions, message contains "bad"
                        case "bad":
                            Assert.Contains(testCase.ExpectedMessage, ex.Message);
                            break;
                        // For ParamName
                        case "value" when ex is ArgumentException argumentException:
                            Assert.Equal(testCase.ExpectedMessage, argumentException.ParamName);
                            break;
                    }
                }
            }

            if (testCase.ExpectedExceptionType == typeof(ApplicationException))
                Assert.Equal("boom", ex.Message);
        }
        finally
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
        }
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.ThrowScoped.Cases), MemberType = typeof(GuardFailureTestData.ThrowScoped))]
    public void Throw_WithScopedPolicy_BehavesAsExpected(GuardFailureTestData.ThrowScoped.Case testCase)
    {
        // Arrange
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        Func<Exception, Exception> scopedReplacer = ex => new NotSupportedException("scoped", ex);

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = GlobalReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.GlobalReplaceDefaultExceptions;

            using (GuardExceptionPolicy.BeginScope(options =>
                   {
                       options.ExceptionReplacer = scopedReplacer;
                       options.ReplaceDefaultExceptions = testCase.ScopedReplaceDefaultExceptions;
                   }))
            {
                static void Act() => GuardFailure.Throw("bad", "value", value: "x");

                // Act
                var ex = Assert.Throws(testCase.ExpectedExceptionType, Act);

                // Assert
                if (testCase.ExpectedExceptionType == typeof(NotSupportedException))
                {
                    Assert.Equal(testCase.ExpectedMessage, ex.Message);
                    Assert.IsType<ArgumentException>(ex.InnerException);
                }
                else if (ex is ArgumentException argumentException)
                {
                    Assert.Equal(testCase.ExpectedMessage, argumentException.ParamName);
                }
            }
        }
        finally
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
        }

        return;

        static Exception GlobalReplacer(Exception ex) => new InvalidOperationException("global", ex);
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.ThrowCreatorPrecedence.Cases), MemberType = typeof(GuardFailureTestData.ThrowCreatorPrecedence))]
    public void Throw_ExceptionCreatorBeatsScopedAndGlobalPolicy(GuardFailureTestData.ThrowCreatorPrecedence.Case testCase)
    {
        // Arrange
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = ex => new InvalidOperationException("global", ex);
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.GlobalReplaceDefaultExceptions;

            using (GuardExceptionPolicy.BeginScope(options =>
                   {
                       options.ExceptionReplacer = ex => new NotSupportedException("scoped", ex);
                       options.ReplaceDefaultExceptions = testCase.ScopedReplaceDefaultExceptions;
                   }))
            {
                // Act
                void Act() => GuardFailure.Throw("bad", "value", value: "x", exceptionCreator: () => new ApplicationException(testCase.ExpectedMessage));

                var ex = Assert.Throws(testCase.ExpectedExceptionType, Act);

                // Assert
                Assert.Equal(testCase.ExpectedMessage, ex.Message);
            }
        }
        finally
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
        }
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.ThrowAndReplace.Cases), MemberType = typeof(GuardFailureTestData.ThrowAndReplace))]
    public void ThrowAndReplace_BehavesAsExpected(GuardFailureTestData.ThrowAndReplace.Case testCase)
    {
        // Arrange
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;

        try
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.ReplaceDefaultExceptions;

            static void Act() => GuardFailure.ThrowAndReplace("bad", "value", value: "x", exceptionCreator: null, exceptionReplacer: defaultException => new InvalidOperationException("wrapped", defaultException));

            // Act
            var ex = Assert.Throws(testCase.ExpectedExceptionType, Act);

            // Assert
            Assert.Equal("wrapped", ex.Message);
            Assert.IsType<ArgumentException>(ex.InnerException);
        }
        finally
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.ThrowAndReplaceWithScopedPolicy.Cases), MemberType = typeof(GuardFailureTestData.ThrowAndReplaceWithScopedPolicy))]
    public void ThrowAndReplace_PerCallReplacerBeatsScopedAndGlobalPolicy(GuardFailureTestData.ThrowAndReplaceWithScopedPolicy.Case testCase)
    {
        // Arrange
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = ex => new InvalidOperationException("global", ex);
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.GlobalReplaceDefaultExceptions;

            using (GuardExceptionPolicy.BeginScope(options =>
                   {
                       options.ExceptionReplacer = ex => new NotSupportedException("scoped", ex);
                       options.ReplaceDefaultExceptions = testCase.ScopedReplaceDefaultExceptions;
                   }))
            {
                static void Act() => GuardFailure.ThrowAndReplace("bad", "value", value: "x", exceptionCreator: null, exceptionReplacer: defaultException => new ApplicationException("per-call", defaultException));

                // Act
                var ex = Assert.Throws(testCase.ExpectedExceptionType, Act);

                // Assert
                Assert.Equal("per-call", ex.Message);
                Assert.IsType<ArgumentException>(ex.InnerException);
            }
        }
        finally
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
        }
    }
}
