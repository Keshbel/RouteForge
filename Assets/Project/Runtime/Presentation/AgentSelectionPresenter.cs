using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class AgentSelectionPresenter : MonoBehaviour
    {
        private RouteEditingService _routeEditingService;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="routeEditingService">The routeEditingService value.</param>
        public void Construct(RouteEditingService routeEditingService)
        {
            _routeEditingService = routeEditingService;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="agentId">The agentId value.</param>
        public void SelectAgent(int agentId)
        {
            _routeEditingService.SelectAgent(new AgentId(agentId));
        }
    }
}
