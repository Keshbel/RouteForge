using System;

namespace RouteForge
{
    /// <summary>
    /// Настройки детерминированной генерации уровня.
    /// </summary>
    public readonly struct LevelGenerationSettings
    {
        /// <summary>
        /// Границы создаваемой доски.
        /// </summary>
        public BoardBounds Bounds { get; }

        /// <summary>
        /// Количество агентов на уровне.
        /// </summary>
        public int AgentCount { get; }

        /// <summary>
        /// Количество заблокированных клеток.
        /// </summary>
        public int BlockedCellCount { get; }

        /// <summary>
        /// Создает настройки генерации.
        /// </summary>
        /// <param name="bounds">Границы доски.</param>
        /// <param name="agentCount">Положительное количество агентов.</param>
        /// <param name="blockedCellCount">Неотрицательное количество препятствий.</param>
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
