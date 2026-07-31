## Purpose

Defines the end-of-game screen shown after a game completes or is quit early, and the statistics it must present to the player.

## ADDED Requirements

### Requirement: End Screen Statistics
When a game ends, whether by completion or by quitting early, the system SHALL display the number of disks used in the game, the number of moves made by the player, the optimal (minimum possible) move count for the chosen disk count, the total elapsed time, and whether the game was completed or quit early.

#### Scenario: Game completed normally
- **WHEN** the player solves the puzzle
- **THEN** the end screen shows the number of disks, the player's move count, the optimal move count, total elapsed time, and a "completed" status

#### Scenario: Game quit early
- **WHEN** the player presses Esc before solving the puzzle
- **THEN** the end screen shows the number of disks, the player's move count so far, the optimal move count, total elapsed time, and a "quit early" status
