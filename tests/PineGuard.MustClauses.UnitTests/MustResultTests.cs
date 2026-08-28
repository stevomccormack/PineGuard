using System.Diagnostics.CodeAnalysis;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustResultTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustResultTestData.Ok.ValidCases), MemberType = typeof(MustResultTestData.Ok))]
    public void Ok_Checks(MustResultTestData.Ok.ValidCase testCase)
    {
        var result = MustResult<int>.Ok(testCase.Value.Result, testCase.Value.Value, testCase.Value.ParamName);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.Value.Result, result.Result);
        Assert.Equal(testCase.Value.Value, result.Value);
        Assert.Equal(testCase.Value.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Fail.ValidCases), MemberType = typeof(MustResultTestData.Fail))]
    public void Fail_Checks(MustResultTestData.Fail.ValidCase testCase)
    {
        var result = MustResult<int>.Fail("test.code", testCase.Value.Msg, testCase.Value.ParamName, testCase.Value.Value);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.Value.ParamName, result.ParamName);
        // Message check logic: "Error {paramName}" -> "Error param" (if param not null)
        if (testCase.Value.ParamName != null)
        {
            Assert.Contains(testCase.Value.ParamName, result.Message);
        }
        else
        {
            Assert.Equal(testCase.Value.Msg, result.Message);
        }
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FromBool.ValidCases), MemberType = typeof(MustResultTestData.FromBool))]
    public void FromBool_Checks(MustResultTestData.FromBool.ValidCase testCase)
    {
        var result = MustResult<int>.FromBool(testCase.Value.Success, "test.code", testCase.Value.Msg, testCase.Value.ParamName, testCase.Value.Value, testCase.Value.Result);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.Value.Result, result.Result);
        if (!testCase.Value.Success)
        {
            Assert.Contains(testCase.Value.ParamName, result.Message);
        }
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Combine.ValidCases), MemberType = typeof(MustResultTestData.Combine))]
    public void Combine_Checks(MustResultTestData.Combine.ValidCase testCase)
    {
        var result = testCase.Value.Combine();
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfAnyFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfAnyFailed))]
    [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: test passes if no exception is thrown")]
    public void ThrowIfAnyFailed_Checks(MustResultTestData.ThrowIfAnyFailed.ValidCase testCase)
    {
        testCase.Value.ThrowIfAnyFailed();
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfAnyFailed.InvalidCases), MemberType = typeof(MustResultTestData.ThrowIfAnyFailed))]
    public void ThrowIfAnyFailed_Throws(MustResultTestData.ThrowIfAnyFailed.InvalidCase testCase)
    {
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => testCase.Value.ThrowIfAnyFailed());
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Deconstruct.ValidCases), MemberType = typeof(MustResultTestData.Deconstruct))]
    public void Deconstruct_Checks(MustResultTestData.Deconstruct.ValidCase testCase)
    {
        var (success, _, paramName, value, res) = testCase.Value;
        Assert.Equal(testCase.Expected, success);
        Assert.Equal(testCase.Value.ParamName, paramName);
        Assert.Equal(testCase.Value.Value, value);
        Assert.Equal(testCase.Value.Result, res);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ImplicitBool.ValidCases), MemberType = typeof(MustResultTestData.ImplicitBool))]
    public void ImplicitBool_Checks(MustResultTestData.ImplicitBool.ValidCase testCase)
    {
        bool result = testCase.Value;
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: test passes if no exception is thrown")]
    public void ThrowIfFailed_Checks(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        testCase.Value.ThrowIfFailed();
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.InvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_Throws(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Handle special custom exception factory case if needed, or just standard check
        if (testCase.Name == "fail custom")
        {
            Assert.Throws(testCase.ExpectedException.Type, () => testCase.Value.ThrowIfFailed((_, _) => new InvalidOperationException()));
        }
        else
        {
            var ex = Assert.Throws(testCase.ExpectedException.Type, () => testCase.Value.ThrowIfFailed());
            ThrowsCaseAssert.Expected(ex, testCase);
        }
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailedCustom.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfFailedCustom))]
    [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: test passes if no exception is thrown")]
    public void ThrowIfFailedCustom_Checks(MustResultTestData.ThrowIfFailedCustom.ValidCase testCase)
    {
        testCase.Value.ThrowIfFailed<InvalidOperationException>((m, _) => new InvalidOperationException(m));
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowNullIfFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowNullIfFailed))]
    [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: test passes if no exception is thrown")]
    public void ThrowNullIfFailed_Checks(MustResultTestData.ThrowNullIfFailed.ValidCase testCase)
    {
        testCase.Value.ThrowNullIfFailed();
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowNullIfFailed.InvalidCases), MemberType = typeof(MustResultTestData.ThrowNullIfFailed))]
    public void ThrowNullIfFailed_Throws(MustResultTestData.ThrowNullIfFailed.InvalidCase testCase)
    {
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => testCase.Value.ThrowNullIfFailed());
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrow.ValidCases), MemberType = typeof(MustResultTestData.OrThrow))]
    public void OrThrow_Checks(MustResultTestData.OrThrow.ValidCase testCase)
    {
        Assert.Equal(testCase.Value.Result, testCase.Value.OrThrow());
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrow.InvalidCases), MemberType = typeof(MustResultTestData.OrThrow))]
    public void OrThrow_Throws(MustResultTestData.OrThrow.InvalidCase testCase)
    {
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => testCase.Value.OrThrow());
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrowWithFallback.ValidCases), MemberType = typeof(MustResultTestData.OrThrowWithFallback))]
    public void OrThrowWithFallback_Checks(MustResultTestData.OrThrowWithFallback.ValidCase testCase)
    {
        var result = testCase.Value.Result.OrThrow(testCase.Value.Fallback);
        Assert.Equal(testCase.Value.Result.Result, result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrowWithFallback.ValidCasesNullable), MemberType = typeof(MustResultTestData.OrThrowWithFallback))]
    public void OrThrowWithFallback_Nullable_Checks(MustResultTestData.OrThrowWithFallback.ValidCaseNullable testCase)
    {
        var result = testCase.Value.Result.OrThrow(testCase.Value.Fallback);
        Assert.Equal(testCase.UseResult ? testCase.Value.Result.Result : testCase.Value.Fallback, result);
    }
}
