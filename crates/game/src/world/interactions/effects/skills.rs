use common::entities::Skill;
use simulation::commands::skills::SkillCommands;
use simulation::queries::character::SkillQueries;

use crate::context::Context;
use crate::world::interactions::EffectType;

/// Applies the given skill effect.
pub fn apply(skill: Skill, effect_type: EffectType, context: &Context) -> Context {
    let skill = context.game_state.character.get_skill_with_level(skill);

    context.clone().modify_game_state(|game_state| {
        game_state.modify_character(|character| match effect_type {
            EffectType::Negative(amount) => character.with_skill(skill.decrease_by(amount)),
            EffectType::Positive(amount) => character.with_skill(skill.increase_by(amount)),
        })
    })
}
