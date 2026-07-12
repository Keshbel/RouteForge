namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class GameCompositionRoot
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public RouteValidator RouteValidator { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public ScoringPolicy ScoringPolicy { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public GameSession Session { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentCount">The agentCount value.</param>
        public GameCompositionRoot(int agentCount)
        {
            RouteValidator = new RouteValidator();
            ScoringPolicy = new ScoringPolicy();
            Session = new GameSession(agentCount, ScoringPolicy);
        }
    }
}
