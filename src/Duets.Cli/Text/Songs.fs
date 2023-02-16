module Duets.Cli.Text.Songs

open Duets.Entities

/// Formats a given length as minutes:seconds.
let length (l: Length) =
    $"{l.Minutes}:{l.Seconds}"
