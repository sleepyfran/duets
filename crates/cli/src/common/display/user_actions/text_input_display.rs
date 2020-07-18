use super::common;
use crate::common::action::ActionResult;
use crate::common::input;

/// Shows the initial text of the screen, takes the user input as a string and
/// calls the given on_action with the provided input.
pub fn handle(text: &String, on_action: &fn(&String) -> ActionResult) -> ActionResult {
    let input = show_text_input_action(text);
    on_action(&input)
}

fn show_text_input_action(text: &String) -> String {
    common::show_start_text_with_new_line(text);
    input::read_line()
}
