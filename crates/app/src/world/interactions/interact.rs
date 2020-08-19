use engine::entities::Object;

use crate::context::Context;

/// Represents the result of an interaction with an object. Has a short description of how it turned
/// out (for example, text to show after practicing an instrument) and the new context that turned
/// out after executing the interaction.
#[derive(Clone)]
pub struct InteractResult {
    pub description: String,
    pub context: Context,
}

/// Defines an interaction with a name and a description that can be shown to the user.
#[derive(Clone)]
pub struct Interaction {
    pub name: String,
    pub description: String,
    pub object: Object,
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
