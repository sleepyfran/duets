use crate::common::action::Choice;
use crate::common::action::CliAction;
use crate::common::display;
use crate::common::input;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(
    text: String,
    choices: Vec<Choice>,
    on_action: Box<dyn FnOnce(&Choice) -> CliAction>,
) -> CliAction {
    let input = show_choice_input_action(&text, &choices);
    on_action(input)
}

fn show_choice_input_action<'a>(text: &String, choices: &'a Vec<Choice>) -> &'a Choice {
    display::show_start_text_with_new_line(text);
    input::read_choice(choices)
}
