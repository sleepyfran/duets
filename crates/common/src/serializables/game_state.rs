use serde::{Deserialize, Serialize};

use engine::entities::{Band, Calendar, Character, City};

use super::{BandDef, CalendarDef, CharacterDef, CityDef};

/// Defines the content of a savegame that can be saved and loaded.
#[derive(Builder, Clone, Deserialize, Serialize, Default)]
pub struct GameState {
    #[serde(with = "BandDef")]
    pub band: Band,
    #[serde(with = "CharacterDef")]
    pub character: Character,
    #[serde(with = "CityDef")]
    pub current_city: City,
    #[serde(with = "CalendarDef")]
    pub calendar: Calendar,
}

impl GameState {
    /// Returns a clone of the current game state with the band set to the given one.
    pub fn with_band(self, band: Band) -> GameState {
        GameState { band, ..self }
    }
}
