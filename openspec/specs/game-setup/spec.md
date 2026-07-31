# game-setup Specification

## Purpose

Defines the start screen shown when the application launches: an explanation of the game and controls, and the disk-count input that configures the game to be played.

## Requirements

### Requirement: Start Screen Content
The system SHALL display a start screen, on launch, containing a summary of the canonical Tower of Hanoi game and a summary of this application's controls.

#### Scenario: Application launched
- **WHEN** the application starts
- **THEN** the start screen is displayed with a rules summary and a controls summary before any gameplay begins

### Requirement: Disk Count Input
The system SHALL provide a numeric input on the start screen for choosing the number of disks to play with, defaulting to 4, and SHALL clamp the accepted value to the range 3 to 9 inclusive.

#### Scenario: Default value
- **WHEN** the start screen is shown and the player has not changed the disk count
- **THEN** the disk count is 4

#### Scenario: Value within range
- **WHEN** the player sets the disk count to a value between 3 and 9 inclusive
- **THEN** the game starts with that number of disks

#### Scenario: Value below minimum
- **WHEN** the player attempts to set the disk count below 3
- **THEN** the system clamps the value to 3

#### Scenario: Value above maximum
- **WHEN** the player attempts to set the disk count above 9
- **THEN** the system clamps the value to 9
