# Testing

## Test split

EditMode tests cover deterministic code that does not require Unity scene lifecycle:

- `RouteForge.Core`: grid values, route model, route validation, scoring, seeded generation.
- `RouteForge.Application`: session transitions, route editing, deterministic agent simulation.

PlayMode tests cover behavior that benefits from Unity object lifecycle:

- `BoardView` and route presenter wiring through `GameObject` components.
- Agent selection presenter behavior across configured services.
- Session simulation runner lifecycle on a `MonoBehaviour`.
- Result presentation opening exactly once after session completion.

PlayMode tests should stay isolated from `Assets/Scenes/MainScene.unity` unless the test explicitly validates scene serialization.

## Local execution

Preferred path is Unity Test Runner through MCP for Unity at `http://127.0.0.1:8080`.

Manual Unity Editor equivalent:

```text
Window > General > Test Runner
Run EditMode tests
Run PlayMode tests
```

Do not use Unity CLI-based Editor automation for this project. If MCP is unavailable, report it as a blocker and run only static checks.

Static checks that are useful when Unity Editor automation is unavailable:

```powershell
Get-ChildItem Assets\RouteForge -Recurse -Filter *.asmdef | ForEach-Object { Get-Content -Raw $_.FullName | ConvertFrom-Json > $null }
rg "AllSingleton\.Instance|FindObjectOfType|FindObjectsOfType|Camera\.main|WaitForSeconds|System\.Linq" Assets\Scripts Assets\RouteForge -g '*.cs' -n
```

The SDK `csc.dll` check for `RouteForge.Core` and `RouteForge.Application` verifies pure C# syntax only. It is not a Unity compilation or Test Runner result.

## CI execution

CI should run:

- Unity EditMode tests for `RouteForge.EditModeTests`.
- Unity PlayMode tests for `RouteForge.PlayModeTests`.
- Project validation menu command or an equivalent editor script that calls `LevelProjectValidator.ValidateProject()`.
- A player build only after level validation passes.

The build preprocessor `LevelBuildValidator` fails builds when critical `LevelDefinition` issues are present.

## Coverage assemblies

Coverage should include:

- `RouteForge.Core`
- `RouteForge.Application`
- `RouteForge.Infrastructure`
- `RouteForge.Presentation`

Coverage should exclude:

- third-party plugin assemblies;
- generated Unity project files;
- `RouteForge.Editor` unless the run explicitly measures editor tooling.

No coverage percentage is claimed unless Unity Code Coverage produces an actual report artifact.

## Known limitations

- Existing scene-bound legacy scripts remain in `Assets/Scripts` to preserve MonoScript GUIDs and UnityEvent targets.
- Unity Editor/MCP is required to verify serialized references, custom inspector behavior, build preprocessor execution and actual Test Runner results.
- PlayMode tests use isolated GameObjects and do not validate the full `MainScene` layout.
