use chrono::NaiveDate;
use serde::{Deserialize, Deserializer};

use engine::entities::{Character, Gender, SkillWithLevel};

use super::SkillWithLevelDef;

#[derive(Deserialize)]
#[serde(remote = "Gender")]
pub enum GenderDef {
    Male,
    Female,
    Other,
}

#[derive(Deserialize)]
#[serde(remote = "Character")]
pub struct CharacterDef {
    pub name: String,
    #[serde(with = "super::naivedate")]
    pub birthday: NaiveDate,
    #[serde(with = "GenderDef")]
    pub gender: Gender,
    pub mood: i8,
    pub health: i8,
    pub fame: i8,
    #[serde(deserialize_with = "vec_skill_with_level")]
    pub skills: Vec<SkillWithLevel>,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
fn vec_skill_with_level<'de, D>(deserializer: D) -> Result<Vec<SkillWithLevel>, D::Error>
where
    D: Deserializer<'de>,
{
    #[derive(Deserialize)]
    struct Wrapper(#[serde(with = "SkillWithLevelDef")] SkillWithLevel);

    let v = Vec::deserialize(deserializer)?;
    Ok(v.into_iter().map(|Wrapper(a)| a).collect())
}
