use crate::shared::action::Prompt;

/// Defines a screen in the CLI that can be shown to the user.
pub struct Screen {
  pub name: String,
  pub action: Prompt,
}
