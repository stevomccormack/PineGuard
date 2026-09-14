# PineGuard.Xml

**XSD conformance for XML payloads, validated through PineGuard.**

CHESS replacement, NPP and SWIFT MX are ISO 20022: hundreds of message definitions, each identified by its root element namespace, each with its own XSD. `System.Xml.Schema` already does the compiling and the checking; what it does not do for you is the three things nearly everyone gets wrong — an unrecognised root namespace produces no schema errors at all (there is nothing to validate against, so it silently passes), the default `XmlResolver` will happily fetch a schema location off the network, and the first `ValidationEventHandler` call most people write stops at the first problem instead of listing every one. This package closes those three traps and feeds the result into the same `MustResult<T>`/`MustValidationResult` pipeline as the rest of PineGuard.

**One rule library. Every call site in your architecture.** `Must.Be.ValidXml(payload, schemas)` sits next to every other `Must.Be.*` call in your codebase, with the same code, message and property-path shape.

## Install

```bash
dotnet add package PineGuard.Xml
```

Depends on [PineGuard.Core](https://www.nuget.org/packages/PineGuard.Core), [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses), [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses) and [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations) — all first-party. There is no FluentValidation adapter in this package (see the recipe below instead): that would force the FluentValidation dependency on every consumer, including the ones who only want the DataAnnotations attribute.

### Supported frameworks

Targets `net8.0`, `net10.0`, and `netstandard2.1`. `System.Xml.Schema` is in-box on all three, so no extra package reference is needed to resolve any of them.

## What you get

- `XmlSchemaSetBuilder` — compiles one or more XSD sources (`AddFile`, `AddStream`, `AddReader`, `AddText`) into a single `XmlSchemaSet`, resolver disabled, single-use.
- `XmlSchemaValidationOptions` — `RequireKnownNamespace` (default `true`) and `TreatWarningsAsViolations` (default `false`).
- `XmlSchemaViolationKind` / `XmlSchemaViolation` — the kind, element path, message and position of one problem.
- `XmlSchemaUtility.TryValidate` — the streaming validation algorithm every other member in this package is built on.
- `XmlSchemaRules.IsValidXml` — the pure predicate.
- `MustXmlSchemaClauses.ValidXml` — `Must.Be.ValidXml(value, schemas)`.
- `GuardXmlSchemaClauses.InvalidXml` — `Guard.Against.InvalidXml(value, schemas)`.
- `ValidXmlAttribute` — `[ValidXml]`, resolving the schema set from the validation context's services.
- `XmlSchemaMustValidator` — an `IMustValidator<string>` that carries every violation, not just the first.

## Examples

Build the schema set once, at startup, and register it:

```csharp
using System.Xml.Schema;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Xml;

XmlSchemaSet schemas = new XmlSchemaSetBuilder()
    .AddFile("schemas/pacs.008.001.08.xsd")
    .Build();

services.AddSingleton(schemas);
```

The compiled `XmlSchemaSet` is safe to share across concurrent validations this way, provided nothing calls `Add` or `Compile` on it afterwards — it is the mutable BCL type, not a value owned by `XmlSchemaSetBuilder`.

A single value, through `Must.Be`:

```csharp
using PineGuard.MustClauses;
using PineGuard.Xml;

var result = Must.Be.ValidXml(payload, schemas);
if (result.Failed)
    Console.WriteLine(result.Message); // e.g. "payload must be valid against the XML schema (2 violation(s), first at 'Document/GrpHdr/MsgId')."
```

A constructor or service boundary, through `Guard.Against`:

```csharp
using PineGuard.GuardClauses;
using PineGuard.Xml;

public MessageEnvelope(string payload, XmlSchemaSet schemas) =>
    Payload = Guard.Against.InvalidXml(payload, schemas);
```

A request DTO, through `[ValidXml]` (requires the schema set — and, optionally, `XmlSchemaValidationOptions` — registered as services on the `ValidationContext`):

```csharp
public sealed class MessageEnvelope
{
    [ValidXml]
    public required string Payload { get; init; }
}
```

Every violation, not just the first, through `XmlSchemaMustValidator`:

```csharp
using PineGuard.Xml;

var validator = new XmlSchemaMustValidator(schemas);
var validation = validator.Validate(payload);

// validation.Failures[0].PropertyPath -> "Document/GrpHdr/MsgId"
// validation.Failures[0].Code         -> "xml.schema.mismatch"
// validation.Failures[1].PropertyPath -> "Document/GrpHdr/CreDtTm"
```

FluentValidation, through the generic `MustBe` bridge in [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation) — no dedicated adapter needed:

```csharp
using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;
using PineGuard.Xml;

RuleFor(x => x.Payload).MustBe(v => Must.Be.ValidXml(v, schemas, paramName: null), null, MustCodes.Xml.Schema.Mismatch);
```

### A note on warnings and lax content

`TreatWarningsAsViolations` defaults to `false`. ISO 20022 messages commonly carry a `SplmtryData` element declared with `xs:any processContents="lax"`, which the validation engine reports as a warning on a perfectly valid message — treating every warning as a failure would reject conforming traffic. What is on by default is `RequireKnownNamespace`: it is the check that stops a document in a namespace nobody registered a schema for from sailing through with zero errors, because there was nothing to check it against. Only XSD 1.0 is supported — .NET's schema engine does not implement XSD 1.1.

## Other layers, same rule library

- **Constructors and service boundaries** → [PineGuard.GuardClauses](https://www.nuget.org/packages/PineGuard.GuardClauses)
- **Result-based, composable validation** → [PineGuard.MustClauses](https://www.nuget.org/packages/PineGuard.MustClauses)
- **Request DTOs and Blazor forms** → [PineGuard.DataAnnotations](https://www.nuget.org/packages/PineGuard.DataAnnotations)
- **Pipeline-style request validators** → [PineGuard.FluentValidation](https://www.nuget.org/packages/PineGuard.FluentValidation)

See the [full documentation](https://github.com/stevomccormack/PineGuard) for the complete rule catalog.

## License

MIT © Steve McCormack
