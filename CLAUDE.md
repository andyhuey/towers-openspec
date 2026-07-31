# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A text-mode Towers of Hanoi game: C#/.NET 8, cross-platform (Windows and Linux), console UI. Original requirements are in `requirements.md`. The project was built through OpenSpec (see `openspec/` below); `openspec/specs/` is the current source of truth for expected behavior.

## Commands

```bash
dotnet build                                              # build the whole solution
dotnet test                                                # run all tests
dotnet test --filter "FullyQualifiedName~GameTests.MethodName"  # run a single test
dotnet run --project src/TowersOfHanoi.Console              # play the game
```

## Architecture

Three projects, referenced as `Console → Core`, `Core.Tests → Core`:

- **`src/TowersOfHanoi.Core`** — all game logic, no UI dependency. `Game` is the aggregate: it owns the three `Tower`s, move legality (`TryMove`), move counting, win detection, elapsed time (via `Stopwatch`), and `OptimalMoveCount` (`2^N - 1`). `Tower` is a simple disk stack (`TryPeek`/`Push`/`TryPop`); `Disk` is just a size. `GameStatus` (`InProgress`/`Completed`/`QuitEarly`) and `MoveResult` (`Success`/`SourceEmpty`/`DestinationDiskTooSmall`) are the two enums the UI branches on.
- **`src/TowersOfHanoi.Console`** — three screens driven directly from `Program.cs` in sequence: `StartScreen.Run()` (disk count picker) → `new Game(diskCount)` → `GameplayScreen.Run(game)` → `EndScreen.Run(game)`. No screen holds a reference to another; state flows one-way through return values and the `Game` instance.
- **`tests/TowersOfHanoi.Core.Tests`** — xUnit, tests `Core` only (no UI tests; cross-platform console behavior is verified manually, see `openspec/changes/archive/2026-07-31-add-towers-of-hanoi/tasks.md` section 7).

### Gameplay screen input model

`GameplayScreen` is a small state machine over a private `InputState` (`Browsing(SelectedTower)` / `Lifted(OriginTower, HoverTower)`), driven by a `while` loop reading one key at a time:
- **Browsing**: Left/Right moves the tower cursor; Space picks up the top disk of the selected tower (no-op if empty) and transitions to `Lifted`.
- **Lifted**: Left/Right moves the hover position between towers with no legality check; Space calls `game.TryMove(OriginTower, HoverTower)` — on success returns to `Browsing` at the destination tower, on failure stays `Lifted` and sets a rejection status message.
- **Esc** is handled outside this state machine, at the top of the loop, and ends the game immediately as quit-early from any state.
- The whole frame is re-rendered from scratch (`Console.Clear()` + full redraw) after every state transition — there's no partial/diffed rendering.

## OpenSpec workflow

This repo uses OpenSpec (`openspec/config.yaml`, schema `spec-driven`) to plan and track feature work:
- `openspec/specs/<capability>/spec.md` — current main specs (game-core, game-results, game-setup, gameplay-controls). Treat these as the authoritative behavior contract.
- `openspec/changes/` — active (in-progress) changes; each has `proposal.md`, `specs/*.md` (deltas), `design.md`, `tasks.md`.
- `openspec/changes/archive/` — completed, archived changes, prefixed with the archive date.

Slash commands under `.claude/commands/opsx/` and matching skills (`openspec-propose`, `openspec-apply-change`, `openspec-sync-specs`, `openspec-archive-change`, `openspec-update-change`, `openspec-explore`) drive this workflow — use `/opsx:propose` to start new feature work rather than editing specs by hand.
