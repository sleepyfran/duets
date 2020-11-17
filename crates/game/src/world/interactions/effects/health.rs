use simulation::commands::character::CharacterCommands;

use crate::world::interactions::{EffectType, Outcome, SequenceOutput};

pub fn apply(effect_type: EffectType, output: &SequenceOutput) -> SequenceOutput {
    output
        .modify_context(|context| {
            context.modify_game_state(|game_state| {
                game_state.modify_character(|character| match effect_type {
                    EffectType::Negative(amount) => character.decrease_health_by(amount),
                    EffectType::Positive(amount) => character.increase_health_by(amount),
                })
            })
        })
        .add_outcome(Outcome::Health(effect_type))
}
