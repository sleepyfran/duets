pub mod prompts;
pub mod styles;

use std::io::Write;

use crate::screens;
use crate::shared::action::{CliAction, PromptText};
use crate::shared::context::Context;
use crate::shared::emoji;

/// Shows the specified screen. Since screens (at least as of right now) have
/// no other thing that just an identifier and an associated action, this simply
/// calls the show function in the prompts module to handle the inner
/// action of the screen.
pub fn show(game_screen: screens::GameScreen, context: &Context) -> CliAction {
    let screen = screens::create(game_screen, context);
    prompts::show(screen.action, context)
}

/// Prints a new line to separate content.
pub fn show_line_break() {
    println!();
}

/// Shows a default line of text (no title, no error) with a new line.
pub fn show_text_with_new_line(text: &str) {
    println!("{}", text)
}

/// Shows a default line of text without appending a new line.
pub fn show_text(text: &str) {
    print_and_flush(text)
}

/// Prints some info to the screen.
pub fn show_info(text: &str) {
    print_and_flush(&format!("{} {}", emoji::for_info(), styles::info(text)))
}

/// Prints a warning to the screen.
pub fn show_warning(text: &str) {
    print_and_flush(&format!(
        "{} {}",
        emoji::for_warning(),
        styles::warning(text)
    ))
}

/// Prints an error to the screen.
pub fn show_error(text: &str) {
    print_and_flush(&format!("{} {}", emoji::for_error(), styles::error(text)))
}

/// Prints the start text of a prompt.
pub fn show_prompt_text_with_new_line(text: PromptText) {
    match text {
        PromptText::WithEmoji(text) => {
            println!("{} {}", emoji::for_speech_bubble(), styles::title(&text))
        }
        PromptText::WithoutEmoji(text) => println!("{}", styles::title(&text)),
    }
}

// Prints the start text of a prompt without a new line.
pub fn show_prompt_text(text: PromptText) {
    match text {
        PromptText::WithEmoji(text) => print_and_flush(&format!(
            "{} {}",
            emoji::for_speech_bubble(),
            styles::title(&text)
        )),
        PromptText::WithoutEmoji(text) => print_and_flush(&format!("{}", styles::title(&text))),
    }
}

// Prints the start text of a prompt without a new line and no emoji.
pub fn show_prompt_text_no_emoji(text: &str) {
    print_and_flush(&format!("{}", styles::title(text)))
}

/// Clears the screen completely.
pub fn clear() {
    print_and_flush(&format!("{}[2J", 27 as char))
}

fn print_and_flush(text: &str) {
    print!("{}", text);
    std::io::stdout().flush().unwrap()
}
