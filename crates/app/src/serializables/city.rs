use serde::{Deserialize, Deserializer};

use engine::entities::City;

#[derive(Deserialize)]
#[serde(remote = "City")]
#[serde(rename_all = "camelCase")]
pub struct CityDef {
    pub name: String,
    pub country_name: String,
    pub population: i32,
}
