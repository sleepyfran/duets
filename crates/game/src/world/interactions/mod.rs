mod instruments;
mod interact;
mod requirement;

pub use interact::*;
pub use requirement::*;

use common::entities::{Object, ObjectType};

use crate::context::Context;

/// Returns all the available interactions for the given object.
pub fn get_for(object: &Object) -> Vec<impl Interaction> {
    match object.r#type {
        ObjectType::Instrument(_) => instruments::get_interactions(),
        _ => vec![],
    }
}

/// Checks for all the requirements of the given interaction and, if all passed, calls the interact
/// method in it.
pub fn interact<I: Interaction>(interaction: I, context: &Context) -> InteractSequence<I::Result> {
    check_requirements::<I>(context, interaction.requirements())?;

    interaction.interact(context)
}
