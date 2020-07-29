pub mod exit;
pub mod help;

use std::sync::Arc;

use crate::common::action::CliAction;
use crate::common::context::Context;

/// Defines the common fields that any command should have.
#[derive(Clone)]
pub struct Command {
    /// Friendly name of the command. Example: help, install.
    pub name: String,

    /// Explanation that will be given when the user asks for help.
    pub explanation: String,

    /// List of matching names that can invoke the command. Example: help, h.
    pub matching_names: Vec<String>,

    /// Function to call when the command is executed. The args passed to the command will be passed
    /// plus the current global context of the game.
    pub execute: Arc<dyn Fn(Vec<String>, &Context) -> CliAction>,
}
