using System.Collections.Generic;
using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Проверяет одну конфигурацию уровня на критические ошибки.
    /// </summary>
    public static class LevelDefinitionValidator
    {
        /// <summary>
        /// Проверяет asset уровня и возвращает найденные ошибки.
        /// </summary>
        /// <param name="levelDefinition">Проверяемая конфигурация уровня.</param>
        /// <param name="assetPath">Путь asset для сообщений об ошибках.</param>
        /// <returns>Список найденных проблем конфигурации.</returns>
        public static LevelValidationIssue[] Validate(LevelDefinition levelDefinition, string assetPath)
        {
            var issues = new List<LevelValidationIssue>();
            if (levelDefinition == null)
            {
                issues.Add(LevelValidationIssue.Critical(assetPath, "LevelDefinition asset is missing."));
                return issues.ToArray();
            }

            if (levelDefinition.Width <= 0 || levelDefinition.Height <= 0)
            {
                issues.Add(LevelValidationIssue.Critical(assetPath, "Invalid board bounds: width and height must be positive."));
                return issues.ToArray();
            }

            BoardBounds bounds = levelDefinition.CreateBounds();
            ValidateDuplicateSpawns(levelDefinition, assetPath, issues);
            ValidateMissingGoals(levelDefinition, assetPath, issues);
            ValidateCellsInsideBounds(levelDefinition.SpawnPositions, bounds, assetPath, "spawn", issues);
            ValidateCellsInsideBounds(levelDefinition.GoalPositions, bounds, assetPath, "goal", issues);
            ValidateCellsInsideBounds(levelDefinition.BlockedCells, bounds, assetPath, "blocked cell", issues);
            ValidateReachability(levelDefinition, bounds, assetPath, issues);
            return issues.ToArray();
        }

        private static void ValidateDuplicateSpawns(LevelDefinition levelDefinition, string assetPath, List<LevelValidationIssue> issues)
        {
            Vector2Int[] spawns = levelDefinition.SpawnPositions;
            for (int i = 0; i < spawns.Length; i++)
            {
                for (int j = i + 1; j < spawns.Length; j++)
                {
                    if (spawns[i] == spawns[j])
                    {
                        issues.Add(LevelValidationIssue.Critical(assetPath, $"Duplicate spawn position at {spawns[i]}."));
                    }
                }
            }
        }

        private static void ValidateMissingGoals(LevelDefinition levelDefinition, string assetPath, List<LevelValidationIssue> issues)
        {
            if (levelDefinition.SpawnPositions.Length != levelDefinition.GoalPositions.Length)
            {
                issues.Add(LevelValidationIssue.Critical(assetPath, "Missing goals: every spawn must have exactly one matching goal."));
            }
        }

        private static void ValidateCellsInsideBounds(
            Vector2Int[] cells,
            BoardBounds bounds,
            string assetPath,
            string label,
            List<LevelValidationIssue> issues)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                var position = new GridPosition(cells[i].x, cells[i].y);
                if (!bounds.Contains(position))
                {
                    issues.Add(LevelValidationIssue.Critical(assetPath, $"Invalid {label} {cells[i]}: cell is outside board bounds."));
                }
            }
        }

        private static void ValidateReachability(LevelDefinition levelDefinition, BoardBounds bounds, string assetPath, List<LevelValidationIssue> issues)
        {
            int pairCount = levelDefinition.SpawnPositions.Length < levelDefinition.GoalPositions.Length
                ? levelDefinition.SpawnPositions.Length
                : levelDefinition.GoalPositions.Length;
            var blocked = new HashSet<GridPosition>();
            Vector2Int[] blockedCells = levelDefinition.BlockedCells;
            for (int i = 0; i < blockedCells.Length; i++)
            {
                blocked.Add(new GridPosition(blockedCells[i].x, blockedCells[i].y));
            }

            for (int i = 0; i < pairCount; i++)
            {
                var start = new GridPosition(levelDefinition.SpawnPositions[i].x, levelDefinition.SpawnPositions[i].y);
                var goal = new GridPosition(levelDefinition.GoalPositions[i].x, levelDefinition.GoalPositions[i].y);
                if (!IsReachable(start, goal, bounds, blocked))
                {
                    issues.Add(LevelValidationIssue.Critical(assetPath, $"Unreachable goal {levelDefinition.GoalPositions[i]} from spawn {levelDefinition.SpawnPositions[i]}."));
                }
            }
        }

        private static bool IsReachable(GridPosition start, GridPosition goal, BoardBounds bounds, HashSet<GridPosition> blocked)
        {
            if (!bounds.Contains(start) || !bounds.Contains(goal) || blocked.Contains(start) || blocked.Contains(goal))
            {
                return false;
            }

            var visited = new HashSet<GridPosition>();
            var queue = new Queue<GridPosition>();
            visited.Add(start);
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                GridPosition current = queue.Dequeue();
                if (current == goal)
                {
                    return true;
                }

                EnqueueIfValid(new GridPosition(current.X + 1, current.Y), bounds, blocked, visited, queue);
                EnqueueIfValid(new GridPosition(current.X - 1, current.Y), bounds, blocked, visited, queue);
                EnqueueIfValid(new GridPosition(current.X, current.Y + 1), bounds, blocked, visited, queue);
                EnqueueIfValid(new GridPosition(current.X, current.Y - 1), bounds, blocked, visited, queue);
            }

            return false;
        }

        private static void EnqueueIfValid(
            GridPosition position,
            BoardBounds bounds,
            HashSet<GridPosition> blocked,
            HashSet<GridPosition> visited,
            Queue<GridPosition> queue)
        {
            if (!bounds.Contains(position) || blocked.Contains(position) || !visited.Add(position))
            {
                return;
            }

            queue.Enqueue(position);
        }
    }
}
