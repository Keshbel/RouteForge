using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="other">The other value.</param>
        /// <returns>The operation result.</returns>
        public int ManhattanDistanceTo(GridPosition other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="other">The other value.</param>
        /// <returns>The operation result.</returns>
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
        /// Describes this API member.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The operation result.</returns>
        public static bool operator ==(GridPosition left, GridPosition right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The operation result.</returns>
        public static bool operator !=(GridPosition left, GridPosition right)
        {
            return !left.Equals(right);
        }
    }
}
