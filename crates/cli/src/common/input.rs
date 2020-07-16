use crate::common::commands::{Command, InvalidCommandError};
use ::std::io::stdin;

/// Reads a single line from the user.
pub fn read_line() -> String {
    return read_from_stdin();
}

/// Reads a line and attempts to parse a number from it. If we're unable
/// to do so, returns an error.
pub fn read_number() -> Result<i32, std::num::ParseIntError> {
    let input = read_from_stdin();
    return input.trim().parse::<i32>();
}

/// Reads a line and attempts to match it with one of the given available
/// commands. If we're unable to do so, returns an error.
pub fn read_command(available_commands: &Vec<Command>) -> Result<&Command, InvalidCommandError> {
    let input = read_from_stdin();
    let trimmed_input = input.trim();

    for command in available_commands {
        if matches_command(trimmed_input, command) {
            return Ok(command);
        }
    }

    Err(InvalidCommandError)
}

fn matches_command(input: &str, command: &Command) -> bool {
    command.name == input || command.matching_names.contains(&input.to_string())
}

fn read_from_stdin() -> String {
    let mut input = String::new();
    stdin()
        .read_line(&mut input)
        .expect("Attempted to read input but was not available.");

    return input;
}
