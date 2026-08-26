using System.Reflection;
using System.Text.RegularExpressions;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Codes;

public static partial class MustCodesTestData
{
    internal static readonly Regex GrammarPattern = new(@"^[a-z][a-z0-9]*(-[a-z0-9]+)*(\.[a-z][a-z0-9]*(-[a-z0-9]+)*){2}$", RegexOptions.Compiled);

    public static class Constants
    {
        public static TheoryData<ConstantCase> Cases
        {
            get
            {
                var data = new TheoryData<ConstantCase>();
                foreach (var fieldInfo in DiscoverConstantFields())
                    data.Add(new ConstantCase(GetIdentifierPath(fieldInfo), fieldInfo));
                return data;
            }
        }

        public sealed record ConstantCase(string Name, FieldInfo Field)
            : BaseCase(Name);
    }

    public static class Prefixes
    {
        public static TheoryData<PrefixCase> Cases
        {
            get
            {
                var data = new TheoryData<PrefixCase>();
                foreach (var type in DiscoverDomainTree())
                    data.Add(new PrefixCase(type.FullName ?? type.Name, type));
                return data;
            }
        }

        public sealed record PrefixCase(string Name, Type Type)
            : BaseCase(Name);
    }

    internal static IEnumerable<FieldInfo> DiscoverConstantFields() =>
        DiscoverDomainTree()
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Where(f => f.IsLiteral && f.FieldType == typeof(string) && !string.Equals(f.Name, "Prefix", StringComparison.Ordinal));

    internal static IEnumerable<Type> DiscoverDomainTree()
    {
        var stack = new Stack<Type>();
        stack.Push(typeof(MustCodes));

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (current != typeof(MustCodes))
                yield return current;

            foreach (var nested in current.GetNestedTypes(BindingFlags.Public))
                stack.Push(nested);
        }
    }

    internal static string GetConstantValue(FieldInfo field) => (string)field.GetRawConstantValue()!;

    internal static string GetPrefix(Type type)
    {
        var field = type.GetField("Prefix", BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException($"{type} does not declare a Prefix constant.");
        return GetConstantValue(field);
    }

    internal static string GetIdentifierPath(FieldInfo field)
    {
        var segments = new List<string> { ToKebabCase(field.Name) };

        for (var type = field.DeclaringType; type is not null && type != typeof(MustCodes); type = type.DeclaringType)
            segments.Insert(0, ToKebabCase(type.Name));

        return string.Join('.', segments);
    }

    internal static string ToKebabCase(string pascalCase) =>
        Regex.Replace(pascalCase, "(?<=[a-z0-9])(?=[A-Z])", "-").ToLowerInvariant();
}
