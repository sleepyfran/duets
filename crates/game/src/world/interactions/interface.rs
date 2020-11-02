use common::entities::{Identity, Skill};
use common::results::InteractResult;

use super::outcomes::*;
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

/// Represents an option that the user can choose from.
#[derive(Clone)]
pub struct InteractOption {
    pub id: String,
    pub text: String,
}

/// Holds the different types of actions that can be done in an interaction sequence.
#[derive(Clone)]
pub enum InteractItem {
    /// Combines two or items.
    Chain(Box<InteractItem>, Box<InteractItem>),
    /// Asks the user to select between different options.
    Options {
        question: String,
        options: Vec<InteractOption>,
    },
    /// Asks the user to confirm something.
    Confirmation {
        question: String,
        /// Represents the different branches that the item can get depending on the answer. If
        /// the user answers yes then the first item will get executed, otherwise the second one
        /// will be chosen.
        branches: (Box<InteractItem>, Box<InteractItem>),
    },
    /// Asks the user to input some text.
    TextInput(String),
    /// End action. Indicates the calling function to compute the result given the parameters of the
    /// interaction and the limitations.
    End,
}

/// Represents a result that when on the ok state holds all the items in the sequence of actions
/// that are required to perform a certain interaction. For example, for composing: asking if
/// new song or continue -> if new song ask for title, style -> compose.
pub type InteractSequence = Result<InteractItem, Requirement>;

/// Represents the amound of time that an interaction consumes.
#[derive(Clone)]
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

/// Represents the different types of input that an user can give.
#[derive(Clone)]
pub enum InputType {
    Text(String),
    Option(InteractOption),
    Confirmation(bool),
}

impl InputType {
    /// Forces the current input type as a text. Panics if the current type is not that variant.
    pub fn as_text(self) -> String {
        match self {
            InputType::Text(text) => text,
            _ => panic!("The variant was not a text. Better check that sequence :)"),
        }
    }

    /// Forces the current input type as an option. Panics if the current type is not that variant.
    pub fn as_option(self) -> InteractOption {
        match self {
            InputType::Option(option) => option,
            _ => panic!("The variant was not an option. Better check that sequence :)"),
        }
    }

    /// Forces the current input type as a confirmation. Panics if the current type is not that variant.
    pub fn as_confirmation(self) -> bool {
        match self {
            InputType::Confirmation(confirmation) => confirmation,
            _ => panic!("The variant was not a confirmation. Better check that sequence :)"),
        }
    }
}

/// Represents the input that has to be given to the result function in order to process both
/// the effects associated with the interaction and the input given by the user.
#[derive(Clone)]
pub struct SequenceInput {
    /// Input given by the user (if any). This input is filled while processing the sequence given
    /// by an interaction.
    pub values: Vec<InputType>,
    pub context: Context,
}

impl SequenceInput {
    /// Adds input from the user into the list.
    pub fn add_input(self, input: InputType) -> Self {
        Self {
            values: self.values.into_iter().chain(vec![input]).collect(),
            ..self
        }
    }

    /// Returns a copy of itself with the input changed by the modify_fn.
    pub fn modify_input<F>(self, modify_fn: F) -> Self
    where
        F: FnOnce(Vec<InputType>) -> Vec<InputType>,
    {
        Self {
            values: modify_fn(self.values),
            ..self
        }
    }

    /// Changes the context and returns a copy of itself.
    pub fn with_context(self, context: &Context) -> Self {
        Self {
            context: context.clone(),
            ..self
        }
    }
}

/// Represents the output of the result function.
#[derive(Clone)]
pub struct SequenceOutput {
    pub values: Vec<InputType>,
    pub context: Context,
    pub outcomes: InteractionOutcome,
}

impl SequenceOutput {
    /// Returns a copy of itself with the values changed by the modify_fn.
    pub fn modify_values<F>(&self, modify_fn: F) -> Self
    where
        F: FnOnce(Vec<InputType>) -> Vec<InputType>,
    {
        let mut mut_self = self.clone();
        mut_self.values = modify_fn(mut_self.values);
        mut_self
    }

    /// Returns a copy of itself with the context changed by the modify_fn.
    pub fn modify_context<F>(&self, modify_fn: F) -> Self
    where
        F: FnOnce(Context) -> Context,
    {
        let mut mut_self = self.clone();
        mut_self.context = modify_fn(mut_self.context);
        mut_self
    }

    /// Adds a new outcome to the output.
    pub fn add_outcome(&self, outcome: Outcome) -> Self {
        let mut mut_self = self.clone();
        mut_self.outcomes = mut_self.outcomes.into_iter().chain(vec![outcome]).collect();
        mut_self
    }
}

/// Defines a common interface for all interactions.
pub trait Interaction: Identity + Send + Sync {
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
}
