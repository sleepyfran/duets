use chrono::NaiveDate;
use serde::{Deserialize, Serialize};

use engine::entities::{Character, Gender, SkillWithLevel};

#[derive(Deserialize, Serialize)]
#[serde(remote = "Gender")]
pub enum GenderDef {
    Male,
    Female,
    Other,
}

#[derive(Deserialize, Serialize)]
#[serde(remote = "Character")]
pub struct CharacterDef {
    pub name: String,
    #[serde(with = "super::naivedate::date")]
    pub birthday: NaiveDate,
    #[serde(with = "GenderDef")]
    pub gender: Gender,
    pub mood: i8,
    pub health: i8,
    pub fame: i8,
    #[serde(with = "skill_with_level_vec")]
    pub skills: Vec<SkillWithLevel>,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
mod skill_with_level_vec {
    use engine::entities::SkillWithLevel;
    use serde::ser::SerializeSeq;
    use serde::{Deserialize, Deserializer, Serialize, Serializer};

    pub fn deserialize<'de, D>(deserializer: D) -> Result<Vec<SkillWithLevel>, D::Error>
    where
        D: Deserializer<'de>,
    {
        #[derive(Deserialize)]
        struct Wrapper(#[serde(with = "super::super::SkillWithLevelDef")] SkillWithLevel);

        let v = Vec::deserialize(deserializer)?;
        Ok(v.into_iter().map(|Wrapper(a)| a).collect())
    }

    pub fn serialize<S>(value: &Vec<SkillWithLevel>, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        #[derive(Serialize)]
        struct Wrapper<'s>(#[serde(with = "super::super::SkillWithLevelDef")] &'s SkillWithLevel);

        let mut seq = serializer.serialize_seq(Some(value.len()))?;
        for e in value {
            seq.serialize_element(&Wrapper(e))?;
        }
        seq.end()
    }
}
