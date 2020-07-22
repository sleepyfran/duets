use engine::entities::{Character, City};

/// Defines the content of a savegame that can be saved and loaded.
pub struct GameState {
    pub character: Character,
    pub current_city: City,
}
