use crate::common::display::styles;

/// Prints the start text of a screen.
pub fn show_start_text_with_new_line(text: &String) {
    println!("{}", styles::title(text))
}

// Prints the start text of a screen without a new line.
pub fn show_start_text(text: &String) {
    print!("{}", styles::title(text))
}
