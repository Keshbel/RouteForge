using System;

namespace RouteForge
{
    /// <summary>
    /// Легковесная доменная точка профилирования без зависимости от UnityEngine.
    /// </summary>
    public readonly struct RouteForgeProfilerMarker
    {
        /// <summary>
        /// Маркер проверки маршрута.
        /// </summary>
        public static readonly RouteForgeProfilerMarker RouteValidation = new RouteForgeProfilerMarker("RouteForge.RouteValidation");

        /// <summary>
        /// Маркер генерации уровня.
        /// </summary>
        public static readonly RouteForgeProfilerMarker LevelGeneration = new RouteForgeProfilerMarker("RouteForge.LevelGeneration");

        /// <summary>
        /// Маркер шага симуляции.
        /// </summary>
        public static readonly RouteForgeProfilerMarker SimulationUpdate = new RouteForgeProfilerMarker("RouteForge.SimulationUpdate");

        /// <summary>
        /// Имя маркера для отчета профилирования.
        /// </summary>
        public string Name { get; }

        private RouteForgeProfilerMarker(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Открывает область измерения для маркера.
        /// </summary>
        /// <returns>Область, закрываемая через Dispose.</returns>
        public Scope Auto()
        {
            return new Scope(this);
        }

        /// <summary>
        /// Область измерения маркера.
        /// </summary>
        public readonly struct Scope : IDisposable
        {
            private readonly RouteForgeProfilerMarker _marker;

            /// <summary>
            /// Создает область измерения.
            /// </summary>
            /// <param name="marker">Маркер, которому принадлежит область.</param>
            public Scope(RouteForgeProfilerMarker marker)
            {
                _marker = marker;
            }

            /// <summary>
            /// Завершает область измерения.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}
