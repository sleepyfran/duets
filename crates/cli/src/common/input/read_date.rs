use chrono::{NaiveDate, ParseError};

use super::common;

/// Reads a line and attempts to parse a NaiveDate from it. If we're unable
/// to do so, returns an error.
pub fn read_date() -> Result<NaiveDate, ParseError> {
    let input = common::read_from_stdin_trimmed();

    NaiveDate::parse_from_str(&input, "%d-%m-%Y")
}
