use serde::Deserialize;

use engine::entities::City;

#[derive(Deserialize)]
#[serde(remote = "City")]
pub struct CityDef {
    pub name: String,
    pub population: i32,
}
