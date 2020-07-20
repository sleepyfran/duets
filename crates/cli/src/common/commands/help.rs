use super::Command;

use crate::common::display;

/// Shows a list of all available commands in the current context to the user.
pub fn create_help_command(available_commands: Vec<Command>) -> Command {
    Command {
        name: String::from("help"),
        matching_names: vec![],
        explanation: String::from("Shows the list of all commands available in the current context with their explanation"),
        execute: Box::new(move |_args, _screen| {
            display::show_text_with_new_line(
                &String::from("Commands available in this screen:")
            );

            for command in &available_commands {
                show_command(&command);
            }
        }),
    }
}

fn show_command(command: &Command) {
    display::show_text_with_new_line(&format!("{}: {}", command.name, command.explanation));
}
