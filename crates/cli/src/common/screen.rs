use crate::common::action::Prompt;

/// Defines a screen in the CLI.
pub struct Screen {
  pub name: String,
  pub action: Prompt,
}
