use serde::{Deserialize, Serialize};

use super::Genre;

/// Represents the different style of vocals that a song can have.
#[derive(Clone, Deserialize, Serialize)]
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
#[derive(Clone, Default, Deserialize, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct Song {
    pub name: String,
    pub vocal_style: VocalStyle,
    pub genre: Genre,
}
