using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RouteForge
{
    public sealed class GameplayPlayModeTests
    {
        private static readonly AgentId AgentA = new AgentId(0);
        private static readonly AgentId AgentB = new AgentId(1);

        [Test]
        public void RouteChanged_WhenBuildingRoute_UpdatesBoardView()
        {
            // Arrange
            BoardViewFixture fixture = BoardViewFixture.Create();
            RouteEditingService service = CreateRouteEditingService();
            var presenterObject = new GameObject("BoardRoutePresenter");
            var presenter = presenterObject.AddComponent<BoardRoutePresenter>();
            presenter.Construct(service, fixture.BoardView);

            // Act
            service.TryAppendCell(new GridPosition(0, 0));
            service.TryAppendCell(new GridPosition(1, 0));
            service.TryAppendCell(new GridPosition(2, 0));

            // Assert
            Assert.That(fixture.PathMap.GetTile(new Vector3Int(0, 0, 0)), Is.SameAs(fixture.PathTile));
            Assert.That(fixture.PathMap.GetTile(new Vector3Int(1, 0, 0)), Is.SameAs(fixture.PathTile));
            Assert.That(fixture.PathMap.GetTile(new Vector3Int(2, 0, 0)), Is.SameAs(fixture.PathTile));

            Object.DestroyImmediate(presenterObject);
            fixture.Destroy();
        }

        [Test]
        public void SelectAgent_WhenEditingSecondAgent_PreservesFirstRoute()
        {
            // Arrange
            RouteEditingService service = CreateRouteEditingService();
            var selectionObject = new GameObject("AgentSelectionPresenter");
            var presenter = selectionObject.AddComponent<AgentSelectionPresenter>();
            presenter.Construct(service);
            service.TryAppendCell(new GridPosition(0, 0));
            service.TryAppendCell(new GridPosition(1, 0));
            service.TryAppendCell(new GridPosition(2, 0));

            // Act
            presenter.SelectAgent(1);
            service.TryAppendCell(new GridPosition(4, 4));
            service.TryAppendCell(new GridPosition(3, 4));
            service.TryAppendCell(new GridPosition(2, 4));
            service.TryAppendCell(new GridPosition(1, 4));
            service.TryAppendCell(new GridPosition(0, 4));

            // Assert
            Assert.That(service.TryGetRoute(AgentA, out Route firstRoute), Is.True);
            Assert.That(firstRoute.Count, Is.EqualTo(3));

            Object.DestroyImmediate(selectionObject);
        }

        [Test]
        public void StartSession_WhenRoutesConfigured_LaunchesEveryAgent()
        {
            // Arrange
            GameSession session = CreatePlanningSession(agentCount: 2);
            AgentRouteSimulation first = new AgentRouteSimulation();
            AgentRouteSimulation second = new AgentRouteSimulation();
            var runnerObject = new GameObject("SessionSimulationRunner");
            var runner = runnerObject.AddComponent<SessionSimulationRunner>();
            runner.Construct(session, new[] { first, second });

            // Act
            runner.StartSession(new[]
            {
                RouteFrom((0, 0), (1, 0)),
                RouteFrom((4, 4), (3, 4))
            });

            // Assert
            Assert.That(runner.StartedAgentCount, Is.EqualTo(2));
            Assert.That(first.IsRunning, Is.True);
            Assert.That(second.IsRunning, Is.True);

            Object.DestroyImmediate(runnerObject);
        }

        [Test]
        public void Pause_WhenSimulationTicks_DoesNotAdvanceProgress()
        {
            // Arrange
            GameSession session = CreatePlanningSession(agentCount: 1);
            AgentRouteSimulation simulation = new AgentRouteSimulation();
            var runnerObject = new GameObject("SessionSimulationRunner");
            var runner = runnerObject.AddComponent<SessionSimulationRunner>();
            runner.Construct(session, new[] { simulation });
            runner.StartSession(new[] { RouteFrom((0, 0), (1, 0), (2, 0)) });
            runner.Tick();
            int progressBeforePause = simulation.CurrentCellIndex;

            // Act
            runner.Pause();
            runner.Tick();

            // Assert
            Assert.That(simulation.CurrentCellIndex, Is.EqualTo(progressBeforePause));

            Object.DestroyImmediate(runnerObject);
        }

        [Test]
        public void Resume_FromPausedSimulation_ContinuesFromSameLogicalProgress()
        {
            // Arrange
            GameSession session = CreatePlanningSession(agentCount: 1);
            AgentRouteSimulation simulation = new AgentRouteSimulation();
            var runnerObject = new GameObject("SessionSimulationRunner");
            var runner = runnerObject.AddComponent<SessionSimulationRunner>();
            runner.Construct(session, new[] { simulation });
            runner.StartSession(new[] { RouteFrom((0, 0), (1, 0), (2, 0)) });
            runner.Tick();
            runner.Pause();
            int progressBeforeResume = simulation.CurrentCellIndex;

            // Act
            runner.Resume();
            runner.Tick();

            // Assert
            Assert.That(simulation.CurrentCellIndex, Is.EqualTo(progressBeforeResume + 1));

            Object.DestroyImmediate(runnerObject);
        }

        [Test]
        public void Restart_WhenRoutesAndResultExist_ClearsRoutesAndPresentation()
        {
            // Arrange
            RouteEditingService service = CreateRouteEditingService();
            GameSession session = CreateRunningSession(agentCount: 1);
            var resultObject = new GameObject("ResultPresenter");
            var resultPresenter = resultObject.AddComponent<ResultPresenter>();
            resultPresenter.Construct(session);
            service.TryAppendCell(new GridPosition(0, 0));
            service.TryAppendCell(new GridPosition(1, 0));
            service.TryAppendCell(new GridPosition(2, 0));
            session.CompleteAgent(new AgentId(0), reachedGoal: true);

            // Act
            service.ClearAll();
            resultPresenter.ResetPresentation();

            // Assert
            Assert.That(service.TryGetRoute(AgentA, out _), Is.False);
            Assert.That(resultPresenter.IsOpen, Is.False);
            Assert.That(resultPresenter.OpenCount, Is.EqualTo(0));

            Object.DestroyImmediate(resultObject);
        }

        [Test]
        public void CompleteAgent_WhenAllAgentsFinish_OpensResultExactlyOnce()
        {
            // Arrange
            GameSession session = CreateRunningSession(agentCount: 2);
            var resultObject = new GameObject("ResultPresenter");
            var resultPresenter = resultObject.AddComponent<ResultPresenter>();
            resultPresenter.Construct(session);

            // Act
            session.CompleteAgent(new AgentId(0), reachedGoal: true);
            session.CompleteAgent(new AgentId(1), reachedGoal: true);
            session.CompleteAgent(new AgentId(1), reachedGoal: true);
            session.CompleteSession(2);

            // Assert
            Assert.That(resultPresenter.IsOpen, Is.True);
            Assert.That(resultPresenter.OpenCount, Is.EqualTo(1));

            Object.DestroyImmediate(resultObject);
        }

        private static RouteEditingService CreateRouteEditingService()
        {
            return new RouteEditingService(
                new RouteValidator(),
                CreateDefaultBoard(),
                AgentA);
        }

        private static GameSession CreatePlanningSession(int agentCount)
        {
            var session = new GameSession(agentCount, new ScoringPolicy());
            session.BeginPlanning();
            return session;
        }

        private static GameSession CreateRunningSession(int agentCount)
        {
            GameSession session = CreatePlanningSession(agentCount);
            session.StartRunning();
            return session;
        }

        private static BoardSnapshot CreateDefaultBoard()
        {
            return new BoardSnapshot(
                new BoardBounds(0, 0, 5, 5),
                null,
                new Dictionary<AgentId, GridPosition>
                {
                    { AgentA, new GridPosition(0, 0) },
                    { AgentB, new GridPosition(4, 4) }
                },
                new Dictionary<AgentId, GridPosition>
                {
                    { AgentA, new GridPosition(2, 0) },
                    { AgentB, new GridPosition(0, 4) }
                });
        }

        private static Route RouteFrom(params (int X, int Y)[] cells)
        {
            var positions = new GridPosition[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                positions[i] = new GridPosition(cells[i].X, cells[i].Y);
            }

            return Route.Create(positions);
        }

        private sealed class BoardViewFixture
        {
            public GameObject Root { get; private set; }

            public BoardView BoardView { get; private set; }

            public Tilemap PathMap { get; private set; }

            public TileBase PathTile { get; private set; }

            public static BoardViewFixture Create()
            {
                var fixture = new BoardViewFixture();
                fixture.Root = new GameObject("BoardViewRoot");
                Grid grid = fixture.Root.AddComponent<Grid>();
                fixture.BoardView = fixture.Root.AddComponent<BoardView>();
                fixture.PathMap = CreateTilemap("PathMap", fixture.Root.transform);
                Tilemap interactiveMap = CreateTilemap("InteractiveMap", fixture.Root.transform);
                Tilemap terrainMap = CreateTilemap("TerrainMap", fixture.Root.transform);
                fixture.PathTile = ScriptableObject.CreateInstance<Tile>();
                TileBase hoverTile = ScriptableObject.CreateInstance<Tile>();
                TileBase blockedTile = ScriptableObject.CreateInstance<Tile>();
                TileBase goalTile = ScriptableObject.CreateInstance<Tile>();

                SetField(fixture.BoardView, "grid", grid);
                SetField(fixture.BoardView, "interactiveMap", interactiveMap);
                SetField(fixture.BoardView, "pathMap", fixture.PathMap);
                SetField(fixture.BoardView, "terrainMap", terrainMap);
                SetField(fixture.BoardView, "hoverTile", hoverTile);
                SetField(fixture.BoardView, "pathTile", fixture.PathTile);
                SetField(fixture.BoardView, "blockedTile", blockedTile);
                SetField(fixture.BoardView, "goalTile", goalTile);
                return fixture;
            }

            public void Destroy()
            {
                Object.DestroyImmediate(Root);
                Object.DestroyImmediate(PathTile);
            }

            private static Tilemap CreateTilemap(string name, Transform parent)
            {
                var gameObject = new GameObject(name);
                gameObject.transform.SetParent(parent);
                return gameObject.AddComponent<Tilemap>();
            }

            private static void SetField(object target, string fieldName, object value)
            {
                FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(target, value);
            }
        }
    }
}
