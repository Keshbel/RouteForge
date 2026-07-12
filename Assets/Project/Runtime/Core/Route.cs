using System;
using System.Collections;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public sealed class Route : IReadOnlyList<GridPosition>
    {
        private static readonly GridPosition[] EmptyCells = new GridPosition[0];

        private readonly GridPosition[] _cells;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public static Route Empty { get; } = new Route(EmptyCells);

        /// <inheritdoc />
        public int Count => _cells.Length;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="index">The index value.</param>
        /// <returns>The operation result.</returns>
        public GridPosition this[int index] => _cells[index];

        private Route(GridPosition[] cells)
        {
            _cells = cells;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="cells">The cells value.</param>
        /// <returns>The operation result.</returns>
        public static Route Create(IEnumerable<GridPosition> cells)
        {
            if (cells == null)
            {
                throw new ArgumentNullException(nameof(cells));
            }

            var buffer = new List<GridPosition>();
            foreach (GridPosition cell in cells)
            {
                buffer.Add(cell);
            }

            if (buffer.Count == 0)
            {
                return Empty;
            }

            return new Route(buffer.ToArray());
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <returns>The operation result.</returns>
        public GridPosition[] ToArray()
        {
            var copy = new GridPosition[_cells.Length];
            Array.Copy(_cells, copy, _cells.Length);
            return copy;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
        public bool Contains(GridPosition position)
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                if (_cells[i] == position)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public IEnumerator<GridPosition> GetEnumerator()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                yield return _cells[i];
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
