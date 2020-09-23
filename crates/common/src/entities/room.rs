use serde::{Deserialize, Serialize};

use super::Identity;
use super::Object;

/// Defines a room in the game. Must belong to a place.
#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Room {
    pub id: String,
    pub name: String,
    pub description: String,
    pub objects: Vec<Object>,
}

impl Identity for Room {
    fn id(&self) -> String {
        self.id.clone()
    }
}
