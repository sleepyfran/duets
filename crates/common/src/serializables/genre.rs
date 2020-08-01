use serde::{Deserialize, Serialize};

use engine::entities::Genre;

#[derive(Deserialize, Serialize)]
#[serde(remote = "Genre")]
#[serde(rename_all = "camelCase")]
pub struct GenreDef {
    pub name: String,
    #[serde(with = "genre_vec")]
    pub compatible_with: Vec<Genre>,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
mod genre_vec {
    use engine::entities::Genre;
    use serde::ser::SerializeSeq;
    use serde::{Deserialize, Deserializer, Serialize, Serializer};

    pub fn deserialize<'de, D>(deserializer: D) -> Result<Vec<Genre>, D::Error>
    where
        D: Deserializer<'de>,
    {
        #[derive(Deserialize)]
        struct Wrapper(#[serde(with = "super::super::GenreDef")] Genre);

        let v = Vec::deserialize(deserializer)?;
        Ok(v.into_iter().map(|Wrapper(a)| a).collect())
    }

    pub fn serialize<S>(value: &Vec<Genre>, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        #[derive(Serialize)]
        struct Wrapper<'s>(#[serde(with = "super::super::GenreDef")] &'s Genre);

        let mut seq = serializer.serialize_seq(Some(value.len()))?;
        for e in value {
            seq.serialize_element(&Wrapper(e))?;
        }
        seq.end()
    }
}
