module Renderer.Render

open View.Actions
open Spectre.Console
open Renderer.Text

/// Writes a message into the buffer.
let renderMessage message =
  AnsiConsole.MarkupLine(toString message) |> ignore

/// Renders the specified prompt and asks the user for a response depending
/// on the specified type of prompt. Returns a string which either represents
/// the raw user input (in case of a TextPrompt) or the ID of the choice that
/// the user chose (in case of a ChoicePrompt).
let renderPrompt prompt =
  match prompt.Content with
  | TextPrompt handler -> AnsiConsole.Ask<string>(toString prompt.Title)
  | ChoicePrompt (choices, handler) ->
      let mutable selectionPrompt = new SelectionPrompt<Choice>()
      selectionPrompt.Title <- toString prompt.Title
      selectionPrompt <- selectionPrompt.AddChoices(choices)
      selectionPrompt <- selectionPrompt.UseConverter(fun c -> toString c.Text)
      AnsiConsole.Prompt(selectionPrompt).Id

/// Initializes the renderer by cleaning the terminal screen.
let init () = System.Console.Clear()
