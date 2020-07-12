use crate::common::action::Action;

/// Defines a screen in the CLI.
pub struct Screen {
  pub name: String,
  pub action: Action,
}
