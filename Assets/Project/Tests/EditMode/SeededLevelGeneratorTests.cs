using NUnit.Framework;

namespace RouteForge
{
    internal sealed class SeededLevelGeneratorTests
    {
        [Test]
        public void Generate_SameSeedAndSettings_ReturnsIdenticalLevel()
        {
            // Arrange
            var generator = new SeededLevelGenerator();
            var settings = new LevelGenerationSettings(new BoardBounds(0, 0, 6, 6), agentCount: 2, blockedCellCount: 6);

            // Act
            GeneratedLevel first = generator.Generate(12345, settings);
            GeneratedLevel second = generator.Generate(12345, settings);

            // Assert
            Assert.That(HaveSameCells(first.SpawnPositions, second.SpawnPositions), Is.True);
            Assert.That(HaveSameCells(first.GoalPositions, second.GoalPositions), Is.True);
            Assert.That(HaveSameCells(first.BlockedCells, second.BlockedCells), Is.True);
        }

        [Test]
        public void Generate_DifferentSeeds_ReturnsDifferentLevelLayout()
        {
            // Arrange
            var generator = new SeededLevelGenerator();
            var settings = new LevelGenerationSettings(new BoardBounds(0, 0, 6, 6), agentCount: 2, blockedCellCount: 6);

            // Act
            GeneratedLevel first = generator.Generate(12345, settings);
            GeneratedLevel second = generator.Generate(54321, settings);

            // Assert
            Assert.That(HaveSameCells(first.BlockedCells, second.BlockedCells), Is.False);
        }

        private static bool HaveSameCells(GridPosition[] first, GridPosition[] second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
