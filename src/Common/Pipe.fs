module Common.Pipe

/// Executes a function that takes the given element and returns the element
/// to continue the pipe.
let tap fn element =
    fn element
    element
