using System;

namespace RouteForge
{
    /// <summary>
    /// Детерминированно симулирует логический прогресс агента по маршруту.
    /// </summary>
    public sealed class AgentRouteSimulation
    {
        private Route _route = Route.Empty;
        private int _currentCellIndex;

        /// <summary>
        /// Вызывается при завершении симуляции маршрута.
        /// </summary>
        public event Action<AgentRouteSimulation> Completed;

        /// <summary>
        /// Признак активного движения.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Признак паузы движения.
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Признак завершенного маршрута.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Текущий индекс клетки маршрута.
        /// </summary>
        public int CurrentCellIndex => _currentCellIndex;

        /// <summary>
        /// Запускает симуляцию по маршруту.
        /// </summary>
        /// <param name="route">Маршрут агента.</param>
        public void Start(Route route)
        {
            _route = route ?? throw new ArgumentNullException(nameof(route));
            _currentCellIndex = 0;
            IsComplete = _route.Count == 0;
            IsPaused = false;
            IsRunning = !IsComplete;

            if (IsComplete)
            {
                Completed?.Invoke(this);
            }
        }

        /// <summary>
        /// Ставит симуляцию на паузу.
        /// </summary>
        public void Pause()
        {
            if (IsRunning)
            {
                IsPaused = true;
            }
        }

        /// <summary>
        /// Возобновляет симуляцию после паузы.
        /// </summary>
        public void Resume()
        {
            if (IsRunning)
            {
                IsPaused = false;
            }
        }

        /// <summary>
        /// Выполняет один детерминированный шаг симуляции.
        /// </summary>
        public void Tick()
        {
            using (RouteForgeProfilerMarker.SimulationUpdate.Auto())
            {
                if (!IsRunning || IsPaused)
                {
                    return;
                }

                _currentCellIndex++;
                if (_currentCellIndex >= _route.Count)
                {
                    _currentCellIndex = _route.Count;
                    IsRunning = false;
                    IsComplete = true;
                    Completed?.Invoke(this);
                }
            }
        }
    }
}
