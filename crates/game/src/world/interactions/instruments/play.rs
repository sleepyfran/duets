use crate::context::Context;
use crate::world::interactions::{
    InteractItem, InteractResult, InteractSequence, Interaction, Requirement,
};

pub enum PlayResult {
    Success,
    Failure,
}

#[derive(Clone, Default)]
pub struct PlayInteraction;

impl Interaction for PlayInteraction {
    type Result = PlayResult;

    fn name(&self) -> String {
        "Play".into()
    }

    fn description(&self) -> String {
        "Playing the instrument will advance the time by one time unit and increase the skill moderately".into()
    }

    fn requirements(&self) -> Vec<Requirement> {
        vec![Requirement::HealthAbove(20), Requirement::MoodAbove(20)]
    }

    fn interact(&self, context: &Context) -> InteractSequence<Self::Result> {
        Ok(InteractItem::Result(InteractResult {
            result: PlayResult::Success,
            context: context.clone(),
        }))
    }
}
