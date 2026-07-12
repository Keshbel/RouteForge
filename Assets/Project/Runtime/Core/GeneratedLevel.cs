namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class GeneratedLevel
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public BoardBounds Bounds { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public GridPosition[] SpawnPositions { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public GridPosition[] GoalPositions { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public GridPosition[] BlockedCells { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="bounds">The bounds value.</param>
        /// <param name="spawnPositions">The spawnPositions value.</param>
        /// <param name="goalPositions">The goalPositions value.</param>
        /// <param name="blockedCells">The blockedCells value.</param>
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
