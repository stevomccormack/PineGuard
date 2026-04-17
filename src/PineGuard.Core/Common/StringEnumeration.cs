namespace PineGuard.Common;

/// <summary>
/// A string-valued enumeration base class for defining type-safe string constants.
/// </summary>
/// <param name="value">The string value of the enumeration member.</param>
/// <param name="name">The display name of the enumeration member.</param>
public abstract class StringEnumeration(string value, string name) : Enumeration<string>(value, name);
