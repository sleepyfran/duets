use std::sync::Arc;

use super::Command;
use crate::common::action::{CliAction, Prompt};

/// Allows the user to exit the game.
pub fn create_exit_command() -> Command {
    Command {
        name: String::from("exit"),
        matching_names: vec![],
        explanation: String::from("Exits from the game saving the progress"),
        execute: Arc::new(move |_args, _global_context| CliAction::Prompt(Prompt::NoOp)),
    }
}
