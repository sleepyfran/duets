use std::sync::Arc;

use super::Command;
use crate::shared::action::{CliAction, Prompt};
use crate::shared::display;

/// Shows a list of all available commands in the current context to the user.
pub fn create_help_command(available_commands: Vec<Command>) -> Command {
    Command {
        name: String::from("help"),
        matching_names: vec![],
        explanation: String::from("Shows the list of all commands available in the current context with their explanation"),
        execute: Arc::new(move |_args, _screen| {
            display::show_text_with_new_line(
                &String::from("Commands available in this screen:")
            );

            for command in &available_commands {
                show_command(command);
            }

            CliAction::Prompt(Prompt::CommandInput {
                text: String::default(),
                show_prompt_emoji: false,
                available_commands: available_commands.clone(),
            })
        }),
    }
}

fn show_command(command: &Command) {
    display::show_text_with_new_line(&format!("{}: {}", command.name, command.explanation));
}
