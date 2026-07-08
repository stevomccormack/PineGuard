using FluentValidation;
using FluentValidation.Results;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.ReadOnlyDictionaryRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentReadOnlyDictionaryExtensionsTestData
{
    public static class Empty
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, int>?>> Cases => F.IsEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsEmpty.PopulatedValue) => new FluentExpected(false, "Dict must be empty."),
            _ => new FluentExpected(true)
        });
    }

    public static class NotEmpty
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, int>?>> Cases => F.IsNotEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsNotEmpty.NullValue) => new FluentExpected(false, "Dict must not be empty and have items."),
            nameof(F.IsNotEmpty.EmptyValue) => new FluentExpected(false, "Dict must not be empty and have items."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasKey
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key)>> Cases => F.HasKey.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Dict must contain the specified key.")
        });
    }

    public static class NotHasKey
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key)>> Cases => F.HasKey.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasKey.NullDictionary) => new FluentExpected(true),
            nameof(F.HasKey.EmptyDictionary) => new FluentExpected(true),
            nameof(F.HasKey.MissingKey) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Dict must not contain the specified key."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, int value)>> Cases => F.HasValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Dict must contain the specified value.")
        });
    }

    public static class NotHasValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, int value)>> Cases => F.HasValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasValue.NullDictionary) => new FluentExpected(true),
            nameof(F.HasValue.EmptyDictionary) => new FluentExpected(true),
            nameof(F.HasValue.MissingValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Dict must not contain the specified value."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasKeyValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>> Cases => F.HasKeyValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Dict must contain the specified key/value pair.")
        });
    }

    public static class NotHasKeyValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>> Cases => F.HasKeyValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasKeyValue.NullDictionary) => new FluentExpected(true),
            nameof(F.HasKeyValue.EmptyDictionary) => new FluentExpected(true),
            nameof(F.HasKeyValue.WrongKey) => new FluentExpected(true),
            nameof(F.HasKeyValue.WrongValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Dict must not contain the specified key/value pair."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasAnyKey
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>> Cases => F.HasAnyKey.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Dict must contain a key that matches the predicate.")
        });
    }

    public static class NotHasAnyKey
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>> Cases => F.HasAnyKey.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAnyKey.NullDictionary) => new FluentExpected(true),
            nameof(F.HasAnyKey.EmptyDictionary) => new FluentExpected(true),
            nameof(F.HasAnyKey.NoMatch) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Dict must not contain a key that matches the predicate."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasAnyValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>> Cases => F.HasAnyValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Dict must contain a value that matches the predicate.")
        });
    }

    public static class NotHasAnyValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>> Cases => F.HasAnyValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAnyValue.NullDictionary) => new FluentExpected(true),
            nameof(F.HasAnyValue.EmptyDictionary) => new FluentExpected(true),
            nameof(F.HasAnyValue.NoMatch) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Dict must not contain a value that matches the predicate."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasAnyItem
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> Cases => F.HasAnyItem.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Dict must contain an item that matches the predicate.")
        });
    }

    public static class NotHasAnyItem
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> Cases => F.HasAnyItem.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAnyItem.NullDictionary) => new FluentExpected(true),
            nameof(F.HasAnyItem.EmptyDictionary) => new FluentExpected(true),
            nameof(F.HasAnyItem.NoMatch) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Dict must not contain an item that matches the predicate."),
            _ => new FluentExpected(true)
        });
    }

    public static class OverloadResolution
    {
        public static TheoryData<FluentCase<Func<ValidationResult>>> Cases =>
        [
            new("Empty-IRuleBuilderOptions", static () => { var validator = new InlineValidator<Model>(); var opts = validator.RuleFor(x => x.Dict).NotEmpty(); opts.Empty(); return validator.Validate(new Model { Dict = new Dictionary<string, int> { { "a", 1 } } }); }, new FluentExpected(false)),
            new("Empty-IRuleBuilder", static () => { var validator = new InlineValidator<Model>(); IRuleBuilder<Model, IReadOnlyDictionary<string, int>?> rb = validator.RuleFor(x => x.Dict); rb.Empty(); return validator.Validate(new Model { Dict = new Dictionary<string, int> { { "a", 1 } } }); }, new FluentExpected(false)),
            new("NotEmpty-IRuleBuilderOptions", static () => { var validator = new InlineValidator<Model>(); var opts = validator.RuleFor(x => x.Dict).Empty(); opts.NotEmpty(); return validator.Validate(new Model { Dict = new Dictionary<string, int>() }); }, new FluentExpected(false)),
            new("NotEmpty-IRuleBuilder", static () => { var validator = new InlineValidator<Model>(); IRuleBuilder<Model, IReadOnlyDictionary<string, int>?> rb = validator.RuleFor(x => x.Dict); rb.NotEmpty(); return validator.Validate(new Model { Dict = new Dictionary<string, int>() }); }, new FluentExpected(false))
        ];

        private sealed record Model
        {
            public IReadOnlyDictionary<string, int>? Dict { get; init; }
        }
    }
}
