use super::common;
use crate::common::action::Choice;
use crate::common::action::CliAction;
use crate::common::input;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(
    text: &String,
    choices: &Vec<Choice>,
    on_action: &fn(&Choice) -> CliAction,
) -> CliAction {
    let input = show_choice_input_action(text, choices);
    on_action(&input)
}

fn show_choice_input_action<'a>(text: &String, choices: &'a Vec<Choice>) -> &'a Choice {
    common::show_start_text_with_new_line(text);

    for choice in choices {
        println!("{}: {}", get_display_index(choice.id), choice.text);
    }

    get_choice(choices)
}

fn get_choice(choices: &Vec<Choice>) -> &Choice {
    match input::read_number() {
        Ok(choice) => {
            let index = &(choice as usize);
            if (1..choices.len() + 1).contains(index) {
                return choices
                    .iter()
                    .filter(|c| is_selected_index(c.id, *index))
                    .nth(0)
                    .unwrap();
            }

            get_choice_with_error(choices)
        }
        Err(_) => get_choice_with_error(choices),
    }
}

fn get_choice_with_error(choices: &Vec<Choice>) -> &Choice {
    println!("Invalid option. Try again:");
    get_choice(choices)
}

fn is_selected_index(iter_index: usize, selected_index: usize) -> bool {
    iter_index + 1 == selected_index
}

fn get_display_index(index: usize) -> usize {
    index + 1
}
