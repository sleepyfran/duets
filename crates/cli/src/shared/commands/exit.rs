use std::sync::Arc;

use super::Command;
use crate::effects;
use crate::shared::action::CliAction;

/// Allows the user to exit the game.
pub fn create_exit_command() -> Command {
    Command {
        name: String::from("exit"),
        matching_names: vec![],
        explanation: String::from("Exits from the game"),
        help: r#"
exit
----
Exits from the game.
        "#
        .into(),
        execute: Arc::new(move |_args, _global_context| {
            effects::exit();
            CliAction::NoOp
        }),
    }
}
