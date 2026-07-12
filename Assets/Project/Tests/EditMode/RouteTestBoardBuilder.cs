using System.Collections.Generic;

namespace RouteForge
{
    internal sealed class RouteTestBoardBuilder
    {
        public static readonly AgentId AgentA = new AgentId(0);
        public static readonly AgentId AgentB = new AgentId(1);

        private readonly List<GridPosition> _blockedCells = new List<GridPosition>();
        private readonly Dictionary<AgentId, GridPosition> _starts = new Dictionary<AgentId, GridPosition>();
        private readonly Dictionary<AgentId, GridPosition> _goals = new Dictionary<AgentId, GridPosition>();
        private BoardBounds _bounds = new BoardBounds(0, 0, 5, 5);

        public static RouteTestBoardBuilder Default()
        {
            return new RouteTestBoardBuilder()
                .WithStart(AgentA, (0, 0))
                .WithGoal(AgentA, (2, 0))
                .WithStart(AgentB, (4, 4))
                .WithGoal(AgentB, (0, 4));
        }

        public static Route RouteFrom(params (int X, int Y)[] cells)
        {
            var positions = new GridPosition[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                positions[i] = new GridPosition(cells[i].X, cells[i].Y);
            }

            return Route.Create(positions);
        }

        public RouteTestBoardBuilder WithBounds(int minX, int minY, int width, int height)
        {
            _bounds = new BoardBounds(minX, minY, width, height);
            return this;
        }

        public RouteTestBoardBuilder WithStart(AgentId agentId, (int X, int Y) position)
        {
            _starts[agentId] = new GridPosition(position.X, position.Y);
            return this;
        }

        public RouteTestBoardBuilder WithGoal(AgentId agentId, (int X, int Y) position)
        {
            _goals[agentId] = new GridPosition(position.X, position.Y);
            return this;
        }

        public RouteTestBoardBuilder WithBlocked(params (int X, int Y)[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                _blockedCells.Add(new GridPosition(cells[i].X, cells[i].Y));
            }

            return this;
        }

        public BoardSnapshot Build()
        {
            return new BoardSnapshot(_bounds, _blockedCells, _starts, _goals);
        }
    }
}
