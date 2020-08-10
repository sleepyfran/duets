use serde::{Deserialize, Serialize};

use engine::entities::{City, Country, Place, Room};

use super::{CityDef, CountryDef, PlaceDef, RoomDef};

/// Defines the position of the character in the world.
#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Position {
    #[serde(with = "CountryDef")]
    pub country: Country,
    #[serde(with = "CityDef")]
    pub city: City,
    #[serde(with = "PlaceDef")]
    pub place: Place,
    #[serde(with = "RoomDef")]
    pub room: Room,
}
