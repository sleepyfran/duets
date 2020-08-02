use chrono::NaiveDate;

use crate::shared::commands::Command;
use crate::shared::context::Context;
use crate::shared::screen::Screen;

/// Defines an action to be performed by the CLI, whether it's to show a
/// prompt to the user, a screen or perform a side effect.
pub enum CliAction {
    Prompt(Prompt),
    Screen(Screen),
    SideEffect(fn() -> Option<CliAction>),
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
        available_commands: Vec<Command>,
        /// Closure to be called in case the command has a return action of Continue, which
        /// basically delegates to the called of this command the action to be returned.
        after_action: Box<dyn FnOnce(&Command, &Context) -> CliAction>,
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
