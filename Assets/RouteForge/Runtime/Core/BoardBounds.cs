using System;

namespace RouteForge
{
    /// <summary>
    /// Прямоугольные границы игровой доски в координатах клеток.
    /// </summary>
    public readonly struct BoardBounds : IEquatable<BoardBounds>
    {
        /// <summary>
        /// Минимальная координата X включительно.
        /// </summary>
        public int MinX { get; }

        /// <summary>
        /// Минимальная координата Y включительно.
        /// </summary>
        public int MinY { get; }

        /// <summary>
        /// Ширина доски в клетках.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Высота доски в клетках.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Максимальная координата X включительно.
        /// </summary>
        public int MaxX => MinX + Width - 1;

        /// <summary>
        /// Максимальная координата Y включительно.
        /// </summary>
        public int MaxY => MinY + Height - 1;

        /// <summary>
        /// Создает границы доски.
        /// </summary>
        /// <param name="minX">Минимальная координата X включительно.</param>
        /// <param name="minY">Минимальная координата Y включительно.</param>
        /// <param name="width">Положительная ширина доски в клетках.</param>
        /// <param name="height">Положительная высота доски в клетках.</param>
        public BoardBounds(int minX, int minY, int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), width, "Board width must be positive.");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), height, "Board height must be positive.");
            }

            MinX = minX;
            MinY = minY;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Проверяет, находится ли клетка внутри границ.
        /// </summary>
        /// <param name="position">Проверяемая клетка.</param>
        /// <returns>Возвращает true, если клетка находится на доске.</returns>
        public bool Contains(GridPosition position)
        {
            return position.X >= MinX
                && position.X <= MaxX
                && position.Y >= MinY
                && position.Y <= MaxY;
        }

        /// <inheritdoc />
        public bool Equals(BoardBounds other)
        {
            return MinX == other.MinX && MinY == other.MinY && Width == other.Width && Height == other.Height;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is BoardBounds other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = MinX;
                hash = (hash * 397) ^ MinY;
                hash = (hash * 397) ^ Width;
                hash = (hash * 397) ^ Height;
                return hash;
            }
        }
    }
}
