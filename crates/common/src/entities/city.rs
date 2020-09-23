use serde::{Deserialize, Serialize};

use super::Identity;
use super::Place;

/// Defines a city in the game. Must belong to a country.
#[derive(Clone, Default, Deserialize, Serialize)]
pub struct City {
    pub id: String,
    pub name: String,
    pub population: i32,
    pub places: Vec<Place>,
}

impl Identity for City {
    fn id(&self) -> String {
        self.id.clone()
    }
}
