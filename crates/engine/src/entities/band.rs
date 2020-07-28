use chrono::NaiveDate;

use super::{Character, Genre, Instrument};

/// Represents the stay of a character in the band, with the date in which they entered the band
/// and the date in which they left, if any.
pub struct BandMember {
    pub character: Character,
    pub since: NaiveDate,
    pub until: Option<NaiveDate>,
    pub instruments: Vec<Instrument>,
}

/// Represents a band in the game.
pub struct Band {
    pub name: String,
    pub genre: Genre,
    pub members: Vec<BandMember>,
}
