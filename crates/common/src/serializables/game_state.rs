use serde::{Deserialize, Serialize};

use engine::entities::{Band, Calendar, Character, Place};

use super::{BandDef, CalendarDef, CharacterDef, Position};

/// Defines the content of a savegame that can be saved and loaded.
#[derive(Builder, Clone, Deserialize, Serialize, Default)]
pub struct GameState {
    #[serde(with = "BandDef")]
    pub band: Band,
    #[serde(with = "CharacterDef")]
    pub character: Character,
    #[serde(with = "CalendarDef")]
    pub calendar: Calendar,
    pub position: Position,
}

impl GameState {
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
}
