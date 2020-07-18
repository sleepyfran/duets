use crate::common::action::UserAction;

/// Defines a screen in the CLI.
pub struct Screen {
  pub name: String,
  pub action: UserAction,
}
