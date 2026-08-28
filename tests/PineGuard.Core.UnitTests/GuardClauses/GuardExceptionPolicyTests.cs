using PineGuard.GuardClauses;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

[Collection(GuardPolicyCollection.Name)]
public sealed class GuardExceptionPolicyTests : BaseUnitTest
{
    private static IMustResult FailedResult => MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.HasMap.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.HasMap))]
    public void HasMap_ReflectsWhetherAMapIsInstalled(GuardExceptionPolicyTestData.HasMap.Case testCase)
    {
        try
        {
            if (testCase.InstallMap)
                GuardExceptionPolicy.Map(failure => new InvalidOperationException(failure.Message));

            Assert.Equal(testCase.Expected, GuardExceptionPolicy.HasMap);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.Clear.Cases), MemberType = typeof(GuardExceptionPolicyTestData.Clear))]
    public void Clear_RemovesTheGlobalMap(bool _)
    {
        GuardExceptionPolicy.Map(failure => new InvalidOperationException(failure.Message));

        GuardExceptionPolicy.Clear();

        Assert.False(GuardExceptionPolicy.HasMap);
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.BeginScope.Cases), MemberType = typeof(GuardExceptionPolicyTestData.BeginScope))]
    public void BeginScope_OverridesGlobalMap_AndRestoresOnDispose(bool _)
    {
        try
        {
            GuardExceptionPolicy.Map(failure => new InvalidOperationException("global: " + failure.Message));

            using (GuardExceptionPolicy.BeginScope(failure => new NotSupportedException("scoped: " + failure.Message)))
            {
                var scoped = Assert.Throws<NotSupportedException>(() => GuardFailure.Throw(FailedResult));
                Assert.StartsWith("scoped: ", scoped.Message, StringComparison.Ordinal);
            }

            var global = Assert.Throws<InvalidOperationException>(() => GuardFailure.Throw(FailedResult));
            Assert.StartsWith("global: ", global.Message, StringComparison.Ordinal);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.NestedScope.Cases), MemberType = typeof(GuardExceptionPolicyTestData.NestedScope))]
    public void BeginScope_NestedScopes_RestoreOuterAfterInnerDispose(bool _)
    {
        try
        {
            GuardExceptionPolicy.Map(failure => new InvalidOperationException("global: " + failure.Message));

            using (GuardExceptionPolicy.BeginScope(failure => new NotSupportedException("outer: " + failure.Message)))
            {
                using (GuardExceptionPolicy.BeginScope(failure => new ApplicationException("inner: " + failure.Message)))
                {
                    var inner = Assert.Throws<ApplicationException>(() => GuardFailure.Throw(FailedResult));
                    Assert.StartsWith("inner: ", inner.Message, StringComparison.Ordinal);
                }

                var outer = Assert.Throws<NotSupportedException>(() => GuardFailure.Throw(FailedResult));
                Assert.StartsWith("outer: ", outer.Message, StringComparison.Ordinal);
            }

            var global = Assert.Throws<InvalidOperationException>(() => GuardFailure.Throw(FailedResult));
            Assert.StartsWith("global: ", global.Message, StringComparison.Ordinal);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.MapInsideActiveScope.Cases), MemberType = typeof(GuardExceptionPolicyTestData.MapInsideActiveScope))]
    public void Map_CalledInsideActiveScope_UpdatesScopeNotGlobal(bool _)
    {
        try
        {
            GuardExceptionPolicy.Map(failure => new InvalidOperationException("global: " + failure.Message));

            using (GuardExceptionPolicy.BeginScope(failure => new NotSupportedException("initial: " + failure.Message)))
            {
                GuardExceptionPolicy.Map(failure => new ApplicationException("replaced: " + failure.Message));

                var replaced = Assert.Throws<ApplicationException>(() => GuardFailure.Throw(FailedResult));
                Assert.StartsWith("replaced: ", replaced.Message, StringComparison.Ordinal);
            }

            var global = Assert.Throws<InvalidOperationException>(() => GuardFailure.Throw(FailedResult));
            Assert.StartsWith("global: ", global.Message, StringComparison.Ordinal);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.ClearInsideActiveScope.Cases), MemberType = typeof(GuardExceptionPolicyTestData.ClearInsideActiveScope))]
    public void Clear_CalledInsideActiveScope_ClearsScopeNotGlobal(bool _)
    {
        try
        {
            GuardExceptionPolicy.Map(failure => new InvalidOperationException("global: " + failure.Message));

            using (GuardExceptionPolicy.BeginScope(failure => new NotSupportedException("scoped: " + failure.Message)))
            {
                GuardExceptionPolicy.Clear();

                Assert.False(GuardExceptionPolicy.HasMap);
                Assert.Throws<ArgumentException>(() => GuardFailure.Throw(FailedResult));
            }

            Assert.True(GuardExceptionPolicy.HasMap);
            Assert.Throws<InvalidOperationException>(() => GuardFailure.Throw(FailedResult));
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.DoubleDispose.Cases), MemberType = typeof(GuardExceptionPolicyTestData.DoubleDispose))]
    public void BeginScope_DoubleDispose_IsIdempotent(bool _)
    {
        try
        {
            var scope = GuardExceptionPolicy.BeginScope(failure => new NotSupportedException(failure.Message));
            Assert.True(GuardExceptionPolicy.HasMap);

            scope.Dispose();
            Assert.False(GuardExceptionPolicy.HasMap);

            scope.Dispose();
            Assert.False(GuardExceptionPolicy.HasMap);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.StaleDispose.Cases), MemberType = typeof(GuardExceptionPolicyTestData.StaleDispose))]
    public void BeginScope_StaleOuterDispose_DoesNotAffectActiveInnerScope(bool _)
    {
        try
        {
            var outerScope = GuardExceptionPolicy.BeginScope(failure => new NotSupportedException("outer: " + failure.Message));

            using (GuardExceptionPolicy.BeginScope(failure => new ApplicationException("inner: " + failure.Message)))
            {
                // Dispose the OUTER lease first, while the inner scope is still active.
                outerScope.Dispose();

                // The inner scope's map must still apply — the disposed outer frame must not resurface.
                var inner = Assert.Throws<ApplicationException>(() => GuardFailure.Throw(FailedResult));
                Assert.StartsWith("inner: ", inner.Message, StringComparison.Ordinal);
            }

            // Both frames are now disposed; the effective map unwinds all the way back to none.
            Assert.False(GuardExceptionPolicy.HasMap);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.ChildContextIsolation.Cases), MemberType = typeof(GuardExceptionPolicyTestData.ChildContextIsolation))]
    public async Task BeginScope_FromChildTask_DoesNotLeakToParentContext(bool _)
    {
        try
        {
            using (GuardExceptionPolicy.BeginScope(failure => new NotSupportedException("outer: " + failure.Message)))
            {
                await Task.Run(() =>
                {
                    // The child inherits the parent's scope via AsyncLocal flow...
                    Assert.True(GuardExceptionPolicy.HasMap);

                    // ...and a scope begun here must only ever affect this child's own context.
                    using var childScope = GuardExceptionPolicy.BeginScope(failure => new ApplicationException("child: " + failure.Message));
                    var child = Assert.Throws<ApplicationException>(() => GuardFailure.Throw(FailedResult));
                    Assert.StartsWith("child: ", child.Message, StringComparison.Ordinal);
                });

                // The parent context's scope must be untouched by the child task's own nested scope.
                var outer = Assert.Throws<NotSupportedException>(() => GuardFailure.Throw(FailedResult));
                Assert.StartsWith("outer: ", outer.Message, StringComparison.Ordinal);
            }

            Assert.False(GuardExceptionPolicy.HasMap);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.NullArgumentGuards.Cases), MemberType = typeof(GuardExceptionPolicyTestData.NullArgumentGuards))]
    public void Map_AndBeginScope_NullMap_Throw(bool _)
    {
        Assert.Throws<ArgumentNullException>(() => GuardExceptionPolicy.Map(null!));
        Assert.Throws<ArgumentNullException>(() => GuardExceptionPolicy.BeginScope(null!));
    }
}
