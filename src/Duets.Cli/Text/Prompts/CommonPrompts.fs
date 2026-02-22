namespace Duets.Cli.Text.Prompts

open Duets.Common
open Duets.Entities

[<RequireQualifiedAccess>]
module Common =
    /// Creates a prompt that improves the response quality of the language model.
    /// Currently tuned for Gemma 3, which requires explicit turn markers to get
    /// anything useful out of it.
    let internal createPrompt prompt =
        $"""
    <start_of_turn>user
    {prompt}
    <end_of_turn>
    <start_of_turn>model
    """

    let internal itemNameForPrompt item =
        let mainProperty = item.Properties |> List.head

        match mainProperty with
        | Key(MovieTicket _) -> "movie ticket"
        | Key(EntranceCard _) -> "entrance card"
        | Rideable(RideableItem.Car _) -> $"{item.Brand} {item.Name}"
        | _ -> item.Name |> String.lowercase
