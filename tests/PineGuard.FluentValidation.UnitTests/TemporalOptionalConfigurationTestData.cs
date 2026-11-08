using System.Globalization;
using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.FluentValidation.UnitTests;

public static class TemporalOptionalConfigurationTestData
{
    public static class InclusionOnNull
    {
        public static TheoryData<FluentCase<Inclusion>> Cases =>
        [
            new("inclusive", Inclusion.Inclusive, new(true)),
            new("exclusive", Inclusion.Exclusive, new(true)),
            new("undefined", (Inclusion)999, new(false))
        ];
    }

    public static class StylesOnNull
    {
        public static TheoryData<FluentCase<DateTimeStyles>> Cases =>
        [
            new("valid", DateTimeStyles.None, new(true)),
            new("undefined", (DateTimeStyles)999, new(false))
        ];
    }

    public static class PrecisionOnNull
    {
        public static TheoryData<FluentCase<DateTimePrecision?>> Cases =>
        [
            new("absent", null, new(true)),
            new("undefined", (DateTimePrecision)999, new(false))
        ];
    }
    public static class NullSelector
    {
        public static TheoryData<Case> Cases =>
        [
            new("Before.DateTime", () => new InlineValidator<DateTime>().RuleFor(value => value).Before((Func<DateTime, DateTime>)null!), new(typeof(ArgumentNullException), "other")),
            new("OnOrBefore.DateTime", () => new InlineValidator<DateTime>().RuleFor(value => value).OnOrBefore((Func<DateTime, DateTime>)null!), new(typeof(ArgumentNullException), "other")),
            new("After.DateTime", () => new InlineValidator<DateTime>().RuleFor(value => value).After((Func<DateTime, DateTime>)null!), new(typeof(ArgumentNullException), "other")),
            new("OnOrAfter.DateTime", () => new InlineValidator<DateTime>().RuleFor(value => value).OnOrAfter((Func<DateTime, DateTime>)null!), new(typeof(ArgumentNullException), "other")),
            new("Before.DateTime?", () => new InlineValidator<DateTime?>().RuleFor(value => value).Before((Func<DateTime?, DateTime>)null!), new(typeof(ArgumentNullException), "other")),
            new("OnOrBefore.DateTime?", () => new InlineValidator<DateTime?>().RuleFor(value => value).OnOrBefore((Func<DateTime?, DateTime>)null!), new(typeof(ArgumentNullException), "other")),
            new("After.DateTime?", () => new InlineValidator<DateTime?>().RuleFor(value => value).After((Func<DateTime?, DateTime>)null!), new(typeof(ArgumentNullException), "other")),
            new("OnOrAfter.DateTime?", () => new InlineValidator<DateTime?>().RuleFor(value => value).OnOrAfter((Func<DateTime?, DateTime>)null!), new(typeof(ArgumentNullException), "other"))
        ];

        public sealed record Case(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
