namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public enum ERouteValidationError
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        None = 0,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        MissingRoute = 1,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        EmptyRoute = 2,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        MissingStart = 3,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        MissingGoal = 4,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        InvalidStart = 5,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        OutOfBounds = 6,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        BlockedCell = 7,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        NonAdjacentSegment = 8,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        Cycle = 9,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        MissingTarget = 10,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        RouteConflict = 11,

        /// <summary>
        /// Describes this API member.
        /// </summary>
        ForeignGoal = 12
    }
}
