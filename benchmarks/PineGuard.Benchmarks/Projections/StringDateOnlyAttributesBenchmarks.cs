using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;
using PineGuard.Benchmarks.Common;
using PineGuard.DataAnnotations;

namespace PineGuard.Benchmarks.Projections;

/// <summary>
/// The DataAnnotations attribute <see cref="PastDateOnlyStringAttribute"/>, measured as one
/// <see cref="ValidationAttribute.GetValidationResult"/> call with an attribute and a <see cref="ValidationContext"/>
/// built once in the global setup; the context's service provider supplies the fixed clock.
/// </summary>
[MemoryDiagnoser]
public class StringDateOnlyAttributesBenchmarks
{
    private string? _value;
    private PastDateOnlyStringAttribute _attribute = null!;
    private ValidationContext _context = null!;

    [ParamsAllValues]
    public PastDateOnlyInput Input { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _value = PastDateOnlyInputs.Value(Input);
        _attribute = new PastDateOnlyStringAttribute { Styles = PastDateOnlyInputs.Styles(Input) };
        _context = new ValidationContext(new object()) { MemberName = "Value" };
        _context.InitializeServiceProvider(serviceType => serviceType == typeof(TimeProvider) ? FixedTimeProvider.Default : null);
    }

    [Benchmark]
    public ValidationResult? PastDateOnlyString() => _attribute.GetValidationResult(_value, _context);
}
