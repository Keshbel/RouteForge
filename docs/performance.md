# Performance Measurement Template

## Scope

Use this document to record real measurements before and after performance work. Do not add estimated numbers.

## Profiler markers

Current marker names:

- `RouteForge.RouteValidation`
- `RouteForge.LevelGeneration`
- `RouteForge.SimulationUpdate`

## Measurement setup

```text
Date:
Unity version:
Target platform:
Build type:
Scene or test:
Profiler mode:
Deep Profile enabled:
Device / CPU / GPU:
Git commit:
```

## Scenario

```text
Scenario name:
Steps to reproduce:
Input size:
Agent count:
Route count:
Board size:
Blocked cell count:
Run duration:
Warm-up duration:
```

## Results

```text
Marker:
Median time:
95th percentile:
GC allocations:
Frame time impact:
Notes:
```

## Change log

```text
Change:
Reason:
Expected effect:
Measured effect:
Regression risk:
Follow-up:
```

## Hygiene checklist

- No `FindObjectOfType`, `FindObjectsOfType`, `Camera.main` or hierarchy scans in runtime hot paths.
- No LINQ in frequent update, simulation, validation or route refresh paths.
- No collection mutation during enumeration.
- No `Vector3.zero` sentinel values.
- Component references are serialized or cached during initialization.
- Tests or profiler captures exist before keeping non-trivial micro-optimizations.
