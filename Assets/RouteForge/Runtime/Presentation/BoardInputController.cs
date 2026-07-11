using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Читает ввод игрока и передает доменные клетки в сервис редактирования маршрута.
    /// </summary>
    public sealed class BoardInputController : MonoBehaviour
    {
        [SerializeField] private Camera inputCamera;
        [SerializeField] private BoardView boardView;
        [SerializeField] private int selectedAgentId;

        private RouteEditingService _routeEditingService;

        /// <summary>
        /// Передает сервис редактирования маршрутов и выбирает стартового агента.
        /// </summary>
        /// <param name="routeEditingService">Application-сервис редактирования.</param>
        public void Construct(RouteEditingService routeEditingService)
        {
            _routeEditingService = routeEditingService;
            _routeEditingService.SelectAgent(new AgentId(selectedAgentId));
        }

        private void Update()
        {
            if (_routeEditingService == null || boardView == null)
            {
                return;
            }

            if (!boardView.TryGetCell(inputCamera, Input.mousePosition, out GridPosition position))
            {
                return;
            }

            if (!boardView.IsWalkable(position))
            {
                return;
            }

            boardView.RenderHover(position);

            if (Input.GetMouseButton(0))
            {
                _routeEditingService.TryAppendCell(position);
            }
            else if (Input.GetMouseButton(1))
            {
                _routeEditingService.TryRemoveCell(position);
            }
        }
    }
}
