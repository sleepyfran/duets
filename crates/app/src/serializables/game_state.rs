use engine::entities::{Calendar, Character, City};

/// Defines the content of a savegame that can be saved and loaded.
#[derive(Builder)]
pub struct GameState {
    pub character: Character,
    pub current_city: City,
    pub calendar: Calendar,
}
