## Context

Greenfield project (see proposal.md - Why). Three projects are mandated by requirements.md: a console exe, a UI-independent logic library, and a unit test project. `System.Console` is used directly for cross-platform (Windows/Linux) text-mode input and rendering — no third-party TUI library is required for a display this simple (three towers, a cursor, a hovering disk, and a status line).

## Goals / Non-Goals

**Goals:**
- Keep `TowersOfHanoi.Core` entirely free of `System.Console` or any I/O — it should be testable with plain unit tests and reusable behind any future UI.
- Give `TowersOfHanoi.Console` a small, explicit screen/state model so the start → gameplay → end flow and the Browsing/Lifted input state machine are easy to follow and extend.

**Non-Goals:**
- No persistence (no saved games, no high-score history) — each run is a single in-memory session.
- No mouse support, resizing/reflow logic, or color theming beyond what's needed to show a highlighted/hovering disk and a status message.
- No animation between disk positions; state transitions render immediately.

## Decisions

**Core domain model**: `Game` owns three `Tower` instances (each an ordered stack of `Disk`, largest-index = bottom), a move counter, an elapsed-time source, and game status (`InProgress` / `Completed` / `QuitEarly`). `Tower.TryPeek()` exposes only the top disk, since only the top disk is ever addressable — this makes "pick up top disk only" structurally enforced rather than something callers must remember to check.

Move application is a single `Game.TryMove(sourceIndex, destIndex)` returning a legality result (success, or a reason enum like `DestinationDiskTooSmall`, `SourceEmpty`). The Console layer calls this exactly once, at drop time — hover movement (left/right while lifted) never calls into Core at all, since hovering has no rule consequence. This keeps the "no legality check while hovering" requirement (gameplay-controls) trivially true by construction: Core simply never sees hover events.

`TryMove` treats source == destination as an automatic success with zero board mutation and no move-count increment, satisfying the same-tower-drop requirement without a special case in the Console layer.

**Console input state machine**: Two explicit states, `Browsing { SelectedTower }` and `Lifted { OriginTower, HoverTower }`, held in the game loop (not inside Core). Left/right, space, and Esc are interpreted differently per state per the gameplay-controls spec. An illegal `TryMove` result on space-while-Lifted keeps the loop in `Lifted` (same `HoverTower`) and sets a status-line string to show on next render, rather than raising an exception or aborting the loop — invalid moves are expected user input, not error conditions.

**Rendering**: Full-frame redraw (clear + rewrite) on every state change rather than incremental diffing. At up to 9 disks and 3 towers the frame is small enough that flicker/performance is a non-issue, and full redraw avoids a whole class of stale-frame bugs.

**Timing**: Elapsed time is measured with `System.Diagnostics.Stopwatch`, started when gameplay begins (after the start screen's disk-count is confirmed) and read at game end — not wall-clock timestamps, to stay robust against system clock changes.

**Testing**: `TowersOfHanoi.Core.Tests` uses xUnit (standard choice for new .NET 8 projects). Tests target `Game`/`Tower` behavior directly: legal/illegal moves, same-tower no-op, win detection, and optimal move count — no Console/rendering code is under test since it has no independent logic to verify beyond manual play.

## Risks / Trade-offs

- **Terminal capability differences (Windows Terminal vs. Linux TTYs)** → Mitigation: use only `System.Console`'s cross-platform APIs (`Console.ReadKey`, `Console.SetCursorPosition`, `Console.Write`), avoid ANSI-only or Windows-only escape sequences, and manually smoke-test on both platforms before considering the change done.
- **Full-frame redraw could flicker on some terminals** → Mitigation: acceptable at this scale (small fixed-size text frame); revisit only if manual testing shows a real problem.
