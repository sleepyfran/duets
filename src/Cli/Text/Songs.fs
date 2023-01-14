module Cli.Text.Songs

open Entities

/// Formats a given length as minutes:seconds.
let length (l: Length) =
    $"{l.Minutes}:{l.Seconds}"
