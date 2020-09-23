use serde::{Deserialize, Serialize};

use super::Identity;
use super::Room;

/// Defines a place in the game. Must belong to a city.
#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Place {
    pub id: String,
    pub name: String,
    pub rooms: Vec<Room>,
}

impl Identity for Place {
    fn id(&self) -> String {
        self.id.clone()
    }
}
