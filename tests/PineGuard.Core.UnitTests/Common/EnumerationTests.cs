using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Common;

public sealed class EnumerationTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(EnumerationTestData.CatalogueMembership.Cases), MemberType = typeof(EnumerationTestData.CatalogueMembership))]
    public void GetAll_UsesClosedInheritedCatalogue(EnumerationTestData.CatalogueMembership.Case testCase)
    {
        // Act
        var members = testCase.Query();
        var again = testCase.Query();

        // Assert
        Assert.Same(members, again);
        Assert.Equal(testCase.ExpectedNames, members.Select(member => member.Name));
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.CatalogueFailure.Cases), MemberType = typeof(EnumerationTestData.CatalogueFailure))]
    public void GetAll_CachesCatalogueFailureWithoutPublishingMembers(EnumerationTestData.CatalogueFailure.Case testCase)
    {
        // Act
        var first = Assert.Throws(testCase.ExceptionType, testCase.Query);
        var second = Assert.Throws(testCase.ExceptionType, testCase.Query);

        // Assert
        Assert.Same(first, second);
        Assert.Contains(testCase.MessageContains, first.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.InitializationQuery.Cases), MemberType = typeof(EnumerationTestData.InitializationQuery))]
    public void GetAll_FirstQueryDuringInitializationRejectsIncompleteFields(EnumerationTestData.InitializationQuery.Case testCase)
    {
        // Act
        var initializationFailure = Assert.Throws<TypeInitializationException>(() => RuntimeHelpers.RunClassConstructor(typeof(EnumerationTestData.InitializingCatalogue).TypeHandle));
        var catalogueFailure = Assert.Throws<InvalidOperationException>(() => Enumeration<int>.GetAll<EnumerationTestData.InitializingCatalogue>());

        // Assert
        Assert.Same(initializationFailure.InnerException, catalogueFailure);
        Assert.Contains(testCase.MissingField, catalogueFailure.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.CatalogueConcurrency.Cases), MemberType = typeof(EnumerationTestData.CatalogueConcurrency))]
    public async Task GetAll_ConcurrentFirstAccessPublishesOneCompleteCatalogue(EnumerationTestData.CatalogueConcurrency.Case testCase)
    {
        // Act
        var readers = Enumerable.Range(0, testCase.ReaderCount)
            .Select(_ => Task.Run(Enumeration<int>.GetAll<EnumerationTestData.ConcurrentCatalogue>));
        var catalogues = await Task.WhenAll(readers);

        // Assert
        Assert.All(catalogues, catalogue =>
        {
            Assert.Same(catalogues[0], catalogue);
            Assert.Equal(testCase.ExpectedNames, catalogue.Select(member => member.Name));
            Assert.All(catalogue, member =>
            {
                Assert.Same(member, Enumeration<int>.FromValue<EnumerationTestData.ConcurrentCatalogue>(member.Value));
                Assert.Same(member, Enumeration<int>.FromName<EnumerationTestData.ConcurrentCatalogue>(member.Name));
            });
        });
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.CatalogueImmutability.Cases), MemberType = typeof(EnumerationTestData.CatalogueImmutability))]
    public void GetAll_DoesNotExposeMutableBackingStorage(EnumerationTestData.CatalogueImmutability.Case testCase)
    {
        // Arrange
        var members = Enumeration<int>.GetAll<EnumerationTestData.TestColor>();
        var collection = Assert.IsAssignableFrom<IList<EnumerationTestData.TestColor>>(members);
        var original = members[0];

        // Act
        var failure = Record.Exception(() => collection[0] = testCase.Replacement);

        // Assert
        Assert.IsType<NotSupportedException>(failure);
        Assert.True(collection.IsReadOnly);
        Assert.Same(original, Enumeration<int>.GetAll<EnumerationTestData.TestColor>()[0]);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.ConstructionRecovery.Cases), MemberType = typeof(EnumerationTestData.ConstructionRecovery))]
    public void ConstructionFailure_DoesNotReserveIdentityOrRegisterInstances(EnumerationTestData.ConstructionRecovery.Case testCase)
    {
        // Act
        var failure = Record.Exception(() => new EnumerationTestData.RecoverableEnumeration(testCase.MemberValue, testCase.MemberName, true));
        var recovered = new EnumerationTestData.RecoverableEnumeration(testCase.MemberValue, testCase.MemberName, false);
        var duplicate = new EnumerationTestData.RecoverableEnumeration(testCase.MemberValue, testCase.MemberName, false);

        // Assert
        Assert.IsType<InvalidOperationException>(failure);
        Assert.Equal(recovered, duplicate);
        Assert.Empty(Enumeration<int>.GetAll<EnumerationTestData.RecoverableEnumeration>());
        Assert.Null(Enumeration<int>.FromValue<EnumerationTestData.RecoverableEnumeration>(testCase.MemberValue));
        Assert.Null(Enumeration<int>.FromName<EnumerationTestData.RecoverableEnumeration>(testCase.MemberName));
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.StringIdentity.Cases), MemberType = typeof(EnumerationTestData.StringIdentity))]
    public void StringLookup_PreservesOrdinalValuesAndCaseInsensitiveNames(EnumerationTestData.StringIdentity.Case testCase)
    {
        // Arrange
        using var culture = UseCulture("tr-TR");

        // Act
        var valueFound = Enumeration<string>.TryFromValue<EnumerationTestData.StringIdentityCatalogue>(testCase.MemberValue, out var byValue);
        var nameFound = Enumeration<string>.TryFromName<EnumerationTestData.StringIdentityCatalogue>(testCase.MemberName, out var byName);

        // Assert
        Assert.Equal(testCase.Expected is not null, valueFound);
        Assert.Equal(testCase.Expected is not null, nameFound);
        Assert.Same(testCase.Expected, byValue);
        Assert.Same(testCase.Expected, byName);
        Assert.NotEqual(EnumerationTestData.StringIdentityCatalogue.Lower, EnumerationTestData.StringIdentityCatalogue.Upper);
        Assert.True(EnumerationTestData.StringIdentityCatalogue.Upper.CompareTo(EnumerationTestData.StringIdentityCatalogue.Lower) < 0);
        Assert.Equal(StringComparer.Ordinal.GetHashCode(EnumerationTestData.StringIdentityCatalogue.Lower.Value), EnumerationTestData.StringIdentityCatalogue.Lower.GetHashCode());
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.NullValueLookup.Cases), MemberType = typeof(EnumerationTestData.NullValueLookup))]
    public void FromValue_RejectsNullReferenceValue(EnumerationTestData.NullValueLookup.Case testCase)
    {
        // Act
        var failure = Assert.Throws<ArgumentNullException>(() => Enumeration<string>.FromValue<EnumerationTestData.StringIdentityCatalogue>(testCase.MemberValue!));

        // Assert
        Assert.Equal("value", failure.ParamName);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.IntConstructor.ValidCases), MemberType = typeof(EnumerationTestData.IntConstructor))]
    public void Ctor_InitializesValueAndName(EnumerationTestData.IntConstructor.ValidCase testCase)
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

    [Theory]
    [MemberData(nameof(EnumerationTestData.TryFromValueNull.EdgeCases), MemberType = typeof(EnumerationTestData.TryFromValueNull))]
    public void TryFromValueNull_ShouldReturnFalse_WhenNull(EnumerationTestData.TryFromValueNull.ValidCase testCase)
    {
        // Act
        var ok = Enumeration<string>.TryFromValue<EnumerationTestData.TestStringColor>(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.ImplicitOperatorString.ValidCases), MemberType = typeof(EnumerationTestData.ImplicitOperatorString))]
    public void ImplicitOperatorString_ShouldReturnName(EnumerationTestData.ImplicitOperatorString.ValidCase testCase)
    {
        // Act
        string name = testCase.Input;

        // Assert
        Assert.Equal(testCase.ExpectedString, name);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.ImplicitOperatorInt.ValidCases), MemberType = typeof(EnumerationTestData.ImplicitOperatorInt))]
    public void ImplicitOperatorInt_ShouldReturnValue(EnumerationTestData.ImplicitOperatorInt.ValidCase testCase)
    {
        // Act
        int value = testCase.Input;

        // Assert
        Assert.Equal(testCase.ExpectedValue, value);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.CompareTo.ValidCases), MemberType = typeof(EnumerationTestData.CompareTo))]
    [MemberData(nameof(EnumerationTestData.CompareTo.EdgeCases), MemberType = typeof(EnumerationTestData.CompareTo))]
    public void CompareTo_ShouldReturnExpectedSign(EnumerationTestData.CompareTo.ValidCase testCase)
    {
        // Act
        var result = testCase.Left.CompareTo(testCase.Right);

        // Assert
        Assert.Equal(testCase.ExpectedResult, result);
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

    [Theory]
    [MemberData(nameof(EnumerationTestData.OperatorEquals.ValidCases), MemberType = typeof(EnumerationTestData.OperatorEquals))]
    [MemberData(nameof(EnumerationTestData.OperatorEquals.EdgeCases), MemberType = typeof(EnumerationTestData.OperatorEquals))]
    public void OperatorEquals_ShouldReturnExpected(EnumerationTestData.OperatorEquals.ValidCase testCase)
    {
        // Act
        var equal = testCase.Left == testCase.Right;
        var notEqual = testCase.Left != testCase.Right;

        // Assert
        Assert.Equal(testCase.Expected, equal);
        Assert.Equal(!testCase.Expected, notEqual);
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.OperatorComparison.ValidCases), MemberType = typeof(EnumerationTestData.OperatorComparison))]
    [MemberData(nameof(EnumerationTestData.OperatorComparison.EdgeCases), MemberType = typeof(EnumerationTestData.OperatorComparison))]
    public void OperatorComparison_ShouldReturnExpected(EnumerationTestData.OperatorComparison.ValidCase testCase)
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

    [Theory]
    [MemberData(nameof(EnumerationTestData.HashCode.ValidCases), MemberType = typeof(EnumerationTestData.HashCode))]
    public void HashCode_ShouldReturnValueHashCode(EnumerationTestData.HashCode.ValidCase testCase)
    {
        // Act
        var result = testCase.Input.GetHashCode();

        // Assert
        Assert.Equal(testCase.ExpectedValue, result);
    }

}
