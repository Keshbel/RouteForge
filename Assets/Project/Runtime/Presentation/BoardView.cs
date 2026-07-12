using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
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
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool IsWalkable(GridPosition position)
        {
            return terrainMap.GetTile(ToUnityCell(position)) != blockedTile;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool IsGoal(GridPosition position)
        {
            return terrainMap.GetTile(ToUnityCell(position)) == goalTile;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="camera">The camera value.</param>
        /// <param name="screenPosition">The screenPosition value.</param>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
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
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public Vector3 GetWorldCenter(GridPosition position)
        {
            return grid.GetCellCenterWorld(ToUnityCell(position));
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
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
        /// Describes this API member.
        /// </summary>
        /// <param name="route">The route value.</param>
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
        /// Describes this API member.
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
