using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class BoardRoutePresenter : MonoBehaviour
    {
        private RouteEditingService _routeEditingService;
        private BoardView _boardView;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="routeEditingService">The routeEditingService value.</param>
        /// <param name="boardView">The boardView value.</param>
        public void Construct(RouteEditingService routeEditingService, BoardView boardView)
        {
            if (_routeEditingService != null)
            {
                _routeEditingService.RouteChanged -= OnRouteChanged;
                _routeEditingService.RoutesCleared -= OnRoutesCleared;
            }

            _routeEditingService = routeEditingService;
            _boardView = boardView;
            _routeEditingService.RouteChanged += OnRouteChanged;
            _routeEditingService.RoutesCleared += OnRoutesCleared;
        }

        private void OnDestroy()
        {
            if (_routeEditingService != null)
            {
                _routeEditingService.RouteChanged -= OnRouteChanged;
                _routeEditingService.RoutesCleared -= OnRoutesCleared;
            }
        }

        private void OnRouteChanged(AgentId agentId, Route route)
        {
            _boardView.RenderRoute(route);
        }

        private void OnRoutesCleared()
        {
            _boardView.ClearRenderedRoute();
        }
    }
}
