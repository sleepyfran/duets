use std::sync::Arc;

use super::{Command, CommandCollection};
use crate::shared::action::CliAction;
use crate::shared::display;

/// Shows a list of all available commands in the current context to the user.
pub fn create_help_command(available_commands: CommandCollection) -> Command {
    Command {
        name: String::from("help"),
        matching_names: vec![],
        explanation: String::from("Shows the list of all commands available in the current context with their explanation"),
        help: r#"
help
----
When invoked with no arguments shows a list of all the commands available in the current
context. It's also possible to call it with a command name to show the complete explanation
of it.
        "#
        .into(),
        execute: Arc::new(move |args, _screen| {
            if args.is_empty() {
                show_command_list(&available_commands);
            } else {
                show_command_help(available_commands.find_by_name(&args[0]))
            }

            CliAction::Continue
        }),
    }
}

fn show_command_list(available_commands: &CommandCollection) {
    display::show_text_with_new_line(&String::from("Commands available in this screen:"));

    for command in available_commands.to_vec() {
        show_command(&command);
    }
}

fn show_command(command: &Command) {
    display::show_text_with_new_line(
        &format!("{}: {}",
        display::styles::title(&command.name),
        command.explanation
    ));
}

fn show_command_help(command_option: Option<Command>) {
    match command_option {
        Some(command) => display::show_text_with_new_line(&command.help),
        None => display::show_error(&String::from("No command found with that name. Invoke help with no arguments to show the list of available commands")),
    }
}
