using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class ResultPresenter : MonoBehaviour
    {
        private GameSession _session;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int OpenCount { get; private set; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="session">The session value.</param>
        public void Construct(GameSession session)
        {
            if (_session != null)
            {
                _session.SessionCompleted -= OnSessionCompleted;
            }

            _session = session;
            _session.SessionCompleted += OnSessionCompleted;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public void ResetPresentation()
        {
            OpenCount = 0;
            IsOpen = false;
        }

        private void OnDestroy()
        {
            if (_session != null)
            {
                _session.SessionCompleted -= OnSessionCompleted;
            }
        }

        private void OnSessionCompleted(ScoreResult result)
        {
            if (IsOpen)
            {
                return;
            }

            IsOpen = true;
            OpenCount++;
        }
    }
}
