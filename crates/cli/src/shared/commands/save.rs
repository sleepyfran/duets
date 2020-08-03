use std::sync::Arc;

use super::Command;
use crate::effects;
use crate::shared::action::CliAction;

/// Saves the content of the game.
pub fn create_save_command() -> Command {
    Command {
        name: String::from("save"),
        matching_names: vec![],
        explanation: String::from("Saves the current game"),
        help: r#"
save
----
Saves the current game into the savegame file.
        "#
        .into(),
        execute: Arc::new(move |_args, global_context| {
            effects::save(global_context);
            CliAction::Continue
        }),
    }
}
