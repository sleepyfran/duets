use std::sync::Arc;

use super::Command;
use crate::display;
use crate::shared::action::CliAction;

/// Allows the user to clear the screen.
pub fn create_clear_command() -> Command {
    Command {
        name: String::from("clear"),
        matching_names: vec![],
        explanation: String::from("Clears the screen"),
        help: r#"
clear
----
clears the screen.
        "#
        .into(),
        execute: Arc::new(move |_args, _global_context| {
            display::clear();
            CliAction::Continue
        }),
    }
}
