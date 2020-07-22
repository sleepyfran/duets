use serde::Deserialize;

use super::city::City;

#[derive(Deserialize)]
pub struct Country {
    pub name: String,
    pub cities: Vec<City>,
    pub population: i32,
}

impl Country {
    /// Returns the engine representation from the serializable one.
    pub fn to_engine(self) -> engine::entities::Country {
        engine::entities::Country {
            name: self.name,
            population: self.population,
            cities: self.cities.into_iter().map(|c| c.to_engine()).collect(),
        }
    }

    pub fn from_engine(country: engine::entities::Country) -> Country {
        Country {
            name: country.name,
            population: country.population,
            cities: country
                .cities
                .into_iter()
                .map(|c| City::from_engine(c))
                .collect(),
        }
    }
}
