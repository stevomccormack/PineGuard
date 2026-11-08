# XML schema validation

Inherits from [global rules](global.md).

Before editing `src/PineGuard.Xml/**`, read:

- [XML project specification](../specs/xml/project.md).
- [Shared vocabulary](../specs/language/vocabulary.md), especially the XML validity exception.
- [Must specification](../specs/must-clauses/project.md) for messages, codes and configuration attribution.
- [Guard specification](../specs/guard-clauses/project.md) and [annotation specification](../specs/data-annotations/project.md) for adapters.

Tests follow the [shared test specification](../specs/testing/unit-test.md),
[fixture architecture](../specs/testing/fixture.md) and [coverage specification](../specs/testing/coverage.md).
Keep the package free of a direct FluentValidation dependency.
