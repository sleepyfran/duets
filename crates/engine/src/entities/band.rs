use chrono::NaiveDate;

use super::{Character, Genre, Instrument};

/// Represents the stay of a character in the band, with the date in which they entered the band
/// and the date in which they left, if any.
#[derive(Clone)]
pub struct BandMember {
    pub character: Character,
    pub since: NaiveDate,
    pub until: Option<NaiveDate>,
    pub instruments: Vec<Instrument>,
}

impl Default for BandMember {
    fn default() -> BandMember {
        BandMember {
            character: Character::default(),
            since: NaiveDate::from_yo(1990, 1),
            until: None,
            instruments: Vec::default(),
        }
    }
}

/// Represents a band in the game.
#[derive(Clone, Default)]
pub struct Band {
    pub name: String,
    pub genre: Genre,
    pub members: Vec<BandMember>,
}
