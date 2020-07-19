use crate::common::bound;

/// Defines the different types that a skill can belong to. Depending on this
/// type, the skill will affect the character in a different way.
#[derive(Clone, Copy)]
pub enum SkillCategory {
    Music,
    Production,
    Social,
}

/// Defines a skill with no level attached to it. Used mainly for representing
/// a skill that is not referenced by any character.
pub struct Skill {
    name: String,
    category: SkillCategory,
}

/// Skill used by a character to represent its level in it.
pub struct SkillWithLevel {
    name: String,
    category: SkillCategory,
    level: i8,
}

impl SkillWithLevel {
    /// Creates a SkillWithLevel from a given Skill with the given level.
    pub fn from(skill: &Skill, level: i8) -> SkillWithLevel {
        SkillWithLevel {
            name: skill.name.clone(),
            category: skill.category,
            level: bound(level),
        }
    }
}
