use super::common;
use crate::common::commands::Command;

/// Reads a line and attempts to match it with one of the given available
/// commands. If we're unable to do so, returns an error.
pub fn read_command(available_commands: &Vec<Command>) -> Option<&Command> {
    let input = common::read_from_stdin_trimmed();

    available_commands
        .iter()
        .filter(|command| matches_command(&input, command))
        .nth(0)
}

fn matches_command(input: &str, command: &Command) -> bool {
    command.name == input || command.matching_names.contains(&input.to_string())
}
