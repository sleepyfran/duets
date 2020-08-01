use serde::{Deserialize, Serialize};

use engine::entities::{Skill, SkillCategory, SkillWithLevel};

/// Defines the different types that a skill can belong to. Depending on this
/// type, the skill will affect the character in a different way.
#[derive(Deserialize, Serialize)]
#[serde(remote = "SkillCategory")]
pub enum SkillCategoryDef {
    Music,
    Production,
    Social,
}

/// Defines a skill with no level attached to it. Used mainly for representing
/// a skill that is not referenced by any character.
#[derive(Deserialize, Serialize)]
#[serde(remote = "Skill")]
pub struct SkillDef {
    pub name: String,
    #[serde(with = "SkillCategoryDef")]
    pub category: SkillCategory,
}

/// Skill used by a character to represent its level in it.
#[derive(Deserialize, Serialize)]
#[serde(remote = "SkillWithLevel")]
pub struct SkillWithLevelDef {
    pub name: String,
    #[serde(with = "SkillCategoryDef")]
    pub category: SkillCategory,
    pub level: i8,
}
