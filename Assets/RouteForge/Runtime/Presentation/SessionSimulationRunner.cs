using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Запускает и обновляет набор детерминированных симуляций агентов.
    /// </summary>
    public sealed class SessionSimulationRunner : MonoBehaviour
    {
        private GameSession _session;
        private AgentRouteSimulation[] _simulations = new AgentRouteSimulation[0];

        /// <summary>
        /// Количество запущенных симуляций в последнем старте.
        /// </summary>
        public int StartedAgentCount { get; private set; }

        /// <summary>
        /// Передает игровую сессию и симуляции агентов.
        /// </summary>
        /// <param name="session">Игровая сессия.</param>
        /// <param name="simulations">Симуляции агентов.</param>
        public void Construct(GameSession session, AgentRouteSimulation[] simulations)
        {
            _session = session;
            _simulations = simulations ?? new AgentRouteSimulation[0];
        }

        /// <summary>
        /// Запускает сессию и все переданные маршруты.
        /// </summary>
        /// <param name="routes">Маршруты агентов.</param>
        public void StartSession(Route[] routes)
        {
            if (_session == null || routes == null || !_session.StartRunning())
            {
                return;
            }

            StartedAgentCount = 0;
            int count = routes.Length < _simulations.Length ? routes.Length : _simulations.Length;
            for (int i = 0; i < count; i++)
            {
                _simulations[i].Start(routes[i]);
                StartedAgentCount++;
            }
        }

        /// <summary>
        /// Ставит сессию и симуляции на паузу.
        /// </summary>
        public void Pause()
        {
            if (_session != null && _session.Pause())
            {
                for (int i = 0; i < _simulations.Length; i++)
                {
                    _simulations[i].Pause();
                }
            }
        }

        /// <summary>
        /// Возобновляет сессию и симуляции.
        /// </summary>
        public void Resume()
        {
            if (_session != null && _session.Resume())
            {
                for (int i = 0; i < _simulations.Length; i++)
                {
                    _simulations[i].Resume();
                }
            }
        }

        /// <summary>
        /// Выполняет один логический шаг всех симуляций.
        /// </summary>
        public void Tick()
        {
            for (int i = 0; i < _simulations.Length; i++)
            {
                _simulations[i].Tick();
            }
        }
    }
}
