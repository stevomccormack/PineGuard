namespace PineGuard.Codes;

// Serves: MustWidgetClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>widget</c> domain: structural checks on widgets.</summary>
    public static class Widget
    {
        /// <summary>The code prefix for this node (<c>"widget"</c>).</summary>
        public const string Prefix = "widget";

        /// <summary>The widget's assembled state.</summary>
        public static class State
        {
            /// <summary>The code prefix for this node (<c>"widget.state"</c>).</summary>
            public const string Prefix = Widget.Prefix + ".state";

            /// <summary><c>widget.state.broken</c></summary>
            public const string Broken = Prefix + ".broken";

            /// <summary><c>widget.state.assembled</c></summary>
            public const string Assembled = Prefix + ".assembled";
        }
    }
}
