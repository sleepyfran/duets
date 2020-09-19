use common::entities::{Place, Room};

use crate::context::Context;

/// Checks whether the character can currently go to the given place and if so modifies the context
/// with the updated location.
pub fn go_to_place(place: Place, context: Context) -> Context {
    context.modify_game_state(|game_state| game_state.with_place(place))
}

/// Checks whether the character can currently go to the given room and if so modifies the context
/// with the updated location.
pub fn go_to_room(room: Room, context: Context) -> Context {
    context.modify_game_state(|game_state| game_state.with_room(room))
}
