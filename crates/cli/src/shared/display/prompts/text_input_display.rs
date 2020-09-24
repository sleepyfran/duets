use crate::shared::action::{CliAction, PromptText};
use crate::shared::context::Context;
use crate::shared::display;
use crate::shared::input;

/// Shows the initial text of the screen, takes the user input as a string and
/// calls the given on_action with the provided input.
pub fn handle(
    text: PromptText,
    on_action: Box<dyn FnOnce(String, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    let input = show_text_input_action(text);
    on_action(input, context)
}

fn show_text_input_action(text: PromptText) -> String {
    display::show_prompt_text_with_new_line(text);
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
