use common::entities::{Identity, Object, Room};
use common::extensions::option::OptionCloneExtensions;

use crate::context::Context;
use crate::data;
use crate::world::places::Places;

/// Adds functionality to quickly retrieve information about the rooms.
pub trait Rooms {
    /// Returns all the available rooms in the current place in which the character is located.
    fn get_rooms_of_place(&self) -> Vec<Room>;

    /// Returns all the reachable rooms from the current place. This effectively removes the current
    /// room that the character is in and those that are locked from entering for any reason.
    fn get_reachable_rooms_of_place(&self) -> Vec<Room>;

    /// Returns the current room in which the character is located.
    fn get_current_room(&self) -> Room;

    /// Returns all the objects in the current room.
    fn get_objects_in_room(&self) -> Vec<Object>;
}

impl Rooms for Context {
    fn get_rooms_of_place(&self) -> Vec<Room> {
        self.get_current_place().rooms
    }

    fn get_reachable_rooms_of_place(&self) -> Vec<Room> {
        let current_room = self.get_current_room();
        self.get_rooms_of_place()
            .into_iter()
            .filter(|room| room.id() != current_room.id())
            .collect()
    }

    fn get_current_room(&self) -> Room {
        data::find_in(
            &self.get_current_place().rooms,
            &self.game_state.position.room,
        )
        .unwrap_cloned()
    }

    fn get_objects_in_room(&self) -> Vec<Object> {
        self.get_current_room().objects
    }
}
