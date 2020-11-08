use common::entities::GameState;
pub use storage::{Error, IoResult};

/// Saves the given game_state into the storage and returns its result.
pub fn save(game_state: GameState) -> IoResult<()> {
    storage::save_game_state(game_state)
}

/// Retrieves the game_state from disk.
pub fn load() -> IoResult<GameState> {
    storage::retrieve_game_state()
}
