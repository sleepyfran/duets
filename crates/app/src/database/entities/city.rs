use serde::Deserialize;

#[derive(Deserialize)]
pub struct City {
    pub name: String,
    pub population: i32,
}

impl City {
    pub fn to_engine(self) -> engine::entities::City {
        engine::entities::City {
            name: self.name,
            population: self.population,
        }
    }

    pub fn from_engine(city: engine::entities::City) -> City {
        City {
            name: city.name,
            population: city.population,
        }
    }
}
