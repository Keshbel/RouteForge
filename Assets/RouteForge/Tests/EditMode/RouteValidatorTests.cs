using System.Collections.Generic;
using NUnit.Framework;

namespace RouteForge
{
    internal sealed class RouteValidatorTests
    {
        [Test]
        public void Validate_EmptyRoute_ReturnsEmptyRouteError()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default().Build();

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, Route.Empty, board, null);

            // Assert
            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.EmptyRoute));
        }

        [Test]
        public void Validate_RouteDoesNotStartAtAgent_ReturnsInvalidStart()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default().Build();
            Route route = RouteTestBoardBuilder.RouteFrom((1, 0), (2, 0));

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, route, board, null);

            // Assert
            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.InvalidStart));
        }

        [Test]
        public void Validate_NonAdjacentCells_ReturnsNonAdjacentSegment()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default().Build();
            Route route = RouteTestBoardBuilder.RouteFrom((0, 0), (2, 0));

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, route, board, null);

            // Assert
            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.NonAdjacentSegment));
        }

        [Test]
        public void Validate_BlockedCell_ReturnsBlockedCell()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default()
                .WithBlocked((1, 0))
                .Build();
            Route route = RouteTestBoardBuilder.RouteFrom((0, 0), (1, 0), (2, 0));

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, route, board, null);

            // Assert
            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.BlockedCell));
        }

        [Test]
        public void Validate_RepeatedCellWhenLoopsDisabled_ReturnsCycle()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default()
                .WithGoal(RouteTestBoardBuilder.AgentA, (2, 0))
                .Build();
            Route route = RouteTestBoardBuilder.RouteFrom((0, 0), (1, 0), (1, 1), (1, 0), (2, 0));

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, route, board, null);

            // Assert
            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.Cycle));
        }

        [Test]
        public void Validate_ContinuousRouteToCorrectGoal_ReturnsSuccess()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default().Build();
            Route route = RouteTestBoardBuilder.RouteFrom((0, 0), (1, 0), (2, 0));

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, route, board, null);

            // Assert
            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void Validate_RouteContainsAnotherAgentsGoal_ReturnsForeignGoal()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default()
                .WithGoal(RouteTestBoardBuilder.AgentA, (3, 0))
                .WithGoal(RouteTestBoardBuilder.AgentB, (1, 0))
                .Build();
            Route route = RouteTestBoardBuilder.RouteFrom((0, 0), (1, 0), (2, 0), (3, 0));

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, route, board, null);

            // Assert
            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.ForeignGoal));
        }

        [Test]
        public void Validate_SharedExclusiveCell_ReturnsRouteConflict()
        {
            // Arrange
            RouteValidator validator = new RouteValidator();
            BoardSnapshot board = RouteTestBoardBuilder.Default()
                .WithGoal(RouteTestBoardBuilder.AgentA, (2, 0))
                .WithGoal(RouteTestBoardBuilder.AgentB, (4, 4))
                .Build();
            Route route = RouteTestBoardBuilder.RouteFrom((0, 0), (1, 0), (2, 0));
            var committedRoutes = new Dictionary<AgentId, Route>
            {
                { RouteTestBoardBuilder.AgentB, RouteTestBoardBuilder.RouteFrom((4, 4), (3, 4), (2, 4), (2, 3), (2, 2), (2, 1), (2, 0)) }
            };

            // Act
            RouteValidationResult result = validator.Validate(RouteTestBoardBuilder.AgentA, route, board, committedRoutes);

            // Assert
            Assert.That(result.Error, Is.EqualTo(ERouteValidationError.RouteConflict));
        }
    }
}
