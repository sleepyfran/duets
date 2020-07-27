use chrono::NaiveDate;

use crate::common::action::{CliAction, DateFormat};
use crate::common::context::Context;
use crate::common::display;
use crate::common::input;

/// Shows the initial text of the screen, takes the user input as a string and
/// calls the given on_action with the provided input.
pub fn handle(
    text: String,
    format: DateFormat,
    on_action: Box<dyn FnOnce(NaiveDate, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    let input = show_date_input_action(text, &format);
    on_action(input, context)
}

fn show_date_input_action(text: String, format: &DateFormat) -> NaiveDate {
    display::show_prompt_text(&text);
    display::show_text(&format!(" Format: {}", get_format_string(format)));
    display::show_line_break();

    get_date(format)
}

fn get_format_string(date_format: &DateFormat) -> String {
    match date_format {
        DateFormat::Year => String::from("YYYY"),
        DateFormat::Full => String::from("dd-mm-YYYY"),
    }
}

fn get_date(format: &DateFormat) -> NaiveDate {
    let choice_or_error = input::read_date(format);
    match choice_or_error {
        Ok(choice) => choice,
        Err(_) => get_date_with_error(format),
    }
}

fn get_date_with_error(format: &DateFormat) -> NaiveDate {
    display::show_error(&format!(
        "Did not recognize a valid date, remember the format is {}. Try again:",
        get_format_string(format)
    ));
    get_date(format)
}
