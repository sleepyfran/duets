use common::entities::GameState;

use crate::shared::action::CliAction;
use crate::shared::context;

/// Sets the current global state.
pub fn modify_state(modify_fn: Box<dyn FnOnce(GameState) -> GameState>) -> Option<CliAction> {
    context::modify_global_context(Box::new(|context| context::Context {
        game_state: modify_fn(context.game_state),
        ..context
    }));

    None
}
