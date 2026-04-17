namespace PineGuard.Common;

/// <summary>
/// Specifies the naming convention (casing style) for string validation.
/// </summary>
public enum StringCasing
{
    /// <summary>
    /// camelCase: first word lowercase, subsequent words capitalized (e.g., <c>myVariable</c>).
    /// </summary>
    CamelCase,

    /// <summary>
    /// PascalCase: every word capitalized (e.g., <c>MyVariable</c>).
    /// </summary>
    PascalCase,

    /// <summary>
    /// snake_case: words separated by underscores, all lowercase (e.g., <c>my_variable</c>).
    /// </summary>
    SnakeCase,

    /// <summary>
    /// UPPER_SNAKE_CASE: words separated by underscores, all uppercase (e.g., <c>MY_VARIABLE</c>).
    /// </summary>
    UpperSnakeCase,

    /// <summary>
    /// kebab-case: words separated by hyphens, all lowercase (e.g., <c>my-variable</c>).
    /// </summary>
    KebabCase,

    /// <summary>
    /// Train-Case: words separated by hyphens, each capitalized (e.g., <c>My-Variable</c>).
    /// </summary>
    TrainCase,

    /// <summary>
    /// <c>dot.case</c>: words separated by dots, all lowercase (e.g., <c>my.variable</c>).
    /// </summary>
    DotCase,

    /// <summary>
    /// space case: words separated by spaces, all lowercase (e.g., <c>my variable</c>).
    /// </summary>
    SpaceCase
}
