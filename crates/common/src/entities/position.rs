use serde::{Deserialize, Serialize};

use super::{City, Country, Place, Room};

/// Defines the position of the character in the world.
#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Position {
    pub country: Country,
    pub city: City,
    pub place: Place,
    pub room: Room,
}
