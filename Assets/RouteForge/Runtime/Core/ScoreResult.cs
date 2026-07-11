namespace RouteForge
{
    /// <summary>
    /// Итоговый результат начисления очков за завершенную сессию.
    /// </summary>
    public readonly struct ScoreResult
    {
        /// <summary>
        /// Количество успешно завершивших маршрут агентов.
        /// </summary>
        public int CompletedAgents { get; }

        /// <summary>
        /// Итоговое количество очков.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Текст результата, соответствующий набранным очкам.
        /// </summary>
        public string ResultText { get; }

        /// <summary>
        /// Создает результат начисления очков.
        /// </summary>
        /// <param name="completedAgents">Количество агентов, достигших цели.</param>
        /// <param name="score">Итоговое количество очков.</param>
        /// <param name="resultText">Текст результата для показа игроку.</param>
        public ScoreResult(int completedAgents, int score, string resultText)
        {
            CompletedAgents = completedAgents;
            Score = score;
            ResultText = resultText;
        }
    }
}
