use chrono::{NaiveDate, ParseError};

use super::common;
use crate::shared::action::DateFormat;

/// Reads a line and attempts to parse a NaiveDate from it. If we're unable
/// to do so, returns an error.
pub fn read_date(format: &DateFormat) -> Result<NaiveDate, ParseError> {
    let input = pre_format_input(common::read_from_stdin_trimmed(), format);

    NaiveDate::parse_from_str(&input, "%d-%m-%Y")
}

pub fn pre_format_input(input: String, format: &DateFormat) -> String {
    match format {
        DateFormat::Year => format!("01-01-{}", input),
        DateFormat::Full => input,
    }
}
