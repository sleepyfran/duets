use chrono::{Duration, NaiveDate, Utc};
use serde::{Deserialize, Serialize};
use std::collections::HashSet;

use crate::shared::bound_to_positive_hundred;

use crate::entities::skill::SkillWithLevel;

/// Defines the gender of the character.
#[derive(Clone, Deserialize, Serialize, Debug)]
pub enum Gender {
    Male,
    Female,
    Other,
}

/// Defines both playable and non-playable characters in the game.
#[derive(Clone, Deserialize, Serialize, Debug)]
pub struct Character {
    pub name: String,
    #[serde(with = "crate::serializables::naivedate::date")]
    pub birthday: NaiveDate,
    pub gender: Gender,
    pub mood: u8,
    pub health: u8,
    pub fame: u8,
    pub skills: HashSet<SkillWithLevel>,
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
            skills: HashSet::new(),
        }
    }
}

impl Character {
    /// Returns a new character given a name with the rest of fields set to
    /// a default value.
    pub fn new(name: String) -> Character {
        Character {
            name,
            birthday: Utc::now().naive_utc().date() - Duration::days(365 * 20),
            gender: Gender::Other,
            fame: 0,
            health: 100,
            mood: 100,
            skills: HashSet::new(),
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
    pub fn with_fame(self, fame: u8) -> Character {
        Character {
            fame: bound_to_positive_hundred(fame),
            ..self
        }
    }

    /// Sets the health of the character. Bounds the given health to 0 or 100 if
    /// below or above.
    pub fn with_health(self, health: u8) -> Character {
        Character {
            health: bound_to_positive_hundred(health),
            ..self
        }
    }

    /// Sets the mood of the character. Bounds the given mood to 0 or 100 if
    /// below or above.
    pub fn with_mood(self, mood: u8) -> Character {
        Character {
            mood: bound_to_positive_hundred(mood),
            ..self
        }
    }

    /// Sets a particular skill of the character if it exists, otherwise it inserts it into the set.
    pub fn with_skill(mut self, skill: SkillWithLevel) -> Character {
        self.skills.replace(skill);
        self
    }

    /// Transforms the gender of the character to a readable string.
    pub fn gender_str(&self) -> String {
        match self.gender {
            Gender::Female => String::from("female"),
            Gender::Male => String::from("male"),
            Gender::Other => String::from("other"),
        }
    }
}
