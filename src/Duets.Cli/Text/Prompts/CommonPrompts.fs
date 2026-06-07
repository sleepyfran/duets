namespace Duets.Cli.Text.Prompts

open Duets.Common
open Duets.Entities

[<RequireQualifiedAccess>]
module Common =
    /// Creates a prompt that improves the response quality of the language model.
    /// Gemma 4 chat formatting is applied from the model metadata by LLamaSharp.
    let internal createPrompt prompt = prompt |> String.trim

    let internal itemNameForPrompt item =
        let mainProperty = item.Properties |> List.head

        match mainProperty with
        | Key(MovieTicket _) -> "movie ticket"
        | Key(EntranceCard _) -> "entrance card"
        | Rideable(RideableItem.Car _) -> $"{item.Brand} {item.Name}"
        | _ -> item.Name |> String.lowercase
