use boolinator::Boolinator;

use engine::operations::character::CharacterOperations;

use super::InteractResult;
use crate::context::Context;

/// Creates an empty ok result for InteractResult.
fn empty_result(context: &Context) -> InteractResult {
    Ok(empty_tuple(context))
}

fn empty_tuple(context: &Context) -> (String, Context) {
    (String::default(), context.clone())
}

/// Defines the different set of requirements that can be possibly added to an interaction.
#[derive(Clone)]
pub enum Requirement {
    HealthAbove(i8),
    MoodAbove(i8),
}

/// Checks that all the given requirements are met. Returns an error if something was not met
/// or an empty result otherwise to continue.
pub fn check_requirements(context: &Context, requirements: Vec<Requirement>) -> InteractResult {
    requirements
        .into_iter()
        .map(|req| check_requirement(context, req))
        .fold(empty_result(context), |acc, req_result| {
            acc.and_then(|_| req_result)
        })
}

/// Checks that the given requirement is met.
fn check_requirement(context: &Context, requirement: Requirement) -> InteractResult {
    match requirement {
        Requirement::HealthAbove(min_health) => context
            .game_state
            .character
            .health_above(min_health)
            .as_result(
                empty_tuple(context),
                format!("Your health should be at least {} to do this", min_health),
            ),
        Requirement::MoodAbove(min_mood) => {
            context.game_state.character.mood_above(min_mood).as_result(
                empty_tuple(context),
                format!("Your mood should be at least {} to do this", min_mood),
            )
        }
    }
}
