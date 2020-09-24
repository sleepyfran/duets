use crate::shared::action::Choice;
use crate::shared::action::CliAction;
use crate::shared::context::Context;
use crate::shared::display;
use crate::shared::input;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(
    text: String,
    choices: Vec<Choice>,
    on_action: Box<dyn FnOnce(&Choice, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    let input = show_text_choice_input_action(&text, &choices);
    on_action(input, context)
}

fn show_text_choice_input_action<'a>(text: &str, choices: &'a [Choice]) -> &'a Choice {
    display::show_prompt_text(&text);
    display::show_text(&String::from(" ["));

    for (index, choice) in choices.iter().enumerate() {
        let separator = if index == choices.len() - 1 { "" } else { "|" };
        print!("{}{}", choice.text, separator);
    }

    display::show_text_with_new_line(&String::from("]"));

    get_choice(choices)
}

fn get_choice(choices: &[Choice]) -> &Choice {
    let choice_or_error = input::read_text_choice(&choices);
    match choice_or_error {
        Some(choice) => choice,
        _ => get_choice_with_error(choices),
    }
}

fn get_choice_with_error(choices: &[Choice]) -> &Choice {
    display::show_error(&String::from("Invalid option. Try again:"));
    get_choice(choices)
}
