use common::entities::GameState;

use crate::data::database::Database;

/// Defines the current context of the game and adds some utility methods to query information
/// about the current status.
#[derive(Clone, Default)]
pub struct Context {
    pub database: Database,
    pub game_state: GameState,
}

impl Context {
    /// Returns a copy of the current context with the game state modified by the given function.
    pub fn modify_game_state<F: FnOnce(GameState) -> GameState>(self, modify_fn: F) -> Context {
        Context {
            game_state: modify_fn(self.game_state),
            ..self
        }
    }
}
