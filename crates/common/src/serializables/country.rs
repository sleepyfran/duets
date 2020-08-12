use serde::{Deserialize, Serialize};

use engine::entities::{City, Country};

#[derive(Deserialize, Serialize)]
#[serde(remote = "Country")]
pub struct CountryDef {
    pub id: String,
    pub name: String,
    #[serde(with = "city_vec")]
    pub cities: Vec<City>,
    pub population: i32,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
mod city_vec {
    use engine::entities::City;
    use serde::ser::SerializeSeq;
    use serde::{Deserialize, Deserializer, Serialize, Serializer};

    pub fn deserialize<'de, D>(deserializer: D) -> Result<Vec<City>, D::Error>
    where
        D: Deserializer<'de>,
    {
        #[derive(Deserialize)]
        struct Wrapper(#[serde(with = "super::super::CityDef")] City);

        let v = Vec::deserialize(deserializer)?;
        Ok(v.into_iter().map(|Wrapper(a)| a).collect())
    }

    pub fn serialize<S>(value: &Vec<City>, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        #[derive(Serialize)]
        struct Wrapper<'s>(#[serde(with = "super::super::CityDef")] &'s City);

        let mut seq = serializer.serialize_seq(Some(value.len()))?;
        for e in value {
            seq.serialize_element(&Wrapper(e))?;
        }
        seq.end()
    }
}
