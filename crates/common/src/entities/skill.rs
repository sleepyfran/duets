use serde::{Deserialize, Serialize};

use crate::shared::bound_to_positive_hundred;

/// Defines the different types that a skill can belong to. Depending on this
/// type, the skill will affect the character in a different way.
#[derive(Clone, Copy, Deserialize, Serialize)]
pub enum SkillCategory {
    Music,
    Production,
    Social,
}

/// Defines a skill with no level attached to it. Used mainly for representing
/// a skill that is not referenced by any character.
#[derive(Clone, Deserialize, Serialize)]
pub struct Skill {
    pub name: String,
    pub category: SkillCategory,
}

/// Skill used by a character to represent its level in it.
#[derive(Clone, Deserialize, Serialize)]
pub struct SkillWithLevel {
    pub name: String,
    pub category: SkillCategory,
    pub level: i8,
}

impl SkillWithLevel {
    /// Creates a SkillWithLevel from a given Skill with the given level.
    pub fn from(skill: &Skill, level: i8) -> SkillWithLevel {
        SkillWithLevel {
            name: skill.name.clone(),
            category: skill.category,
            level: bound_to_positive_hundred(level),
        }
    }
}
