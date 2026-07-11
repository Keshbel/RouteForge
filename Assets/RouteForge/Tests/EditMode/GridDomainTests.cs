using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RouteForge.Tests
{
    internal sealed class GridDomainTests
    {
        [Test]
        public void GridPosition_AdjacentCells_ReturnsTrueOnlyForOrthogonalNeighbor()
        {
            var origin = new GridPosition(2, 3);

            Assert.That(origin.IsAdjacentTo(new GridPosition(3, 3)), Is.True);
            Assert.That(origin.IsAdjacentTo(new GridPosition(2, 4)), Is.True);
            Assert.That(origin.IsAdjacentTo(new GridPosition(3, 4)), Is.False);
            Assert.That(origin.IsAdjacentTo(origin), Is.False);
        }

        [Test]
        public void AgentId_NegativeValue_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AgentId(-1));
        }

        [Test]
        public void BoardSnapshot_WalkableCell_RequiresBoundsAndNoBlock()
        {
            var agentId = new AgentId(0);
            var starts = new Dictionary<AgentId, GridPosition>
            {
                { agentId, new GridPosition(0, 0) }
            };
            var goals = new Dictionary<AgentId, GridPosition>
            {
                { agentId, new GridPosition(2, 2) }
            };
            var snapshot = new BoardSnapshot(
                new BoardBounds(0, 0, 3, 3),
                new[] { new GridPosition(1, 1) },
                starts,
                goals);

            Assert.That(snapshot.IsWalkable(new GridPosition(0, 1)), Is.True);
            Assert.That(snapshot.IsWalkable(new GridPosition(1, 1)), Is.False);
            Assert.That(snapshot.IsWalkable(new GridPosition(3, 1)), Is.False);
            Assert.That(snapshot.IsGoalForAgent(agentId, new GridPosition(2, 2)), Is.True);
        }
    }
}
