# Duets > Crates 🎸

The game is divided into different `crates` that act as different layers. These layers are:

- **cli (and ui in the future)** - Outermost layer that takes care of interacting with the user and keep the game state.
- **game** - Orchestration layer between the user interface and the game core logic. Takes care of things like validations, database handling and logic that can generally be shared between the `cli` and the `ui` interface flavors.
- **simulation** - Core simulation logic. The purpose of having its own layer is to keep it completely stateless and (hopefully) pure and as testable as possible.

There's also other cross-cutting layers:

- **common** - Contains anything that can be used accross all crates.
- **storage** - Contains logic regarding the storage and retrieval of data.
