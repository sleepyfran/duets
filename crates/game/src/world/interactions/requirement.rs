use boolinator::Boolinator;

use simulation::queries::character::Status;

use super::{InteractItem, InteractSequence};
use crate::context::Context;

/// Creates an empty ok result for InteractSequence.
fn empty_result<R: Default>(context: &Context) -> InteractSequence<R> {
    Ok(empty_tuple(context))
}

fn empty_tuple<R: Default>(context: &Context) -> InteractItem<R> {
    InteractItem::NoOp
}

/// Defines the different set of requirements that can be possibly added to an interaction.
#[derive(Clone)]
pub enum Requirement {
    HealthAbove(i8),
    MoodAbove(i8),
}

/// Checks that all the given requirements are met. Returns an error if something was not met
/// or an empty result otherwise to continue.
pub fn check_requirements<R: Default>(
    context: &Context,
    requirements: Vec<Requirement>,
) -> InteractSequence<R> {
    requirements
        .into_iter()
        .map(|req| check_requirement(context, req))
        .fold(empty_result(context), |acc, req_result| {
            acc.and_then(|_| req_result)
        })
}

/// Checks that the given requirement is met.
fn check_requirement<R: Default>(
    context: &Context,
    requirement: Requirement,
) -> InteractSequence<R> {
    match requirement {
        Requirement::HealthAbove(min_health) => context
            .game_state
            .character
            .health_above(min_health)
            .as_result(empty_tuple(context), requirement),
        Requirement::MoodAbove(min_mood) => context
            .game_state
            .character
            .mood_above(min_mood)
            .as_result(empty_tuple(context), requirement),
    }
}
