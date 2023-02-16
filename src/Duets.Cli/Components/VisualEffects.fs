[<AutoOpen>]
module Duets.Cli.Components.VisualEffects

[<Measure>]
type millisecond

/// Stops the execution of the CLI for the given amount of seconds. Used to
/// provide a little dramatic pause.
let wait (amount: int<millisecond>) =
    System.Threading.Thread.Sleep(amount / 1<millisecond>)
