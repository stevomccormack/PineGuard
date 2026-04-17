# PineGuard.FluentValidation

**Already using FluentValidation? Don't rewrite your rules in a second dialect.**

PineGuard.FluentValidation exposes every PineGuard rule as an `IRuleBuilder` extension — `.Email()`, `.HttpsUrl()`, `.OwaspSafe()`, and the rest — so `AbstractValidator<T>` reads exactly the way you expect. Same rule library as Must and Guard, same answer every time.

FluentValidation is the .NET idiom for request, command, and DTO validation — application requests, MediatR commands, ASP.NET Core models, and anywhere your pipeline needs a validator. It's a **perfect fit for Clean Architecture**, where the always-valid-state principle holds at the Application boundary: put your `AbstractValidator<T>` in the Application layer, run it before the handler, and nothing reaches your domain model that hasn't been cleaned first.

**One rule library. Every call site in your architecture.** The same `Must.Be.Email` your services call and the same `Guard.Against.NotEmail` your domain constructors call power `RuleFor(x => x.Email).Email()` in your validator. No parallel dialects. No drift. When the rule changes, every layer changes with it.

## Install

```bash
dotnet add package PineGuard.FluentValidation
```

Targets `net8.0`, `net10.0`, and `netstandard2.1`. Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses), and [FluentValidation](https://www.nuget.org/packages/FluentValidation).

## Examples

```csharp
using FluentValidation;
using PineGuard.FluentValidation;

public sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        // The four canonical rules, as IRuleBuilder extensions
        RuleFor(x => x.Email).Required().Email();
        RuleFor(x => x.StrictEmailAddress).Required().StrictEmail();
        RuleFor(x => x.UserInput).Required().OwaspSafe();
        RuleFor(x => x.Callback).Required().HttpsUrl();
    }
}
```

### Chain with FluentValidation's built-ins

PineGuard extensions compose with every FluentValidation API — `.WithMessage()`, `.When()`, `.NotEmpty()`, `.MaximumLength()`, and the rest — so you can stack PineGuard rules alongside length limits, conditional checks, and custom error messages on a single property.

```csharp
RuleFor(x => x.Website)
    .Required()
    .HttpsUrl()
    .MaximumLength(256)
    .WithMessage("Website must be an HTTPS URL no longer than 256 characters.");

RuleFor(x => x.CompanyEmail)
    .Required()
    .StrictEmail()
    .When(x => x.IsCorporateUser);

RuleFor(x => x.Bio)
    .NotEmpty()
    .MaximumLength(500)
    .OwaspSafe();
```

### A note on naming

FluentValidation already defines `.NotNull()` and `.Null()`. PineGuard uses `.Required()` / `.NotRequired()` for presence checks so the two libraries never collide. Every other rule keeps its natural name.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Attribute-driven models** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete extension-method catalog.

## License

MIT © Steve McCormack
