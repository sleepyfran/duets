use simulation::commands::character::CharacterCommands;

use crate::context::Context;
use crate::world::interactions::EffectType;

/// Applies the given energy effect.
pub fn apply(effect_type: EffectType, context: &Context) -> Context {
    context.clone().modify_game_state(|game_state| {
        game_state.modify_character(|character| match effect_type {
            EffectType::Negative(amount) => character.decrease_energy_by(amount),
            EffectType::Positive(amount) => character.increase_energy_by(amount),
        })
    })
}
