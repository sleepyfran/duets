use std::str::FromStr;
use strum::{EnumProperty, IntoEnumIterator};
use strum_macros::{EnumIter, EnumString};

use common::entities::{Instrument, Object};

use super::interact::{Interact, InteractResult, Interaction, Interactions};
use crate::context::Context;

/// Represents the different types of interactions that the character can do with the instrument.
#[derive(Display, EnumProperty, EnumString, EnumIter)]
pub enum InstrumentInteractionType {
    #[strum(props(
        Description = "Playing the instrument will advance the time by one time unit and increase the skill moderately"
    ))]
    Play,
    #[strum(props(
        Description = "Composing will create a new song and generate some ideas for it which you can improve later"
    ))]
    Compose,
}

/// Represents an interaction with an instrument.
pub struct InstrumentInteraction {
    pub instrument: Instrument,
    pub interaction: Interaction,
}

impl Interactions for InstrumentInteraction {
    fn interactions(object: Object) -> Vec<Interaction> {
        InstrumentInteractionType::iter()
            .map(|interaction| Interaction {
                name: interaction.to_string(),
                description: interaction.get_str("Description").unwrap().to_string(),
                object: object.clone(),
            })
            .collect()
    }
}

impl Interact for InstrumentInteraction {
    fn interact(self, context: &Context) -> InteractResult {
        // Unwrapping should be safe since these interactions are created by this same module.
        let interaction = InstrumentInteractionType::from_str(&self.interaction.name).unwrap();

        // TODO: Actually do something useful here.
        match interaction {
            InstrumentInteractionType::Play => InteractResult {
                description: "You played some things".into(),
                context: context.clone(),
            },
            InstrumentInteractionType::Compose => InteractResult {
                description: "You composed some things".into(),
                context: context.clone(),
            },
        }
    }
}
