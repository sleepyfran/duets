use common::entities::{Band, BandMember, Calendar, Character, Genre, Instrument};

/// Generates a new builder that allows easy creation of bands.
#[derive(Builder)]
#[builder(pattern = "owned", derive(Clone))]
pub struct BandStart {
    pub name: String,
    pub genre: Genre,
    pub starting_character: Character,
    pub starting_instrument: Instrument,
}

impl BandStart {
    pub fn to_band(self, calendar: &Calendar) -> Band {
        Band {
            name: self.name,
            genre: self.genre,
            members: vec![BandMember {
                character: self.starting_character.clone(),
                since: calendar.date.clone(),
                until: None,
                instruments: vec![self.starting_instrument.clone()],
            }],
        }
    }
}
