using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class AgentRouteSimulation
    {
        private Route _route = Route.Empty;
        private int _currentCellIndex;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public event Action<AgentRouteSimulation> Completed;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int CurrentCellIndex => _currentCellIndex;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="route">The route value.</param>
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
        /// Describes this API member.
        /// </summary>
        public void Pause()
        {
            if (IsRunning)
            {
                IsPaused = true;
            }
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public void Resume()
        {
            if (IsRunning)
            {
                IsPaused = false;
            }
        }

        /// <summary>
        /// Describes this API member.
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
