use crate::common::command::Command;
use crate::common::screen::Screen;

/// Defines a choice that the user can make.
pub struct Choice {
  pub id: i32,
  pub text: String,
}

/// Defines the result of an action, which can be a screen to redirect to,
/// another action to execute or another kind of side effect.
pub enum ActionResult {
  Action(Action),
  Screen(Screen),
  SideEffect(fn() -> ()),
}

/// Defines the different kinds of actions that the user can do as a response
/// to a certain screen.
pub enum Action {
  TextInput {
    text: String,
    on_action: fn(&String, &Screen) -> ActionResult,
  },
  CommandInput {
    text: String,
    on_action: fn(&String, &Screen) -> ActionResult,
    available_commands: Vec<Command>,
  },
  ChoiceInput {
    text: String,
    on_action: fn(&Choice, &Screen) -> ActionResult,
    choices: Vec<Choice>,
  },
  NoOp,
}
