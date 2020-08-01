use ::std::io::stdin;

/// Reads a line from the stding and returns it.
pub fn read_from_stdin() -> String {
    let mut input = String::new();
    stdin()
        .read_line(&mut input)
        .expect("Attempted to read input but was not available.");

    input
}

/// Reads a trimmed line and returns it.
pub fn read_from_stdin_trimmed() -> String {
    let input = read_from_stdin();
    input.trim().to_string()
}
