use super::common;

/// Reads a line and attempts to parse a number from it. If we're unable
/// to do so, returns an error.
pub fn read_number() -> Result<i32, std::num::ParseIntError> {
    let input = common::read_from_stdin();
    input.trim().parse::<i32>()
}
