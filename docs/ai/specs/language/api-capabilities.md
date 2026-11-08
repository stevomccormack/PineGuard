# API naming and target capabilities

This matrix accompanies the [shared vocabulary](vocabulary.md). It records intentional platform
boundaries; a missing entry in an adapter is not automatically justified by a target limitation.

| Family | netstandard2.1 | net8.0 / net10.0 | Naming and result contract |
|---|---|---|---|
| Decimal string numeric rules and adapters | Available | Available | Flattened operations end in `Decimal`; successful Must/Guard output is decimal. |
| Int32/Int64 string parsing and ranges | Available | Available | `Int32`, `Int64`, `InRangeInt32`, `OutOfRangeInt64`; no generic-math dependency. |
| String parity and floating-point predicates | Available | Available | `EvenInt32` / `OddInt32`; `FiniteDouble` / `NaNDouble` and their complements. |
| Coordinate strings | Available | Available | `Latitude`, `Longitude`, `GeoLocation`; invariant parsing with explicit styles. |
| Generic-math numeric validation | Unavailable | Available | Requires platform `INumber<T>`; no emulated generic-math facade. Core decimal overloads cover common decimal predicates on every target. |
| DateOnly / TimeOnly and their ranges | Unavailable | Available | Requires platform types. String adapters also expose those types in configuration or results. |
| DateTime / DateTimeOffset / TimeSpan | Available | Available | String generic operations carry the parsed type; domain-local Core names remain short. |
| XML syntax / schema validation | Available | Available | `Xml` is well-formedness; `ValidXml` / `InvalidXml` is XSD conformance. |
| Collections | Available | Available | Explicit comparers govern membership/subset checks; nullable-item validation returns a non-null-item snapshot without dropping entries. |

FluentValidation has no dedicated schema extension in PineGuard.Xml, which intentionally has no
FluentValidation dependency. Compose `Must.Be.ValidXml` through the existing `MustBe` adapter.
DataAnnotations accepts metadata-compatible configuration: no comparer or delegate parameters;
decimal configuration is invariant text so exact values remain usable in actual attribute syntax.

`MustResult.Result` is the parsed/normalized successful value and `Value` is the original input.
Guard returns `Result`. FluentValidation and annotations validate a property without replacing it.
Null-skipping adapters still validate configuration before skipping an absent property.

Tests execute on .NET 8 and .NET 10. The netstandard2.1 assets are compiled as part of the
all-target solution build; they are not claimed to be a separate executable test runtime.
