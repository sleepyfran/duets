pub mod character;
mod command_collection;
pub mod exit;
pub mod help;
pub mod map;
pub mod save;
pub mod time;

pub use command_collection::CommandCollection;

use std::borrow::Borrow;
use std::hash::{Hash, Hasher};
use std::sync::Arc;

use crate::shared::action::CliAction;
use crate::shared::context::Context;

/// Defines the common fields that any command should have.
#[derive(Clone)]
pub struct Command {
    /// Friendly name of the command. Example: help, install.
    pub name: String,

    /// Quick explanation that will be given when the user asks for a list of available commands.
    pub explanation: String,

    /// Explanation that will be shown when the user asks for help for this specific command.
    pub help: String,

    /// List of matching names that can invoke the command. Example: help, h.
    pub matching_names: Vec<String>,

    /// Function to call when the command is executed. The args passed to the command will be passed
    /// plus the current global context of the game.
    pub execute: Arc<dyn Fn(Vec<String>, &Context) -> CliAction>,
}

impl Hash for Command {
    fn hash<H: Hasher>(&self, hasher: &mut H) {
        self.name.hash(hasher)
    }
}

impl PartialEq for Command {
    fn eq(&self, other: &Self) -> bool {
        self.name == other.name
    }
}

impl Eq for Command {}

impl Borrow<String> for Command {
    fn borrow(&self) -> &String {
        &self.name
    }
}
