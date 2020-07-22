use serde::Deserialize;

use super::Country;

#[derive(Deserialize)]
pub struct City {
    pub name: String,
    pub population: i32,
    pub country: Country,
}

impl City {
    pub fn to_engine(self) -> engine::entities::City {
        engine::entities::City {
            name: self.name,
            population: self.population,
            country: self.country.to_engine(),
        }
    }

    pub fn from_engine(city: engine::entities::City) -> City {
        City {
            name: city.name,
            population: city.population,
            country: Country::from_engine(city.country),
        }
    }
}
