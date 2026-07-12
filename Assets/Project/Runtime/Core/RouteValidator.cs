using System;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class RouteValidator
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        /// <param name="route">The route value.</param>
        /// <param name="board">The board value.</param>
        /// <param name="committedRoutes">The committedRoutes value.</param>
        /// <returns>The operation result.</returns>
        public RouteValidationResult Validate(
            AgentId agentId,
            Route route,
            BoardSnapshot board,
            IReadOnlyDictionary<AgentId, Route> committedRoutes)
        {
            using (RouteForgeProfilerMarker.RouteValidation.Auto())
            {
                if (route == null)
                {
                    return RouteValidationResult.Failure(ERouteValidationError.MissingRoute, null, "Route is not provided.");
                }

                if (board == null)
                {
                    throw new ArgumentNullException(nameof(board));
                }

                if (route.Count == 0)
                {
                    return RouteValidationResult.Failure(ERouteValidationError.EmptyRoute, null, "Route contains no cells.");
                }

                if (!board.TryGetStart(agentId, out GridPosition start))
                {
                    return RouteValidationResult.Failure(ERouteValidationError.MissingStart, null, "Agent start is not configured.");
                }

                if (!board.TryGetGoal(agentId, out GridPosition goal))
                {
                    return RouteValidationResult.Failure(ERouteValidationError.MissingGoal, null, "Agent goal is not configured.");
                }

                GridPosition firstCell = route[0];
                if (firstCell != start)
                {
                    return RouteValidationResult.Failure(ERouteValidationError.InvalidStart, firstCell, "Route does not start at the agent.");
                }

                var visited = new HashSet<GridPosition>();
                bool containsGoal = false;
                for (int i = 0; i < route.Count; i++)
                {
                    GridPosition cell = route[i];
                    if (!board.Bounds.Contains(cell))
                    {
                        return RouteValidationResult.Failure(ERouteValidationError.OutOfBounds, cell, "Route contains a cell outside board bounds.");
                    }

                    if (board.IsBlocked(cell))
                    {
                        return RouteValidationResult.Failure(ERouteValidationError.BlockedCell, cell, "Route contains a blocked cell.");
                    }

                    if (i > 0 && !route[i - 1].IsAdjacentTo(cell))
                    {
                        return RouteValidationResult.Failure(ERouteValidationError.NonAdjacentSegment, cell, "Route contains non-adjacent cells.");
                    }

                    if (!visited.Add(cell))
                    {
                        return RouteValidationResult.Failure(ERouteValidationError.Cycle, cell, "Route visits the same cell twice.");
                    }

                    if (board.IsGoalForOtherAgent(agentId, cell))
                    {
                        return RouteValidationResult.Failure(ERouteValidationError.ForeignGoal, cell, "Route contains another agent goal.");
                    }

                    if (cell == goal)
                    {
                        containsGoal = true;
                    }
                }

                if (!containsGoal)
                {
                    return RouteValidationResult.Failure(ERouteValidationError.MissingTarget, goal, "Route does not include the agent goal.");
                }

                RouteValidationResult conflictResult = ValidateConflicts(agentId, route, committedRoutes);
                if (!conflictResult.IsValid)
                {
                    return conflictResult;
                }

                return RouteValidationResult.Success;
            }
        }

        private static RouteValidationResult ValidateConflicts(
            AgentId agentId,
            Route route,
            IReadOnlyDictionary<AgentId, Route> committedRoutes)
        {
            if (committedRoutes == null || committedRoutes.Count == 0)
            {
                return RouteValidationResult.Success;
            }

            foreach (KeyValuePair<AgentId, Route> pair in committedRoutes)
            {
                if (pair.Key == agentId || pair.Value == null)
                {
                    continue;
                }

                Route otherRoute = pair.Value;
                for (int i = 0; i < route.Count; i++)
                {
                    GridPosition cell = route[i];
                    if (otherRoute.Contains(cell))
                    {
                        return RouteValidationResult.Failure(ERouteValidationError.RouteConflict, cell, "Route conflicts with another agent route.");
                    }
                }
            }

            return RouteValidationResult.Success;
        }
    }
}
