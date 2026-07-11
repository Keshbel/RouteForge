namespace RouteForge
{
    /// <summary>
    /// Состояние игровой сессии route-building puzzle.
    /// </summary>
    public enum ESessionState
    {
        /// <summary>
        /// Сессия создается и еще не принимает ввод игрока.
        /// </summary>
        Booting = 0,

        /// <summary>
        /// Игрок планирует маршруты агентов.
        /// </summary>
        Planning = 1,

        /// <summary>
        /// Агенты выполняют зафиксированные маршруты.
        /// </summary>
        Running = 2,

        /// <summary>
        /// Выполнение сессии временно остановлено.
        /// </summary>
        Paused = 3,

        /// <summary>
        /// Сессия завершена и больше не принимает игровые команды.
        /// </summary>
        Completed = 4
    }
}
