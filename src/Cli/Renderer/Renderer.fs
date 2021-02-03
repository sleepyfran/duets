module Renderer

open Action
open Spectre.Console

/// Writes a message into the buffer.
let renderMessage (message: string) =
  AnsiConsole.Write message |> ignore
  None

/// Renders the specified prompt and asks the user for a response depending
/// on the specified type of prompt. Returns a string which either represents
/// the raw user input (in case of a TextPrompt) or the ID of the choice that
/// the user chose (in case of a ChoicePrompt).
let renderPrompt prompt =
  match prompt.Content with
  | TextPrompt ->
      Some(AnsiConsole.Ask<string> prompt.Title)
  | ChoicePrompt choices ->
      Some ((Array.head choices).Id)

/// Renders an action and returns an option that encapsulates the user response
/// to it.
let render action =
  match action with
  | Message message -> renderMessage message
  | Prompt prompt -> renderPrompt prompt
  | NoOp -> None
