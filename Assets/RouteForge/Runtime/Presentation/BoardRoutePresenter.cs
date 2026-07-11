using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Синхронизирует изменения доменного маршрута с отображением доски.
    /// </summary>
    public sealed class BoardRoutePresenter : MonoBehaviour
    {
        private RouteEditingService _routeEditingService;
        private BoardView _boardView;

        /// <summary>
        /// Передает зависимости отображения маршрута.
        /// </summary>
        /// <param name="routeEditingService">Сервис маршрутов.</param>
        /// <param name="boardView">Отображение доски.</param>
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
