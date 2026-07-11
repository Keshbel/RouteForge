using System;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Неизменяемый снимок топологии доски, стартов агентов и целевых клеток.
    /// </summary>
    public sealed class BoardSnapshot
    {
        private readonly HashSet<GridPosition> _blockedCells;
        private readonly Dictionary<AgentId, GridPosition> _agentStarts;
        private readonly Dictionary<AgentId, GridPosition> _agentGoals;

        /// <summary>
        /// Границы доски, в которых разрешены маршруты.
        /// </summary>
        public BoardBounds Bounds { get; }

        /// <summary>
        /// Создает снимок доски для доменных проверок.
        /// </summary>
        /// <param name="bounds">Границы доступной области доски.</param>
        /// <param name="blockedCells">Клетки, недоступные для маршрута.</param>
        /// <param name="agentStarts">Стартовая клетка каждого агента.</param>
        /// <param name="agentGoals">Целевая клетка каждого агента.</param>
        public BoardSnapshot(
            BoardBounds bounds,
            IEnumerable<GridPosition> blockedCells,
            IReadOnlyDictionary<AgentId, GridPosition> agentStarts,
            IReadOnlyDictionary<AgentId, GridPosition> agentGoals)
        {
            Bounds = bounds;
            _blockedCells = blockedCells != null
                ? new HashSet<GridPosition>(blockedCells)
                : new HashSet<GridPosition>();
            _agentStarts = agentStarts != null
                ? new Dictionary<AgentId, GridPosition>(agentStarts)
                : throw new ArgumentNullException(nameof(agentStarts));
            _agentGoals = agentGoals != null
                ? new Dictionary<AgentId, GridPosition>(agentGoals)
                : throw new ArgumentNullException(nameof(agentGoals));
        }

        /// <summary>
        /// Проверяет, доступна ли клетка для построения маршрута.
        /// </summary>
        /// <param name="position">Проверяемая клетка.</param>
        /// <returns>Возвращает true, если клетка внутри доски и не заблокирована.</returns>
        public bool IsWalkable(GridPosition position)
        {
            return Bounds.Contains(position) && !_blockedCells.Contains(position);
        }

        /// <summary>
        /// Проверяет, является ли клетка заблокированной.
        /// </summary>
        /// <param name="position">Проверяемая клетка.</param>
        /// <returns>Возвращает true, если клетка помечена как препятствие.</returns>
        public bool IsBlocked(GridPosition position)
        {
            return _blockedCells.Contains(position);
        }

        /// <summary>
        /// Возвращает стартовую клетку агента.
        /// </summary>
        /// <param name="agentId">Идентификатор агента.</param>
        /// <param name="position">Найденная стартовая клетка.</param>
        /// <returns>Возвращает true, если старт агента задан.</returns>
        public bool TryGetStart(AgentId agentId, out GridPosition position)
        {
            return _agentStarts.TryGetValue(agentId, out position);
        }

        /// <summary>
        /// Возвращает целевую клетку агента.
        /// </summary>
        /// <param name="agentId">Идентификатор агента.</param>
        /// <param name="position">Найденная целевая клетка.</param>
        /// <returns>Возвращает true, если цель агента задана.</returns>
        public bool TryGetGoal(AgentId agentId, out GridPosition position)
        {
            return _agentGoals.TryGetValue(agentId, out position);
        }

        /// <summary>
        /// Проверяет, является ли клетка целью агента.
        /// </summary>
        /// <param name="agentId">Идентификатор агента.</param>
        /// <param name="position">Проверяемая клетка.</param>
        /// <returns>Возвращает true, если клетка совпадает с целью агента.</returns>
        public bool IsGoalForAgent(AgentId agentId, GridPosition position)
        {
            return _agentGoals.TryGetValue(agentId, out GridPosition goal) && goal == position;
        }
    }
}
