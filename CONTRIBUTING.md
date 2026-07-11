# Contributing

RouteForge uses a small, explicit workflow so code review can focus on behavior, architecture and Unity serialization safety.

## Development workflow

1. Create a topic branch from `main`.
2. Keep changes scoped to one concern.
3. Run the relevant EditMode and PlayMode tests before opening a pull request.
4. Document any Unity Editor manual checks when scenes, prefabs or ScriptableObjects change.
5. Open a pull request into `main`.

The repository uses squash merge only. The final squash commit must follow the Conventional Commit rules below.

## Conventional commits

Every new commit subject must match:

```text
type(scope): lowercase description
```

Allowed types:

- `feat`
- `fix`
- `docs`
- `style`
- `refactor`
- `perf`
- `test`
- `build`
- `ci`
- `chore`
- `revert`

Rules:

- scope is required;
- description starts with a lowercase letter;
- maximum subject length is 100 characters;
- recommended subject length is 72 characters or less;
- merge commits are ignored by CI commit validation;
- CI validates only commits introduced by the current pull request or push, not legacy repository history.

Examples:

```text
test(routes): cover invalid route adjacency
ci(github): validate commits and run unity tests
docs(architecture): document technical decisions
```

## Unity version

Use the Unity version declared in `ProjectSettings/ProjectVersion.txt`. At the time this workflow was documented, the project uses Unity `6000.3.11f1`.

## Tests

The CI workflow runs these test modes:

- EditMode tests from `RouteForge.EditModeTests`;
- PlayMode tests from `RouteForge.PlayModeTests`.

Local execution can be done through the Unity Test Runner in the Unity Editor. This repository does not require command-line Unity Editor automation for local contribution.

## Required GitHub secrets

CI and release workflows expect Unity activation credentials configured as repository or organization secrets:

- `UNITY_LICENSE`
- `UNITY_EMAIL`
- `UNITY_PASSWORD`

The exact activation method must match the Unity license type used by the repository. Do not add Personal Access Tokens for release publishing while `GITHUB_TOKEN` is sufficient.

## Branch protection

Recommended `main` settings:

- require a pull request before merging;
- require Unity test checks and Conventional commit validation;
- allow squash merge only;
- block force pushes;
- prevent deleting the branch.

Recommended tag protection:

- protect `v*` tags;
- restrict release tag creation to maintainers or release automation.

## Repository hygiene

Do not commit generated IDE directories, local Unity cache data, build output, credentials, machine-specific paths or package downloads. Keep `.meta` files with their assets so Unity GUIDs remain stable.
