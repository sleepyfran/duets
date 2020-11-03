use crate::shared::action::CliAction;

/// Defines a screen in the CLI that can be shown to the user.
pub struct Screen {
    pub name: String,
    pub action: CliAction,
}
