use crate::context::Context;

use super::InteractResult;

/// Creates an empty ok result for InteractResult.
fn empty_result(context: &Context) -> InteractResult {
    Ok((String::default(), context.clone()))
}

/// Defines the different set of requirements that can be possibly added to an interaction.
#[derive(Clone)]
pub enum Requirement {
    Health(i8),
    Mood(i8),
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
        Requirement::Health(min_health) => {
            if context.game_state.character.health <= min_health {
                Err(format!(
                    "Your health should be at least {} to do this",
                    min_health
                ))
            } else {
                empty_result(context)
            }
        }
        Requirement::Mood(min_mood) => {
            if context.game_state.character.mood <= min_mood {
                Err(format!("Your mood should be at least {} to do this", min_mood).into())
            } else {
                empty_result(context)
            }
        }
    }
}
