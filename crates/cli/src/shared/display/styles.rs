use console::style;

type StyledString<'a> = console::StyledObject<&'a str>;

/// Default style for titles.
pub fn title(text: &str) -> StyledString {
    style(text).cyan().bold()
}

/// Default style for errors.
pub fn error(text: &str) -> StyledString {
    style(text).red().bold()
}

/// Default style for warning messages.
pub fn warning(text: &str) -> StyledString {
    style(text).yellow().bold()
}

/// Default style for info messages.
pub fn info(text: &str) -> StyledString {
    style(text).green().bold()
}
