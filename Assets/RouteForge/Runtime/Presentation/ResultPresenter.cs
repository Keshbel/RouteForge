using UnityEngine;

namespace RouteForge
{
    /// <summary>
    /// Показывает результат завершенной игровой сессии.
    /// </summary>
    public sealed class ResultPresenter : MonoBehaviour
    {
        private GameSession _session;

        /// <summary>
        /// Количество открытий результата в текущем жизненном цикле.
        /// </summary>
        public int OpenCount { get; private set; }

        /// <summary>
        /// Признак открытого результата.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Передает сессию, события которой управляют результатом.
        /// </summary>
        /// <param name="session">Игровая сессия.</param>
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
        /// Скрывает результат и сбрасывает счетчик открытий.
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
