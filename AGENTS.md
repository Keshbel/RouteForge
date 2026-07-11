## Engineering Baseline
- Act as a senior Unity engineer.
- Optimize decisions in this order: performance, scalability, clean architecture, minimal GC allocations.
- Keep solutions simple and avoid duplication.

## Unity Editor Automation
- Do not use CLI-based Unity Editor automation for this project.
- Use MCP for Unity at `http://127.0.0.1:8080` for Unity Editor automation.
- If MCP for Unity is unavailable, report the blocker instead of falling back to CLI-based Unity Editor automation.

## Git Workflow
- When asked to create commits, use Conventional Commits 1.0.0.
- Prefer the format `<type>[optional scope]: <description>`, for example `fix(auth): handle empty token`.
- Use concise lowercase types such as `feat`, `fix`, `perf`, `refactor`, `docs`, `test`, `build`, `ci`, or `chore`.
- Mark breaking changes with `!` before the colon or a `BREAKING CHANGE:` footer.
- When asked to push, push only the current branch; do not create or switch branches.

## Unity Code Rules
- Prefer serialized references, DI, cached initialization, and explicit prefab wiring.
- Prefer placing each class, interface, and enum in a separate file. Private classes, interfaces, and enums may stay nested when that keeps the owning type clearer.
- Prefix enum names with `E`, for example `EBuildingInfoHintKind`.
- When implementing interface members, prefer `<inheritdoc />` instead of duplicating interface documentation.
- Avoid runtime `GetComponent`, `GetComponentsInChildren`, `FindObjectOfType`, and hierarchy scans in UI/gameplay paths.
- If lookup is unavoidable, isolate it to initialization/editor-only code, cache the result, and document why direct wiring is impractical.
- Validate required UI references early; screens must not discover siblings/children during interaction.
- Treat backend, mocked, and editor-preview data as incomplete. Add null/range/error boundaries at screen/service edges and keep UI usable with fallbacks.
- Keep preview/mock services behind interfaces so production flows do not depend on them.
- Avoid repeated allocations in refresh/update paths: reuse buffers, avoid LINQ in hot paths, and use explicit loops for frequent filtering/sorting.

## Documentation Rules
- Document new public classes, interfaces, methods, DTOs, properties, enums, enum values, and ScriptableObject configs with concise Russian XML docs.
- Use `<summary>`, plus `<param>`, `<returns>`, or `<value>` when applicable; describe purpose/meaning, not the member name.
- Public DTO properties and ScriptableObject config fields must explain the value and its effect; editor-facing configs should be clear for game designers.
- Architectural interfaces must document their role and method contracts; implementations may omit duplicate summaries unless they add special behavior.

## Workflow (mandatory before implementation)
1. Analyze the problem.
2. Propose architecture.
3. Design APIs.
4. List edge cases.

Only then implement production-ready code.
