namespace RouteForge
{
    /// <summary>
    /// Причина, по которой маршрут не может быть принят доменной моделью.
    /// </summary>
    public enum ERouteValidationError
    {
        /// <summary>
        /// Ошибка отсутствует, маршрут валиден.
        /// </summary>
        None = 0,

        /// <summary>
        /// Маршрут не задан.
        /// </summary>
        MissingRoute = 1,

        /// <summary>
        /// Маршрут не содержит ни одной клетки.
        /// </summary>
        EmptyRoute = 2,

        /// <summary>
        /// Для агента не задана стартовая клетка.
        /// </summary>
        MissingStart = 3,

        /// <summary>
        /// Для агента не задана целевая клетка.
        /// </summary>
        MissingGoal = 4,

        /// <summary>
        /// Первая клетка маршрута не начинается у стартовой позиции агента.
        /// </summary>
        InvalidStart = 5,

        /// <summary>
        /// Маршрут содержит клетку за пределами доски.
        /// </summary>
        OutOfBounds = 6,

        /// <summary>
        /// Маршрут содержит заблокированную клетку.
        /// </summary>
        BlockedCell = 7,

        /// <summary>
        /// Соседние элементы маршрута не являются смежными клетками.
        /// </summary>
        NonAdjacentSegment = 8,

        /// <summary>
        /// Маршрут повторно посещает клетку и образует цикл.
        /// </summary>
        Cycle = 9,

        /// <summary>
        /// Маршрут не проходит через целевую клетку агента.
        /// </summary>
        MissingTarget = 10,

        /// <summary>
        /// Маршрут пересекается с маршрутом другого агента.
        /// </summary>
        RouteConflict = 11,

        /// <summary>
        /// Маршрут проходит через цель другого агента.
        /// </summary>
        ForeignGoal = 12
    }
}
