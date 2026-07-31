# game-core Specification

## Purpose

Defines the interface-independent Towers of Hanoi rules: tower/disk state, legal-move checking, win detection, and move/optimal-move counting, with no dependency on any user interface.

## Requirements

### Requirement: Initial Game State
The system SHALL initialize a game with a configurable number of disks, all stacked on the leftmost tower in descending size order (largest at the bottom, smallest at the top), with the other two towers empty.

#### Scenario: New game setup
- **WHEN** a game is started with N disks
- **THEN** the leftmost tower contains all N disks ordered largest-at-bottom to smallest-at-top, and the middle and rightmost towers are empty

### Requirement: Move Legality
The system SHALL consider a move of the top disk from a source tower to a destination tower legal only if the source tower has at least one disk, and the destination tower is either empty or its top disk is larger than the disk being moved.

#### Scenario: Move onto empty tower
- **WHEN** the source tower's top disk is moved to an empty destination tower
- **THEN** the move is legal

#### Scenario: Move onto larger disk
- **WHEN** the source tower's top disk is smaller than the destination tower's top disk
- **THEN** the move is legal

#### Scenario: Move onto smaller disk
- **WHEN** the source tower's top disk is larger than the destination tower's top disk
- **THEN** the move is illegal

### Requirement: Move Execution and Counting
The system SHALL execute only legal moves and SHALL increment the player's move count only when a disk is moved to a tower different from its source tower.

#### Scenario: Move to a different tower
- **WHEN** a legal move relocates a disk from one tower to a different tower
- **THEN** the disk is relocated and the player's move count increases by one

#### Scenario: Move back to the same tower
- **WHEN** a disk is placed back onto the tower it was picked up from
- **THEN** the disk's position is unchanged and the player's move count does not increase

### Requirement: Win Detection
The system SHALL report the game as won when all disks are stacked on the rightmost tower in descending size order (largest at the bottom, smallest at the top), and the other two towers are empty.

#### Scenario: All disks relocated
- **WHEN** every disk has been legally moved so that the rightmost tower holds all disks in the correct order and the other towers are empty
- **THEN** the system reports the game as won

#### Scenario: Game in progress
- **WHEN** at least one disk remains outside the rightmost tower, or the rightmost tower does not yet hold every disk
- **THEN** the system reports the game as not yet won

### Requirement: Optimal Move Count
The system SHALL calculate the minimum number of moves required to solve the puzzle for N disks as 2^N - 1.

#### Scenario: Optimal count for a given disk count
- **WHEN** a game is configured with N disks
- **THEN** the system reports 2^N - 1 as the optimal (minimum possible) move count
