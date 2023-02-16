module Duets.Common.Func

/// Transforms an F# function into a System.Func.
let toFunc<'a, 'b> f = System.Func<'a, 'b>(f)
