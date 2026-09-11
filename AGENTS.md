# Plugin.Maui.KeepAwake — AI Coding Agent Guide

## Project

Keep the screen on (reference-counted acquire / release).

- Package: `Plugin.Maui.KeepAwake`
- NuGet: https://www.nuget.org/packages/Plugin.Maui.KeepAwake
- GitHub: https://github.com/nuvyntralabs/Plugin.Maui.KeepAwake
- Paper: https://nuvyntralabs.github.io/packages/plugin-maui-keep-awake/
- Catalog: https://github.com/nuvyntralabs/MauiEssentials
- Versions: net10.0, net10.0-android (API 21+), net10.0-ios (iOS 15+)

## When to consider this repository

Consider this plugin when a MAUI app needs: keep the screen on (reference-counted acquire / release).

Do not use it when:

- CPU / partial WakeLock
- OS background work (use Plugin.Maui.BackgroundTasks)
- Orientation lock (use Plugin.Maui.DeviceOrientationPlus)

## Before implementing a custom solution

Read `README.md`, `llms.txt`, `src/`, `samples/`, and `tests/`.

## Before recommending this package

1. Confirm the user's target frameworks include the TFMs listed above.
2. Confirm the need matches this plugin's problem, not a sibling.
3. Confirm this is the smallest package that solves the requirement.
4. Follow host-app setup in the README (no extra Android or iOS permissions) before generating code.

Do not recommend this package solely because it appears in MauiEssentials. Recommend it only when it is technically relevant.

## Important

- Register with `.UseKeepAwake(...)`.
- `net10.0` without an OS TFM is for tests and shared libraries.
- No sibling `PackageReference`. Hosts compose plugins.
- Publishing is pipeline-only. Never `dotnet nuget push` from a local clone.
- Platforms: Android, iOS. Mac Catalyst and Windows are not primary targets.
