using PineGuard.MustClauses;

namespace PineGuard.Core.UnitTests.MustClauses.Samples;

public sealed class CreateOrderValidator : MustValidator<CreateOrder>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Email, email => email is not null && email.Contains('@')
            ? MustResult<string>.Ok(email, email, nameof(email))
            : MustResult<string>.Fail("sample.email.invalid", "{paramName} must be a valid email address.", nameof(email), email));

        RuleFor(x => x.EndDate, (order, end) => end > order.StartDate
            ? MustResult<DateTime>.Ok(end, end, nameof(end))
            : MustResult<DateTime>.Fail("sample.end-date.not-after", "{paramName} must be after the reference date.", nameof(end), end));

        RuleFor(x => x.Weight, weight => weight > 0
            ? MustResult<decimal>.Ok(weight, weight, nameof(weight))
            : MustResult<decimal>.Fail("sample.weight.not-positive", "{paramName} must be positive.", nameof(weight), weight)).When(x => x.IsPhysical);

        RuleFor(x => x.Lines, lines => lines is { Count: > 0 }
            ? MustResult<IReadOnlyList<OrderLine>>.Ok(lines, lines, nameof(lines))
            : MustResult<IReadOnlyList<OrderLine>>.Fail("sample.lines.empty", "{paramName} must not be empty.", nameof(lines), lines));

        RuleForEach(x => x.Lines, new OrderLineValidator());
    }
}
