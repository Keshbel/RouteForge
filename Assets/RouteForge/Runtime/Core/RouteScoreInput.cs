using System;

namespace RouteForge
{
    /// <summary>
    /// Данные одного маршрута для подсчета очков.
    /// </summary>
    public readonly struct RouteScoreInput
    {
        /// <summary>
        /// Признак достижения цели агентом.
        /// </summary>
        public bool ReachedGoal { get; }

        /// <summary>
        /// Количество сегментов в построенном маршруте.
        /// </summary>
        public int SegmentCount { get; }

        /// <summary>
        /// Минимально необходимое количество сегментов до цели.
        /// </summary>
        public int OptimalSegmentCount { get; }

        /// <summary>
        /// Создает данные маршрута для подсчета очков.
        /// </summary>
        /// <param name="reachedGoal">Признак достижения цели.</param>
        /// <param name="segmentCount">Фактическое количество сегментов маршрута.</param>
        /// <param name="optimalSegmentCount">Оптимальное количество сегментов маршрута.</param>
        public RouteScoreInput(bool reachedGoal, int segmentCount, int optimalSegmentCount)
        {
            if (segmentCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(segmentCount), segmentCount, "Segment count must be non-negative.");
            }

            if (optimalSegmentCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(optimalSegmentCount), optimalSegmentCount, "Optimal segment count must be non-negative.");
            }

            ReachedGoal = reachedGoal;
            SegmentCount = segmentCount;
            OptimalSegmentCount = optimalSegmentCount;
        }
    }
}
