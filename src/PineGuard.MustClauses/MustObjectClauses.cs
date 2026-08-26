using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate object identity, type, and equality.
/// </summary>
/// <seealso cref="ObjectRules"/>
/// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
public static class MustObjectClauses
{
    /// <summary>
    /// Validates that the specified value is equal to another value using default equality semantics.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The expected value to compare against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> equals <paramref name="other"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsEqualTo{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must be equal to the expected value."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.EqualTo(status, "active");
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="ObjectRules.IsEqualTo{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<T> EqualTo<T>(this IMustClause _,
        T? value,
        T? other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be equal to the expected value.";

        var ok = ObjectRules.IsEqualTo(value, other);
        return MustResult<T>.FromBool(ok, MustCodes.Value.Equality.NotEqual, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified value is not equal to another value using default equality semantics.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not equal <paramref name="other"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsEqualTo{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be equal to the expected value."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotEqualTo(role, "admin");
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="ObjectRules.IsEqualTo{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<T> NotEqualTo<T>(this IMustClause _,
        T? value,
        T? other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be equal to the expected value.";

        var ok = !ObjectRules.IsEqualTo(value, other);
        return MustResult<T>.FromBool(ok, MustCodes.Value.Equality.Equal, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified object is exactly of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The expected exact runtime type.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The object to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is exactly type <typeparamref name="T"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsOfType{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must be of the expected type."</c>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var result = Must.Be.OfType<string>(obj);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// ]]></code>
    /// </example>
    /// <seealso cref="ObjectRules.IsOfType{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<object> OfType<T>(this IMustClause _,
        object? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be of the expected type.";

        var ok = ObjectRules.IsOfType<T>(value);
        return MustResult<object>.FromBool(ok, MustCodes.Value.Identity.WrongType, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified object is not exactly of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to check against.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The object to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not exactly type <typeparamref name="T"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsOfType{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be of the expected type."</c>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var result = Must.Be.NotOfType<string>(obj);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// ]]></code>
    /// </example>
    /// <seealso cref="ObjectRules.IsOfType{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<object> NotOfType<T>(this IMustClause _,
        object? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be of the expected type.";

        var ok = !ObjectRules.IsOfType<T>(value);
        return MustResult<object>.FromBool(ok, MustCodes.Value.Identity.SameType, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified object is assignable to type <typeparamref name="T"/> (i.e., is an instance
    /// of <typeparamref name="T"/> or a derived type).
    /// </summary>
    /// <typeparam name="T">The target type to check assignability to.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The object to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is assignable to <typeparamref name="T"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsAssignableToType{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must be assignable to the expected type."</c>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var result = Must.Be.AssignableToType<IDisposable>(obj);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// ]]></code>
    /// </example>
    /// <seealso cref="ObjectRules.IsAssignableToType{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<object> AssignableToType<T>(this IMustClause _,
        object? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be assignable to the expected type.";

        var ok = ObjectRules.IsAssignableToType<T>(value);
        return MustResult<object>.FromBool(ok, MustCodes.Value.Identity.NotAssignable, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified object is not assignable to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The target type to check assignability against.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The object to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not assignable to <typeparamref name="T"/>, or <see langword="false"/> with
    /// a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsAssignableToType{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be assignable to the expected type."</c>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var result = Must.Be.NotAssignableToType<IDisposable>(obj);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// ]]></code>
    /// </example>
    /// <seealso cref="ObjectRules.IsAssignableToType{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<object> NotAssignableToType<T>(this IMustClause _,
        object? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be assignable to the expected type.";

        var ok = !ObjectRules.IsAssignableToType<T>(value);
        return MustResult<object>.FromBool(ok, MustCodes.Value.Identity.Assignable, messageTemplate, paramName, value, result: value!);
    }

    /// <summary>
    /// Validates that the specified reference-type value refers to the same object instance as another value.
    /// </summary>
    /// <typeparam name="T">The reference type of the values being compared.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="a">The value to validate.</param>
    /// <param name="b">The expected same-reference value.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="a"/> and <paramref name="b"/> are the same object instance, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsSameReferenceAs{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must reference the same instance."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.SameReferenceAs(obj, singleton);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="ObjectRules.IsSameReferenceAs{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<T> SameReferenceAs<T>(this IMustClause _,
        T? a,
        T? b,
        [CallerArgumentExpression(nameof(a))] string? paramName = null)
        where T : class
    {
        const string messageTemplate = "{paramName} must reference the same instance.";

        var ok = ObjectRules.IsSameReferenceAs(a, b);
        return MustResult<T>.FromBool(ok, MustCodes.Value.Identity.NotSameReference, messageTemplate, paramName, a, result: a!);
    }

    /// <summary>
    /// Validates that the specified reference-type value does not refer to the same object instance as another value.
    /// </summary>
    /// <typeparam name="T">The reference type of the values being compared.</typeparam>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="a">The value to validate.</param>
    /// <param name="b">The value to compare against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="a"/> and <paramref name="b"/> are different object instances, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="ObjectRules.IsSameReferenceAs{T}"/>. The failure message follows the pattern
    /// <c>"{paramName} must not reference the same instance."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotSameReferenceAs(copy, original);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="ObjectRules.IsSameReferenceAs{T}"/>
    /// <seealso href="https://pineguard.ai/docs/must/object">Object Must Clauses documentation</seealso>
    public static MustResult<T> NotSameReferenceAs<T>(this IMustClause _,
        T? a,
        T? b,
        [CallerArgumentExpression(nameof(a))] string? paramName = null)
        where T : class
    {
        const string messageTemplate = "{paramName} must not reference the same instance.";

        var ok = !ObjectRules.IsSameReferenceAs(a, b);
        return MustResult<T>.FromBool(ok, MustCodes.Value.Identity.SameReference, messageTemplate, paramName, a, result: a!);
    }
}
