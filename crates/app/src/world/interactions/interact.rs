use common::entities::Object;

use crate::context::Context;

/// Defines the result of an interaction. Either we return a string with the description of the
/// interaction that was made and the new context to apply or just a description of whatever
/// went wrong.
pub type InteractResult = Result<(String, Context), String>;

/// Creates an empty ok result for InteractResult.
fn empty_result(context: &Context) -> InteractResult {
    Ok((String::default(), context.clone()))
}

/// Defines an interaction with a name and a description that can be shown to the user.
#[derive(Clone)]
pub struct Interaction {
    pub name: String,
    pub description: String,
    pub object: Object,
    pub requirements: Vec<Requirement>,
}

/// Defines the different set of requirements that can be possibly added to an interaction.
#[derive(Clone)]
pub enum Requirement {
    Health(i8),
    Mood(i8),
}

/// Adds a method called interactions that allows to retrieve the list of available interactions
/// for a given object.
pub trait Interactions {
    fn interactions(object: Object) -> Vec<Interaction>;
}

/// Adds a method called interact to an interaction type with an object. Normally this trait should
/// be implemented at the "object" level, this means that instead of implementing the trait for a
/// specific interaction (example: playing the guitar) this should go on a higher level (instrument)
/// so that each type of interaction will be handled manually by the interact method.
pub trait Interact {
    fn interact(self, context: &Context) -> InteractResult;
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
