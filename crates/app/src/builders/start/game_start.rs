use chrono::{Datelike, NaiveDate};

use engine::entities::{Band, Calendar, Character, City, Gender};

use crate::serializables::GameState;

pub enum ValidationError {
    InvalidName,
    BirthdayTooEarly,
    CharacterNot18WhenGameStarts,
}

/// Generates a builder that allows chainable calls to create an struct representing the basic
/// data we need to start a new game.
#[derive(Builder)]
#[builder(pattern = "owned", derive(Clone))]
pub struct GameStart {
    #[builder(setter(into))]
    pub name: String,
    pub birthday: NaiveDate,
    pub gender: Gender,
    pub start_city: City,
    pub start_year: i16,
}

impl GameStart {
    /// Returns a game state based on the current parameters of the builder.
    pub fn to_game_state(self) -> GameState {
        GameState {
            band: Band::default(),
            character: Character::new(self.name)
                .with_gender(self.gender)
                .with_birthday(self.birthday),
            current_city: self.start_city,
            calendar: Calendar::from_year(self.start_year),
        }
    }
}

impl GameStartBuilder {
    /// Validates all the required fields in the builder and returns specific errors about it.
    pub fn validate(self) -> Result<(), ValidationError> {
        if self.name.is_none() || self.name.unwrap().is_empty() {
            Err(ValidationError::InvalidName)
        } else if self.birthday.unwrap().year() < 1950 {
            Err(ValidationError::BirthdayTooEarly)
        } else if (self.start_year.unwrap() - self.birthday.unwrap().year() as i16) < 18 {
            Err(ValidationError::CharacterNot18WhenGameStarts)
        } else {
            Ok(())
        }
    }
}
