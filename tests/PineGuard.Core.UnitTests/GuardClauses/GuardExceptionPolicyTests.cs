using PineGuard.GuardClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

[Collection(GuardPolicyCollection.Name)]
public sealed class GuardExceptionPolicyTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.ExceptionReplacer.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.ExceptionReplacer))]
    public void ExceptionReplacer_CanBeSetAndRetrieved(GuardExceptionPolicyTestData.ExceptionReplacer.Case testCase)
    {
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = testCase.Value;

            if (testCase.Value is null)
                Assert.Null(GuardExceptionPolicy.ExceptionReplacer);
            else
                Assert.Same(testCase.Value, GuardExceptionPolicy.ExceptionReplacer);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.ReplaceDefaultExceptions.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.ReplaceDefaultExceptions))]
    public void ReplaceDefaultExceptions_CanBeSetAndRetrieved(GuardExceptionPolicyTestData.ReplaceDefaultExceptions.Case testCase)
    {
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;

        try
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.Value;

            Assert.Equal(testCase.Value, GuardExceptionPolicy.ReplaceDefaultExceptions);
        }
        finally
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.ShouldReplace.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.ShouldReplace))]
    public void ShouldReplace_ReturnsExpected(GuardExceptionPolicyTestData.ShouldReplace.Case testCase)
    {
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;

        try
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.ReplaceDefaultExceptions;

            Assert.Equal(testCase.Expected, GuardExceptionPolicy.ShouldReplace(testCase.Value));
        }
        finally
        {
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.BeginScope.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.BeginScope))]
    public void BeginScope_OverridesAndRestoresEffectivePolicy(GuardExceptionPolicyTestData.BeginScope.Case testCase)
    {
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        Func<Exception, Exception> globalReplacer = ex => new InvalidOperationException("global", ex);
        Func<Exception, Exception> scopedReplacer = ex => new NotSupportedException("scoped", ex);

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = globalReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.GlobalReplaceDefaultExceptions;

            using (GuardExceptionPolicy.BeginScope(options =>
                   {
                       options.ExceptionReplacer = scopedReplacer;
                       options.ReplaceDefaultExceptions = testCase.ScopedReplaceDefaultExceptions;
                   }))
            {
                Assert.Same(scopedReplacer, GuardExceptionPolicy.ExceptionReplacer);
                Assert.Equal(testCase.ScopedReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);
            }

            Assert.Same(globalReplacer, GuardExceptionPolicy.ExceptionReplacer);
            Assert.Equal(testCase.GlobalReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.NestedScope.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.NestedScope))]
    public void BeginScope_NestedScopes_RestoreOuterPolicyAfterInnerDispose(GuardExceptionPolicyTestData.NestedScope.Case testCase)
    {
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        Func<Exception, Exception> globalReplacer = ex => new InvalidOperationException("global", ex);
        Func<Exception, Exception> outerReplacer = ex => new NotSupportedException("outer", ex);
        Func<Exception, Exception> innerReplacer = ex => new ApplicationException("inner", ex);

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = globalReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.GlobalReplaceDefaultExceptions;

            using (GuardExceptionPolicy.BeginScope(options =>
                   {
                       options.ExceptionReplacer = outerReplacer;
                       options.ReplaceDefaultExceptions = testCase.OuterReplaceDefaultExceptions;
                   }))
            {
                Assert.Same(outerReplacer, GuardExceptionPolicy.ExceptionReplacer);
                Assert.Equal(testCase.OuterReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);

                using (GuardExceptionPolicy.BeginScope(options =>
                       {
                           options.ExceptionReplacer = innerReplacer;
                           options.ReplaceDefaultExceptions = testCase.InnerReplaceDefaultExceptions;
                       }))
                {
                    Assert.Same(innerReplacer, GuardExceptionPolicy.ExceptionReplacer);
                    Assert.Equal(testCase.InnerReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);
                }

                Assert.Same(outerReplacer, GuardExceptionPolicy.ExceptionReplacer);
                Assert.Equal(testCase.OuterReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);
            }

            Assert.Same(globalReplacer, GuardExceptionPolicy.ExceptionReplacer);
            Assert.Equal(testCase.GlobalReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.SetPropertyInsideScope.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.SetPropertyInsideScope))]
    public void SetPropertyInsideScope_UpdatesScopeNotGlobal(GuardExceptionPolicyTestData.SetPropertyInsideScope.Case testCase)
    {
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        Func<Exception, Exception> globalReplacer = ex => new InvalidOperationException("global", ex);
        Func<Exception, Exception> scopeOverrideReplacer = ex => new NotSupportedException("override", ex);

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = globalReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = false;

            using (GuardExceptionPolicy.BeginScope(_ => { }))
            {
                if (testCase.SetExceptionReplacer)
                {
                    GuardExceptionPolicy.ExceptionReplacer = scopeOverrideReplacer;
                    Assert.Same(scopeOverrideReplacer, GuardExceptionPolicy.ExceptionReplacer);
                }

                if (testCase.SetReplaceDefaultExceptions)
                {
                    GuardExceptionPolicy.ReplaceDefaultExceptions = true;
                    Assert.True(GuardExceptionPolicy.ReplaceDefaultExceptions);
                }
            }

            Assert.Same(globalReplacer, GuardExceptionPolicy.ExceptionReplacer);
            Assert.False(GuardExceptionPolicy.ReplaceDefaultExceptions);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.DoubleDispose.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.DoubleDispose))]
    public void BeginScope_DoubleDispose_IsIdempotent(GuardExceptionPolicyTestData.DoubleDispose.Case testCase)
    {
        _ = testCase;
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = null;
            GuardExceptionPolicy.ReplaceDefaultExceptions = false;

            var scope = GuardExceptionPolicy.BeginScope(options =>
            {
                options.ReplaceDefaultExceptions = true;
            });

            Assert.True(GuardExceptionPolicy.ReplaceDefaultExceptions);

            scope.Dispose();
            Assert.False(GuardExceptionPolicy.ReplaceDefaultExceptions);

            scope.Dispose();
            Assert.False(GuardExceptionPolicy.ReplaceDefaultExceptions);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.StaleDispose.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.StaleDispose))]
    public void BeginScope_StaleDispose_DoesNotAffectCurrentScope(GuardExceptionPolicyTestData.StaleDispose.Case testCase)
    {
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        Func<Exception, Exception> globalReplacer = ex => new InvalidOperationException("global", ex);
        Func<Exception, Exception> outerReplacer = ex => new NotSupportedException("outer", ex);
        Func<Exception, Exception> innerReplacer = ex => new ApplicationException("inner", ex);

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = globalReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = testCase.GlobalReplaceDefaultExceptions;

            var outerScope = GuardExceptionPolicy.BeginScope(options =>
            {
                options.ExceptionReplacer = outerReplacer;
                options.ReplaceDefaultExceptions = testCase.OuterReplaceDefaultExceptions;
            });

            using (GuardExceptionPolicy.BeginScope(options =>
                   {
                       options.ExceptionReplacer = innerReplacer;
                       options.ReplaceDefaultExceptions = testCase.InnerReplaceDefaultExceptions;
                   }))
            {
                // Dispose the OUTER lease first, while the inner scope is still alive.
                outerScope.Dispose();

                // The inner scope's policy must still apply — the disposed outer frame's options
                // (a different replacer and a different flag value) must not resurface.
                Assert.Same(innerReplacer, GuardExceptionPolicy.ExceptionReplacer);
                Assert.Equal(testCase.InnerReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);
            }

            // The inner scope has now also disposed. Out-of-order disposal must not leave the
            // already-disposed outer frame's options resurfacing: both frames are marked disposed
            // and skipped when resolving the ambient policy, so the effective policy unwinds all
            // the way back to the global value.
            Assert.Same(globalReplacer, GuardExceptionPolicy.ExceptionReplacer);
            Assert.Equal(testCase.GlobalReplaceDefaultExceptions, GuardExceptionPolicy.ReplaceDefaultExceptions);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.ScopeClearsInheritedReplacer.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.ScopeClearsInheritedReplacer))]
    public void BeginScope_ExplicitNullReplacer_DisablesInheritedGlobalReplacer(GuardExceptionPolicyTestData.ScopeClearsInheritedReplacer.Case testCase)
    {
        _ = testCase;
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        Func<Exception, Exception> globalReplacer = ex => new InvalidOperationException("global", ex);

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = globalReplacer;

            using (GuardExceptionPolicy.BeginScope(options => options.ExceptionReplacer = null))
            {
                Assert.Null(GuardExceptionPolicy.ExceptionReplacer);
            }

            Assert.Same(globalReplacer, GuardExceptionPolicy.ExceptionReplacer);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.GetEffectivePolicy.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.GetEffectivePolicy))]
    public void GetEffectivePolicy_ResolvesReplacerAndFlagFromTheSameFrame(GuardExceptionPolicyTestData.GetEffectivePolicy.Case testCase)
    {
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;
        Func<Exception, Exception> globalReplacer = ex => new InvalidOperationException("global", ex);
        Func<Exception, Exception> scopedReplacer = ex => new NotSupportedException("scoped", ex);

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = globalReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = false;

            if (testCase.ScopeActive)
            {
                using (GuardExceptionPolicy.BeginScope(options =>
                       {
                           options.ExceptionReplacer = scopedReplacer;
                           options.ReplaceDefaultExceptions = testCase.ScopedReplaceDefaultExceptions;
                       }))
                {
                    var (replacer, replaceDefaultExceptions) = GuardExceptionPolicy.GetEffectivePolicy();

                    Assert.Same(scopedReplacer, replacer);
                    Assert.Equal(testCase.ScopedReplaceDefaultExceptions, replaceDefaultExceptions);
                }
            }
            else
            {
                var (replacer, replaceDefaultExceptions) = GuardExceptionPolicy.GetEffectivePolicy();

                Assert.Same(globalReplacer, replacer);
                Assert.False(replaceDefaultExceptions);
            }
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }

    [Theory]
    [MemberData(nameof(GuardExceptionPolicyTestData.ChildContextPropertyMutation.ValidCases), MemberType = typeof(GuardExceptionPolicyTestData.ChildContextPropertyMutation))]
    public async Task SetPropertyInsideScope_FromChildTask_DoesNotLeakToParentContext(GuardExceptionPolicyTestData.ChildContextPropertyMutation.Case testCase)
    {
        _ = testCase;
        var originalExceptionReplacer = GuardExceptionPolicy.ExceptionReplacer;
        var originalReplaceDefaultExceptions = GuardExceptionPolicy.ReplaceDefaultExceptions;

        try
        {
            GuardExceptionPolicy.ExceptionReplacer = null;
            GuardExceptionPolicy.ReplaceDefaultExceptions = false;

            using (GuardExceptionPolicy.BeginScope(options => options.ReplaceDefaultExceptions = true))
            {
                Assert.True(GuardExceptionPolicy.ReplaceDefaultExceptions);

                await Task.Run(() =>
                {
                    // The child inherits the parent's scope via AsyncLocal flow...
                    Assert.True(GuardExceptionPolicy.ReplaceDefaultExceptions);

                    // ...and setting the property here must only ever affect this child's own context.
                    GuardExceptionPolicy.ReplaceDefaultExceptions = false;
                    Assert.False(GuardExceptionPolicy.ReplaceDefaultExceptions);
                });

                // The parent context's scope must be untouched by the child task's mutation: before the
                // fix, both contexts shared the same mutable Options instance, so the child's write was
                // instantly visible here too.
                Assert.True(GuardExceptionPolicy.ReplaceDefaultExceptions);
            }

            Assert.False(GuardExceptionPolicy.ReplaceDefaultExceptions);
        }
        finally
        {
            GuardExceptionPolicy.ExceptionReplacer = originalExceptionReplacer;
            GuardExceptionPolicy.ReplaceDefaultExceptions = originalReplaceDefaultExceptions;
        }
    }
}
