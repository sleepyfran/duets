use serde::{Deserialize, Serialize};
use std::hash::{Hash, Hasher};

use crate::shared::bound_to_positive_hundred;

/// Defines the different types that a skill can belong to. Depending on this
/// type, the skill will affect the character in a different way.
#[derive(Clone, Copy, Deserialize, Serialize, PartialEq, Eq, Hash, Debug)]
pub enum SkillCategory {
    Music,
    Production,
    Social,
}

impl Default for SkillCategory {
    fn default() -> SkillCategory {
        SkillCategory::Music
    }
}

/// Defines a skill with no level attached to it. Used mainly for representing
/// a skill that is not referenced by any character.
#[derive(Clone, Deserialize, Serialize, Default, Debug)]
pub struct Skill {
    pub name: String,
    pub category: SkillCategory,
}

/// Skill used by a character to represent its level in it.
#[derive(Clone, Deserialize, Serialize, Eq, Debug)]
pub struct SkillWithLevel {
    pub name: String,
    pub category: SkillCategory,
    pub level: u8,
}

impl Hash for SkillWithLevel {
    fn hash<H: Hasher>(&self, state: &mut H) {
        self.name.hash(state);
        self.category.hash(state);
    }
}

impl PartialEq for SkillWithLevel {
    fn eq(&self, other: &Self) -> bool {
        self.name == other.name
    }
}

impl SkillWithLevel {
    /// Creates a SkillWithLevel from a given Skill with the given level.
    pub fn from(skill: &Skill, level: u8) -> SkillWithLevel {
        SkillWithLevel {
            name: skill.name.clone(),
            category: skill.category,
            level: bound_to_positive_hundred(level),
        }
    }
}
