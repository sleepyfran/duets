module rec Duets.Cli.Text.Focused

open Duets.Entities
open Duets.Entities.SituationTypes

let actionPrompt date dayMoment attributes activity =
    $"""{Generic.infoBar date dayMoment attributes}
{activityEmoji activity} {activityName activity}"""
    |> Styles.prompt

let activityName activity =
    match activity with
    | FocusSituation.UsingComputer(item, computer) ->
        match computer.ComputerState with
        | Booting -> "" (* Won't be shown, it'll be switched right after. *)
        | AppSwitcher -> $"Using {item.Name}"
        | AppRunning(WordProcessor) -> "Using Word"
    |> Styles.faded

let activityEmoji activity =
    match activity with
    | FocusSituation.UsingComputer(_, computer) ->
        match computer.ComputerState with
        | Booting -> ""
        | AppSwitcher -> $"{Emoji.computer} "
        | AppRunning(WordProcessor) -> Emoji.writing
