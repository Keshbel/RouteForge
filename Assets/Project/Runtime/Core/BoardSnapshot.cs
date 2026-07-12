using System;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class BoardSnapshot
    {
        private readonly HashSet<GridPosition> _blockedCells;
        private readonly Dictionary<AgentId, GridPosition> _agentStarts;
        private readonly Dictionary<AgentId, GridPosition> _agentGoals;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public BoardBounds Bounds { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="bounds">The bounds value.</param>
        /// <param name="blockedCells">The blockedCells value.</param>
        /// <param name="agentStarts">The agentStarts value.</param>
        /// <param name="agentGoals">The agentGoals value.</param>
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
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool IsWalkable(GridPosition position)
        {
            return Bounds.Contains(position) && !_blockedCells.Contains(position);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool IsBlocked(GridPosition position)
        {
            return _blockedCells.Contains(position);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool TryGetStart(AgentId agentId, out GridPosition position)
        {
            return _agentStarts.TryGetValue(agentId, out position);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool TryGetGoal(AgentId agentId, out GridPosition position)
        {
            return _agentGoals.TryGetValue(agentId, out position);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool IsGoalForAgent(AgentId agentId, GridPosition position)
        {
            return _agentGoals.TryGetValue(agentId, out GridPosition goal) && goal == position;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool IsGoalForOtherAgent(AgentId agentId, GridPosition position)
        {
            foreach (KeyValuePair<AgentId, GridPosition> pair in _agentGoals)
            {
                if (pair.Key != agentId && pair.Value == position)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
