use engine::entities::{Band, Calendar, Character, City};

/// Defines the content of a savegame that can be saved and loaded.
#[derive(Builder, Clone, Default)]
pub struct GameState {
    pub band: Band,
    pub character: Character,
    pub current_city: City,
    pub calendar: Calendar,
}

impl GameState {
    /// Returns a clone of the current game state with the band set to the given one.
    pub fn with_band(self, band: Band) -> GameState {
        GameState { band, ..self }
    }
}
