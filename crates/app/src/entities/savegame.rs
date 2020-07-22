use engine::entities::{Character, City};

/// Defines the content of a savegame that can be saved and loaded.
pub struct Savegame {
    character: Character,
    current_city: City,
}
