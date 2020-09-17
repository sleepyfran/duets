use common::entities::GameState;

use crate::shared::action::CliAction;
use crate::shared::context;

/// Sets the current global state.
pub fn set_state(game_state: GameState) -> CliAction {
    CliAction::SideEffect(
        Box::new(
            move || {
                context::modify_global_context(Box::new(move |context| context::Context {
                    game_state,
                    ..context
                }));

                CliAction::Continue
            }
        )
    )
}
