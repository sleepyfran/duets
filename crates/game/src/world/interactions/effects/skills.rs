use common::entities::Skill;
use simulation::commands::skills::SkillCommands;
use simulation::queries::character::SkillQueries;

use crate::world::interactions::{EffectType, Outcome, SequenceOutput};

/// Applies the given skill effect.
pub fn apply(skill: Skill, effect_type: EffectType, output: &SequenceOutput) -> SequenceOutput {
    let skill = output
        .context
        .clone()
        .game_state
        .character
        .get_skill_with_level(&skill);

    let skill = match effect_type {
        EffectType::Negative(amount) => skill.decrease_by(amount),
        EffectType::Positive(amount) => skill.increase_by(amount),
    };

    output
        .modify_context(|context| {
            context.modify_game_state(|game_state| {
                game_state.modify_character(|character| character.with_skill(skill.clone()))
            })
        })
        .add_outcome(Outcome::SkillLevelModified(skill, effect_type))
}
