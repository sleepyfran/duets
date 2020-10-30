use serde::{Deserialize, Serialize};
use std::hash::{Hash, Hasher};
use strum_macros::{Display, EnumIter, EnumString};
use uuid::Uuid;

use super::Genre;
use super::Identity;
use crate::shared::bound_to_positive_hundred;

/// Represents the different style of vocals that a song can have.
#[derive(Clone, Deserialize, Serialize, Display, EnumIter, EnumString, PartialEq, Eq)]
pub enum VocalStyle {
    Instrumental,
    Clean,
    Growl,
    Scream,
    Spoken,
    Rap,
}

impl Default for VocalStyle {
    fn default() -> VocalStyle {
        VocalStyle::Clean
    }
}

/// Represents a song composed by the user or another band in the game. These songs can be recorded
/// and added to an album, played live and anything that you can imagine doing with a song.
#[derive(Clone, Default, Deserialize, Serialize, Eq)]
#[serde(rename_all = "camelCase")]
pub struct Song {
    pub id: String,
    pub name: String,
    pub vocal_style: VocalStyle,
    pub genre: Genre,
    pub quality: u8,
}

impl Song {
    pub fn create(name: String, genre: Genre, vocal_style: VocalStyle) -> Self {
        Self {
            id: Uuid::new_v4().to_string(),
            name,
            genre,
            vocal_style,
            quality: 0,
        }
    }

    pub fn from_id(id: String) -> Self {
        Self {
            id,
            name: String::default(),
            genre: Genre::default(),
            vocal_style: VocalStyle::Instrumental,
            quality: 0,
        }
    }

    pub fn with_quality(self, quality: u8) -> Self {
        Self {
            quality: bound_to_positive_hundred(quality),
            ..self
        }
    }
}

impl Identity for Song {
    fn id(&self) -> String {
        self.id.clone()
    }
}

impl Hash for Song {
    fn hash<H: Hasher>(&self, state: &mut H) {
        self.id.hash(state);
    }
}

impl PartialEq for Song {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}
