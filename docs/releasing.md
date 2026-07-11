# Releasing

RouteForge releases are created from annotated Git tags. The release workflow runs tests again before any platform build is published.

## Tag format

Release tags must match one of these formats:

- stable: `vMAJOR.MINOR.PATCH`
- prerelease: `vMAJOR.MINOR.PATCH-prerelease`

Examples:

- `v1.0.0`
- `v1.1.0-rc.1`
- `v2.0.0-beta.2`

Tags such as `legacy-prototype-2022` do not match the workflow trigger and must not publish a release.

## Release commands

Run the release from an up-to-date `main` branch:

```bash
git checkout main
git pull --ff-only
git tag -a v1.0.0 -m "RouteForge 1.0.0"
git push origin v1.0.0
```

The release workflow will:

1. validate the tag format;
2. run EditMode and PlayMode tests;
3. stop before publishing if tests fail;
4. build StandaloneWindows64, StandaloneLinux64 and WebGL;
5. package every build as a separate zip;
6. generate `SHA256SUMS.txt`;
7. create a GitHub Release with generated release notes;
8. mark tags with a prerelease suffix as GitHub prereleases.

## Required secrets

Configure the Unity activation secrets in GitHub:

- `UNITY_LICENSE`
- `UNITY_EMAIL`
- `UNITY_PASSWORD`

The exact activation method must match the Unity license used by the project. The workflow uses `GITHUB_TOKEN` for release publishing and should not need a Personal Access Token.

## Branch protection

Configure repository protection before cutting a public release:

- require pull requests into `main`;
- require Unity tests;
- require Conventional commits;
- allow squash merge only;
- block force pushes;
- protect `v*` tags.

## Manual verification

Before creating a release tag, review:

- Unity version in `ProjectSettings/ProjectVersion.txt`;
- enabled build scenes in `ProjectSettings/EditorBuildSettings.asset`;
- test assemblies under `Assets/RouteForge/Tests`;
- release notes generated from merged pull requests;
- WebGL output manually in a browser when the first release workflow succeeds.
