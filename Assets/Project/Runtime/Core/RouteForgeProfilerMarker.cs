using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct RouteForgeProfilerMarker
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public static readonly RouteForgeProfilerMarker RouteValidation = new RouteForgeProfilerMarker("RouteForge.RouteValidation");

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public static readonly RouteForgeProfilerMarker LevelGeneration = new RouteForgeProfilerMarker("RouteForge.LevelGeneration");

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public static readonly RouteForgeProfilerMarker SimulationUpdate = new RouteForgeProfilerMarker("RouteForge.SimulationUpdate");

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public string Name { get; }

        private RouteForgeProfilerMarker(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public Scope Auto()
        {
            return new Scope(this);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public readonly struct Scope : IDisposable
        {
            private readonly RouteForgeProfilerMarker _marker;

            /// <summary>
            /// Describes this API member.
            /// </summary>
            /// <param name="marker">The marker value.</param>
            public Scope(RouteForgeProfilerMarker marker)
            {
                _marker = marker;
            }

            /// <summary>
            /// Describes this API member.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}
