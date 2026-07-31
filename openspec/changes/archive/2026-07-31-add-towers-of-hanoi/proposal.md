## Why

We want a simple, playable Towers of Hanoi game with a text-mode UI, built as a clean C# solution (console shell, logic library, unit tests) that runs on both Windows and Linux under .NET 8. No code exists yet — this change stands up the whole application from scratch.

## What Changes

- New .NET 8 solution with three projects: `TowersOfHanoi.Console` (exe, UI/input/rendering), `TowersOfHanoi.Core` (class library, all game rules and state), `TowersOfHanoi.Core.Tests` (unit tests against Core).
- Start screen: canonical Tower of Hanoi rules summary, this app's controls summary, and a numeric disk-count input (default 4, clamped to [3, 9]).
- Gameplay screen: renders three towers and disks in text; cursor navigation between towers; pick-up/hover/drop disk-moving interaction (space to lift the top disk, arrows to hover a target tower, space to attempt a drop); illegal drops are rejected with a status message and keep the disk lifted for retry; same-tower drops are a legal no-op that doesn't count as a move; Esc quits at any time.
- Win detection: game ends when all disks are stacked on the rightmost tower in ascending size order.
- End screen: number of disks used, player's move count, optimal move count (2^n - 1) for comparison, total elapsed time, and completed-vs-quit-early status. ("Number of disks moved" from the original requirements draft is dropped as redundant with the move count.)

## Capabilities

### New Capabilities
- `game-core`: Tower/disk model, move legality rules, win detection, player move counting, optimal move count calculation. No UI dependency.
- `game-setup`: Start screen content (rules summary, controls summary) and disk-count input/validation.
- `gameplay-controls`: Gameplay screen rendering and the pick-up/hover/drop input state machine, including illegal-drop feedback and quit-at-any-time.
- `game-results`: End screen content and the stats it must display.

### Modified Capabilities
(none — greenfield project)

## Impact

- New solution and three new projects (`TowersOfHanoi.Console`, `TowersOfHanoi.Core`, `TowersOfHanoi.Core.Tests`); no existing code affected.
- No external dependencies beyond .NET 8 SDK and a unit test framework for the Tests project.
