pub mod prompts;
pub mod styles;

use crate::common::action::CliAction;
use crate::common::context::Context;
use crate::common::screen::Screen;

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
    print!("{}", text)
}

pub fn show_warning(text: &String) {
    print!("⚠️ {}", styles::warning(text))
}

/// Prints the start text of a prompt.
pub fn show_prompt_text_with_new_line(text: &String) {
    println!("💬 {}", styles::title(text))
}

// Prints the start text of a prompt without a new line.
pub fn show_prompt_text(text: &String) {
    print!("💬 {}", styles::title(text))
}

/// Prints an error to the screen.
pub fn show_error(text: &String) {
    println!("❌ {}", styles::error(text))
}
