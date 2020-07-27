use console::style;

type StyledString<'a> = console::StyledObject<&'a String>;

/// Default style for titles.
pub fn title(text: &String) -> StyledString {
    style(text).cyan().bold()
}

/// Default style for errors.
pub fn error(text: &String) -> StyledString {
    style(text).red().bold()
}

/// Default style for info messages.
pub fn warning(text: &String) -> StyledString {
    style(text).yellow().bold()
}
