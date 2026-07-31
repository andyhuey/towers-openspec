# Towers of Hanoi

A text-mode Towers of Hanoi game for the console. C#/.NET 8, cross-platform (Windows and Linux).

## Running

```bash
dotnet run --project src/TowersOfHanoi.Console
```

## Controls

| Key | Action |
| --- | --- |
| Left / Right | Move the cursor between towers, or move a lifted disk between towers |
| Space | Pick up the selected tower's top disk, or drop a lifted disk |
| Esc | Quit the current game early |

Move all disks from the leftmost tower to the rightmost tower, one at a time, never placing a disk on top of a smaller one. Choose the number of disks (3-9, default 4) on the start screen. The end screen shows your move count, the optimal move count, elapsed time, and whether you completed the game or quit early.

## Development

```bash
dotnet build   # build the solution
dotnet test    # run the unit tests
```

The solution has three projects:
- `src/TowersOfHanoi.Core` — game logic, no UI dependency
- `src/TowersOfHanoi.Console` — console UI
- `tests/TowersOfHanoi.Core.Tests` — xUnit tests for `Core`

See `CLAUDE.md` for architecture details and `openspec/` for the behavior specs.
