pub mod prompts;
pub mod styles;

use std::io::Write;

use crate::shared::action::CliAction;
use crate::shared::context::Context;
use crate::shared::emoji;
use crate::shared::screen::Screen;

/// Shows the specified screen. Since screens (at least as of right now) have
/// no other thing that just an identifier and an associated action, this simply
/// calls the show function in the prompts module to handle the inner
/// action of the screen.
pub fn show(screen: Screen, context: &Context) -> CliAction {
    prompts::show(screen.action, context)
}

/// Prints a new line to separate content.
pub fn show_line_break() {
    println!();
}

/// Shows a predefined exit message.
pub fn show_exit_message() {
    println!("Bye!");
}

/// Shows a default line of text (no title, no error) with a new line.
pub fn show_text_with_new_line(text: &String) {
    println!("{}", text)
}

/// Shows a default line of text without appending a new line.
pub fn show_text(text: &String) {
    print_and_flush(text.to_string())
}

/// Prints some info to the screen.
pub fn show_info(text: &String) {
    print_and_flush(format!("{} {}", emoji::for_info(), styles::info(text)))
}

/// Prints a warning to the screen.
pub fn show_warning(text: &String) {
    print_and_flush(format!(
        "{} {}",
        emoji::for_warning(),
        styles::warning(text)
    ))
}

/// Prints an error to the screen.
pub fn show_error(text: &String) {
    print_and_flush(format!("{} {}", emoji::for_error(), styles::error(text)))
}

/// Prints the start text of a prompt.
pub fn show_prompt_text_with_new_line(text: &String) {
    println!("{} {}", emoji::for_speech_bubble(), styles::title(text))
}

// Prints the start text of a prompt without a new line.
pub fn show_prompt_text(text: &String) {
    print_and_flush(format!(
        "{} {}",
        emoji::for_speech_bubble(),
        styles::title(text)
    ))
}

/// Prints the start text of a prompt with a new line and no emoji.
pub fn show_prompt_text_with_new_line_no_emoji(text: &String) {
    println!("{}", styles::title(text))
}

// Prints the start text of a prompt without a new line and no emoji.
pub fn show_prompt_text_no_emoji(text: &String) {
    print_and_flush(format!("{}", styles::title(text)))
}

/// Clears the screen completely.
pub fn clear() {
    print_and_flush(format!("{}[2J", 27 as char))
}

fn print_and_flush(text: String) {
    print!("{}", text);
    std::io::stdout().flush().unwrap()
}
