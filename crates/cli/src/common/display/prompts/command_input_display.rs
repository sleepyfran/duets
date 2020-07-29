use crate::common::action::CliAction;
use crate::common::commands::Command;
use crate::common::context::Context;
use crate::common::display;
use crate::common::input;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(text: String, available_commands: Vec<Command>, context: &Context) -> CliAction {
    let (command, args) = show_command_input_action(&text, &available_commands);
    (command.execute)(args, context)
}

fn show_command_input_action<'a>(
    text: &String,
    available_commands: &'a Vec<Command>,
) -> (&'a Command, Vec<String>) {
    display::show_prompt_text_with_new_line(&text);
    get_command(available_commands)
}

fn get_command(available_commands: &Vec<Command>) -> (&Command, Vec<String>) {
    let command_or_error = input::read_command(&available_commands);

    match command_or_error {
        Some((command, args)) => (command, args),
        _ => {
            show_help(available_commands);
            get_command(available_commands)
        }
    }
}

fn show_help(available_commands: &Vec<Command>) {
    display::show_error(&String::from(
        "Unrecognized command. Use 'help' to show the list of all commands available right now",
    ))
}
