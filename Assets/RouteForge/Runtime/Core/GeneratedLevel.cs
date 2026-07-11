namespace RouteForge
{
    /// <summary>
    /// Детерминированно созданные данные уровня.
    /// </summary>
    public sealed class GeneratedLevel
    {
        /// <summary>
        /// Границы созданной доски.
        /// </summary>
        public BoardBounds Bounds { get; }

        /// <summary>
        /// Стартовые клетки агентов.
        /// </summary>
        public GridPosition[] SpawnPositions { get; }

        /// <summary>
        /// Целевые клетки агентов.
        /// </summary>
        public GridPosition[] GoalPositions { get; }

        /// <summary>
        /// Заблокированные клетки уровня.
        /// </summary>
        public GridPosition[] BlockedCells { get; }

        /// <summary>
        /// Создает данные уровня.
        /// </summary>
        /// <param name="bounds">Границы доски.</param>
        /// <param name="spawnPositions">Стартовые клетки агентов.</param>
        /// <param name="goalPositions">Целевые клетки агентов.</param>
        /// <param name="blockedCells">Заблокированные клетки.</param>
        public GeneratedLevel(
            BoardBounds bounds,
            GridPosition[] spawnPositions,
            GridPosition[] goalPositions,
            GridPosition[] blockedCells)
        {
            Bounds = bounds;
            SpawnPositions = Copy(spawnPositions);
            GoalPositions = Copy(goalPositions);
            BlockedCells = Copy(blockedCells);
        }

        private static GridPosition[] Copy(GridPosition[] source)
        {
            if (source == null || source.Length == 0)
            {
                return new GridPosition[0];
            }

            var copy = new GridPosition[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                copy[i] = source[i];
            }

            return copy;
        }
    }
}
