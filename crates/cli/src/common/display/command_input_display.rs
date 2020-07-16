use super::common;
use crate::common::action::ActionResult;
use crate::common::commands::Command;
use crate::common::input;
use crate::common::screen::Screen;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(
    screen: &Screen,
    text: &String,
    available_commands: &Vec<Command>,
    on_action: &fn(&Command, &Screen) -> ActionResult,
) -> ActionResult {
    let command = show_command_input_action(text, available_commands);
    on_action(&command, screen)
}

fn show_command_input_action<'a>(
    text: &String,
    available_commands: &'a Vec<Command>,
) -> &'a Command {
    common::show_start_text(text);
    get_command(available_commands)
}

fn get_command(available_commands: &Vec<Command>) -> &Command {
    let commandOrError = input::read_command(available_commands);

    match commandOrError {
        Ok(command) => command,
        _ => {
            show_help(available_commands);
            get_command(available_commands)
        }
    }
}

fn show_help(available_commands: &Vec<Command>) {
    println!(
        "Unrecognized command. Use 'help' to show the list of all commands available right now"
    )
}
