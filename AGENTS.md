# AGENTS.md

This file provides guidance for coding agents when working with code in this repository.

# About Duets

Duets is a music/life simulation game built with F# as an interactive CLI game. Players create their own character and band, exploring cities, performing gigs, composing songs, and managing their music career and personal life.

**Important:** The game uses a fork of Spectre.Console as a git submodule. Always clone with `git clone --recurse-submodules` and ensure the submodule is present when building.

**Note:** First build downloads a quantized version of Gemma 3 1B (~1GB) for LLM-generated content (descriptions, dialogues), so initial build will be slower.

# Development Commands

## Building and Running

```bash
# Build the project
dotnet build

# Run the game
dotnet run --project src/Cli/Cli.fsproj

# Or using Docker
docker build -t duets .
docker run -it duets
```

## Testing

```bash
# Run all tests
dotnet test

# Run tests for a specific project
dotnet test tests/Simulation.Tests/Simulation.Tests.fsproj
dotnet test tests/Entities.Tests/Entities.Tests.fsproj
dotnet test tests/Agents.Tests/Agents.Tests.fsproj
dotnet test tests/Data.Tests/Data.Tests.fsproj

# Run a specific test (use --filter with test name pattern)
dotnet test --filter "TestName~SomeSpecificTest"
```

# Architecture Overview

Duets uses an **effect-based event sourcing** architecture with functional programming principles. All game logic is pure, deterministic, and testable.

## Project Structure

- **Duets.Entities** - Domain model: All game types, entities, and state structure. Contains type definitions and validation logic. All types have corresponding lenses defined in `Lenses.fs` (see Aether library).

- **Duets.Simulation** - Game logic layer: Pure, stateless functions that take game state and return effects. **NEVER contains side effects or translation strings** (those belong in CLI). Contains:
    - `Interactions/` - Player actions (Sleep.fs, Item.Interactions.fs, etc.)
    - `Effects/` - Effect definitions and application logic
    - `Events/` - Domain event handlers (Band, Career, Character, Concert, etc.)
    - `Queries/` - Read-only state queries using lenses (27 query modules)
    - `Config/` - Game balance parameters and configuration

- **Duets.Agents** - State management: MailboxProcessor-based concurrent agents
    - `StateAgent` - Holds current game state, thread-safe get/set operations
    - `SavegameAgent` - Asynchronous save/load with migration support
    - `RandomGenAgent` - Encapsulated RNG for deterministic behavior

- **Duets.Cli** - UI layer: Text-based interface, command handlers, and rendering
    - `Scenes/` - Scene-based UI (MainMenu, World, Phone, etc.)
    - `Components/` - Reusable UI components (Table, Calendar, Map, CommandPrompt)
    - Commands and effect interpretation

- **Duets.Data** - Static content: World layout (cities, venues), genres, instruments, careers, NPCs. Also contains savegame migration logic.

- **Duets.Common** - Shared utilities: F#-friendly wrappers for .NET methods, general-purpose functions.

## Core Architectural Patterns

### Effect System

The game uses an effect-driven state management pattern:

```
User Action → Interaction → Effect(s) Generated → Simulation Applies Effects → State Updated → Associated Effects Triggered → UI Re-rendered
```

**Key files:**
- `Duets.Entities/Types/Effect.Types.fs` - 100+ effect types
- `Duets.Simulation/Simulation.fs` - Core tick engine that recursively applies effects

**Effect chains:**
```fsharp
type AssociatedEffectType =
    | BreakChain of EffectFn list    // Discard remaining effects
    | ContinueChain of EffectFn list  // Continue processing
```

Effects can interrupt chains (e.g., character hospitalized stops all actions).

### State Management with Lenses

All state updates use **Aether lenses** for type-safe, composable, immutable updates:
- Getter/Setter pairs defined in `Duets.Entities/Lenses.fs`
- Example: `Lenses.State.bands_` for accessing/modifying bands
- Read the [Aether guide for Lenses](https://xyncro.tech/aether/guides/lenses.html)

### Time Model

- Day divided into `DayMoment` units (180 minutes each)
- Actions consume `TurnMinutes`
- Time advancing triggers cascading effects (hunger, drunkenness, etc.)
- Configured in `Config.Time`

### Scene-Based UI

```fsharp
type Scene =
    | MainMenu | CharacterCreator | BandCreator
    | World    // Main gameplay loop
    | Phone    // Mobile apps interface
    | Exit
```

Recursive scene dispatcher in `Program.fs`: each scene returns the next scene.

## Data Flow Example

1. User types command (e.g., "sleep until 10:00") in CLI
2. CLI calls `Interactions.Sleep.sleep` with parameters
3. Sleep returns `Effect list`: `[CharacterSlept(...), CharacterAttributeChanged(...)]`
4. `Simulation.tickMultiple` applies effects recursively
5. Each effect triggers associated effects (e.g., `CharacterSlept` → `TimeAdvanced`)
6. Final state stored via `StateAgent.set`
7. `SavegameAgent` asynchronously persists state to disk
8. UI subscribes to state changes and re-renders

## Adding a New Feature

1. **Define types** in `Duets.Entities` project
    - Add domain types to appropriate `Types.fs` file
    - Add lenses to `Lenses.fs` if needed
    - Add effects to `Effect.Types.fs`

2. **Implement logic** in `Duets.Simulation` project
    - Create pure functions that receive state and return effects
    - No side effects, no translation strings (those go in CLI)
    - Add state update logic to `State.fs`

3. **Write tests** in `tests/Simulation.Tests/`
    - All core game logic should have unit tests
    - Use pure functional approach for deterministic testing

4. **Add UI** in `Duets.Cli` project
    - Interpret effects and display results
    - Use existing components from `Components/` folder (layouts, notifications, tables, bar charts)
    - Add command handlers if needed

5. **Add static data** in `Duets.Data` if needed
    - World layout, careers, genres, items, etc.

## Key Technical Insights

- **Pure Functional Simulation**: No mutable state in Simulation layer. Same input state + same effects = same output state. Fully deterministic and testable.

- **Immutable Records**: No null references (use `Option` types). State mutations through lens composition.

- **Modular Event System**: Each domain (Band, Career, Character, Concert) has an `Events` module returning `AssociatedEffectType` from effects.

- **Migration Strategy**: Savegame versioning with incremental migrations in `Duets.Data`. Old saves transform to new schema.

- **Configuration-Driven Balance**: All game balance values in `Duets.Simulation.Config` (energy rates, mood modifiers, concert mechanics, etc.).

## Test Projects

- `Test.Common` - Shared test utilities and builders
- `Simulation.Tests` - Core game logic tests (primary test suite)
- `Entities.Tests` - Domain entity validation tests
- `Agents.Tests` - Concurrent agent tests
- `Data.Tests` - Static data and migration tests

## Important Principles

- **Simulation layer is pure**: Never add side effects or UI strings to Duets.Simulation
- **CLI interprets effects**: All user-facing text and rendering happens in Duets.Cli
- **Use lenses for state updates**: Never manually update nested records
- **Test core logic**: All game mechanics in Simulation should have tests
- **Follow the effect pattern**: Actions return effects, effects modify state, state triggers more effects
