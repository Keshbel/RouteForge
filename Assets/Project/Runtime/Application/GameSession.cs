using System;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class GameSession
    {
        private readonly int _agentCount;
        private readonly ScoringPolicy _scoringPolicy;
        private readonly HashSet<AgentId> _finishedAgents = new HashSet<AgentId>();
        private int _completedAgents;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public event Action<ESessionState> StateChanged;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public event Action<ScoreResult> SessionCompleted;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public ESessionState State { get; private set; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentCount">The agentCount value.</param>
        /// <param name="scoringPolicy">The scoringPolicy value.</param>
        public GameSession(int agentCount, ScoringPolicy scoringPolicy)
        {
            if (agentCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(agentCount), agentCount, "Agent count must be positive.");
            }

            _agentCount = agentCount;
            _scoringPolicy = scoringPolicy ?? throw new ArgumentNullException(nameof(scoringPolicy));
            State = ESessionState.Booting;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public bool BeginPlanning()
        {
            if (State != ESessionState.Booting)
            {
                return false;
            }

            SetState(ESessionState.Planning);
            return true;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public bool StartRunning()
        {
            if (State != ESessionState.Planning)
            {
                return false;
            }

            SetState(ESessionState.Running);
            return true;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public bool Pause()
        {
            if (State != ESessionState.Running)
            {
                return false;
            }

            SetState(ESessionState.Paused);
            return true;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public bool Resume()
        {
            if (State != ESessionState.Paused)
            {
                return false;
            }

            SetState(ESessionState.Running);
            return true;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        /// <param name="reachedGoal">The reachedGoal value.</param>
        /// <returns>The operation result.</returns>
        public bool CompleteAgent(AgentId agentId, bool reachedGoal)
        {
            if (State != ESessionState.Running || !_finishedAgents.Add(agentId))
            {
                return false;
            }

            if (reachedGoal)
            {
                _completedAgents++;
            }

            if (_finishedAgents.Count >= _agentCount)
            {
                CompleteSession();
            }

            return true;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public bool CompleteSession()
        {
            return CompleteSession(_completedAgents);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="completedAgents">The completedAgents value.</param>
        /// <returns>The operation result.</returns>
        public bool CompleteSession(int completedAgents)
        {
            if (State == ESessionState.Completed)
            {
                return false;
            }

            if (completedAgents < 0 || completedAgents > _agentCount)
            {
                throw new ArgumentOutOfRangeException(nameof(completedAgents), completedAgents, "Completed agents must fit session agent count.");
            }

            _completedAgents = completedAgents;
            SetState(ESessionState.Completed);
            SessionCompleted?.Invoke(_scoringPolicy.Calculate(_completedAgents));
            return true;
        }

        private void SetState(ESessionState nextState)
        {
            if (State == nextState)
            {
                return;
            }

            State = nextState;
            StateChanged?.Invoke(nextState);
        }
    }
}
