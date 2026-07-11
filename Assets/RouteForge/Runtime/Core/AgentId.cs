using System;

namespace RouteForge
{
    /// <summary>
    /// Идентификатор агента, для которого строится и выполняется маршрут.
    /// </summary>
    public readonly struct AgentId : IEquatable<AgentId>
    {
        /// <summary>
        /// Числовое значение идентификатора агента.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Создает идентификатор агента.
        /// </summary>
        /// <param name="value">Неотрицательное значение идентификатора.</param>
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
        /// Проверяет равенство двух идентификаторов.
        /// </summary>
        /// <param name="left">Левый идентификатор.</param>
        /// <param name="right">Правый идентификатор.</param>
        /// <returns>Возвращает true, если значения идентификаторов совпадают.</returns>
        public static bool operator ==(AgentId left, AgentId right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Проверяет различие двух идентификаторов.
        /// </summary>
        /// <param name="left">Левый идентификатор.</param>
        /// <param name="right">Правый идентификатор.</param>
        /// <returns>Возвращает true, если значения идентификаторов различаются.</returns>
        public static bool operator !=(AgentId left, AgentId right)
        {
            return !left.Equals(right);
        }
    }
}
