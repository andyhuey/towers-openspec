## 1. Solution & Project Setup

- [x] 1.1 Create the .NET 8 solution file and the three projects: `TowersOfHanoi.Console` (exe), `TowersOfHanoi.Core` (classlib), `TowersOfHanoi.Core.Tests` (xUnit test project)
- [x] 1.2 Add project references: Console → Core, Core.Tests → Core
- [x] 1.3 Verify `dotnet build` succeeds on the empty solution

## 2. Core Domain Model

- [x] 2.1 Implement `Disk` (size) and `Tower` (ordered stack, `TryPeek`, push/pop) types
- [x] 2.2 Implement `Game` initial-state setup: N disks stacked on the leftmost tower, descending size order, per game-core spec
- [x] 2.3 Implement `Game.TryMove(sourceIndex, destIndex)` with legality checking (empty destination or larger top disk) per game-core spec
- [x] 2.4 Implement same-tower move as a no-op success that does not increment the move counter
- [x] 2.5 Implement player move counting (increments only on a move to a different tower)
- [x] 2.6 Implement win detection (all disks on rightmost tower, correct order)
- [x] 2.7 Implement optimal move count calculation (2^N - 1)
- [x] 2.8 Implement elapsed-time tracking via `Stopwatch`, and game status (`InProgress` / `Completed` / `QuitEarly`)

## 3. Core Unit Tests

- [x] 3.1 Test initial game state for various disk counts (3, 4, 9)
- [x] 3.2 Test legal moves (onto empty tower, onto larger disk)
- [x] 3.3 Test illegal moves (onto smaller disk, from empty tower) and confirm state is unchanged
- [x] 3.4 Test same-tower move: no board change, move count unchanged
- [x] 3.5 Test move counting increments only on cross-tower moves
- [x] 3.6 Test win detection: false during play, true only when all disks correctly stacked on rightmost tower
- [x] 3.7 Test optimal move count formula for N = 3 through 9
- [x] 3.8 Test full solve sequence (play out the standard recursive solution) ends in a won state with move count equal to the optimal count

## 4. Console: Start Screen

- [x] 4.1 Implement rendering of the canonical Tower of Hanoi rules summary and this app's controls summary
- [x] 4.2 Implement the disk-count numeric input with default 4 and clamping to [3, 9] per game-setup spec
- [x] 4.3 Wire start-screen confirmation to initialize a new `Game` and transition to the gameplay screen

## 5. Console: Gameplay Screen

- [x] 5.1 Implement full-frame rendering of the three towers, their disks, the cursor/selection, a lifted/hovering disk, and a status-message line
- [x] 5.2 Implement the Browsing input state: left/right moves the tower cursor
- [x] 5.3 Implement pick-up: space lifts the selected tower's top disk (no-op if tower is empty), entering the Lifted state
- [x] 5.4 Implement hover movement: left/right while Lifted moves the hover position between towers with no legality check
- [x] 5.5 Implement drop attempt: space while Lifted calls `Game.TryMove`; on success return to Browsing; on failure keep Lifted state at the same hover tower and show the rejection status message
- [x] 5.6 Implement Esc handling to end the game immediately as quit-early, from any gameplay state
- [x] 5.7 Implement automatic transition to the end screen when `Game` reports a win

## 6. Console: End Screen

- [x] 6.1 Implement end-screen rendering showing number of disks, player move count, optimal move count, total elapsed time, and completed/quit-early status per game-results spec

## 7. Cross-Platform Verification

- [ ] 7.1 Manually play a full game to completion on Linux, confirming rendering and all controls behave per spec
- [ ] 7.2 Manually play a full game to completion on Windows, confirming rendering and all controls behave per spec
- [ ] 7.3 Manually verify Esc-to-quit and an illegal-drop-then-retry sequence on at least one platform
