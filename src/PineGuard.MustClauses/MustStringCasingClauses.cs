using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate string casing styles,
/// delegating to <see cref="StringRules"/> for core validation logic.
/// </summary>
/// <seealso cref="StringRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
public static class MustStringCasingClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified string conforms to the given <see cref="StringCasing"/> style.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="style">The <see cref="StringCasing"/> style to require.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> matches the specified casing <paramref name="style"/>, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsCaseStyle"/>.
    /// The failure message follows the pattern <c>"{paramName} must be in the specified casing style."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> CaseStyle(this IMustClause _,
        string? value,
        StringCasing style,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Mismatch, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be in the specified casing style.";

        var ok = StringRules.IsCaseStyle(value, style);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Mismatch, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in camelCase format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is camelCase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsCamelCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be camelCase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> CamelCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotCamel, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be camelCase.";

        var ok = StringRules.IsCamelCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotCamel, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in PascalCase format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is PascalCase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsPascalCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be PascalCase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> PascalCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotPascal, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be PascalCase.";

        var ok = StringRules.IsPascalCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotPascal, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in snake_case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is snake_case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsSnakeCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be snake_case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> SnakeCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotSnake, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be snake_case.";

        var ok = StringRules.IsSnakeCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotSnake, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in UPPER_SNAKE_CASE format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is UPPER_SNAKE_CASE, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsUpperSnakeCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be UPPER_SNAKE_CASE."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> UpperSnakeCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotUpperSnake, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be UPPER_SNAKE_CASE.";

        var ok = StringRules.IsUpperSnakeCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotUpperSnake, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in kebab-case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is kebab-case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsKebabCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be kebab-case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> KebabCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotKebab, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be kebab-case.";

        var ok = StringRules.IsKebabCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotKebab, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in Train-Case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is Train-Case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsTrainCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be Train-Case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> TrainCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotTrain, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be Train-Case.";

        var ok = StringRules.IsTrainCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotTrain, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in <c>dot.case</c> format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is <c>dot.case</c>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsDotCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be dot.case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> DotCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotDot, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be dot.case.";

        var ok = StringRules.IsDotCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotDot, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is in space case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is space case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsSpaceCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be space case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> SpaceCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotSpace, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be space case.";

        var ok = StringRules.IsSpaceCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotSpace, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is equal to its upper invariant form.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is upper invariant, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsUpperInvariant"/>.
    /// The failure message follows the pattern <c>"{paramName} must be upper invariant."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> UpperInvariant(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotUpperInvariant, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be upper invariant.";

        var ok = StringRules.IsUpperInvariant(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotUpperInvariant, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is equal to its lower invariant form.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is lower invariant, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsLowerInvariant"/>.
    /// The failure message follows the pattern <c>"{paramName} must be lower invariant."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> LowerInvariant(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotLowerInvariant, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be lower invariant.";

        var ok = StringRules.IsLowerInvariant(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotLowerInvariant, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not conform to the given <see cref="StringCasing"/> style.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="style">The <see cref="StringCasing"/> style to reject.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not match the specified casing <paramref name="style"/>, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsCaseStyle"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be in the specified casing style."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotCaseStyle(this IMustClause _,
        string? value,
        StringCasing style,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Match, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be in the specified casing style.";

        var ok = !StringRules.IsCaseStyle(value, style);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Match, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in camelCase format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not camelCase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsCamelCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be camelCase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotCamelCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Camel, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be camelCase.";

        var ok = !StringRules.IsCamelCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Camel, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in PascalCase format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not PascalCase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsPascalCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be PascalCase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotPascalCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Pascal, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be PascalCase.";

        var ok = !StringRules.IsPascalCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Pascal, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in snake_case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not snake_case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsSnakeCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be snake_case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotSnakeCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Snake, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be snake_case.";

        var ok = !StringRules.IsSnakeCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Snake, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in UPPER_SNAKE_CASE format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not UPPER_SNAKE_CASE, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsUpperSnakeCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be UPPER_SNAKE_CASE."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotUpperSnakeCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.UpperSnake, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be UPPER_SNAKE_CASE.";

        var ok = !StringRules.IsUpperSnakeCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.UpperSnake, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in kebab-case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not kebab-case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsKebabCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be kebab-case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotKebabCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Kebab, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be kebab-case.";

        var ok = !StringRules.IsKebabCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Kebab, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in Train-Case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not Train-Case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsTrainCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be Train-Case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotTrainCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Train, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be Train-Case.";

        var ok = !StringRules.IsTrainCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Train, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in <c>dot.case</c> format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not <c>dot.case</c>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsDotCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be dot.case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotDotCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Dot, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be dot.case.";

        var ok = !StringRules.IsDotCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Dot, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not in space case format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not space case, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsSpaceCase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be space case."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotSpaceCase(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Space, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be space case.";

        var ok = !StringRules.IsSpaceCase(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Space, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not equal to its upper invariant form.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not upper invariant, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsUpperInvariant"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be upper invariant."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotUpperInvariant(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.UpperInvariant, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be upper invariant.";

        var ok = !StringRules.IsUpperInvariant(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.UpperInvariant, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not equal to its lower invariant form.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not lower invariant, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsLowerInvariant"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be lower invariant."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-casing">String Casing Must Clauses documentation</seealso>
    public static MustResult<string> NotLowerInvariant(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.LowerInvariant, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be lower invariant.";

        var ok = !StringRules.IsLowerInvariant(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.LowerInvariant, messageTemplate, paramName, value, value);
    }
}
