use std::str::FromStr;
use strum_macros::EnumString;

use common::entities::{Instrument, Object};

use super::interact::{Interact, InteractResult, Interaction, Interactions};
use super::requirement::Requirement;
use crate::context::Context;

/// Represents the different types of interactions that the character can do with the instrument.
#[derive(Display, EnumString)]
pub enum InstrumentInteractionType {
    Play,
    Compose,
}

/// Represents an interaction with an instrument.
pub struct InstrumentInteraction {
    pub instrument: Instrument,
    pub interaction: Interaction,
}

impl Interactions for InstrumentInteraction {
    fn interactions(object: Object) -> Vec<Interaction> {
        vec![
            Interaction {
                name: InstrumentInteractionType::Play.to_string(),
                description: "Playing the instrument will advance the time by one time unit and increase the skill moderately".into(),
                object: object.clone(),
                requirements: vec![
                    Requirement::Health(20),
                    Requirement::Mood(20),
                ]
            },
            Interaction {
                name: InstrumentInteractionType::Compose.to_string(),
                description: "Composing will create a new song and generate some ideas for it which you can improve later".into(),
                object: object.clone(),
                requirements: vec![
                    Requirement::Health(20),
                    Requirement::Mood(20),
                ],
            }
        ]
    }
}

impl Interact for InstrumentInteraction {
    fn interact(self, context: &Context) -> InteractResult {
        // Unwrapping should be safe since these interactions are created by this same module.
        let interaction = InstrumentInteractionType::from_str(&self.interaction.name).unwrap();

        // TODO: Actually do something useful here.
        match interaction {
            InstrumentInteractionType::Play => {
                Ok(("You played some things".into(), context.clone()))
            }
            InstrumentInteractionType::Compose => {
                Ok(("You composed some things".into(), context.clone()))
            }
        }
    }
}
