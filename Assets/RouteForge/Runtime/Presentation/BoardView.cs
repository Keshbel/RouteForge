using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RouteForge
{
    /// <summary>
    /// Отвечает за преобразование координат доски и отображение tilemap-слоев.
    /// </summary>
    public sealed class BoardView : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap interactiveMap;
        [SerializeField] private Tilemap pathMap;
        [SerializeField] private Tilemap terrainMap;
        [SerializeField] private TileBase hoverTile;
        [SerializeField] private TileBase pathTile;
        [SerializeField] private TileBase blockedTile;
        [SerializeField] private TileBase goalTile;

        private readonly List<Vector3Int> _renderedPathCells = new List<Vector3Int>();
        private Vector3Int _previousHoverCell = new Vector3Int(int.MinValue, int.MinValue, 0);

        /// <summary>
        /// Проверяет, может ли клетка использоваться для маршрута.
        /// </summary>
        /// <param name="position">Проверяемая доменная позиция.</param>
        /// <returns>Возвращает true, если клетка не является препятствием.</returns>
        public bool IsWalkable(GridPosition position)
        {
            return terrainMap.GetTile(ToUnityCell(position)) != blockedTile;
        }

        /// <summary>
        /// Проверяет, является ли клетка целью.
        /// </summary>
        /// <param name="position">Проверяемая доменная позиция.</param>
        /// <returns>Возвращает true, если на клетке находится целевой tile.</returns>
        public bool IsGoal(GridPosition position)
        {
            return terrainMap.GetTile(ToUnityCell(position)) == goalTile;
        }

        /// <summary>
        /// Преобразует экранную позицию указателя в клетку доски.
        /// </summary>
        /// <param name="camera">Камера, через которую выполняется raycast.</param>
        /// <param name="screenPosition">Позиция указателя на экране.</param>
        /// <param name="position">Найденная доменная позиция клетки.</param>
        /// <returns>Возвращает true, если raycast попал в игровую плоскость.</returns>
        public bool TryGetCell(Camera camera, Vector3 screenPosition, out GridPosition position)
        {
            position = default(GridPosition);
            if (camera == null || grid == null)
            {
                return false;
            }

            Ray ray = camera.ScreenPointToRay(screenPosition);
            if (!Physics.Raycast(ray, out RaycastHit hit))
            {
                return false;
            }

            Vector3Int cell = grid.WorldToCell(hit.point);
            position = ToDomainPosition(cell);
            return true;
        }

        /// <summary>
        /// Возвращает мировую позицию центра клетки.
        /// </summary>
        /// <param name="position">Доменная позиция клетки.</param>
        /// <returns>Мировая позиция центра клетки.</returns>
        public Vector3 GetWorldCenter(GridPosition position)
        {
            return grid.GetCellCenterWorld(ToUnityCell(position));
        }

        /// <summary>
        /// Отображает hover-состояние клетки.
        /// </summary>
        /// <param name="position">Клетка под указателем.</param>
        public void RenderHover(GridPosition position)
        {
            Vector3Int cell = ToUnityCell(position);
            if (cell == _previousHoverCell)
            {
                return;
            }

            interactiveMap.SetTile(_previousHoverCell, null);
            interactiveMap.SetTile(cell, hoverTile);
            _previousHoverCell = cell;
        }

        /// <summary>
        /// Перерисовывает path tilemap для переданного маршрута.
        /// </summary>
        /// <param name="route">Маршрут, который нужно показать на доске.</param>
        public void RenderRoute(Route route)
        {
            ClearRenderedRoute();
            if (route == null)
            {
                return;
            }

            for (int i = 0; i < route.Count; i++)
            {
                Vector3Int cell = ToUnityCell(route[i]);
                pathMap.SetTile(cell, pathTile);
                _renderedPathCells.Add(cell);
            }
        }

        /// <summary>
        /// Очищает ранее отрисованный маршрут.
        /// </summary>
        public void ClearRenderedRoute()
        {
            for (int i = 0; i < _renderedPathCells.Count; i++)
            {
                pathMap.SetTile(_renderedPathCells[i], null);
            }

            _renderedPathCells.Clear();
        }

        private static GridPosition ToDomainPosition(Vector3Int cell)
        {
            return new GridPosition(cell.x, cell.y);
        }

        private static Vector3Int ToUnityCell(GridPosition position)
        {
            return new Vector3Int(position.X, position.Y, 0);
        }
    }
}
