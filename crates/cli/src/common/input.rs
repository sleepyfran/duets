use ::std::io::stdin;

/// Reads a single line from the user.
pub fn read_line() -> String {
    return read_from_stdin();
}

/// Reads a line and attempts to parse a number from it. If we're unable
/// to do so, returns an error.
pub fn read_number() -> Result<i32, std::num::ParseIntError> {
    let input = read_from_stdin();
    return input.trim().parse::<i32>();
}

fn read_from_stdin() -> String {
    let mut input = String::new();
    stdin()
        .read_line(&mut input)
        .expect("Attempted to read input but was not available.");

    return input;
}
