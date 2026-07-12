using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
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
        /// Describes this API member.
        /// </summary>
        public int Width => width;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int Height => height;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public Vector2Int Origin => origin;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public Vector2Int[] SpawnPositions => spawnPositions;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public Vector2Int[] GoalPositions => goalPositions;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public Vector2Int[] BlockedCells => blockedCells;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public Vector2Int[] RoutePreview => routePreview;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public BoardBounds CreateBounds()
        {
            return new BoardBounds(origin.x, origin.y, width, height);
        }
    }
}
