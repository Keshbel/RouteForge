using System;

namespace RouteForge
{
    /// <summary>
    /// Детерминированно рассчитывает очки и текст результата завершенной сессии.
    /// </summary>
    public sealed class ScoringPolicy
    {
        private readonly int _pointsPerCompletedAgent;

        /// <summary>
        /// Создает политику подсчета очков.
        /// </summary>
        /// <param name="pointsPerCompletedAgent">Количество очков за агента, достигшего цели.</param>
        public ScoringPolicy(int pointsPerCompletedAgent = 50)
        {
            if (pointsPerCompletedAgent < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pointsPerCompletedAgent), pointsPerCompletedAgent, "Points must be non-negative.");
            }

            _pointsPerCompletedAgent = pointsPerCompletedAgent;
        }

        /// <summary>
        /// Рассчитывает результат сессии по количеству успешных агентов.
        /// </summary>
        /// <param name="completedAgents">Количество агентов, достигших цели.</param>
        /// <returns>Итоговый счет и текст результата.</returns>
        public ScoreResult Calculate(int completedAgents)
        {
            if (completedAgents < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(completedAgents), completedAgents, "Completed agents must be non-negative.");
            }

            int score = completedAgents * _pointsPerCompletedAgent;
            string resultText;
            switch (score)
            {
                case 0:
                    resultText = "Try again...";
                    break;
                case 50:
                    resultText = "You almost won...";
                    break;
                default:
                    resultText = "You won!";
                    break;
            }

            return new ScoreResult(completedAgents, score, resultText);
        }
    }
}
