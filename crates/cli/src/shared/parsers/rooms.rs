use app::world::World;
use common::entities::Room;

use crate::context::Context;
use crate::shared::lang;

/// Attempts to parse a room from all the available ones in the current room.
pub fn parse_room_from(args: &[String], global_context: &Context) -> Option<Room> {
    let rooms = global_context.get_rooms_of_place();
    let room_name = lang::transformations::join_vec(args);

    rooms.iter().find(|room| room.name == room_name).cloned()
}
