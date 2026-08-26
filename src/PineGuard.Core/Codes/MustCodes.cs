namespace PineGuard.Codes;

/// <summary>
/// The Must error-code catalogue: <c>&lt;domain&gt;.&lt;aspect&gt;.&lt;condition&gt;</c>.
/// </summary>
/// <remarks>
/// <para>
/// A code is an address, not a label — it addresses a rule the way an XPath addresses a node.
/// Every code has exactly three segments: the <b>domain</b> (the family of value being validated,
/// fixed by the clause class), the <b>aspect</b> (the facet of the value the rule looks at), and the
/// <b>condition</b> (the failure state observed on the aspect — the exact complement of the rule).
/// </para>
/// <para>
/// The identifier path mirrors the code one-to-one: <c>MustCodes.Email.Address.Invalid</c> ↔
/// <c>"email.address.invalid"</c>. Every node declares its own <c>Prefix</c> and every value is
/// composed from its parent (<c>Prefix + ".invalid"</c>), which the compiler folds to a constant, so
/// codes stay legal in attributes and constant patterns while each segment is spelled exactly once.
/// </para>
/// <para>
/// This type is split into one partial file per domain (<c>MustCodes.&lt;Domain&gt;.cs</c>), each
/// declaring one nested domain class. See <c>docs/ai/plans/new-surfaces-missing-validation-cases-00-program.md</c>
/// §5.4 for the full grammar, domain map, and controlled condition vocabulary.
/// </para>
/// </remarks>
public static partial class MustCodes;
