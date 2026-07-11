using System.Collections.Generic;
using NUnit.Framework;

namespace RouteForge
{
    internal sealed class RouteValidatorTests
    {
        private readonly AgentId _agentA = new AgentId(0);
        private readonly AgentId _agentB = new AgentId(1);
        private readonly RouteValidator _validator = new RouteValidator();

        [Test]
        public void Validate_AdjacentRouteWithGoal_ReturnsSuccess()
        {
            Route route = Route.Create(new[]
            {
                new GridPosition(1, 0),
                new GridPosition(2, 0)
            });

            RouteValidationResult result = _validator.Validate(_agentA, route, CreateBoard(), null);

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void Validate_FirstCellFarFromStart_ReturnsInvalidStart()
        {
            Route route = Route.Create(new[]
            {
                new GridPosition(2, 1),
                new GridPosition(2, 0)
            });

            RouteValidationResult result = _validator.Validate(_agentA, route, CreateBoard(), null);

            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.InvalidStart));
        }

        [Test]
        public void Validate_NonAdjacentSegment_ReturnsNonAdjacentSegment()
        {
            Route route = Route.Create(new[]
            {
                new GridPosition(1, 0),
                new GridPosition(2, 2)
            });

            RouteValidationResult result = _validator.Validate(_agentA, route, CreateBoard(), null);

            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.NonAdjacentSegment));
        }

        [Test]
        public void Validate_BlockedCell_ReturnsBlockedCell()
        {
            Route route = Route.Create(new[]
            {
                new GridPosition(0, 1),
                new GridPosition(1, 1),
                new GridPosition(2, 1),
                new GridPosition(2, 0)
            });

            RouteValidationResult result = _validator.Validate(_agentA, route, CreateBoard(), null);

            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.BlockedCell));
        }

        [Test]
        public void Validate_RepeatedCell_ReturnsCycle()
        {
            Route route = Route.Create(new[]
            {
                new GridPosition(1, 0),
                new GridPosition(1, 1),
                new GridPosition(0, 1),
                new GridPosition(1, 1),
                new GridPosition(2, 1),
                new GridPosition(2, 0)
            });

            RouteValidationResult result = _validator.Validate(_agentA, route, CreateBoardWithoutBlockedCells(), null);

            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.Cycle));
        }

        [Test]
        public void Validate_MissingGoal_ReturnsMissingTarget()
        {
            Route route = Route.Create(new[]
            {
                new GridPosition(1, 0),
                new GridPosition(1, 1)
            });

            RouteValidationResult result = _validator.Validate(_agentA, route, CreateBoard(), null);

            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.MissingTarget));
        }

        [Test]
        public void Validate_CellOwnedByOtherRoute_ReturnsRouteConflict()
        {
            Route route = Route.Create(new[]
            {
                new GridPosition(1, 0),
                new GridPosition(2, 0)
            });
            var committedRoutes = new Dictionary<AgentId, Route>
            {
                {
                    _agentB,
                    Route.Create(new[]
                    {
                        new GridPosition(4, 4),
                        new GridPosition(3, 4),
                        new GridPosition(2, 4),
                        new GridPosition(2, 3),
                        new GridPosition(2, 2),
                        new GridPosition(2, 1),
                        new GridPosition(2, 0)
                    })
                }
            };

            RouteValidationResult result = _validator.Validate(_agentA, route, CreateBoardWithoutBlockedCells(), committedRoutes);

            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.RouteConflict));
        }

        private BoardSnapshot CreateBoard()
        {
            return new BoardSnapshot(
                new BoardBounds(0, 0, 5, 5),
                new[] { new GridPosition(1, 1) },
                CreateStarts(),
                CreateGoals());
        }

        private BoardSnapshot CreateBoardWithoutBlockedCells()
        {
            return new BoardSnapshot(
                new BoardBounds(0, 0, 5, 5),
                null,
                CreateStarts(),
                CreateGoals());
        }

        private Dictionary<AgentId, GridPosition> CreateStarts()
        {
            return new Dictionary<AgentId, GridPosition>
            {
                { _agentA, new GridPosition(0, 0) },
                { _agentB, new GridPosition(4, 4) }
            };
        }

        private Dictionary<AgentId, GridPosition> CreateGoals()
        {
            return new Dictionary<AgentId, GridPosition>
            {
                { _agentA, new GridPosition(2, 0) },
                { _agentB, new GridPosition(0, 4) }
            };
        }
    }
}
