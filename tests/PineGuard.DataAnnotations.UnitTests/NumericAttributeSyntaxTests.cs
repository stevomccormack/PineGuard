using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class NumericAttributeSyntaxTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    private sealed class OptionalToleranceAttribute() : NumberAttributeBase(MustCodes.Number.Proximity.NotApproximate)
    {
        protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
            InvokeAndMap(nameof(MustNumberClauses.Approximately), value, validationContext, 10d, null);
    }

    [Theory]
    [MemberData(nameof(NumericAttributeSyntaxTestData.NullToleranceInputs), MemberType = typeof(NumericAttributeSyntaxTestData))]
    public void CustomNumericAdapter_ForwardsNullOptionalArguments(double value)
    {
        // Arrange
        var attribute = new OptionalToleranceAttribute();
        var context = new ValidationContext(new object());

        // Act
        var result = attribute.GetValidationResult(value, context);

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(NumericAttributeSyntaxTestData.InvalidStyles), MemberType = typeof(NumericAttributeSyntaxTestData))]
    public void NullValues_DoNotHideParsingConfigurationAcrossNumericAttributes(NumberStyles styles)
    {
        // Arrange
        var context = new ValidationContext(new object());

        // Act / Assert
        Assert.All(CreateNumericAttributes(styles), attribute => Assert.NotNull(attribute.GetValidationResult(null, context)));
    }

    [Theory]
    [MemberData(nameof(NumericAttributeSyntaxTestData.ValidStyles), MemberType = typeof(NumericAttributeSyntaxTestData))]
    public void NullValues_AreOptionalAfterNumericConfigurationSucceeds(NumberStyles styles)
    {
        // Arrange
        var context = new ValidationContext(new object());

        // Act / Assert
        Assert.All(CreateNumericAttributes(styles), attribute => Assert.Null(attribute.GetValidationResult(null, context)));
    }

    private static ValidationAttribute[] CreateNumericAttributes(NumberStyles styles) =>
    [
        new PositiveStringAttribute { Styles = styles }, new NegativeStringAttribute { Styles = styles },
        new ZeroStringAttribute { Styles = styles }, new NotZeroStringAttribute { Styles = styles },
        new ZeroOrPositiveStringAttribute { Styles = styles }, new ZeroOrNegativeStringAttribute { Styles = styles },
        new EvenStringAttribute { Styles = styles }, new OddStringAttribute { Styles = styles },
        new GreaterThanStringAttribute("0") { Styles = styles }, new LessThanStringAttribute("1") { Styles = styles },
        new GreaterThanOrEqualStringAttribute("0") { Styles = styles }, new LessThanOrEqualStringAttribute("1") { Styles = styles },
        new InRangeStringAttribute("0", "1") { Styles = styles }, new OutOfRangeStringAttribute("0", "1") { Styles = styles },
        new MultipleOfStringAttribute("1") { Styles = styles }, new NotMultipleOfStringAttribute("1") { Styles = styles },
        new ApproximatelyStringAttribute("0", "1") { Styles = styles }, new NotApproximatelyStringAttribute("0", "1") { Styles = styles },
        new FiniteStringAttribute { Styles = styles }, new NotFiniteStringAttribute { Styles = styles },
        new NotNaNStringAttribute { Styles = styles }, new PercentageStringAttribute { Styles = styles },
        new DecimalStringAttribute { Styles = styles }, new ExactDecimalStringAttribute { Styles = styles },
        new Int32StringAttribute { Styles = styles }, new Int64StringAttribute { Styles = styles },
        new InRangeInt32StringAttribute(0, 1) { Styles = styles }, new OutOfRangeInt32StringAttribute(0, 1) { Styles = styles },
        new InRangeInt64StringAttribute(0, 1) { Styles = styles }, new OutOfRangeInt64StringAttribute(0, 1) { Styles = styles }
    ];

    private sealed class AnnotatedModel
    {
        [GreaterThanString("9")]
        [LessThanString("11")]
        [GreaterThanOrEqualString("10.000000000000000000000000001")]
        [LessThanOrEqualString("10.000000000000000000000000001")]
        [InRangeString("10.000000000000000000000000001", "10.000000000000000000000000002")]
        [OutOfRangeString("0", "1")]
        [MultipleOfString("0.000000000000000000000000001")]
        [NotMultipleOfString("2")]
        [ApproximatelyString("10.000000000000000000000000001", "0")]
        [NotApproximatelyString("0", "1")]
        public string DecimalText { get; init; } = "10.000000000000000000000000001";

        [ApproximatelyNumber(10, 1)]
        [NotApproximatelyNumber(0, 1)]
        public double Number { get; init; } = 10;
    }

    [Theory]
    [MemberData(nameof(NumericAttributeSyntaxTestData.DecimalValues), MemberType = typeof(NumericAttributeSyntaxTestData))]
    public void DecoratedModel_CompilesAndPreservesDecimalPrecision(string value, bool expected)
    {
        // Arrange
        var model = new AnnotatedModel { DecimalText = value };
        var results = new List<ValidationResult>();

        // Act
        var valid = Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);

        // Assert
        Assert.Equal(expected, valid);
    }

    [Theory]
    [MemberData(nameof(NumericAttributeSyntaxTestData.MalformedDecimals), MemberType = typeof(NumericAttributeSyntaxTestData))]
    public void MalformedDecimalConfiguration_FailsBeforeNullInput(string configuration)
    {
        // Arrange
        ValidationAttribute[] attributes =
        [
            new GreaterThanStringAttribute(configuration), new LessThanStringAttribute(configuration),
            new GreaterThanOrEqualStringAttribute(configuration), new LessThanOrEqualStringAttribute(configuration),
            new InRangeStringAttribute(configuration, "1"), new InRangeStringAttribute("0", configuration),
            new OutOfRangeStringAttribute(configuration, "1"), new OutOfRangeStringAttribute("0", configuration),
            new MultipleOfStringAttribute(configuration), new NotMultipleOfStringAttribute(configuration),
            new ApproximatelyStringAttribute(configuration, "1"), new ApproximatelyStringAttribute("0", configuration),
            new NotApproximatelyStringAttribute(configuration, "1"), new NotApproximatelyStringAttribute("0", configuration)
        ];
        var context = new ValidationContext(new object());

        // Act / Assert
        Assert.All(attributes, attribute => Assert.NotNull(attribute.GetValidationResult(null, context)));
    }

    [Theory]
    [MemberData(nameof(NumericAttributeSyntaxTestData.InvalidInclusions), MemberType = typeof(NumericAttributeSyntaxTestData))]
    public void TypedNumericConfiguration_FailsBeforeNullInput(Inclusion inclusion)
    {
        // Arrange
        ValidationAttribute[] attributes =
        [
            new InRangeNumberAttribute(0, 1, inclusion), new OutOfRangeNumberAttribute(0, 1, inclusion),
            new InRangeNumberAttribute(1, 0), new OutOfRangeNumberAttribute(1, 0),
            new MultipleOfNumberAttribute(0), new NotMultipleOfNumberAttribute(0),
            new ApproximatelyNumberAttribute(0, -1), new NotApproximatelyNumberAttribute(0, -1)
        ];
        var context = new ValidationContext(new object());

        // Act / Assert
        Assert.All(attributes, attribute => Assert.NotNull(attribute.GetValidationResult(null, context)));
    }
}
