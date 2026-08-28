using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public static class GuardExceptionPolicyTestData
{
    public static class HasMap
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("no map installed: HasMap is false", InstallMap: false, Expected: false),
            new("a map installed: HasMap is true", InstallMap: true, Expected: true)
        ];

        public sealed record Case(string Name, bool InstallMap, bool Expected) : BaseCase(Name);
    }

    public static class Clear
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class BeginScope
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class NestedScope
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class MapInsideActiveScope
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class ClearInsideActiveScope
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class DoubleDispose
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class StaleDispose
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class ChildContextIsolation
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class NullArgumentGuards
    {
        public static TheoryData<bool> Cases => [true];
    }
}
