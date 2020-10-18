use serde::{Deserialize, Serialize};

use super::{ActionRegistry, Band, Calendar, Character, Place, Position, Room};

/// Defines the content of a savegame that can be saved and loaded.
#[derive(Builder, Clone, Deserialize, Serialize, Default)]
pub struct GameState {
    pub band: Band,
    pub character: Character,
    pub calendar: Calendar,
    pub position: Position,
    pub action_registry: ActionRegistry,
}

impl GameState {
    /// Returns a copy of the current game state with the character modified by the given function.
    pub fn modify_character<F: FnOnce(Character) -> Character>(self, modify_fn: F) -> GameState {
        GameState {
            character: modify_fn(self.character),
            ..self
        }
    }

    /// Returns a copy of the current game state with the calendar modified by the given function.
    pub fn modify_calendar<F: FnOnce(Calendar) -> Calendar>(self, modify_fn: F) -> GameState {
        GameState {
            calendar: modify_fn(self.calendar),
            ..self
        }
    }

    /// Returns a copy of the current game state with the action registry modified by the given function.
    pub fn modify_action_registry<F: FnOnce(ActionRegistry) -> ActionRegistry>(
        self,
        modify_fn: F,
    ) -> GameState {
        GameState {
            action_registry: modify_fn(self.action_registry),
            ..self
        }
    }

    /// Returns a clone of the current game state with the band set to the given one.
    pub fn with_band(self, band: Band) -> GameState {
        GameState { band, ..self }
    }

    /// Returns a clone of the current game sate with the place set to the given one.
    pub fn with_place(self, place: Place) -> GameState {
        GameState {
            position: Position {
                country: self.position.country.clone(),
                city: self.position.city.clone(),
                place: place.clone(),
                room: place.rooms[0].clone(),
            },
            ..self
        }
    }

    /// Returns a clone of the current game state with the room set to the given one.
    pub fn with_room(self, room: Room) -> GameState {
        GameState {
            position: Position {
                country: self.position.country.clone(),
                city: self.position.city.clone(),
                place: self.position.place.clone(),
                room,
            },
            ..self
        }
    }
}
