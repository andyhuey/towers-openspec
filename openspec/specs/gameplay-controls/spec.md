# gameplay-controls Specification

## Purpose

Defines the gameplay screen's visual representation of the towers and disks, and the keyboard-driven pick-up/hover/drop interaction used to move disks, including feedback for illegal moves and quitting early.

## Requirements

### Requirement: Gameplay Screen Rendering
The system SHALL display a text-mode representation of the three towers and their disks, updated after every state change (cursor movement, pick-up, hover, drop, or rejected drop).

#### Scenario: Screen reflects current state
- **WHEN** the game state changes for any reason
- **THEN** the displayed towers and disks are updated to match the new state

### Requirement: Tower Cursor Navigation
While no disk is lifted, the system SHALL move the selection cursor between towers when the player presses the left or right arrow key.

#### Scenario: Move cursor left
- **WHEN** no disk is lifted and the player presses the left arrow key
- **THEN** the cursor moves to the adjacent tower to the left, if one exists

#### Scenario: Move cursor right
- **WHEN** no disk is lifted and the player presses the right arrow key
- **THEN** the cursor moves to the adjacent tower to the right, if one exists

### Requirement: Disk Pick-Up
The system SHALL lift the top disk of the currently selected tower when the player presses the space bar while no disk is lifted and the selected tower is not empty, entering a lifted/hovering state over that tower.

#### Scenario: Pick up top disk
- **WHEN** no disk is lifted, the selected tower has at least one disk, and the player presses space
- **THEN** the tower's top disk becomes lifted and is shown hovering over its origin tower

#### Scenario: Pick up from empty tower
- **WHEN** no disk is lifted and the selected tower is empty, and the player presses space
- **THEN** no disk is lifted and the state is unchanged

### Requirement: Hover Movement
While a disk is lifted, the system SHALL move the hover position between towers when the player presses the left or right arrow key, without performing any move-legality check.

#### Scenario: Hover to adjacent tower
- **WHEN** a disk is lifted and the player presses the left or right arrow key
- **THEN** the hover position moves to the adjacent tower in that direction, if one exists, regardless of whether dropping there would be legal

### Requirement: Disk Drop Attempt
While a disk is lifted, the system SHALL attempt to drop it onto the tower under the current hover position when the player presses the space bar. A legal drop places the disk on that tower and returns to the non-lifted state. An illegal drop is rejected, the disk remains lifted over the same hover position, and the system displays a status message explaining the rejection.

#### Scenario: Legal drop
- **WHEN** a disk is lifted, hovering over a tower, and the drop is legal per the game's move rules
- **THEN** the disk is placed on that tower and the lifted state ends

#### Scenario: Illegal drop
- **WHEN** a disk is lifted, hovering over a tower, and the drop is illegal per the game's move rules
- **THEN** the disk remains lifted over the same tower, the state is otherwise unchanged, and a status message explains why the drop was rejected

#### Scenario: Drop back on origin tower
- **WHEN** a disk is lifted and the player drops it while hovering over the tower it was lifted from
- **THEN** the disk is placed back on its origin tower and the lifted state ends, without counting as a move

### Requirement: Quit At Any Time
The system SHALL end the current game immediately, marking it as quit early, whenever the player presses the Esc key.

#### Scenario: Quit during gameplay
- **WHEN** the player presses Esc at any point during gameplay
- **THEN** the game ends immediately and is recorded as quit early rather than completed

### Requirement: Automatic End On Win
The system SHALL end the current game and mark it as completed as soon as the win condition is met, without requiring further player input.

#### Scenario: Last disk placed to win
- **WHEN** a legal drop results in the win condition being met
- **THEN** the game ends immediately and is recorded as completed
