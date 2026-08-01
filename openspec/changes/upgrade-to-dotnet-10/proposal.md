## Why

The project targets .NET 8 across all three projects. .NET 10 is installed on the development machine and is the current LTS release; moving onto it keeps the toolchain current and picks up runtime/SDK improvements with no impact on game behavior.

## What Changes

- Retarget `TowersOfHanoi.Core`, `TowersOfHanoi.Console`, and `TowersOfHanoi.Core.Tests` from `net8.0` to `net10.0`.
- Bump test dependencies (`Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, `coverlet.collector`) to versions compatible with .NET 10, if the current pinned versions don't build/run cleanly on the new TFM.
- Update `CLAUDE.md` references to .NET 8 to say .NET 10.
- No application code, game logic, or UI behavior changes.

## Capabilities

### New Capabilities
None.

### Modified Capabilities
None — this is a pure tooling/target-framework change with no spec-level behavior change (`skip_specs: true`).

## Impact

- Affected files: all three `.csproj` files, `CLAUDE.md`.
- Build/test commands (`dotnet build`, `dotnet test`, `dotnet run`) must continue to work unchanged.
- Requires the .NET 10 SDK to be present wherever the project is built (already installed locally: `10.0.110`).
