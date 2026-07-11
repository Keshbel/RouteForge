namespace RouteForge
{
    /// <summary>
    /// Результат проверки маршрута доменным валидатором.
    /// </summary>
    public readonly struct RouteValidationResult
    {
        /// <summary>
        /// Успешный результат проверки.
        /// </summary>
        public static RouteValidationResult Success { get; } = new RouteValidationResult(true, ERouteValidationError.None, null, null);

        /// <summary>
        /// Признак валидного маршрута.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Код первой найденной ошибки.
        /// </summary>
        public ERouteValidationError Error { get; }

        /// <summary>
        /// Клетка, на которой была найдена ошибка, если она применима.
        /// </summary>
        public GridPosition? Position { get; }

        /// <summary>
        /// Техническое описание причины отказа для логов и отладки.
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
        /// Создает результат с ошибкой проверки.
        /// </summary>
        /// <param name="error">Код ошибки.</param>
        /// <param name="position">Клетка, связанная с ошибкой.</param>
        /// <param name="message">Описание причины отказа.</param>
        /// <returns>Невалидный результат проверки.</returns>
        public static RouteValidationResult Failure(ERouteValidationError error, GridPosition? position, string message)
        {
            return new RouteValidationResult(false, error, position, message);
        }
    }
}
