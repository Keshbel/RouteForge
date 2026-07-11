using System;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Управляет жизненным циклом игровой сессии и всеми переходами состояния.
    /// </summary>
    public sealed class GameSession
    {
        private readonly int _agentCount;
        private readonly ScoringPolicy _scoringPolicy;
        private readonly HashSet<AgentId> _finishedAgents = new HashSet<AgentId>();
        private int _completedAgents;

        /// <summary>
        /// Вызывается после изменения состояния сессии.
        /// </summary>
        public event Action<ESessionState> StateChanged;

        /// <summary>
        /// Вызывается при финальном завершении сессии.
        /// </summary>
        public event Action<ScoreResult> SessionCompleted;

        /// <summary>
        /// Текущее состояние сессии.
        /// </summary>
        public ESessionState State { get; private set; }

        /// <summary>
        /// Создает игровую сессию.
        /// </summary>
        /// <param name="agentCount">Количество агентов, участвующих в сессии.</param>
        /// <param name="scoringPolicy">Политика подсчета очков после завершения.</param>
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
        /// Переводит сессию из загрузки в планирование.
        /// </summary>
        /// <returns>Возвращает true, если переход был выполнен.</returns>
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
        /// Запускает выполнение маршрутов из состояния планирования.
        /// </summary>
        /// <returns>Возвращает true, если переход был выполнен.</returns>
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
        /// Ставит выполнение маршрутов на паузу.
        /// </summary>
        /// <returns>Возвращает true, если переход был выполнен.</returns>
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
        /// Возобновляет выполнение маршрутов после паузы.
        /// </summary>
        /// <returns>Возвращает true, если переход был выполнен.</returns>
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
        /// Регистрирует завершение движения агента.
        /// </summary>
        /// <param name="agentId">Идентификатор завершившего агента.</param>
        /// <param name="reachedGoal">Признак достижения целевой клетки.</param>
        /// <returns>Возвращает true, если завершение было принято.</returns>
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
        /// Завершает сессию вручную с текущим количеством успешных агентов.
        /// </summary>
        /// <returns>Возвращает true, если переход был выполнен.</returns>
        public bool CompleteSession()
        {
            if (State == ESessionState.Completed)
            {
                return false;
            }

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
