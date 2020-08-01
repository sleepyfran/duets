use common::serializables::GameState;

use crate::shared::action::CliAction;
use crate::shared::context;

/// Sets the current global state.
pub fn set_state(game_state: GameState) -> Option<CliAction> {
    context::modify_global_context(Box::new(|context| context::Context {
        game_state,
        ..context
    }));

    None
}
