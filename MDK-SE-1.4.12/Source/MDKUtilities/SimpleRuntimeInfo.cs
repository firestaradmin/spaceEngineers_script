using System;
using Sandbox.ModAPI.Ingame;

namespace Malware.MDKUtilities
{
    /// <summary>
    ///     This is the default implementation of the mocked grid program runtime information.
    /// </summary>
    public class SimpleRuntimeInfo : IMyGridProgramRuntimeInfo
    {
        /// <summary>
        ///     Gets the time elapsed since the last time the Main method of this program was
        ///     run. This property returns no valid data neither in the constructor nor the Save
        ///     method.
        /// </summary>
        public virtual TimeSpan TimeSinceLastRun { get; set; } = TimeSpan.Zero;

        /// <summary>
        ///     Gets the number of milliseconds it took to execute the Main method the last time
        ///     it was run. This property returns no valid data neither in the constructor nor
        ///     the Save method.
        /// </summary>
        public virtual double LastRunTimeMs { get; set; } = 0;

        /// <summary>
        ///     Gets the maximum number of significant instructions that can be executing during
        ///     a single run, including any other programmable blocks invoked immediately.
        /// </summary>
        /// <remarks>
        ///     A "significant instruction" is a code junction; things like method calls, switches, if/else, loops etc.
        /// </remarks>
        public virtual int MaxInstructionCount { get; set; } = 50000;

        /// <summary>
        ///     Gets the current number of significant instructions executed.
        /// </summary>
        /// <remarks>
        ///     A "significant instruction" is a code junction; things like method calls, switches, if/else, loops etc.
        /// </remarks>
        public virtual int CurrentInstructionCount { get; set; } = 0;

        /// <summary>
        /// The maximum number of method calls that may be performed in a chained manner (one method calling the next calling the next etc.)
        /// </summary>
        public int MaxCallChainDepth { get; set; } = 0;

        /// <summary>
        /// The current number of method calls that may be performed in a chained manner (one method calling the next calling the next etc.)
        /// </summary>
        public int CurrentCallChainDepth { get; } = 0;

        /// <summary>
        ///     This property is obsolete and no longer holds any meaning.
        /// </summary>
        [Obsolete("This property no longer holds any meaning.")]
        public virtual int MaxMethodCallCount { get; } = int.MaxValue;

        /// <summary>
        ///     This property is obsolete and no longer holds any meaning.
        /// </summary>
        [Obsolete("This property no longer holds any meaning.")]
        public virtual int CurrentMethodCallCount { get; } = 0;

        /// <summary>
        //     Gets or sets how frequently this script will run itself
        /// </summary>
        public UpdateFrequency UpdateFrequency { get; set; }
    }
}
