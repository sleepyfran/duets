use chrono::Duration;

use common::entities::{Skill, SkillCategory, SkillWithLevel};
use simulation::commands::skills::SkillCommands;

fn get_skill() -> Skill {
    Skill {
        name: "test".into(),
        category: SkillCategory::Music,
    }
}

fn get_skill_with_level(level: u8) -> SkillWithLevel {
    SkillWithLevel::from(&get_skill(), level)
}

fn compare_skills(first: SkillWithLevel, second: SkillWithLevel) {
    assert_eq!(first.name, second.name);
    assert_eq!(first.category, second.category);
    assert_eq!(first.level, second.level);
}

/* COMMANDS. */
/* increase_by */
#[test]
fn increase_by_0_should_return_same_skill() {
    compare_skills(
        get_skill_with_level(0).increase_by(0),
        get_skill_with_level(0),
    )
}

#[test]
fn increase_by_1_should_return_skill_with_one_more_level() {
    compare_skills(
        get_skill_with_level(0).increase_by(1),
        get_skill_with_level(1),
    )
}

#[test]
fn increase_by_n_should_return_skill_with_n_more_levels() {
    for n in 2..55 {
        compare_skills(
            get_skill_with_level(0).increase_by(n.into()),
            get_skill_with_level(n.into()),
        )
    }
}

#[test]
fn increase_by_more_than_100_should_return_skill_capped_to_100() {
    compare_skills(
        get_skill_with_level(0).increase_by(120),
        get_skill_with_level(100),
    )
}

/* decrease_by */
#[test]
fn decrease_by_0_should_return_same_skill() {
    compare_skills(
        get_skill_with_level(0).decrease_by(0),
        get_skill_with_level(0),
    )
}

#[test]
fn decrease_by_1_should_return_skill_with_one_less_level() {
    compare_skills(
        get_skill_with_level(10).decrease_by(1),
        get_skill_with_level(9),
    )
}

#[test]
fn decrease_by_n_should_return_skill_with_n_more_levels() {
    for n in 2..55 {
        compare_skills(
            get_skill_with_level(100).decrease_by(n.into()),
            get_skill_with_level(100 - n),
        )
    }
}

#[test]
fn decrease_by_more_than_100_should_return_skill_capped_to_0() {
    compare_skills(
        get_skill_with_level(100).decrease_by(120),
        get_skill_with_level(0),
    )
}
