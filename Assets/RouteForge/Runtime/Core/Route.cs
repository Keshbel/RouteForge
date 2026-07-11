using System;
using System.Collections;
using System.Collections.Generic;

namespace RouteForge
{
    /// <summary>
    /// Упорядоченный неизменяемый маршрут агента по клеткам доски.
    /// </summary>
    public sealed class Route : IReadOnlyList<GridPosition>
    {
        private static readonly GridPosition[] EmptyCells = new GridPosition[0];

        private readonly GridPosition[] _cells;

        /// <summary>
        /// Пустой маршрут без клеток.
        /// </summary>
        public static Route Empty { get; } = new Route(EmptyCells);

        /// <inheritdoc />
        public int Count => _cells.Length;

        /// <summary>
        /// Возвращает клетку маршрута по порядковому индексу.
        /// </summary>
        /// <param name="index">Индекс клетки в маршруте.</param>
        /// <returns>Клетка маршрута.</returns>
        public GridPosition this[int index] => _cells[index];

        private Route(GridPosition[] cells)
        {
            _cells = cells;
        }

        /// <summary>
        /// Создает маршрут из переданной последовательности клеток.
        /// </summary>
        /// <param name="cells">Упорядоченные клетки маршрута.</param>
        /// <returns>Новый маршрут с собственной копией клеток.</returns>
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
        /// Создает копию клеток маршрута для безопасной передачи в presentation.
        /// </summary>
        /// <returns>Новый массив с клетками маршрута в исходном порядке.</returns>
        public GridPosition[] ToArray()
        {
            var copy = new GridPosition[_cells.Length];
            Array.Copy(_cells, copy, _cells.Length);
            return copy;
        }

        /// <summary>
        /// Проверяет наличие клетки в маршруте.
        /// </summary>
        /// <param name="position">Искомая клетка.</param>
        /// <returns>Возвращает true, если маршрут содержит клетку.</returns>
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
