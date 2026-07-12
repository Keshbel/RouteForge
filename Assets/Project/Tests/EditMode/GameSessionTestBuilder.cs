namespace RouteForge
{
    internal static class GameSessionTestBuilder
    {
        public static GameSession CreateSession(int agentCount)
        {
            return new GameSession(agentCount, new ScoringPolicy());
        }

        public static GameSession CreatePlanningSession(int agentCount)
        {
            GameSession session = CreateSession(agentCount);
            session.BeginPlanning();
            return session;
        }

        public static GameSession CreateRunningSession(int agentCount)
        {
            GameSession session = CreatePlanningSession(agentCount);
            session.StartRunning();
            return session;
        }
    }
}
