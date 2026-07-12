using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct RouteScoreInput
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public bool ReachedGoal { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int SegmentCount { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int OptimalSegmentCount { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="reachedGoal">The reachedGoal value.</param>
        /// <param name="segmentCount">The segmentCount value.</param>
        /// <param name="optimalSegmentCount">The optimalSegmentCount value.</param>
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
