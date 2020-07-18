use chrono::NaiveDate;

use crate::common::commands::Command;
use crate::common::screen::Screen;

/// Defines a choice that the user can make.
pub struct Choice {
  pub id: usize,
  pub text: String,
}

/// Defines the result of an action, which can be a screen to redirect to,
/// another action to execute or another kind of side effect.
pub enum ActionResult {
  Prompt(Prompt),
  Screen(Screen),
  SideEffect(fn() -> Option<ActionResult>),
}

/// Defines the different kinds of actions that the user can do as a response
/// to a certain screen.
pub enum Prompt {
  /// Represents a simple free text input.
  TextInput {
    text: String,
    on_action: fn(&String) -> ActionResult,
  },
  /// Represents an input that only accepts a set of commands.
  CommandInput {
    text: String,
    available_commands: Vec<Command>,
    on_action: fn(&Command) -> ActionResult,
  },
  /// Represents an input that only accepts a set of choices by asking the user
  /// to input its ID (a number).
  ChoiceInput {
    text: String,
    choices: Vec<Choice>,
    on_action: fn(&Choice) -> ActionResult,
  },
  /// Represents an input that accepts a set of predefined strings. For example
  /// a yes/no input.
  TextChoiceInput {
    text: String,
    choices: Vec<Choice>,
    on_action: fn(&Choice) -> ActionResult,
  },
  /// Represents a NaiveDate input.
  DateInput {
    text: String,
    on_action: fn(&NaiveDate) -> ActionResult,
  },
  /// Represents a no operation. Basically tells the program to stop.
  NoOp,
}
