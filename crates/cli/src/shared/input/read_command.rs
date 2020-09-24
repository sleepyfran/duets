use super::common;
use crate::shared::commands::{Command, CommandCollection};

/// Reads a line and attempts to match it with one of the given available
/// commands. If we're unable to do so, returns an error.
pub fn read_command(available_commands: &CommandCollection) -> Option<(Command, Vec<String>)> {
    let input = common::read_from_stdin_trimmed();
    let input_keywords: Vec<&str> = input.split(' ').collect();
    let command_name: String = input_keywords[0].into();

    let command_or_none = available_commands.find_by_name(&command_name);

    match command_or_none {
        Some(command) => Some((command, map_args(input_keywords))),
        None => None,
    }
}

fn map_args(args: Vec<&str>) -> Vec<String> {
    args.iter().skip(1).map(|arg| arg.to_string()).collect()
}
