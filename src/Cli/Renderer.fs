module Cli.View.Renderer

open Cli.View.Actions
open Spectre.Console
open Text

/// Writes a message into the buffer.
let renderMessage message =
  AnsiConsole.MarkupLine(toString message)

/// Renders a figlet (see: http://www.figlet.org).
let renderFiglet text =
  let figlet = FigletText(toString text).Centered()
  figlet.Color <- Color.Blue
  AnsiConsole.Render(figlet)

let renderChoicePrompt title (choices: Choice list) =
  let mutable selectionPrompt = SelectionPrompt<Choice>()
  selectionPrompt.Title <- toString title
  selectionPrompt <- selectionPrompt.AddChoices(choices)
  selectionPrompt <- selectionPrompt.UseConverter(fun c -> toString c.Text)
  AnsiConsole.Prompt(selectionPrompt).Id

let renderMandatoryPrompt title (content: MandatoryChoiceHandler) =
  renderChoicePrompt title content.Choices

let renderOptionalPrompt title (content: OptionalChoiceHandler) =
  [ { Id = "back"; Text = content.BackText } ]
  |> List.append content.Choices
  |> renderChoicePrompt title

/// Renders the specified prompt and asks the user for a response depending
/// on the specified type of prompt. Returns a string which either represents
/// the raw user input (in case of a TextPrompt) or the ID of the choice that
/// the user chose (in case of a ChoicePrompt).
let renderPrompt prompt =
  match prompt.Content with
  | ChoicePrompt content ->
      match content with
      | MandatoryChoiceHandler content ->
          renderMandatoryPrompt prompt.Title content
      | OptionalChoiceHandler content ->
          renderOptionalPrompt prompt.Title content
  | ConfirmationPrompt _ ->
      AnsiConsole.Confirm(toString prompt.Title)
      |> string
  | NumberPrompt _ ->
      AnsiConsole.Ask<int>(toString prompt.Title)
      |> string
  | TextPrompt _ -> AnsiConsole.Ask<string>(toString prompt.Title)

let private sleepForProgressBar content =
  async { do! Async.Sleep(content.StepDuration * 1000 / 4 |> int) }
  |> Async.RunSynchronously

let private renderProgressBarSync content =
  AnsiConsole
    .Progress()
    .Start(fun ctx ->
      content.StepNames
      |> List.iter
           (fun stepName ->
             let task = ctx.AddTask(toString stepName)

             for i in 0 .. 4 do
               task.Increment 25.0
               sleepForProgressBar content))

let private renderProgressBarAsync content =
  AnsiConsole
    .Progress()
    .Start(fun ctx ->
      let tasks =
        content.StepNames
        |> List.map (fun name -> ctx.AddTask(toString name))
        |> ResizeArray

      let random = System.Random()

      for i in 0 .. 4 * tasks.Count - 1 do
        let randomIndex = random.Next(0, tasks.Count)
        let taskToIncrement = tasks.[randomIndex]

        taskToIncrement.Increment 25.0

        if taskToIncrement.IsFinished then
          tasks.RemoveAt(randomIndex)

        sleepForProgressBar content)

/// Renders a progressbar using Spectre's progress.
let renderProgressBar content =
  if content.Async then
    renderProgressBarAsync content
  else
    renderProgressBarSync content

/// Clears the terminal console.
let clear () = System.Console.Clear()

/// Writes an empty line to the console.
let separator () =
  let rule = Rule().Centered()
  rule.Style <- Style.Parse("blue dim")
  AnsiConsole.Render(rule)

/// Waits until the user inputs something.
let waitForInput explanationText =
  System.Console.WriteLine()
  renderMessage explanationText
  System.Console.Read() |> ignore
