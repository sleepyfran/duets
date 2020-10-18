use common::entities::Instrument;
use common::results::InteractResult;
use simulation::interactions::instruments::{play, PlayInteractionInput};

use crate::context::Context;
use crate::world::interactions::{
    InteractEnd, InteractItem, InteractSequence, Interaction, Requirement,
};

#[derive(Clone, Default)]
pub struct PlayInteraction {
    pub instrument: Instrument,
}

impl Interaction for PlayInteraction {
    fn id(&self) -> String {
        "play".into()
    }

    fn name(&self) -> String {
        "Play".into()
    }

    fn description(&self) -> String {
        "Playing the instrument will advance the time by one time unit and increase the skill moderately".into()
    }

    fn requirements(&self) -> Vec<Requirement> {
        vec![Requirement::HealthAbove(20), Requirement::MoodAbove(20)]
    }

    fn interact(&self, context: &Context) -> InteractSequence {
        let result = play(PlayInteractionInput {
            instrument: self.instrument.clone(),
            interaction_id: self.id(),
            game_state: context.game_state.clone(),
        });
        Ok(InteractItem::Result(InteractEnd {
            result: result.0.clone(),
            context: context.clone().modify_game_state(|_| result.1),
        }))
    }
}
