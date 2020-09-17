use common::entities::Place;

use crate::context::Context;

/// Checks whether the character can currently go to the given place and if so modifies the context
/// with the updated location.
pub fn go_to_place(place: Place, context: Context) -> Context {
    context.modify_game_state(|game_state| game_state.with_place(place))
}