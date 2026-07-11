using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Передает выбор агента из presentation в сервис редактирования маршрута.
    /// </summary>
    public sealed class AgentSelectionPresenter : MonoBehaviour
    {
        private RouteEditingService _routeEditingService;

        /// <summary>
        /// Передает зависимость сервиса редактирования.
        /// </summary>
        /// <param name="routeEditingService">Сервис редактирования маршрутов.</param>
        public void Construct(RouteEditingService routeEditingService)
        {
            _routeEditingService = routeEditingService;
        }

        /// <summary>
        /// Выбирает агента по числовому идентификатору.
        /// </summary>
        /// <param name="agentId">Идентификатор агента.</param>
        public void SelectAgent(int agentId)
        {
            _routeEditingService.SelectAgent(new AgentId(agentId));
        }
    }
}
