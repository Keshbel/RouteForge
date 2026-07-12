using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct BoardBounds : IEquatable<BoardBounds>
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int MinX { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int MinY { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int MaxX => MinX + Width - 1;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int MaxY => MinY + Height - 1;

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="minX">The minX value.</param>
        /// <param name="minY">The minY value.</param>
        /// <param name="width">The width value.</param>
        /// <param name="height">The height value.</param>
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
        /// Describes this API member.
        /// </summary>
        /// <param name="position">The position value.</param>
        /// <returns>The operation result.</returns>
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
