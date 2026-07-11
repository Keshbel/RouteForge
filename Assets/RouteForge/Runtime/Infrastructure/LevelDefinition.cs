using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Конфигурация уровня: границы доски, старты, цели, препятствия и preview маршрута.
    /// </summary>
    [CreateAssetMenu(menuName = "RouteForge/Level Definition", fileName = "LevelDefinition")]
    public sealed class LevelDefinition : ScriptableObject
    {
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private Vector2Int origin;
        [SerializeField] private Vector2Int[] spawnPositions = new Vector2Int[0];
        [SerializeField] private Vector2Int[] goalPositions = new Vector2Int[0];
        [SerializeField] private Vector2Int[] blockedCells = new Vector2Int[0];
        [SerializeField] private Vector2Int[] routePreview = new Vector2Int[0];

        /// <summary>
        /// Ширина доски в клетках.
        /// </summary>
        public int Width => width;

        /// <summary>
        /// Высота доски в клетках.
        /// </summary>
        public int Height => height;

        /// <summary>
        /// Начальная клетка прямоугольной доски.
        /// </summary>
        public Vector2Int Origin => origin;

        /// <summary>
        /// Стартовые клетки агентов.
        /// </summary>
        public Vector2Int[] SpawnPositions => spawnPositions;

        /// <summary>
        /// Целевые клетки агентов.
        /// </summary>
        public Vector2Int[] GoalPositions => goalPositions;

        /// <summary>
        /// Заблокированные клетки, недоступные для маршрутов.
        /// </summary>
        public Vector2Int[] BlockedCells => blockedCells;

        /// <summary>
        /// Клетки preview маршрута для editor gizmos.
        /// </summary>
        public Vector2Int[] RoutePreview => routePreview;

        /// <summary>
        /// Создает доменные границы доски.
        /// </summary>
        /// <returns>Границы доски.</returns>
        public BoardBounds CreateBounds()
        {
            return new BoardBounds(origin.x, origin.y, width, height);
        }
    }
}
