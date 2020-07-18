use console::style;

type StyledString<'a> = console::StyledObject<&'a String>;

/// Default style for titles.
pub fn title(text: &String) -> StyledString {
    style(text).cyan().bold()
}
