namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct ScoreResult
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int CompletedAgents { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public string ResultText { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="completedAgents">The completedAgents value.</param>
        /// <param name="score">The score value.</param>
        /// <param name="resultText">The resultText value.</param>
        public ScoreResult(int completedAgents, int score, string resultText)
        {
            CompletedAgents = completedAgents;
            Score = score;
            ResultText = resultText;
        }
    }
}
