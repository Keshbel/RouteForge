using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Рисует editor gizmos для стартов, целей, препятствий и preview маршрута уровня.
    /// </summary>
    public sealed class LevelDefinitionGizmos : MonoBehaviour
    {
        [SerializeField] private LevelDefinition levelDefinition;
        [SerializeField] private float cellSize = 1f;

        private void OnDrawGizmos()
        {
            if (levelDefinition == null)
            {
                return;
            }

            DrawCells(levelDefinition.SpawnPositions, Color.green, 0.35f);
            DrawCells(levelDefinition.GoalPositions, Color.yellow, 0.35f);
            DrawCells(levelDefinition.BlockedCells, Color.black, 0.45f);
            DrawRoutePreview();
        }

        private void DrawCells(Vector2Int[] cells, Color color, float sizeMultiplier)
        {
            if (cells == null)
            {
                return;
            }

            Gizmos.color = color;
            Vector3 size = Vector3.one * cellSize * sizeMultiplier;
            for (int i = 0; i < cells.Length; i++)
            {
                Gizmos.DrawCube(ToWorld(cells[i]), size);
            }
        }

        private void DrawRoutePreview()
        {
            Vector2Int[] routePreview = levelDefinition.RoutePreview;
            if (routePreview == null || routePreview.Length == 0)
            {
                return;
            }

            Gizmos.color = Color.cyan;
            for (int i = 0; i < routePreview.Length; i++)
            {
                Gizmos.DrawWireSphere(ToWorld(routePreview[i]), cellSize * 0.2f);
                if (i > 0)
                {
                    Gizmos.DrawLine(ToWorld(routePreview[i - 1]), ToWorld(routePreview[i]));
                }
            }
        }

        private Vector3 ToWorld(Vector2Int cell)
        {
            return transform.position + new Vector3(cell.x * cellSize, 0f, cell.y * cellSize);
        }
    }
}
