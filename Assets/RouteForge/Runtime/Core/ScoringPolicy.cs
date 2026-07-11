using System;

namespace RouteForge
{
    /// <summary>
    /// Детерминированно рассчитывает очки и текст результата завершенной сессии.
    /// </summary>
    public sealed class ScoringPolicy
    {
        private readonly int _pointsPerCompletedAgent;
        private readonly int _perfectRouteBonus;
        private readonly int _unnecessarySegmentPenalty;

        /// <summary>
        /// Создает политику подсчета очков.
        /// </summary>
        /// <param name="pointsPerCompletedAgent">Количество очков за агента, достигшего цели.</param>
        public ScoringPolicy(int pointsPerCompletedAgent = 50, int perfectRouteBonus = 25, int unnecessarySegmentPenalty = 5)
        {
            if (pointsPerCompletedAgent < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pointsPerCompletedAgent), pointsPerCompletedAgent, "Points must be non-negative.");
            }

            if (perfectRouteBonus < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(perfectRouteBonus), perfectRouteBonus, "Bonus must be non-negative.");
            }

            if (unnecessarySegmentPenalty < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(unnecessarySegmentPenalty), unnecessarySegmentPenalty, "Penalty must be non-negative.");
            }

            _pointsPerCompletedAgent = pointsPerCompletedAgent;
            _perfectRouteBonus = perfectRouteBonus;
            _unnecessarySegmentPenalty = unnecessarySegmentPenalty;
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

        /// <summary>
        /// Рассчитывает результат сессии по детальным данным маршрутов.
        /// </summary>
        /// <param name="routes">Маршруты агентов, участвующих в сессии.</param>
        /// <returns>Итоговый счет с бонусом за идеальные маршруты и штрафом за лишние сегменты.</returns>
        public ScoreResult Calculate(RouteScoreInput[] routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            int completedAgents = 0;
            int score = 0;
            for (int i = 0; i < routes.Length; i++)
            {
                RouteScoreInput route = routes[i];
                if (!route.ReachedGoal)
                {
                    continue;
                }

                completedAgents++;
                score += _pointsPerCompletedAgent;
                if (route.SegmentCount == route.OptimalSegmentCount)
                {
                    score += _perfectRouteBonus;
                    continue;
                }

                if (route.SegmentCount > route.OptimalSegmentCount)
                {
                    score -= (route.SegmentCount - route.OptimalSegmentCount) * _unnecessarySegmentPenalty;
                }
            }

            if (score < 0)
            {
                score = 0;
            }

            return new ScoreResult(completedAgents, score, ResolveResultText(score));
        }

        private static string ResolveResultText(int score)
        {
            if (score <= 0)
            {
                return "Try again...";
            }

            if (score < 100)
            {
                return "You almost won...";
            }

            return "You won!";
        }
    }
}
