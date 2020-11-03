use common::entities::{Character, Skill, SkillWithLevel};

pub trait SkillQueries {
    /// Given a skill, returns the skill with level of the character. If the character has not
    /// learned the skill yet, it'll return a new instance with a 0 level.
    fn get_skill_with_level(&self, skill: &Skill) -> SkillWithLevel;
}

impl SkillQueries for Character {
    fn get_skill_with_level(&self, skill: &Skill) -> SkillWithLevel {
        self.skills
            .iter()
            .cloned()
            .find(|s| s.name == skill.name)
            .unwrap_or_else(|| SkillWithLevel::from(&skill, 0))
    }
}
