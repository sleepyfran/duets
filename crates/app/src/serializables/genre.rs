use serde::{Deserialize, Deserializer};

use engine::entities::Genre;

#[derive(Deserialize)]
#[serde(remote = "Genre")]
#[serde(rename_all = "camelCase")]
pub struct GenreDef {
    pub name: String,
    #[serde(deserialize_with = "vec_genre")]
    pub compatible_with: Vec<Genre>,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
fn vec_genre<'de, D>(deserializer: D) -> Result<Vec<Genre>, D::Error>
where
    D: Deserializer<'de>,
{
    #[derive(Deserialize)]
    struct Wrapper(#[serde(with = "GenreDef")] Genre);

    let v = Vec::deserialize(deserializer)?;
    Ok(v.into_iter().map(|Wrapper(a)| a).collect())
}
