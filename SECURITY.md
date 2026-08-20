# Security Policy

## Supported Versions

PineGuard is pre-1.0. Security fixes ship only in the **latest `0.x` alpha**
on nuget.org; earlier alpha revisions are not patched. Once 1.0 ships, fixes
will land in the latest stable minor.

| Version                        | Supported |
|--------------------------------|-----------|
| Latest `0.x` alpha on nuget.org| :white_check_mark: |
| Earlier `0.x` alpha revisions  | :x: |

## Reporting a Vulnerability

**Please do not open a public GitHub issue for security reports.**

Use GitHub's [private vulnerability reporting](https://github.com/stevomccormack/PineGuard/security/advisories/new).
This notifies the maintainer privately and creates a trackable advisory.

Fallback: email `hello@iamstevo.co` with subject `[PineGuard Security]`.

## Scope

**In scope**
- Validation bypass: a rule returning `IsValid = true` for input it should reject
  (especially OWASP categories: XSS, SQLi, path traversal, command injection).
- Denial of service: catastrophic regex backtracking, unbounded allocation,
  or stack overflow from adversarial input.
- Incorrect OWASP-safe claims: `OwaspRules.IsOwaspSafe` returning `true` for
  demonstrably unsafe input.

**Out of scope**
- Missing validation rules that don't exist yet (feature request).
- Issues in tests or internal tooling that don't ship in any package.
- Vulnerabilities in transitive NuGet dependencies (report upstream;
  Dependabot tracks them here).

## Response

| Stage              | Target |
|--------------------|--------|
| Acknowledgement    | 3 business days |
| Triage decision    | 7 business days |
| Critical fix       | 14 days |
| High fix           | 30 days |
| Coordinated disclosure | 90 days or fix release, whichever first |

Reporters are credited in release notes unless they request anonymity.
