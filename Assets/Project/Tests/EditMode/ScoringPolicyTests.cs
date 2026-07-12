using NUnit.Framework;

namespace RouteForge
{
    internal sealed class ScoringPolicyTests
    {
        [Test]
        public void Calculate_PerfectRoute_ReturnsPerfectRouteBonus()
        {
            // Arrange
            var scoringPolicy = new ScoringPolicy(pointsPerCompletedAgent: 50, perfectRouteBonus: 25, unnecessarySegmentPenalty: 5);
            var routes = new[]
            {
                new RouteScoreInput(reachedGoal: true, segmentCount: 4, optimalSegmentCount: 4)
            };

            // Act
            ScoreResult result = scoringPolicy.Calculate(routes);

            // Assert
            Assert.That(result.Score, Is.EqualTo(75));
            Assert.That(result.CompletedAgents, Is.EqualTo(1));
        }

        [Test]
        public void Calculate_RouteWithUnnecessarySegments_ReturnsPenalty()
        {
            // Arrange
            var scoringPolicy = new ScoringPolicy(pointsPerCompletedAgent: 50, perfectRouteBonus: 25, unnecessarySegmentPenalty: 5);
            var routes = new[]
            {
                new RouteScoreInput(reachedGoal: true, segmentCount: 7, optimalSegmentCount: 4)
            };

            // Act
            ScoreResult result = scoringPolicy.Calculate(routes);

            // Assert
            Assert.That(result.Score, Is.EqualTo(35));
            Assert.That(result.CompletedAgents, Is.EqualTo(1));
        }
    }
}
