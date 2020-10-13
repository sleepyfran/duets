use common::entities::Object;

use crate::context::Context;

use super::requirement::*;

/// Defines the result of an interaction. Either we return a string with the description of the
/// interaction that was made and the new context to apply or just a description of whatever
/// went wrong.
pub type InteractResult = Result<(String, Context), String>;

/// Defines an interaction with a name and a description that can be shown to the user.
#[derive(Clone)]
pub struct Interaction {
    pub name: String,
    pub description: String,
    pub object: Object,
    pub requirements: Vec<Requirement>,
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
