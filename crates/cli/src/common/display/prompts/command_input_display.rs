use crate::common::action::CliAction;
use crate::common::commands;
use crate::common::commands::Command;
use crate::common::context::Context;
use crate::common::display;
use crate::common::input;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(
    text: String,
    show_prompt_emoji: bool,
    available_commands: Vec<Command>,
    context: &Context,
) -> CliAction {
    let (command, args) = show_command_input_action(&text, show_prompt_emoji, &available_commands);
    (command.execute)(args, context)
}

fn show_command_input_action(
    text: &String,
    show_prompt_emoji: bool,
    available_commands: &Vec<Command>,
) -> (Command, Vec<String>) {
    if show_prompt_emoji {
        display::show_prompt_text_with_new_line(&text);
    } else {
        display::show_prompt_text_with_new_line_no_emoji(&text);
    }

    get_command(available_commands)
}

fn get_command(available_commands: &Vec<Command>) -> (Command, Vec<String>) {
    let commands_with_defaults = get_commands_with_defaults(available_commands);

    display::show_text(&String::from("> "));
    let command_or_error = input::read_command(&commands_with_defaults);

    match command_or_error {
        Some((command, args)) => (command.clone(), args),
        _ => {
            show_help();
            get_command(available_commands)
        }
    }
}

fn get_commands_with_defaults(available_commands: &Vec<Command>) -> Vec<Command> {
    // We might get called from a command that previously added the defaults already, so skip if
    // this is the case.
    let commands_with_defaults = if includes_default_commands(available_commands) {
        available_commands.to_vec()
    } else {
        available_commands
            .clone()
            .into_iter()
            .chain(vec![commands::exit::create_exit_command()])
            .collect()
    };

    commands_with_defaults
        .clone()
        .into_iter()
        .chain(vec![commands::help::create_help_command(
            commands_with_defaults,
        )])
        .collect()
}

fn includes_default_commands(commands: &Vec<Command>) -> bool {
    commands
        .iter()
        .filter(|command| command.name == "exit")
        .count()
        > 0
}

fn show_help() {
    display::show_error(&String::from(
        "Unrecognized command. Use 'help' to show the list of all commands available right now",
    ))
}
