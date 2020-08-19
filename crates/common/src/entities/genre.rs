use serde::{Deserialize, Serialize};

/// Represents a genre that can be associated with a band, an album or a song.
#[derive(Clone, Default, Deserialize, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct Genre {
    pub name: String,
    pub compatible_with: Vec<Genre>,
}
