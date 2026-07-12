using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct LevelGenerationSettings
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public BoardBounds Bounds { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int AgentCount { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int BlockedCellCount { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="bounds">The bounds value.</param>
        /// <param name="agentCount">The agentCount value.</param>
        /// <param name="blockedCellCount">The blockedCellCount value.</param>
        public LevelGenerationSettings(BoardBounds bounds, int agentCount, int blockedCellCount)
        {
            if (agentCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(agentCount), agentCount, "Agent count must be positive.");
            }

            if (blockedCellCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(blockedCellCount), blockedCellCount, "Blocked cell count must be non-negative.");
            }

            int cellCount = bounds.Width * bounds.Height;
            int reservedCells = agentCount * 2;
            if (reservedCells + blockedCellCount > cellCount)
            {
                throw new ArgumentOutOfRangeException(nameof(blockedCellCount), blockedCellCount, "Level does not have enough cells for requested content.");
            }

            Bounds = bounds;
            AgentCount = agentCount;
            BlockedCellCount = blockedCellCount;
        }
    }
}
