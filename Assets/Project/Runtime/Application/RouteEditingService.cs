using System;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class RouteEditingService
    {
        private readonly RouteValidator _routeValidator;
        private readonly Dictionary<AgentId, Route> _routes = new Dictionary<AgentId, Route>();
        private readonly List<GridPosition> _editBuffer = new List<GridPosition>();

        private BoardSnapshot _boardSnapshot;
        private AgentId _selectedAgentId;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public event Action<AgentId, Route> RouteChanged;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public event Action RoutesCleared;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public event Action<RouteValidationResult> RouteRejected;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="routeValidator">The routeValidator value.</param>
        /// <param name="initialBoardSnapshot">The initialBoardSnapshot value.</param>
        /// <param name="initialAgentId">The initialAgentId value.</param>
        public RouteEditingService(RouteValidator routeValidator, BoardSnapshot initialBoardSnapshot, AgentId initialAgentId)
        {
            _routeValidator = routeValidator ?? throw new ArgumentNullException(nameof(routeValidator));
            _boardSnapshot = initialBoardSnapshot ?? throw new ArgumentNullException(nameof(initialBoardSnapshot));
            _selectedAgentId = initialAgentId;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="boardSnapshot">The boardSnapshot value.</param>
        public void SetBoardSnapshot(BoardSnapshot boardSnapshot)
        {
            _boardSnapshot = boardSnapshot ?? throw new ArgumentNullException(nameof(boardSnapshot));
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        public void SelectAgent(AgentId agentId)
        {
            _selectedAgentId = agentId;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        /// <param name="route">The route value.</param>
        /// <returns>The operation result.</returns>
        public bool TryGetRoute(AgentId agentId, out Route route)
        {
            return _routes.TryGetValue(agentId, out route);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool TryAppendCell(GridPosition position)
        {
            CopySelectedRouteToBuffer();
            if (_editBuffer.Contains(position))
            {
                return true;
            }

            _editBuffer.Add(position);
            return TryCommitBuffer();
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool TryRemoveCell(GridPosition position)
        {
            CopySelectedRouteToBuffer();
            bool removed = _editBuffer.Remove(position);
            if (!removed)
            {
                return true;
            }

            return TryCommitBuffer();
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public void ClearAll()
        {
            _routes.Clear();
            _editBuffer.Clear();
            RoutesCleared?.Invoke();
        }

        private void CopySelectedRouteToBuffer()
        {
            _editBuffer.Clear();
            if (!_routes.TryGetValue(_selectedAgentId, out Route route))
            {
                return;
            }

            for (int i = 0; i < route.Count; i++)
            {
                _editBuffer.Add(route[i]);
            }
        }

        private bool TryCommitBuffer()
        {
            Route route = Route.Create(_editBuffer);
            RouteValidationResult result = route.Count > 0
                ? _routeValidator.Validate(_selectedAgentId, route, _boardSnapshot, _routes)
                : RouteValidationResult.Success;

            if (!result.IsValid && result.Error != ERouteValidationError.MissingTarget)
            {
                RouteRejected?.Invoke(result);
                return false;
            }

            _routes[_selectedAgentId] = route;
            RouteChanged?.Invoke(_selectedAgentId, route);
            return true;
        }
    }
}
