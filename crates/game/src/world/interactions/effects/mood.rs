use simulation::commands::character::CharacterCommands;

use crate::world::interactions::{EffectType, Outcome, SequenceOutput};

pub fn apply(effect_type: EffectType, output: &SequenceOutput) -> SequenceOutput {
    output
        .modify_context(|context| {
            context.modify_game_state(|game_state| {
                game_state.modify_character(|character| match effect_type {
                    EffectType::Negative(amount) => character.decrease_mood_by(amount),
                    EffectType::Positive(amount) => character.increase_mood_by(amount),
                })
            })
        })
        .add_outcome(Outcome::Mood(effect_type))
}
