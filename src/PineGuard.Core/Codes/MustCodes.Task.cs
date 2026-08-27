namespace PineGuard.Codes;

// Serves: MustTaskClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>task</c> domain: the terminal state of an asynchronous operation.</summary>
    public static class Task
    {
        /// <summary>The code prefix for this node (<c>"task"</c>).</summary>
        public const string Prefix = "task";

        /// <summary>The state the task was observed in.</summary>
        public static class Status
        {
            /// <summary>The code prefix for this node (<c>"task.status"</c>).</summary>
            public const string Prefix = Task.Prefix + ".status";

            /// <summary><c>task.status.not-completed</c></summary>
            public const string NotCompleted = Prefix + ".not-completed";

            /// <summary><c>task.status.completed</c></summary>
            public const string Completed = Prefix + ".completed";

            /// <summary><c>task.status.not-canceled</c></summary>
            public const string NotCanceled = Prefix + ".not-canceled";

            /// <summary><c>task.status.canceled</c></summary>
            public const string Canceled = Prefix + ".canceled";

            /// <summary><c>task.status.not-faulted</c></summary>
            public const string NotFaulted = Prefix + ".not-faulted";

            /// <summary><c>task.status.faulted</c></summary>
            public const string Faulted = Prefix + ".faulted";
        }
    }
}
