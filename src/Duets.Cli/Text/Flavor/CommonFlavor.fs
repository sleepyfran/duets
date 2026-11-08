namespace Duets.Cli.Text.Flavor

open Duets.Common

/// Helpers to procedurally build flavor text out of pools of fragments.
[<RequireQualifiedAccess>]
module Common =
    /// Picks one random fragment out of all the given pools.
    let internal pickFrom (pools: string list list) =
        pools |> List.concat |> List.sample

    /// Joins the given sentences into a single paragraph.
    let internal paragraph (sentences: string list) =
        sentences |> List.filter (String.isEmpty >> not) |> String.join " "
