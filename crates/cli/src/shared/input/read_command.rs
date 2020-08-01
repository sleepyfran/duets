use super::common;
use crate::shared::commands::Command;

/// Reads a line and attempts to match it with one of the given available
/// commands. If we're unable to do so, returns an error.
pub fn read_command(available_commands: &Vec<Command>) -> Option<(&Command, Vec<String>)> {
    let input = common::read_from_stdin_trimmed();
    let input_keywords: Vec<&str> = input.split(" ").collect();
    let command_name: String = input_keywords[0].into();

    let command_or_none = available_commands
        .iter()
        .filter(|command| matches_command(&command_name, command))
        .nth(0);

    match command_or_none {
        Some(command) => Some((command, map_args(input_keywords))),
        None => None,
    }
}

fn map_args(args: Vec<&str>) -> Vec<String> {
    args.iter().map(|arg| arg.to_string()).collect()
}

fn matches_command(input: &str, command: &Command) -> bool {
    command.name == input || command.matching_names.contains(&input.to_string())
}
