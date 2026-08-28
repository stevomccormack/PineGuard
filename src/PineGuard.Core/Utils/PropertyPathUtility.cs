using System.Linq.Expressions;
using PineGuard.Common;

namespace PineGuard.Utils;

/// <summary>
/// Builds and transforms property paths (<c>Address.City</c>, <c>Lines[2].Sku</c>) used by
/// <see cref="PineGuard.MustClauses.MustFailure"/> and <see cref="PineGuard.MustClauses.MustValidator{T}"/>.
/// Pure string/expression-tree work — no reflection.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/must">Must Clauses documentation</seealso>
public static class PropertyPathUtility
{
    /// <summary>
    /// The separator between identifier segments of a property path.
    /// </summary>
    public const char PropertySeparator = '.';

    /// <summary>
    /// Appends an identifier segment to a parent path.
    /// </summary>
    /// <param name="parent">The parent path, or <see langword="null"/>/empty for the root.</param>
    /// <param name="property">The identifier segment to append.</param>
    /// <returns><c>property</c> when <paramref name="parent"/> is empty; otherwise <c>"{parent}.{property}"</c>.</returns>
    public static string Combine(string? parent, string property)
    {
        ThrowHelper.ThrowIfNull(property);

        return string.IsNullOrEmpty(parent) ? property : $"{parent}{PropertySeparator}{property}";
    }

    /// <summary>
    /// Appends a collection index segment to a parent path.
    /// </summary>
    /// <param name="parent">The parent path, or <see langword="null"/>/empty for the root.</param>
    /// <param name="index">The zero-based element index.</param>
    /// <returns><c>"[index]"</c> when <paramref name="parent"/> is empty; otherwise <c>"{parent}[index]"</c>.</returns>
    public static string Index(string? parent, int index) =>
        string.IsNullOrEmpty(parent) ? $"[{index}]" : $"{parent}[{index}]";

    /// <summary>
    /// Appends a dictionary key segment to a parent path.
    /// </summary>
    /// <param name="parent">The parent path, or <see langword="null"/>/empty for the root.</param>
    /// <param name="key">The dictionary key. Rendered without quotes, consistent with <see cref="Index"/>.</param>
    /// <returns><c>"[key]"</c> when <paramref name="parent"/> is empty; otherwise <c>"{parent}[key]"</c>.</returns>
    public static string Key(string? parent, string key)
    {
        ThrowHelper.ThrowIfNull(key);

        return string.IsNullOrEmpty(parent) ? $"[{key}]" : $"{parent}[{key}]";
    }

    /// <summary>
    /// Applies <paramref name="segmentTransform"/> to every identifier segment of <paramref name="path"/>,
    /// leaving any <c>[…]</c> index/key suffix on each segment untouched.
    /// </summary>
    /// <param name="path">The path to transform, or <see langword="null"/>/empty for the root.</param>
    /// <param name="segmentTransform">The transform applied to each identifier segment (e.g. a naming policy).</param>
    /// <returns>The transformed path.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="segmentTransform"/> is <see langword="null"/>.</exception>
    public static string Transform(string? path, Func<string, string> segmentTransform)
    {
        ThrowHelper.ThrowIfNull(segmentTransform);

        if (string.IsNullOrEmpty(path))
            return string.Empty;

        var segments = path.Split(PropertySeparator);
        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];
            var bracketIndex = segment.IndexOf('[', StringComparison.Ordinal);
            segments[i] = bracketIndex < 0
                ? segmentTransform(segment)
                : segmentTransform(segment[..bracketIndex]) + segment[bracketIndex..];
        }

        return string.Join(PropertySeparator, segments);
    }

    /// <summary>
    /// Extracts a property path from a member-access lambda expression.
    /// </summary>
    /// <param name="expression">
    /// A lambda whose body is either the parameter itself (<c>x =&gt; x</c>, root path) or a chain of
    /// property/field accesses rooted at the parameter (<c>x =&gt; x.Address.City</c>), optionally
    /// wrapped in a boxing conversion (e.g. <c>Expression&lt;Func&lt;T, object&gt;&gt;</c>).
    /// </param>
    /// <returns><see cref="string.Empty"/> for the root parameter; otherwise the dotted member path.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the body contains a node other than a member-access chain rooted at the parameter
    /// (e.g. a method call or an indexer).
    /// </exception>
    public static string FromExpression(LambdaExpression expression)
    {
        ThrowHelper.ThrowIfNull(expression);

        var parameter = expression.Parameters[0];
        var current = Unwrap(expression.Body);

        if (current == parameter)
            return string.Empty;

        var segments = new List<string>();
        while (current is MemberExpression member)
        {
            segments.Add(member.Member.Name);
            current = Unwrap(member.Expression);
        }

        if (current != parameter)
            throw new ArgumentException(
                $"Unsupported expression node type '{current?.NodeType.ToString() ?? "null"}'. Only member access chains rooted at the lambda parameter are supported.",
                nameof(expression));

        segments.Reverse();
        return string.Join(PropertySeparator, segments);
    }

    private static Expression? Unwrap(Expression? expression) =>
        expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary
            ? unary.Operand
            : expression;
}
