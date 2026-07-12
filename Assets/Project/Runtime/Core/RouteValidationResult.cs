namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct RouteValidationResult
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public static RouteValidationResult Success { get; } = new RouteValidationResult(true, ERouteValidationError.None, null, null);

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public ERouteValidationError Error { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public GridPosition? Position { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public string Message { get; }

        private RouteValidationResult(bool isValid, ERouteValidationError error, GridPosition? position, string message)
        {
            IsValid = isValid;
            Error = error;
            Position = position;
            Message = message;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="error">The error value.</param>
        /// <param name="position">The position value.</param>
        /// <param name="message">The message value.</param>
        /// <returns>The operation result.</returns>
        public static RouteValidationResult Failure(ERouteValidationError error, GridPosition? position, string message)
        {
            return new RouteValidationResult(false, error, position, message);
        }
    }
}
