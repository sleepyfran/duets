use boolinator::Boolinator;

use simulation::queries::character::Status;

use super::{InteractItem, InteractSequence};
use crate::context::Context;

/// Creates an empty ok result for InteractSequence.
fn empty_result(context: &Context) -> InteractSequence {
    Ok(empty_item(context))
}

fn empty_item(context: &Context) -> InteractItem {
    InteractItem::End
}

/// Defines the different set of requirements that can be possibly added to an interaction.
#[derive(Clone)]
pub enum Requirement {
    EnergyAbove(u8),
    HealthAbove(u8),
    MoodAbove(u8),
}

/// Checks that all the given requirements are met. Returns an error if something was not met
/// or an empty result otherwise to continue.
pub fn check_requirements(context: &Context, requirements: Vec<Requirement>) -> InteractSequence {
    requirements
        .into_iter()
        .map(|req| check_requirement(context, req))
        .fold(empty_result(context), |acc, req_result| {
            acc.and_then(|_| req_result)
        })
}

/// Checks that the given requirement is met.
fn check_requirement(context: &Context, requirement: Requirement) -> InteractSequence {
    match requirement {
        Requirement::EnergyAbove(min_energy) => context
            .game_state
            .character
            .energy_above(min_energy)
            .as_result(empty_item(context), requirement),
        Requirement::HealthAbove(min_health) => context
            .game_state
            .character
            .health_above(min_health)
            .as_result(empty_item(context), requirement),
        Requirement::MoodAbove(min_mood) => context
            .game_state
            .character
            .mood_above(min_mood)
            .as_result(empty_item(context), requirement),
    }
}
