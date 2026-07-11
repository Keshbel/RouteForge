using System;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Управляет редактированием маршрутов выбранных агентов без зависимости от Unity presentation.
    /// </summary>
    public sealed class RouteEditingService
    {
        private readonly RouteValidator _routeValidator;
        private readonly Dictionary<AgentId, Route> _routes = new Dictionary<AgentId, Route>();
        private readonly List<GridPosition> _editBuffer = new List<GridPosition>();

        private BoardSnapshot _boardSnapshot;
        private AgentId _selectedAgentId;

        /// <summary>
        /// Вызывается после успешного изменения маршрута агента.
        /// </summary>
        public event Action<AgentId, Route> RouteChanged;

        /// <summary>
        /// Вызывается после очистки всех маршрутов.
        /// </summary>
        public event Action RoutesCleared;

        /// <summary>
        /// Вызывается, когда редактирование приводит к невалидному маршруту.
        /// </summary>
        public event Action<RouteValidationResult> RouteRejected;

        /// <summary>
        /// Создает сервис редактирования маршрутов.
        /// </summary>
        /// <param name="routeValidator">Валидатор доменных правил маршрута.</param>
        /// <param name="initialBoardSnapshot">Начальный снимок доски.</param>
        /// <param name="initialAgentId">Изначально выбранный агент.</param>
        public RouteEditingService(RouteValidator routeValidator, BoardSnapshot initialBoardSnapshot, AgentId initialAgentId)
        {
            _routeValidator = routeValidator ?? throw new ArgumentNullException(nameof(routeValidator));
            _boardSnapshot = initialBoardSnapshot ?? throw new ArgumentNullException(nameof(initialBoardSnapshot));
            _selectedAgentId = initialAgentId;
        }

        /// <summary>
        /// Обновляет снимок доски, с которым сверяются маршруты.
        /// </summary>
        /// <param name="boardSnapshot">Новый снимок доски.</param>
        public void SetBoardSnapshot(BoardSnapshot boardSnapshot)
        {
            _boardSnapshot = boardSnapshot ?? throw new ArgumentNullException(nameof(boardSnapshot));
        }

        /// <summary>
        /// Выбирает агента для последующего редактирования маршрута.
        /// </summary>
        /// <param name="agentId">Идентификатор выбранного агента.</param>
        public void SelectAgent(AgentId agentId)
        {
            _selectedAgentId = agentId;
        }

        /// <summary>
        /// Возвращает маршрут агента, если он уже был создан.
        /// </summary>
        /// <param name="agentId">Идентификатор агента.</param>
        /// <param name="route">Найденный маршрут.</param>
        /// <returns>Возвращает true, если маршрут существует.</returns>
        public bool TryGetRoute(AgentId agentId, out Route route)
        {
            return _routes.TryGetValue(agentId, out route);
        }

        /// <summary>
        /// Добавляет клетку в конец маршрута выбранного агента.
        /// </summary>
        /// <param name="position">Добавляемая клетка.</param>
        /// <returns>Возвращает true, если маршрут был принят.</returns>
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
        /// Удаляет клетку из маршрута выбранного агента.
        /// </summary>
        /// <param name="position">Удаляемая клетка.</param>
        /// <returns>Возвращает true, если маршрут был обновлен.</returns>
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
        /// Удаляет все построенные маршруты.
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
