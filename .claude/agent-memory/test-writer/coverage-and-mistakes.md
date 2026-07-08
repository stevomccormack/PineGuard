---
name: coverage-and-mistakes
description: 100% line+branch coverage requirements and the recurring mistakes to avoid when writing PineGuard tests.
metadata:
  type: feedback
---

### Coverage Requirements
- 100% line AND branch coverage for every class
- Test null inputs, empty strings, whitespace, min/max values, edge cases
- Test both `true` and `false` paths for every condition
- Config parameter null tests (attribute failure to config param name, not value)

### Common Mistakes to Avoid
- DO NOT skip null input tests (every nullable param needs null test case)
- DO NOT forget branch coverage (every if/else needs both paths tested)
- DO NOT use ad-hoc patterns — follow the spec EXACTLY
- DO NOT put TestData inline in test methods
- DO NOT forget to test CallerArgumentExpression propagation
