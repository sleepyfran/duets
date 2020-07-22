use chrono::NaiveDate;
use serde::Deserialize;

use super::deserializers;
use super::SkillWithLevel;

#[derive(Deserialize)]
pub enum Gender {
    Male,
    Female,
    Other,
}

#[derive(Deserialize)]
pub struct Character {
    name: String,
    #[serde(with = "deserializers::naivedate")]
    birthday: NaiveDate,
    gender: Gender,
    mood: i8,
    health: i8,
    fame: i8,
    skills: Vec<SkillWithLevel>,
}
