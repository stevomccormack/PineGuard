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
        var enumeration = new DynamicIntEnumeration(testCase.InputValue, testCase.EnumerationName);

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
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = new DynamicIntEnumeration(invalidCase.EnumerationValue, invalidCase.EnumerationName!));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Fact]
    public void Ctor_WhenNameIsDuplicate_IgnoresCase_Throws()
    {
        // Arrange
        _ = new DynamicIntEnumeration(1, "Alpha");

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _ = new DynamicIntEnumeration(2, "ALPHA"));

        // Assert
        Assert.Equal("name", ex.ParamName);
        Assert.Contains("already exists", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Ctor_WhenValueIsDuplicate_Throws_AndRollsBackNameRegistration()
    {
        // Arrange
        _ = new DynamicIntEnumeration(1, "Alpha");

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _ = new DynamicIntEnumeration(1, "Bravo"));

        // Assert
        Assert.Equal("value", ex.ParamName);

        var recovered = new DynamicIntEnumeration(2, "Bravo");
        Assert.Equal(2, recovered.Value);
        Assert.Equal("Bravo", recovered.Name);
    }

    [Fact]
    public void GetAll_ReturnsPublicStaticDeclaredFieldsOnly()
    {
        // Act
        var all = Enumeration<int>.GetAll<Color>();

        // Assert
        Assert.Equal(3, all.Count);
        Assert.Contains(Color.Red, all);
        Assert.Contains(Color.Green, all);
        Assert.Contains(Color.Blue, all);

        Assert.All(all, e => Assert.IsType<Color>(e));
    }

    [Fact]
    public void FromValue_ReturnsMatchOrNull_Repeatable()
    {
        // Act
        var red1 = Enumeration<int>.FromValue<Color>(1);
        var red2 = Enumeration<int>.FromValue<Color>(1);
        var missing = Enumeration<int>.FromValue<Color>(999);

        // Assert
        Assert.Same(Color.Red, red1);
        Assert.Same(Color.Red, red2);
        Assert.Null(missing);
    }

    [Fact]
    public void TryFromValue_WhenNull_ReturnsFalse()
    {
        // Act
        var ok = Enumeration<string>.TryFromValue(null, out DynamicStringEnumeration? result);

        // Assert
        Assert.False(ok);
        Assert.Null(result);
    }

    [Fact]
    public void FromName_ReturnsMatch_IgnoresCase()
    {
        // Act
        var green = Enumeration<int>.FromName<Color>("gReEn");

        // Assert
        Assert.Same(Color.Green, green);
    }

    [Fact]
    public void FromName_WhenNullOrWhitespace_Throws()
    {
        // Act
        var exNull = Assert.Throws<ArgumentNullException>(() => _ = Enumeration<int>.FromName<Color>(null!));
        var exEmpty = Assert.Throws<ArgumentException>(() => _ = Enumeration<int>.FromName<Color>(""));
        var exWhitespace = Assert.Throws<ArgumentException>(() => _ = Enumeration<int>.FromName<Color>(" "));

        // Assert
        Assert.Equal("name", exNull.ParamName);
        Assert.Equal("name", exEmpty.ParamName);
        Assert.Equal("name", exWhitespace.ParamName);
    }

    [Fact]
    public void TryFromName_WhenNullOrWhitespace_ReturnsFalse()
    {
        // Act
        var okNull = Enumeration<int>.TryFromName<Color>(null, out var nullResult);
        var okEmpty = Enumeration<int>.TryFromName<Color>("", out var emptyResult);
        var okWhitespace = Enumeration<int>.TryFromName<Color>(" ", out var whitespaceResult);

        // Assert
        Assert.False(okNull);
        Assert.Null(nullResult);

        Assert.False(okEmpty);
        Assert.Null(emptyResult);

        Assert.False(okWhitespace);
        Assert.Null(whitespaceResult);
    }

    [Fact]
    public void TryFromName_WhenMatch_ReturnsTrue()
    {
        var ok = Enumeration<int>.TryFromName<Color>("red", out var result);

        Assert.True(ok);
        Assert.Same(Color.Red, result);
    }

    [Fact]
    public void TryFromValue_WhenMatch_ReturnsTrue_AndWhenMissing_ReturnsFalse()
    {
        var ok = Enumeration<int>.TryFromValue<Color>(1, out var found);
        var missingOk = Enumeration<int>.TryFromValue<Color>(999, out var missing);

        Assert.True(ok);
        Assert.Same(Color.Red, found);

        Assert.False(missingOk);
        Assert.Null(missing);
    }

    [Fact]
    public void Equals_WhenOtherIsNull_ReturnsFalse()
    {
        Assert.False(Color.Red.Equals(null));
    }

    [Fact]
    public void GetHashCode_UsesUnderlyingValueHashCode()
    {
        var expected = Color.Red.Value.GetHashCode();

        var hash = Color.Red.GetHashCode();

        Assert.Equal(expected, hash);
    }

    [Fact]
    public void Operators_HandleNulls_AsExpected()
    {
        Enumeration<int>? leftNull = null;
        Enumeration<int>? rightNull = null;

        Assert.True(leftNull == rightNull);
        Assert.False(leftNull != rightNull);

        Assert.False(leftNull == Color.Red);
        Assert.True(leftNull != Color.Red);

        Assert.False(Color.Red == rightNull);
        Assert.True(Color.Red != rightNull);
    }

    [Fact]
    public void Equality_Operators_CompareTo_AndImplicitConversions_AreConsistent()
    {
        // Arrange
        var left = Color.Red;
        var right = Color.Red;
        var otherTypeSameValue = Status.Active;

        // Act
        var equalsSame = left.Equals(right);
        var equalsObject = left.Equals((object)right);
        var notEqualDifferentType = left.Equals(otherTypeSameValue);

        var compareToNull = left.CompareTo(null);
        var compareToSelf = left.CompareTo(right);
        var compareToOther = Color.Red.CompareTo(Color.Blue);

        string implicitName = left;
        int implicitValue = left;

        // Assert
        Assert.True(equalsSame);
        Assert.True(equalsObject);
        Assert.False(notEqualDifferentType);

        Assert.True(left == right);
        Assert.False(left != right);

        Assert.False(Color.Red == Color.Blue);
        Assert.True(Color.Red != Color.Blue);

        Assert.True(compareToNull > 0);
        Assert.Equal(0, compareToSelf);
        Assert.True(compareToOther < 0);

        Assert.Equal(left.Name, implicitName);
        Assert.Equal(left.Value, implicitValue);
    }

    [Fact]
    public void Equals_Object_ReturnsFalse_WhenObjectIsNotEnumeration()
    {
        Assert.False(Color.Red.Equals(new object()));

        object other = "Red";
        object enumeration = Color.Red;
        Assert.False(enumeration.Equals(other));
    }

    [Theory]
    [MemberData(nameof(EnumerationTestData.StringConstructor.InvalidCases), MemberType = typeof(EnumerationTestData.StringConstructor))]
    public void StringEnumeration_Ctor_Throws_ForNullValueOrBadName(EnumerationTestData.StringConstructor.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = new DynamicStringEnumeration(invalidCase.EnumerationValue!, invalidCase.EnumerationName!));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    private static void ClearRegistries(Type closedEnumerationType)
    {
        ClearStaticConcurrentDictionary(closedEnumerationType, "NameRegistries");
        ClearStaticConcurrentDictionary(closedEnumerationType, "ValueRegistries");
    }

    private static void ClearStaticConcurrentDictionary(Type closedEnumerationType, string fieldName)
    {
        var field = closedEnumerationType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
        if (field is null)
        {
            return;
        }

        var instance = field.GetValue(null);
        if (instance is null)
        {
            return;
        }

        var clear = instance.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance, binder: null, types: Type.EmptyTypes, modifiers: null);
        clear?.Invoke(instance, parameters: null);
    }

    private sealed class DynamicIntEnumeration(int value, string name) : Enumeration<int>(value, name);

    private sealed class DynamicStringEnumeration(string value, string name) : StringEnumeration(value, name);

    private sealed class Color : Enumeration<int>
    {
        public static readonly Color Red = new(1, "Red");
        public static readonly Color Green = new(2, "Green");
        public static readonly Color Blue = new(3, "Blue");

        private Color(int value, string name) : base(value, name)
        {

        }
    }

    private sealed class Status : Enumeration<int>
    {
        public static readonly Status Active = new(1, "Active");

        private Status(int value, string name) : base(value, name)
        {
        }
    }
}
