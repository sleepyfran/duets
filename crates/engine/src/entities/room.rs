use super::Object;

/// Defines a room in the game. Must belong to a place.
#[derive(Clone, Default)]
pub struct Room {
    pub id: String,
    pub description: String,
    pub objects: Vec<Object>,
}
