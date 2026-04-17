using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using PineGuard.Testing.Common;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.UnitTests;

public sealed class BaseUnitTestTests
{
    private sealed class TestableUnit : BaseUnitTest
    {
        public bool OnDisposeCalled { get; private set; }

        protected override void OnDispose() => OnDisposeCalled = true;

        public new static IDisposable UseCulture(string cultureName) => BaseUnitTest.UseCulture(cultureName);
        public new static IDisposable UseEnvironmentVariable(string key, string? value) => BaseUnitTest.UseEnvironmentVariable(key, value);
        public new static Random CreateDeterministicRandom(int seed = 123456789) => BaseUnitTest.CreateDeterministicRandom(seed);
        public new static CancellationToken CreateCancelledToken() => BaseUnitTest.CreateCancelledToken();
        public new void WriteLine(string message) => base.WriteLine(message);
        public void CallDisposeProtected(bool disposing) => Dispose(disposing);
    }

    private sealed class BaseOnlyTestableUnit : BaseUnitTest;

    private sealed class StubTestOutputHelper : ITestOutputHelper
    {
        public string? LastMessage { get; private set; }
        void ITestOutputHelper.WriteLine(string message) => LastMessage = message;
        void ITestOutputHelper.WriteLine(string format, params object[] args) => LastMessage = string.Format(format, args);
    }

    private sealed class TestableUnitWithOutput(ITestOutputHelper output) : BaseUnitTest(output)
    {
        public new void WriteLine(string message) => base.WriteLine(message);
    }

    public static class UseCulture
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.UseCulture.ValidCases), MemberType = typeof(BaseUnitTestTestData.UseCulture))]
        public static void ShouldSetAndRestoreCulture(BaseUnitTestTestData.UseCulture.ValidCase testCase)
        {
            var target = new TestableUnit();
            var before = CultureInfo.CurrentCulture;

            using (TestableUnit.UseCulture(testCase.CultureName))
            {
                Assert.Equal(testCase.CultureName, CultureInfo.CurrentCulture.Name);
            }

            Assert.Equal(before, CultureInfo.CurrentCulture);
            target.Dispose();
        }

        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.UseCulture.InvalidCases), MemberType = typeof(BaseUnitTestTestData.UseCulture))]
        public static void ShouldThrowForInvalidCultureName(IThrowsCase testCase)
        {
            var t = (BaseUnitTestTestData.UseCulture.InvalidCase)testCase;
            var target = new TestableUnit();
            var ex = Assert.Throws(t.ExpectedException.Type, () => TestableUnit.UseCulture(t.Value!));
            ThrowsCaseAssert.Expected(ex, t);
            target.Dispose();
        }
    }

    public static class UseEnvironmentVariable
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.UseEnvironmentVariable.ValidCases), MemberType = typeof(BaseUnitTestTestData.UseEnvironmentVariable))]
        [MemberData(nameof(BaseUnitTestTestData.UseEnvironmentVariable.EdgeCases), MemberType = typeof(BaseUnitTestTestData.UseEnvironmentVariable))]
        public static void ShouldSetAndRestoreEnvironmentVariable(BaseUnitTestTestData.UseEnvironmentVariable.ValidCase testCase)
        {
            var (key, value) = testCase.Value;
            var target = new TestableUnit();
            var original = Environment.GetEnvironmentVariable(key);

            using (TestableUnit.UseEnvironmentVariable(key, value))
            {
                Assert.Equal(value, Environment.GetEnvironmentVariable(key));
            }

            Assert.Equal(original, Environment.GetEnvironmentVariable(key));
            target.Dispose();
        }

        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.UseEnvironmentVariable.InvalidCases), MemberType = typeof(BaseUnitTestTestData.UseEnvironmentVariable))]
        public static void ShouldThrowForInvalidKey(IThrowsCase testCase)
        {
            var t = (BaseUnitTestTestData.UseEnvironmentVariable.InvalidCase)testCase;
            var target = new TestableUnit();
            var ex = Assert.Throws(t.ExpectedException.Type, () => TestableUnit.UseEnvironmentVariable(t.Value!, "any"));
            ThrowsCaseAssert.Expected(ex, t);
            target.Dispose();
        }
    }

    public static class CreateDeterministicRandom
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.CreateDeterministicRandom.ValidCases), MemberType = typeof(BaseUnitTestTestData.CreateDeterministicRandom))]
        [MemberData(nameof(BaseUnitTestTestData.CreateDeterministicRandom.EdgeCases), MemberType = typeof(BaseUnitTestTestData.CreateDeterministicRandom))]
        public static void ShouldReturnDeterministicRandom(BaseUnitTestTestData.CreateDeterministicRandom.Case testCase)
        {
            var target = new TestableUnit();

            var r1 = TestableUnit.CreateDeterministicRandom(testCase.Seed);
            var r2 = TestableUnit.CreateDeterministicRandom(testCase.Seed);

            Assert.Equal(r1.Next(), r2.Next());
            target.Dispose();
        }
    }

    public static class CreateCancelledToken
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.CreateCancelledToken.ValidCases), MemberType = typeof(BaseUnitTestTestData.CreateCancelledToken))]
        public static void ShouldReturnAlreadyCancelledToken(BaseUnitTestTestData.CreateCancelledToken.Case testCase)
        {
            _ = testCase;
            var target = new TestableUnit();

            var token = TestableUnit.CreateCancelledToken();

            Assert.True(token.IsCancellationRequested);
            target.Dispose();
        }
    }

    public static class Dispose
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.Dispose.ValidCases), MemberType = typeof(BaseUnitTestTestData.Dispose))]
        [MemberData(nameof(BaseUnitTestTestData.Dispose.EdgeCases), MemberType = typeof(BaseUnitTestTestData.Dispose))]
        public static void ShouldDisposeIdempotently(BaseUnitTestTestData.Dispose.Case testCase)
        {
            var target = new TestableUnit();

            for (var i = 0; i < testCase.DisposeCount; i++)
                target.Dispose();

            Assert.True(target.OnDisposeCalled);
        }
    }

    public static class WriteLine
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.WriteLine.ValidCases), MemberType = typeof(BaseUnitTestTestData.WriteLine))]
        [MemberData(nameof(BaseUnitTestTestData.WriteLine.EdgeCases), MemberType = typeof(BaseUnitTestTestData.WriteLine))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: test passes if no exception is thrown")]
        public static void ShouldNotThrowForAnyInput(BaseUnitTestTestData.WriteLine.Case testCase)
        {
            var target = new TestableUnit();

            target.WriteLine(testCase.Message!);

            target.Dispose();
        }
    }

    public static class WriteLineWithOutput
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.WriteLineWithOutput.ValidCases), MemberType = typeof(BaseUnitTestTestData.WriteLineWithOutput))]
        public static void ShouldWriteMessageToOutputHelper(BaseUnitTestTestData.WriteLineWithOutput.Case testCase)
        {
            var stub = new StubTestOutputHelper();
            var target = new TestableUnitWithOutput(stub);

            target.WriteLine(testCase.Message);

            Assert.Equal(testCase.Message, stub.LastMessage);
            target.Dispose();
        }
    }

    public static class ScopeDispose
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.ScopeDispose.EdgeCases), MemberType = typeof(BaseUnitTestTestData.ScopeDispose))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: test passes if no exception is thrown")]
        public static void ShouldDisposeIdempotently(BaseUnitTestTestData.ScopeDispose.Case testCase)
        {
            _ = testCase;
            var target = new TestableUnit();
            var scope = TestableUnit.UseCulture("en-US");

            scope.Dispose();
            scope.Dispose();

            target.Dispose();
        }
    }

    public static class DisposeProtected
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.DisposeProtected.ValidCases), MemberType = typeof(BaseUnitTestTestData.DisposeProtected))]
        [MemberData(nameof(BaseUnitTestTestData.DisposeProtected.EdgeCases), MemberType = typeof(BaseUnitTestTestData.DisposeProtected))]
        public static void ShouldHandleDisposingFlag(BaseUnitTestTestData.DisposeProtected.Case testCase)
        {
            var target = new TestableUnit();

            target.CallDisposeProtected(testCase.Disposing);

            Assert.Equal(testCase.Disposing, target.OnDisposeCalled);
        }
    }

    public static class OnDisposeBase
    {
        [Theory]
        [MemberData(nameof(BaseUnitTestTestData.OnDisposeBase.ValidCases), MemberType = typeof(BaseUnitTestTestData.OnDisposeBase))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: test passes if no exception is thrown")]
        public static void ShouldNotThrow(BaseUnitTestTestData.OnDisposeBase.Case testCase)
        {
            _ = testCase;
            var target = new BaseOnlyTestableUnit();

            target.Dispose();
        }
    }
}
