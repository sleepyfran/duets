module Cli.View.Renderer

open Cli.View.Actions
open Cli.View.TextConstants
open Common
open Entities
open Spectre.Console
open Cli.Localization.English

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

let renderConfirmationPrompt title = AnsiConsole.Confirm(toString title)

let renderNumberPrompt title = AnsiConsole.Ask<int>(toString title)

let renderTextPrompt title = AnsiConsole.Ask<string>(toString title)

let renderMandatoryPrompt title (content: MandatoryChoiceHandler) =
    renderChoicePrompt title content.Choices

let renderOptionalPrompt title (content: OptionalChoiceHandler) =
    [ { Id = "back"; Text = content.BackText } ]
    |> List.append content.Choices
    |> renderChoicePrompt title

let renderMultiChoicePrompt title (content: MultiChoiceHandler) =
    let mutable multiSelectionPrompt = MultiSelectionPrompt<Choice>()
    multiSelectionPrompt.Title <- toString title

    multiSelectionPrompt.MoreChoicesText <-
        toString
        <| TextConstant CommonMultiChoiceMoreChoices

    multiSelectionPrompt.InstructionsText <-
        toString
        <| TextConstant CommonMultiChoiceInstructions

    multiSelectionPrompt.Required <- true
    multiSelectionPrompt.PageSize <- 10
    multiSelectionPrompt <- multiSelectionPrompt.AddChoices(content.Choices)

    multiSelectionPrompt <-
        multiSelectionPrompt.UseConverter(fun c -> toString c.Text)

    AnsiConsole.Prompt(multiSelectionPrompt)
    |> List.ofSeq
    |> List.map (fun c -> c.Id)

let renderLengthPrompt title =
    let mutable lengthPrompt = TextPrompt<string>(toString title)

    let validate (length: string) =
        match Time.Length.parse length with
        | Ok _ -> ValidationResult.Success()
        | Error _ ->
            ValidationResult.Error(toString <| TextConstant CommonInvalidLength)

    lengthPrompt.Validator <- Func.toFunc validate

    AnsiConsole.Prompt(lengthPrompt)

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

/// Writes a line break to the console.
let renderLineBreak () = AnsiConsole.WriteLine()

/// Renders the current version of the game.
let renderGameInfo version =
    let gameInfo = $"v{version}"
    let styledGameInfo = $"[bold blue dim]{gameInfo}[/]"

    System.Console.SetCursorPosition(
        (System.Console.WindowWidth - gameInfo.Length) / 2,
        System.Console.CursorTop
    )

    AnsiConsole.MarkupLine(styledGameInfo)
