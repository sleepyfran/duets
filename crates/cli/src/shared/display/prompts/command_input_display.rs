use crate::shared::action::CliAction;
use crate::shared::commands;
use crate::shared::commands::{Command, CommandCollection};
use crate::shared::context::Context;
use crate::shared::display;
use crate::shared::input;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(
    text: String,
    show_prompt_emoji: bool,
    available_commands: CommandCollection,
    after_action: Box<dyn FnOnce(&Command, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    let (command, args) = show_command_input_action(&text, show_prompt_emoji, &available_commands);
    let result = (command.execute)(args, context);

    match result {
        CliAction::Continue => after_action(&command, context),
        _ => result,
    }
}

fn show_command_input_action(
    text: &String,
    show_prompt_emoji: bool,
    available_commands: &CommandCollection,
) -> (Command, Vec<String>) {
    if show_prompt_emoji {
        display::show_prompt_text_with_new_line(&text);
    } else {
        display::show_prompt_text_with_new_line_no_emoji(&text);
    }

    get_command(available_commands)
}

fn get_command(available_commands: &CommandCollection) -> (Command, Vec<String>) {
    let commands_with_defaults = get_commands_with_defaults(&available_commands);

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

fn get_commands_with_defaults(available_commands: &CommandCollection) -> CommandCollection {
    // We might get called from a command that previously added the defaults already, so skip if
    // this is the case.
    let mut clonned = available_commands.clone();
    let commands_with_defaults = if includes_default_commands(available_commands) {
        clonned
    } else {
        clonned
            .chain(vec![
                commands::exit::create_exit_command(),
                commands::save::create_save_command(),
            ])
            .clone()
    };

    commands_with_defaults
        .clone()
        .chain(vec![commands::help::create_help_command(
            commands_with_defaults.clone(),
        )])
        .clone()
}

fn includes_default_commands(commands: &CommandCollection) -> bool {
    commands.find_by_name(&String::from("exit")).is_some()
}

fn show_help() {
    display::show_error(&String::from(
        "Unrecognized command. Use 'help' to show the list of all commands available right now",
    ))
}
