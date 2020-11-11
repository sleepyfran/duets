use serde::{Deserialize, Serialize};

use super::Identity;

/// Represents a genre that can be associated with a band, an album or a song.
#[derive(Clone, Default, Deserialize, Serialize, Eq)]
#[serde(rename_all = "camelCase")]
pub struct Genre {
    pub id: String,
    pub name: String,
    pub compatible_with: Vec<String>,
}

impl PartialEq for Genre {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Identity for Genre {
    fn id(&self) -> String {
        self.id.clone()
    }
}
