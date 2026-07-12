using System;
using System.Collections;
using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class AgentMovementView : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float height = 2f;

        private GridPosition[] _routeSnapshot;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public event Action<AgentMovementView> MovementCompleted;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="route">The route value.</param>
        public void StartMovement(Route route)
        {
            _routeSnapshot = route != null ? route.ToArray() : Array.Empty<GridPosition>();
            StopAllCoroutines();
            StartCoroutine(MoveRoute());
        }

        private IEnumerator MoveRoute()
        {
            for (int i = 0; i < _routeSnapshot.Length; i++)
            {
                Vector3 target = boardView.GetWorldCenter(_routeSnapshot[i]);
                target.y = height;
                if (speed <= 0f)
                {
                    transform.position = target;
                    continue;
                }

                while (Vector3.SqrMagnitude(transform.position - target) > 0.0001f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    yield return null;
                }

                transform.position = target;
            }

            MovementCompleted?.Invoke(this);
        }
    }
}
