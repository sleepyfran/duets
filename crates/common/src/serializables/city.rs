use serde::{Deserialize, Serialize};

use engine::entities::City;

#[derive(Deserialize, Serialize)]
#[serde(remote = "City")]
#[serde(rename_all = "camelCase")]
pub struct CityDef {
    pub name: String,
    pub country_name: String,
    pub population: i32,
}
