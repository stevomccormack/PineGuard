using System.Reflection;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class EnumerationTests : BaseUnitTest
{
    protected override void OnDispose()
    {
        ClearRegistries(typeof(Enumeration<int>));
        ClearRegistries(typeof(Enumeration<string>));
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.IntConstructor.ValidCases), MemberType = typeof(EnumerationTestData.IntConstructor))]
    public void Ctor_RegistersValueAndName(EnumerationTestData.IntConstructor.ValidCase testCase)
    {
        // Act
        var enumeration = new EnumerationTestData.DynamicIntEnumeration(testCase.InputValue, testCase.EnumerationName);

        // Assert
        Assert.Equal(testCase.InputValue, enumeration.Value);
        Assert.Equal(testCase.EnumerationName, enumeration.Name);
        Assert.Equal(testCase.EnumerationName, enumeration.ToString());
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.IntConstructor.InvalidCases), MemberType = typeof(EnumerationTestData.IntConstructor))]
    public void Ctor_WhenNameIsNullOrWhitespace_Throws(EnumerationTestData.IntConstructor.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type,
            () => _ = new EnumerationTestData.DynamicIntEnumeration(invalidCase.EnumerationValue,
                invalidCase.EnumerationName!));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.DuplicateName.Cases), MemberType = typeof(EnumerationTestData.DuplicateName))]
    public void Ctor_WhenNameIsDuplicate_IgnoresCase_Throws(EnumerationTestData.DuplicateName.Case testCase)
    {
        // Arrange
        _ = new EnumerationTestData.DynamicIntEnumeration(testCase.FirstValue, testCase.FirstName);

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type,
            () => _ = new EnumerationTestData.DynamicIntEnumeration(testCase.SecondValue, testCase.SecondName));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.DuplicateValue.Cases), MemberType = typeof(EnumerationTestData.DuplicateValue))]
    public void Ctor_WhenValueIsDuplicate_Throws_AndRollsBackNameRegistration(
        EnumerationTestData.DuplicateValue.Case testCase)
    {
        // Arrange
        _ = new EnumerationTestData.DynamicIntEnumeration(testCase.FirstValue, testCase.FirstName);

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type,
            () => _ = new EnumerationTestData.DynamicIntEnumeration(testCase.SecondValue, testCase.SecondName));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);

        var recovered = new EnumerationTestData.DynamicIntEnumeration(testCase.SecondValue + 1, testCase.SecondName);
        Assert.Equal(testCase.SecondValue + 1, recovered.Value);
        Assert.Equal(testCase.SecondName, recovered.Name);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.GetAll.Cases), MemberType = typeof(EnumerationTestData.GetAll))]
    public void GetAll_ReturnsPublicStaticDeclaredFieldsOnly(EnumerationTestData.GetAll.Case testCase)
    {
        // Act
        var all = Enumeration<int>.GetAll<EnumerationTestData.TestColor>();

        // Assert
        Assert.Equal(testCase.ExpectedCount, all.Count);
        foreach (var item in testCase.ExpectedItems)
        {
            Assert.Contains(item, all);
        }
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.FromValue.Cases), MemberType = typeof(EnumerationTestData.FromValue))]
    public void FromValue_ReturnsMatchOrNull_Repeatable(EnumerationTestData.FromValue.Case testCase)
    {
        // Act
        var result1 = Enumeration<int>.FromValue<EnumerationTestData.TestColor>(testCase.Input);
        var result2 = Enumeration<int>.FromValue<EnumerationTestData.TestColor>(testCase.Input);

        // Assert
        if (testCase.Expected != null)
        {
            Assert.Same(testCase.Expected, result1);
            Assert.Same(testCase.Expected, result2);
        }
        else
        {
            Assert.Null(result1);
        }
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.TryFromValue.Cases), MemberType = typeof(EnumerationTestData.TryFromValue))]
    public void TryFromValue_ReturnsExpected(EnumerationTestData.TryFromValue.Case testCase)
    {
        // Act
        var ok = Enumeration<int>.TryFromValue<EnumerationTestData.TestColor>(testCase.Input, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        if (testCase.Expected)
        {
            Assert.Same(testCase.ExpectedOut, result);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.TryFromValueDefault.Cases), MemberType = typeof(EnumerationTestData.TryFromValueDefault))]
    public void TryFromValue_FindsMember_WhenValueIsDefaultOfTValue(EnumerationTestData.TryFromValueDefault.Case testCase)
    {
        // Act
        var ok = Enumeration<int>.TryFromValue<EnumerationTestData.TestStatus>(testCase.Input, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Same(testCase.ExpectedOut, result);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.FromName.Cases), MemberType = typeof(EnumerationTestData.FromName))]
    public void FromName_ReturnsMatch_IgnoresCase(EnumerationTestData.FromName.Case testCase)
    {
        // Act
        var result = Enumeration<int>.FromName<EnumerationTestData.TestColor>(testCase.Input);

        // Assert
        Assert.Same(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.FromName.InvalidCases), MemberType = typeof(EnumerationTestData.FromName))]
    public void FromName_WhenInvalid_Throws(EnumerationTestData.FromName.InvalidCase testCase)
    {
        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type,
            () => _ = Enumeration<int>.FromName<EnumerationTestData.TestColor>(testCase.Input));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.TryFromName.Cases), MemberType = typeof(EnumerationTestData.TryFromName))]
    public void TryFromName_ReturnsExpected(EnumerationTestData.TryFromName.Case testCase)
    {
        // Act
        var ok = Enumeration<int>.TryFromName<EnumerationTestData.TestColor>(testCase.Input, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        if (testCase.Expected)
        {
            Assert.Same(testCase.ExpectedOut, result);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.Equality.Cases), MemberType = typeof(EnumerationTestData.Equality))]
    public void Equality_BehavesAsExpected(EnumerationTestData.Equality.Case testCase)
    {
        // Act
        var left = testCase.Left;

        bool result;
        if (left is EnumerationTestData.TestColor tc)
        {
            result = tc.Equals(testCase.Right);
        }
        else
        {
            result = Equals(left, testCase.Right);
        }

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.StringConstructor.InvalidCases), MemberType = typeof(EnumerationTestData.StringConstructor))]
    public void StringEnumeration_Ctor_Throws_ForNullValueOrBadName(
        EnumerationTestData.StringConstructor.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type,
            () => _ = new EnumerationTestData.DynamicStringEnumeration(invalidCase.EnumerationValue!,
                invalidCase.EnumerationName!));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    public static class TryFromValueNull
    {
        [Theory]
        [MemberData(nameof(EnumerationTestData.TryFromValueNull.EdgeCases), MemberType = typeof(EnumerationTestData.TryFromValueNull))]
        public static void ShouldReturnFalse_WhenNull(EnumerationTestData.TryFromValueNull.ValidCase testCase)
        {
            // Act
            var ok = Enumeration<string>.TryFromValue<EnumerationTestData.TestStringColor>(testCase.Value, out var result);

            // Assert
            Assert.Equal(testCase.Expected, ok);
            Assert.Null(result);
        }
    }

    public static class ImplicitOperatorString
    {
        [Theory]
        [MemberData(nameof(EnumerationTestData.ImplicitOperatorString.ValidCases), MemberType = typeof(EnumerationTestData.ImplicitOperatorString))]
        public static void ShouldReturnName(EnumerationTestData.ImplicitOperatorString.ValidCase testCase)
        {
            // Act
            string name = testCase.Input;

            // Assert
            Assert.Equal(testCase.ExpectedString, name);
        }
    }

    public static class ImplicitOperatorInt
    {
        [Theory]
        [MemberData(nameof(EnumerationTestData.ImplicitOperatorInt.ValidCases), MemberType = typeof(EnumerationTestData.ImplicitOperatorInt))]
        public static void ShouldReturnValue(EnumerationTestData.ImplicitOperatorInt.ValidCase testCase)
        {
            // Act
            int value = testCase.Input;

            // Assert
            Assert.Equal(testCase.ExpectedValue, value);
        }
    }

    public static class CompareTo
    {
        [Theory]
        [MemberData(nameof(EnumerationTestData.CompareTo.ValidCases), MemberType = typeof(EnumerationTestData.CompareTo))]
        [MemberData(nameof(EnumerationTestData.CompareTo.EdgeCases), MemberType = typeof(EnumerationTestData.CompareTo))]
        public static void ShouldReturnExpectedSign(EnumerationTestData.CompareTo.ValidCase testCase)
        {
            // Act
            var result = testCase.Left.CompareTo(testCase.Right);

            // Assert
            Assert.Equal(testCase.ExpectedResult, result);
        }
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.CompareToOrdinalString.Cases), MemberType = typeof(EnumerationTestData.CompareToOrdinalString))]
    public void CompareTo_UsesOrdinalComparison_ForStringValues_RegardlessOfCurrentCulture(EnumerationTestData.CompareToOrdinalString.Case testCase)
    {
        // Arrange
        using var _ = UseCulture("cs-CZ");
        var left = new EnumerationTestData.DynamicStringEnumeration(testCase.LeftValue, testCase.Name + " (left)");
        var right = new EnumerationTestData.DynamicStringEnumeration(testCase.RightValue, testCase.Name + " (right)");

        // Act
        var result = left.CompareTo(right);

        // Assert
        Assert.Equal(testCase.ExpectedLessThanZero, result < 0);
    }

    public static class OperatorEquals
    {
        [Theory]
        [MemberData(nameof(EnumerationTestData.OperatorEquals.ValidCases), MemberType = typeof(EnumerationTestData.OperatorEquals))]
        [MemberData(nameof(EnumerationTestData.OperatorEquals.EdgeCases), MemberType = typeof(EnumerationTestData.OperatorEquals))]
        public static void ShouldReturnExpected(EnumerationTestData.OperatorEquals.ValidCase testCase)
        {
            // Act
            var equal = testCase.Left == testCase.Right;
            var notEqual = testCase.Left != testCase.Right;

            // Assert
            Assert.Equal(testCase.Expected, equal);
            Assert.Equal(!testCase.Expected, notEqual);
        }
    }

    public static class OperatorComparison
    {
        [Theory]
        [MemberData(nameof(EnumerationTestData.OperatorComparison.ValidCases), MemberType = typeof(EnumerationTestData.OperatorComparison))]
        [MemberData(nameof(EnumerationTestData.OperatorComparison.EdgeCases), MemberType = typeof(EnumerationTestData.OperatorComparison))]
        public static void ShouldReturnExpected(EnumerationTestData.OperatorComparison.ValidCase testCase)
        {
            // Arrange
            var (left, right) = testCase.Value;

            // Act
            var lessThan = left < right;
            var lessThanOrEqual = left <= right;
            var greaterThan = left > right;
            var greaterThanOrEqual = left >= right;

            // Assert
            Assert.Equal(testCase.ExpectedLessThan, lessThan);
            Assert.Equal(testCase.ExpectedLessThanOrEqual, lessThanOrEqual);
            Assert.Equal(testCase.ExpectedGreaterThan, greaterThan);
            Assert.Equal(testCase.ExpectedGreaterThanOrEqual, greaterThanOrEqual);
        }
    }

    public static class HashCode
    {
        [Theory]
        [MemberData(nameof(EnumerationTestData.HashCode.ValidCases), MemberType = typeof(EnumerationTestData.HashCode))]
        public static void ShouldReturnValueHashCode(EnumerationTestData.HashCode.ValidCase testCase)
        {
            // Act
            var result = testCase.Input.GetHashCode();

            // Assert
            Assert.Equal(testCase.ExpectedValue, result);
        }
    }

    private static void ClearRegistries(Type closedEnumerationType)
    {
        ClearStaticConcurrentDictionary(closedEnumerationType, "NameRegistries");
        ClearStaticConcurrentDictionary(closedEnumerationType, "ValueRegistries");
    }

    private static void ClearStaticConcurrentDictionary(Type closedEnumerationType, string fieldName)
    {
        var field = closedEnumerationType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

        var instance = field?.GetValue(null);
        if (instance is null)
        {
            return;
        }

        var clear = instance.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance, binder: null,
            types: Type.EmptyTypes, modifiers: null);
        clear?.Invoke(instance, parameters: null);
    }
}
