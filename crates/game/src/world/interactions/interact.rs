use common::results::InteractResult;

use super::requirement::*;
use crate::context::Context;

/// Result of the interaction with a generic type that represents the outcome of the interaction
/// and the updated context.
pub struct InteractEnd {
    /// Generic result of the interaction. Depends on the specific interaction.
    pub result: InteractResult,
    /// Context with the changes that happened during the interaction.
    pub context: Context,
}

/// Holds the different types of actions that can be done in an interaction sequence.
pub enum InteractItem {
    /// End action. Returns the result of the interaction held in a generic type that depends
    /// on the type of interaction that was returned and the updated context.
    Result(InteractEnd),
    /// Nothing, nada. Initial state for things like requirement which don't depend on any result
    /// in particular.
    NoOp,
}

/// Represents a result that when on the ok state holds all the items in the sequence of actions
/// that are required to perform a certain interaction. For example, for composing: asking if
/// new song or continue -> if new song ask for title, style -> compose.
pub type InteractSequence = Result<InteractItem, Requirement>;

/// Defines a common interface for all interactions.
pub trait Interaction: Clone {
    /// Returns the ID associated with this interaction. Used in the action_registry to identify
    /// the interaction.
    fn id(&self) -> String;
    fn name(&self) -> String;
    fn description(&self) -> String;
    fn requirements(&self) -> Vec<Requirement>;
    fn interact(&self, context: &Context) -> InteractSequence;
}
