use common::entities::{Identity, Skill};
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
    /// End action. Indicates the calling function to compute the result given the parameters of the
    /// interaction and the limitations.
    End,
}

/// Represents a result that when on the ok state holds all the items in the sequence of actions
/// that are required to perform a certain interaction. For example, for composing: asking if
/// new song or continue -> if new song ask for title, style -> compose.
pub type InteractSequence = Result<InteractItem, Requirement>;

/// Represents the amound of time that an interaction consumes.
pub enum TimeConsumption {
    None,
    TimeUnit(u8),
    Days(u8),
}

/// Represents the type of effect over some resource.
pub enum EffectType {
    Positive(u8),
    Negative(u8),
}

/// Represents an effect of the interaction over the environment or the character.
pub enum InteractionEffect {
    Time(TimeConsumption),
    Health(EffectType),
    Energy(EffectType),
    Skill(Skill, EffectType),
    Song(EffectType),
}

/// Represents how the interaction affects the character and the environment.
pub struct InteractionEffects {
    /// Effects specified in here will always be applied, even if the limits are not met.
    pub always_applied: Vec<InteractionEffect>,
    /// Effects specified in here will only be applied after the limits are checked to be okay.
    pub applied_after_interaction: Vec<InteractionEffect>,
}

/// Represents the number of times that the character is allowed to do the interaction.
pub enum InteractionTimes {
    Unlimited,
    Once,
    Multiple(u8),
}

/// Defines a common interface for all interactions.
pub trait Interaction: Identity {
    /// Friendly name to show to the user.
    fn name(&self) -> String;
    /// Friendly description to show to the user.
    fn description(&self) -> String;
    /// Indicates the requirements from the character to perform the interaction.
    fn requirements(&self) -> Vec<Requirement>;
    /// Indicates whether the interaction should be tracked in the action_registry.
    fn track_action(&self) -> bool;
    /// Indicates how many times the character is allowed to do this interaction in one day. Needs
    /// to be used in conjunction with track_action since otherwise there's no way of knowing how
    /// many times the character performs the action. If the limit is reached the result of the
    /// interaction will be failure but the interaction consumptions will still apply.
    fn limit_daily_interactions(&self) -> InteractionTimes;
    /// Indicates the effects that the interaction has on the character and the environment. The
    /// negative effects will always be applied while the positive will only be applied if the
    /// limit was not reached.
    fn effects(&self, context: &Context) -> InteractionEffects;
    /// Returns the sequence (if any) between the user and the interaction until reaching the end
    /// state that provides a result to compute.
    fn sequence(&self, context: &Context) -> InteractSequence;
    /// Returns the messages to show to the user, the first one indicates the message to show
    /// when the interaction is succesful and the second when one of the limits fails.
    fn messages(&self, context: &Context) -> (String, String);
}
