using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class SeededLevelGenerator
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="seed">The seed value.</param>
        /// <param name="settings">The settings value.</param>
        /// <returns>The operation result.</returns>
        public GeneratedLevel Generate(int seed, LevelGenerationSettings settings)
        {
            using (RouteForgeProfilerMarker.LevelGeneration.Auto())
            {
                int cellCount = settings.Bounds.Width * settings.Bounds.Height;
                var cells = new GridPosition[cellCount];
                int index = 0;
                for (int y = settings.Bounds.MinY; y <= settings.Bounds.MaxY; y++)
                {
                    for (int x = settings.Bounds.MinX; x <= settings.Bounds.MaxX; x++)
                    {
                        cells[index] = new GridPosition(x, y);
                        index++;
                    }
                }

                Shuffle(cells, seed);

                var spawns = new GridPosition[settings.AgentCount];
                var goals = new GridPosition[settings.AgentCount];
                var blocked = new GridPosition[settings.BlockedCellCount];
                index = 0;

                for (int i = 0; i < settings.AgentCount; i++)
                {
                    spawns[i] = cells[index];
                    index++;
                }

                for (int i = 0; i < settings.AgentCount; i++)
                {
                    goals[i] = cells[index];
                    index++;
                }

                for (int i = 0; i < settings.BlockedCellCount; i++)
                {
                    blocked[i] = cells[index];
                    index++;
                }

                return new GeneratedLevel(settings.Bounds, spawns, goals, blocked);
            }
        }

        private static void Shuffle(GridPosition[] cells, int seed)
        {
            var random = new DeterministicRandom(seed);
            for (int i = cells.Length - 1; i > 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                GridPosition temp = cells[i];
                cells[i] = cells[swapIndex];
                cells[swapIndex] = temp;
            }
        }

        private struct DeterministicRandom
        {
            private uint _state;

            public DeterministicRandom(int seed)
            {
                _state = seed == 0 ? 0x6D2B79F5u : unchecked((uint)seed);
            }

            public int Next(int exclusiveMax)
            {
                if (exclusiveMax <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(exclusiveMax), exclusiveMax, "Max value must be positive.");
                }

                _state = unchecked((_state * 1664525u) + 1013904223u);
                return (int)(_state % (uint)exclusiveMax);
            }
        }
    }
}
