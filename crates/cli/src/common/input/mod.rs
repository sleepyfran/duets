pub mod read_choice;
pub use read_choice::read_choice;

use chrono::{NaiveDate, ParseError};

use crate::common::action::Choice;
use crate::common::commands::{Command, InvalidInputError};
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
pub fn read_command(available_commands: &Vec<Command>) -> Result<&Command, InvalidInputError> {
    let input = read_from_stdin_trimmed();

    for command in available_commands {
        if matches_command(&input, command) {
            return Ok(command);
        }
    }

    Err(InvalidInputError)
}

/// Reads a line and attempts to match it with one of the given choices by
/// matching it with its text. If we're unable to do so, returns an error.
pub fn read_text_choice(choices: &Vec<Choice>) -> Result<&Choice, InvalidInputError> {
    let input = read_from_stdin_trimmed();

    for choice in choices {
        if choice.text == input {
            return Ok(choice);
        }
    }

    Err(InvalidInputError)
}

/// Reads a line and attempts to parse a NaiveDate from it. If we're unable
/// to do so, returns an error.
pub fn read_date() -> Result<NaiveDate, ParseError> {
    let input = read_from_stdin_trimmed();

    NaiveDate::parse_from_str(&input, "%d-%m-%Y")
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

fn read_from_stdin_trimmed() -> String {
    let input = read_from_stdin();
    input.trim().to_string()
}
