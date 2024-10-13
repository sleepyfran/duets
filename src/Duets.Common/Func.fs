module Duets.Common.Func

/// Transforms an F# function into a System.Func.
let toFunc<'a, 'b> f = System.Func<'a, 'b>(f)

/// Wraps a value in a function that ignores its input and returns the value.
let toConst<'a> (value: 'a) _ = value
