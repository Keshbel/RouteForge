using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class BoardInputController : MonoBehaviour
    {
        [SerializeField] private Camera inputCamera;
        [SerializeField] private BoardView boardView;
        [SerializeField] private int selectedAgentId;

        private RouteEditingService _routeEditingService;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="routeEditingService">The routeEditingService value.</param>
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
