module Renderer

open Action
open Spectre.Console

let renderMessage (message: string) =
  AnsiConsole.Write message |> ignore
  None

let renderPrompt prompt =
  match prompt.Content with
  | TextPrompt ->
      Some(AnsiConsole.Ask<string> prompt.Title)
  | ChoicePrompt choices ->
      Some ((Array.head choices).Id)

let render action =
  match action with
  | Message message -> renderMessage message
  | Prompt prompt -> renderPrompt prompt
  | NoOp -> None
