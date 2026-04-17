using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PineGuard.DataAnnotations.Common;

/// <summary>
/// Provides the base class for dictionary validation attributes that resolve generic type arguments at
/// runtime via reflection.
/// </summary>
/// <remarks>
/// <para>
/// Subclasses target dictionary-like types (e.g., <see cref="IDictionary{TKey,TValue}"/> or
/// <see cref="IReadOnlyDictionary{TKey,TValue}"/>) by specifying the open generic interface type
/// and the static must-clause class containing the validation methods.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <param name="interfaceType">The open generic dictionary interface (e.g., <c>typeof(IDictionary&lt;,&gt;)</c>).</param>
/// <param name="mustClausesType">The static class containing the must-clause methods to invoke.</param>
/// <seealso cref="ValidationAttributeBase"/>
/// <seealso href="https://pineguard.ai/docs/annotations">Annotation documentation</seealso>
public abstract class GenericDictionaryAttributeBase(Type interfaceType, Type mustClausesType) : ValidationAttributeBase(typeof(object), allowNull: true)
{
    /// <summary>
    /// Invokes the named must-clause method on the configured must-clauses type,
    /// resolving generic type arguments from the runtime dictionary type.
    /// </summary>
    /// <param name="methodName">The name of the static must-clause method to invoke.</param>
    /// <param name="value">The dictionary value to validate.</param>
    /// <param name="ctx">The validation context for the current member.</param>
    /// <param name="args">Additional arguments beyond the dictionary value.</param>
    /// <returns>
    /// <see langword="null"/> on success, or a <see cref="ValidationResult"/> describing the failure.
    /// </returns>
    protected ValidationResult? InvokeDictionaryMust(string methodName, object? value, ValidationContext ctx, params object?[] args)
    {
        var type = value!.GetType();
        var iDict = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);

        if (iDict == null) return ValidationResult.Success;

        var argsType = iDict.GetGenericArguments();

        var method = mustClausesType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)!;

        var genericMethod = method.MakeGenericMethod(argsType[0], argsType[1]);
        var invokeArgs = BuildInvokeArgs(genericMethod, value, args);
        return InvokeAndMapResult(genericMethod, invokeArgs, ctx);
    }
}
