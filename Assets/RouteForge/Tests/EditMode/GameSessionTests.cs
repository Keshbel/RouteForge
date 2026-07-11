using System.Collections.Generic;
using NUnit.Framework;

namespace RouteForge
{
    internal sealed class GameSessionTests
    {
        [Test]
        public void StartRunning_FromBooting_ReturnsFalseAndKeepsBooting()
        {
            // Arrange
            GameSession session = GameSessionTestBuilder.CreateSession(agentCount: 2);

            // Act
            bool changed = session.StartRunning();

            // Assert
            Assert.That(changed, Is.False);
            Assert.That(session.State, Is.EqualTo(ESessionState.Booting));
        }

        [Test]
        public void PauseAndResume_FromRunning_ReturnsToRunning()
        {
            // Arrange
            GameSession session = GameSessionTestBuilder.CreatePlanningSession(agentCount: 2);
            session.StartRunning();

            // Act
            bool paused = session.Pause();
            bool resumed = session.Resume();

            // Assert
            Assert.That(paused, Is.True);
            Assert.That(resumed, Is.True);
            Assert.That(session.State, Is.EqualTo(ESessionState.Running));
        }

        [Test]
        public void CompleteAgent_OnlyOneOfTwoAgentsFinished_KeepsRunning()
        {
            // Arrange
            GameSession session = GameSessionTestBuilder.CreateRunningSession(agentCount: 2);

            // Act
            bool accepted = session.CompleteAgent(new AgentId(0), reachedGoal: true);

            // Assert
            Assert.That(accepted, Is.True);
            Assert.That(session.State, Is.EqualTo(ESessionState.Running));
        }

        [Test]
        public void CompleteAgent_EveryActiveAgentFinished_CompletesOnce()
        {
            // Arrange
            GameSession session = GameSessionTestBuilder.CreateRunningSession(agentCount: 2);
            int completedEvents = 0;
            ScoreResult scoreResult = default(ScoreResult);
            session.SessionCompleted += result =>
            {
                completedEvents++;
                scoreResult = result;
            };

            // Act
            bool firstAccepted = session.CompleteAgent(new AgentId(0), reachedGoal: true);
            bool secondAccepted = session.CompleteAgent(new AgentId(1), reachedGoal: false);
            bool duplicateAccepted = session.CompleteAgent(new AgentId(1), reachedGoal: true);

            // Assert
            Assert.That(firstAccepted, Is.True);
            Assert.That(secondAccepted, Is.True);
            Assert.That(duplicateAccepted, Is.False);
            Assert.That(session.State, Is.EqualTo(ESessionState.Completed));
            Assert.That(completedEvents, Is.EqualTo(1));
            Assert.That(scoreResult.CompletedAgents, Is.EqualTo(1));
        }

        [Test]
        public void StateChanged_PlanningRunningPausedRunning_ReportsOrderedTransitions()
        {
            // Arrange
            GameSession session = GameSessionTestBuilder.CreateSession(agentCount: 1);
            var states = new List<ESessionState>();
            session.StateChanged += states.Add;

            // Act
            session.BeginPlanning();
            session.StartRunning();
            session.Pause();
            session.Resume();

            // Assert
            Assert.That(states, Is.EqualTo(new[]
            {
                ESessionState.Planning,
                ESessionState.Running,
                ESessionState.Paused,
                ESessionState.Running
            }));
        }
    }
}
