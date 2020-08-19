use chrono::{Duration, NaiveDate, Utc};
use serde::{Deserialize, Serialize};

use crate::shared::bound_to_positive_hundred;

use crate::entities::skill::SkillWithLevel;

/// Defines the gender of the character.
#[derive(Clone, Deserialize, Serialize)]
pub enum Gender {
    Male,
    Female,
    Other,
}

/// Defines both playable and non-playable characters in the game.
#[derive(Clone, Deserialize, Serialize)]
pub struct Character {
    pub name: String,
    #[serde(with = "crate::serializables::naivedate::date")]
    pub birthday: NaiveDate,
    pub gender: Gender,
    pub mood: i8,
    pub health: i8,
    pub fame: i8,
    pub skills: Vec<SkillWithLevel>,
}

impl Default for Character {
    fn default() -> Character {
        Character {
            name: String::default(),
            birthday: NaiveDate::from_yo(1990, 1),
            gender: Gender::Other,
            mood: 100,
            health: 100,
            fame: 0,
            skills: vec![],
        }
    }
}

impl Character {
    /// Returns a new character given a name with the rest of fields set to
    /// a default value.
    pub fn new(name: String) -> Character {
        Character {
            name: name,
            birthday: Utc::now().naive_utc().date() - Duration::days(365 * 20),
            gender: Gender::Other,
            fame: 0,
            health: 100,
            mood: 100,
            skills: Vec::new(),
        }
    }

    /// Sets the birthday of the character.
    pub fn with_birthday(self, birthday: NaiveDate) -> Character {
        Character { birthday, ..self }
    }

    /// Sets the gender of the character.
    pub fn with_gender(self, gender: Gender) -> Character {
        Character { gender, ..self }
    }

    /// Sets the fame of the character. Bounds the given fame to 0 or 100 if
    /// below or above.
    pub fn with_fame(self, fame: i8) -> Character {
        Character {
            fame: bound_to_positive_hundred(fame),
            ..self
        }
    }

    /// Sets the health of the character. Bounds the given health to 0 or 100 if
    /// below or above.
    pub fn with_health(self, health: i8) -> Character {
        Character {
            health: bound_to_positive_hundred(health),
            ..self
        }
    }

    /// Sets the mood of the character. Bounds the given mood to 0 or 100 if
    /// below or above.
    pub fn with_mood(self, mood: i8) -> Character {
        Character {
            mood: bound_to_positive_hundred(mood),
            ..self
        }
    }

    /// Sets the skills of the character.
    pub fn with_skills(self, skills: Vec<SkillWithLevel>) -> Character {
        Character { skills, ..self }
    }

    pub fn gender_str(&self) -> String {
        match self.gender {
            Gender::Female => String::from("female"),
            Gender::Male => String::from("male"),
            Gender::Other => String::from("other"),
        }
    }
}
