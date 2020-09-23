use serde::{Deserialize, Serialize};

use super::city::City;
use super::Identity;

/// Defines a country in the game.
#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Country {
    pub id: String,
    pub name: String,
    pub cities: Vec<City>,
    pub population: i32,
}

impl Identity for Country {
    fn id(&self) -> String {
        self.id.clone()
    }
}
