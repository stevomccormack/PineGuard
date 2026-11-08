# Core/Common API surface evidence

The [machine-readable map](core-common-api-surface-map.json) compares pristine commit `e88762c6ab2bd33d12bf93a3ad047bb706f55daf` with the completed implementation using rich Roslyn inventories targeting net10.0. The baseline was extracted directly with `git archive`; it is not the earlier in-progress snapshot.

Every unique externally visible public documentation ID is accounted for:

| Disposition | Count |
|---|---:|
| Retained, no direct declaration/contract change | 3,848 |
| Retained, contract changed | 174 |
| Retained, direct source tokens changed only | 310 |
| Baseline IDs absent from final | 413 |
| Final IDs absent from baseline | 454 |
| Total baseline IDs | 4,745 |
| Total final IDs | 4,786 |

Removed/added IDs include renamed members and changed signatures. They do not alone imply semantic removal/addition. `baselineDeclarationTransitions` accounts for all 413 absent baseline IDs and identifies exact final declaration sets or explicit removal. Where overload shapes changed, it does not invent a one-to-one overload correspondence. The 203 recorded intermediate renames and 44 parameter changes are preserved separately as historical execution evidence.

The map records changed contract fields, including parameter names, defaults, nullability, constraints, attributes and return metadata. Trivia-free token hashes identify direct implementation changes without asserting that every source change changes observable behavior. Changes inherited through dependencies remain covered by the implementation report and tests. Type body hashes are excluded from duplicate implementation-change reporting; member changes are recorded individually.

Only the net10.0 surface was independently extracted. Other-target availability is described in the [capability matrix](../specs/language/api-capabilities.md) and verified by separate all-target builds. The map is evidence of API reconciliation, not a substitute for test or coverage results.
