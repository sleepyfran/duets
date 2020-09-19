use chrono::NaiveDate;

use crate::screens::GameScreen;
use crate::shared::commands::CommandCollection;
use crate::shared::context::Context;

/// Defines an action to be performed by the CLI, whether it's to show a
/// prompt to the user, a screen or perform a side effect.
pub enum CliAction {
    Chain(Box<CliAction>, Box<CliAction>),
    Prompt(Prompt),
    Screen(GameScreen),
    SideEffect(Box<dyn FnOnce() -> CliAction>),
    Continue,
    NoOp,
}

/// Defines a choice that the user can make.
pub struct Choice {
    pub id: usize,
    pub text: String,
}

/// Defines a choice that the user can make in a confirmation input.
pub enum ConfirmationChoice {
    Yes,
    No,
}

/// Defines the different formats that we can retrieve with a DateInput.
pub enum DateFormat {
    Year,
    Full,
}

/// Defines how many repetitions are allowed when invoking a command input.
pub enum CommandInputRepetition {
    One,
    Until(Box<dyn FnOnce(&CliAction) -> Repeat>),
}

/// Defines whether the command input should repeat taking commands or not.
pub enum Repeat {
    Yes,
    No,
}

/// Defines the different kinds of actions that the user can do as a response
/// to a certain screen.
pub enum Prompt {
    /// Represents a simple free text input.
    TextInput {
        text: String,
        on_action: Box<dyn FnOnce(String, &Context) -> CliAction>,
    },
    /// Represents an input that only accepts a set of commands.
    CommandInput {
        text: String,
        show_prompt_emoji: bool,
        available_commands: CommandCollection,
        repetition: CommandInputRepetition,
        /// Closure to be called with the result of the command in case the calling action needs
        /// to override it when repetition is set to Until.
        after_action: Box<dyn FnOnce(CliAction, &Context) -> CliAction>,
    },
    /// Represents an input that only accepts a set of choices by asking the user
    /// to input its ID (a number).
    ChoiceInput {
        text: String,
        choices: Vec<Choice>,
        on_action: Box<dyn FnOnce(&Choice, &Context) -> CliAction>,
    },
    /// Represents an input that accepts a set of predefined strings. For example
    /// a yes/no input.
    TextChoiceInput {
        text: String,
        choices: Vec<Choice>,
        on_action: Box<dyn FnOnce(&Choice, &Context) -> CliAction>,
    },
    /// Basic yes/no input.
    ConfirmationInput {
        text: String,
        on_action: Box<dyn FnOnce(&ConfirmationChoice, &Context) -> CliAction>,
    },
    /// Represents a NaiveDate input.
    DateInput {
        text: String,
        format: DateFormat,
        on_action: Box<dyn FnOnce(NaiveDate, &Context) -> CliAction>,
    },
}
