use crate::common::action::CliAction;
use crate::common::context::Context;
use crate::common::display;
use crate::common::input;

/// Shows the initial text of the screen, takes the user input as a string and
/// calls the given on_action with the provided input.
pub fn handle(
    text: String,
    on_action: Box<dyn FnOnce(String, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    let input = show_text_input_action(text);
    on_action(input, context)
}

fn show_text_input_action(text: String) -> String {
    display::show_start_text_with_new_line(&text);
    get_input()
}

fn get_input() -> String {
    let input = input::read_line_trimmed();

    if input.is_empty() {
        get_input_with_error()
    } else {
        input
    }
}

fn get_input_with_error() -> String {
    display::show_error(&String::from("An empty input is not valid. Try again:"));
    get_input()
}
