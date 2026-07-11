using System.Collections.Generic;
using NUnit.Framework;

namespace RouteForge
{
    internal sealed class GameSessionTests
    {
        [Test]
        public void BeginPlanning_FromBooting_ChangesState()
        {
            var session = new GameSession(2, new ScoringPolicy());

            bool changed = session.BeginPlanning();

            Assert.That(changed, Is.True);
            Assert.That(session.State, Is.EqualTo(ESessionState.Planning));
        }

        [Test]
        public void StartRunning_FromBooting_ReturnsFalse()
        {
            var session = new GameSession(2, new ScoringPolicy());

            bool changed = session.StartRunning();

            Assert.That(changed, Is.False);
            Assert.That(session.State, Is.EqualTo(ESessionState.Booting));
        }

        [Test]
        public void PauseAndResume_FromRunning_ChangesState()
        {
            var session = new GameSession(2, new ScoringPolicy());
            session.BeginPlanning();
            session.StartRunning();

            bool paused = session.Pause();
            bool resumed = session.Resume();

            Assert.That(paused, Is.True);
            Assert.That(resumed, Is.True);
            Assert.That(session.State, Is.EqualTo(ESessionState.Running));
        }

        [Test]
        public void CompleteAgent_DuplicateAgent_IsIgnored()
        {
            var session = new GameSession(2, new ScoringPolicy());
            session.BeginPlanning();
            session.StartRunning();

            bool first = session.CompleteAgent(new AgentId(0), true);
            bool duplicate = session.CompleteAgent(new AgentId(0), true);

            Assert.That(first, Is.True);
            Assert.That(duplicate, Is.False);
            Assert.That(session.State, Is.EqualTo(ESessionState.Running));
        }

        [Test]
        public void CompleteAgent_AllAgentsFinished_CompletesWithScore()
        {
            var session = new GameSession(2, new ScoringPolicy());
            var states = new List<ESessionState>();
            ScoreResult scoreResult = default(ScoreResult);
            session.StateChanged += states.Add;
            session.SessionCompleted += result => scoreResult = result;
            session.BeginPlanning();
            session.StartRunning();

            session.CompleteAgent(new AgentId(0), true);
            session.CompleteAgent(new AgentId(1), false);

            Assert.That(session.State, Is.EqualTo(ESessionState.Completed));
            Assert.That(scoreResult.CompletedAgents, Is.EqualTo(1));
            Assert.That(scoreResult.Score, Is.EqualTo(50));
            Assert.That(scoreResult.ResultText, Is.EqualTo("You almost won..."));
            Assert.That(states, Does.Contain(ESessionState.Completed));
        }
    }
}
