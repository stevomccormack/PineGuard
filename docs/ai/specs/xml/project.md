---
spec:
  id: pineguard.ai.xml.project-spec
  title: "PineGuard.Xml Project Specification"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../project.md
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.Xml/**"
---

# PineGuard.Xml Project Specification

The package, assembly and namespace are `PineGuard.Xml`. It adds XSD conformance to Core's XML
well-formedness checks and follows the same Utility → Rules → Must → Guard/annotation pipeline.
The original design rationale is [XML schema validation](../../plans/xml-schema-validation.md).

## Vocabulary and package boundaries

XML **well-formedness** is syntax: Core `XmlRules.IsXml`, `Must.Be.Xml`,
`Guard.Against.NotXml`, `XmlStringAttribute`. XML **validity** is schema conformance:
`XmlSchemaRules.IsValidXml`, `Must.Be.ValidXml`, `Guard.Against.InvalidXml`, `ValidXmlAttribute`.
Preserve this distinction. The Valid/Invalid spelling is an exact domain exception recorded in
[the shared vocabulary](../language/vocabulary.md), not a general naming pattern.

`XmlSchemaRules` avoids collision with Core's `XmlRules`. `XmlSchemaMustValidator` distinguishes
the Must-validation adapter from the BCL `XmlSchemaValidator`. The package references first-party
Core, Must, Guard and annotation packages; it has no direct FluentValidation dependency.
Consumers compose schema validation through the existing Fluent `MustBe` adapter when needed.

## Contracts

- `XmlSchemaUtility.TryValidate` owns streaming schema validation and diagnostic collection.
  Rules delegate to it. Must owns stable codes/templates; Guard and attributes reuse Must.
- Schemas are required configuration. Must validates them before input-dependent returns and
  attributes configuration failures to `schemas`.
- Disable external resolution and DTD processing. Never enable instance-supplied inline schemas
  or schema locations. Schema building is the explicit IO boundary, not the validation predicate.
- Preserve the root-namespace policy and warning options. Malformed documents, unknown namespaces
  and schema violations retain distinct diagnostic kinds and canonical codes.
- Successful single-value clauses return the original XML string; they do not normalize or
  rewrite XML. The multi-failure validator preserves diagnostic paths and does not echo payloads.
- The annotation resolves required schema services before skipping an optional null value.
  A missing schema service throws `InvalidOperationException` as an adapter setup error, even for
  null input; it cannot be hidden by optionality. Caller-provided schema sets must not be mutated
  during validation.

## Verification

Use shared testing/fixture conventions. Exercise well-formedness separately from schema validity,
known/unknown namespaces, lax-content warnings, multiple violations, configuration failures,
adapter messages/codes and import collisions with System.Xml namespaces. Compile all supported
targets and require the repository's 100% line and branch coverage gates on supported test TFMs.
