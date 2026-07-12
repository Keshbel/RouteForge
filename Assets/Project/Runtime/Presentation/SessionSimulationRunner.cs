using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class SessionSimulationRunner : MonoBehaviour
    {
        private GameSession _session;
        private AgentRouteSimulation[] _simulations = new AgentRouteSimulation[0];

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int StartedAgentCount { get; private set; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="session">The session value.</param>
        /// <param name="simulations">The simulations value.</param>
        public void Construct(GameSession session, AgentRouteSimulation[] simulations)
        {
            _session = session;
            _simulations = simulations ?? new AgentRouteSimulation[0];
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="routes">The routes value.</param>
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
        /// Describes this API member.
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
        /// Describes this API member.
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
        /// Describes this API member.
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
