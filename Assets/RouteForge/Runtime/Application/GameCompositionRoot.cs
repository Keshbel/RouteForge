namespace RouteForge
{
    /// <summary>
    /// Создает и хранит доменные и application-зависимости игровой сцены.
    /// </summary>
    public sealed class GameCompositionRoot
    {
        /// <summary>
        /// Доменный валидатор маршрутов.
        /// </summary>
        public RouteValidator RouteValidator { get; }

        /// <summary>
        /// Политика подсчета очков.
        /// </summary>
        public ScoringPolicy ScoringPolicy { get; }

        /// <summary>
        /// Явное состояние текущей игровой сессии.
        /// </summary>
        public GameSession Session { get; }

        /// <summary>
        /// Создает набор зависимостей для игровой сцены.
        /// </summary>
        /// <param name="agentCount">Количество агентов в текущей сцене.</param>
        public GameCompositionRoot(int agentCount)
        {
            RouteValidator = new RouteValidator();
            ScoringPolicy = new ScoringPolicy();
            Session = new GameSession(agentCount, ScoringPolicy);
        }
    }
}
