using System;

namespace RouteForge
{
    /// <summary>
    /// Неизменяемая позиция клетки на игровой сетке.
    /// </summary>
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        /// <summary>
        /// Горизонтальная координата клетки.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Вертикальная координата клетки.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Создает позицию клетки.
        /// </summary>
        /// <param name="x">Горизонтальная координата.</param>
        /// <param name="y">Вертикальная координата.</param>
        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Возвращает манхэттенское расстояние до другой клетки.
        /// </summary>
        /// <param name="other">Клетка, до которой измеряется расстояние.</param>
        /// <returns>Количество ортогональных шагов между клетками.</returns>
        public int ManhattanDistanceTo(GridPosition other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        /// <summary>
        /// Проверяет ортогональную смежность с другой клеткой.
        /// </summary>
        /// <param name="other">Клетка для проверки смежности.</param>
        /// <returns>Возвращает true, если клетки имеют общую сторону.</returns>
        public bool IsAdjacentTo(GridPosition other)
        {
            return ManhattanDistanceTo(other) == 1;
        }

        /// <inheritdoc />
        public bool Equals(GridPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        /// <summary>
        /// Проверяет равенство двух позиций.
        /// </summary>
        /// <param name="left">Левая позиция.</param>
        /// <param name="right">Правая позиция.</param>
        /// <returns>Возвращает true, если обе координаты совпадают.</returns>
        public static bool operator ==(GridPosition left, GridPosition right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Проверяет различие двух позиций.
        /// </summary>
        /// <param name="left">Левая позиция.</param>
        /// <param name="right">Правая позиция.</param>
        /// <returns>Возвращает true, если хотя бы одна координата различается.</returns>
        public static bool operator !=(GridPosition left, GridPosition right)
        {
            return !left.Equals(right);
        }
    }
}
