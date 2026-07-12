using System;

namespace RouteForge
{
    /// <summary>
    /// Describes this API member.
    /// </summary>
    public readonly struct AgentId : IEquatable<AgentId>
    {
        /// <summary>
        /// Describes this API member.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="value">The value value.</param>
        public AgentId(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Agent id must be non-negative.");
            }

            Value = value;
        }

        /// <inheritdoc />
        public bool Equals(AgentId other)
        {
            return Value == other.Value;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is AgentId other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The operation result.</returns>
        public static bool operator ==(AgentId left, AgentId right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Describes this API member.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The operation result.</returns>
        public static bool operator !=(AgentId left, AgentId right)
        {
            return !left.Equals(right);
        }
    }
}
