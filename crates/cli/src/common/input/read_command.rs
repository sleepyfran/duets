use super::common;
use super::common::InvalidInputError;
use crate::common::commands::Command;

/// Reads a line and attempts to match it with one of the given available
/// commands. If we're unable to do so, returns an error.
pub fn read_command(available_commands: &Vec<Command>) -> Result<&Command, InvalidInputError> {
    let input = common::read_from_stdin_trimmed();

    for command in available_commands {
        if matches_command(&input, command) {
            return Ok(command);
        }
    }

    Err(InvalidInputError)
}

fn matches_command(input: &str, command: &Command) -> bool {
    command.name == input || command.matching_names.contains(&input.to_string())
}
