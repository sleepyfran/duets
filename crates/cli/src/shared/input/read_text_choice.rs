use super::common;
use crate::shared::action::Choice;

/// Reads a line and attempts to match it with one of the given choices by
/// matching it with its text. If we're unable to do so, returns an error.
pub fn read_text_choice(choices: &Vec<Choice>) -> Option<&Choice> {
    let input = common::read_from_stdin_trimmed();
    choices.iter().filter(|c| c.text == input).nth(0)
}
