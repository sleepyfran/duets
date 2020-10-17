use crate::context::Context;

use super::requirement::*;

/// Result of the interaction with a generic type that represents the outcome of the interaction
/// and the updated context.
pub struct InteractResult<R> {
    /// Generic result of the interaction. Depends on the specific interaction.
    pub result: R,
    /// Context with the changes that happened during the interaction.
    pub context: Context,
}

/// Holds the different types of actions that can be done in an interaction sequence.
pub enum InteractItem<R> {
    /// End action. Returns the result of the interaction held in a generic type that depends
    /// on the type of interaction that was returned and the updated context.
    Result(InteractResult<R>),
    /// Nothing, nada. Initial state for things like requirement which don't depend on any result
    /// in particular.
    NoOp,
}

/// Represents a result that when on the ok state holds all the items in the sequence of actions
/// that are required to perform a certain interaction. For example, for composing: asking if
/// new song or continue -> if new song ask for title, style -> compose.
pub type InteractSequence<R> = Result<InteractItem<R>, Requirement>;

/// Defines a common interface for all interactions.
pub trait Interaction: Clone + Default {
    type Result;

    fn name(&self) -> String;
    fn description(&self) -> String;
    fn requirements(&self) -> Vec<Requirement>;
    fn interact(&self, context: &Context) -> InteractSequence<Self::Result>;
}
