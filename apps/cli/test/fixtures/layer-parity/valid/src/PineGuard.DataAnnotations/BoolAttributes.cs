namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated value is true (layer-parity valid/ fixture).
/// DataAnnotations concepts are read from the <c>Must.Be.&lt;Name&gt;</c> call
/// inside the attribute, not the attribute's own type name — see
/// src/audit/rules/layer-parity.ts's header comment for why.
/// </summary>
public sealed class TrueAttribute : ValidationAttributeBase
{
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.True((bool)value!, paramName: null);
        return FromMustResult(result, validationContext);
    }

    protected object CheckRoot(object value) => InvokeCollectionMust(nameof(MustXmlClauses.HasXmlRoot), value);
    protected object CheckNumber(object value) => InvokeAndMap(nameof(MustNumberClauses.Positive), value);
    protected object NotADispatch(object value) => Describe(nameof(MustNumberClauses.NotImplemented), value);
    protected object WrongOwner(object value) => InvokeAndMap(nameof(OtherClauses.NotImplemented), value);
    protected object QuotedText(object value) => InvokeAndMap("nameof(MustNumberClauses.NotImplemented)", value);

}
