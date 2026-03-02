module Duets.UI.Text.World.Places

open Avalonia.FuncUI.Types
open Duets.Entities
open Duets.UI.Components
open Duets.UI.Text.Prompts

/// Generates a streaming LLM description of the given place using the current
/// game state and available interactions for context.
let text state (interactions: InteractionWithMetadata list) : IView =
    createRoomDescriptionPrompt state interactions |> StreamingText.create
