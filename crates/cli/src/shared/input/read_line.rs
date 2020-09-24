use super::common;

/// Reads a single line from the user trimming the start and the beginning of it.
pub fn read_line_trimmed() -> String {
    common::read_from_stdin_trimmed()
}
