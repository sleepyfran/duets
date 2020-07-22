use serde::Deserialize;

/// Defines the different types that a skill can belong to. Depending on this
/// type, the skill will affect the character in a different way.
#[derive(Deserialize)]
pub enum SkillCategory {
    Music,
    Production,
    Social,
}

/// Defines a skill with no level attached to it. Used mainly for representing
/// a skill that is not referenced by any character.
#[derive(Deserialize)]
pub struct Skill {
    name: String,
    category: SkillCategory,
}

/// Skill used by a character to represent its level in it.
#[derive(Deserialize)]
pub struct SkillWithLevel {
    name: String,
    category: SkillCategory,
    level: i8,
}
