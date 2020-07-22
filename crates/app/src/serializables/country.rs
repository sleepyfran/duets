use serde::{Deserialize, Deserializer};

use engine::entities::{City, Country};

use super::city::CityDef;

#[derive(Deserialize)]
#[serde(remote = "Country")]
pub struct CountryDef {
    pub name: String,
    #[serde(deserialize_with = "vec_city")]
    pub cities: Vec<City>,
    pub population: i32,
}

fn vec_city<'de, D>(deserializer: D) -> Result<Vec<City>, D::Error>
where
    D: Deserializer<'de>,
{
    #[derive(Deserialize)]
    struct Wrapper(#[serde(with = "CityDef")] City);

    let v = Vec::deserialize(deserializer)?;
    Ok(v.into_iter().map(|Wrapper(a)| a).collect())
}
