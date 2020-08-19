mod instrument_interaction;
mod interact;

pub use instrument_interaction::*;
pub use interact::*;

use common::entities::{Instrument, Object, ObjectType, Skill, SkillCategory};

use crate::context::Context;

/// Returns all the available interactions for the given object.
pub fn r#for(object: &Object) -> Vec<Interaction> {
    match object.r#type {
        ObjectType::Instrument(_) => InstrumentInteraction::interactions(object.clone()),
        _ => vec![],
    }
}

/// Performs the given interaction.
pub fn interact_with(interaction: Interaction, context: &Context) -> InteractResult {
    let interaction = match &interaction.object.r#type {
        ObjectType::Instrument(instrument) => InstrumentInteraction {
            instrument: instrument.clone(),
            interaction,
        },
        _ => InstrumentInteraction {
            instrument: Instrument {
                name: "TODO: Remove".into(),
                allows_another_instrument: true,
                associated_skill: Skill {
                    name: "TODO: Remove".into(),
                    category: SkillCategory::Social,
                },
            },
            interaction,
        },
    };

    interaction.interact(context)
}
