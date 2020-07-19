use super::common;
use super::common::InvalidInputError;
use crate::common::action::Choice;

/// Reads a line and attempts to match it with one of the given choices by
/// matching it with its text. If we're unable to do so, returns an error.
pub fn read_text_choice(choices: &Vec<Choice>) -> Result<&Choice, InvalidInputError> {
    let input = common::read_from_stdin_trimmed();

    for choice in choices {
        if choice.text == input {
            return Ok(choice);
        }
    }

    Err(InvalidInputError)
}
