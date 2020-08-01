use crate::shared::action::Prompt;

/// Defines a screen in the CLI.
pub struct Screen {
  pub name: String,
  pub action: Prompt,
}
