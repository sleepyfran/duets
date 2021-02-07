module Renderer.Render

open Orchestrator
open View.Actions
open Spectre.Console
open Renderer.Text

type CliRenderer() =
  interface IRenderer with
    /// Writes a message into the buffer.
    member this.RenderMessage message =
      AnsiConsole.MarkupLine(toString message) |> ignore

    /// Renders the specified prompt and asks the user for a response depending
    /// on the specified type of prompt. Returns a string which either represents
    /// the raw user input (in case of a TextPrompt) or the ID of the choice that
    /// the user chose (in case of a ChoicePrompt).
    member this.RenderPrompt prompt =
      match prompt.Content with
      | ChoicePrompt (choices, handler) ->
          let mutable selectionPrompt = new SelectionPrompt<Choice>()
          selectionPrompt.Title <- toString prompt.Title
          selectionPrompt <- selectionPrompt.AddChoices(choices)

          selectionPrompt <-
            selectionPrompt.UseConverter(fun c -> toString c.Text)

          AnsiConsole.Prompt(selectionPrompt).Id
      | ConfirmationPrompt handler ->
          AnsiConsole.Confirm(toString prompt.Title)
          |> string
      | NumberPrompt handler ->
          AnsiConsole.Ask<int>(toString prompt.Title)
          |> string
      | TextPrompt handler -> AnsiConsole.Ask<string>(toString prompt.Title)

    /// Clears the terminal console.
    member this.Clear() = System.Console.Clear()
