namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module Concert =
    /// Shows a progress bar with the default speech progress text.
    let showSpeechProgress () =
        showProgressBarSync [ Concert.speechProgress ] 2<second>

    type private ActionFun<'a> =
        State -> OngoingConcert -> OngoingConcertEventResponse<'a>

    type private AfterActionFun<'a> = 'a -> int -> unit

    /// Generic function that creates a command with the given information
    /// and handling the usual flow of a concert action: execute action,
    /// show progress/result based on points, update ongoing concert inside
    /// of situation and execute any effect returned by the action before
    /// showing the world scene again.
    let createCommand
        name
        description
        (action: ActionFun<'a>)
        (afterActionFn: AfterActionFun<'a>)
        ongoingConcert
        =
        { Name = name
          Description = description
          Handler =
            (fun _ ->
                let response = action (State.get ()) ongoingConcert

                afterActionFn response.Result response.Points

                response.Effects |> Duets.Cli.Effect.applyMultiple

                Scene.World) }

    /// Generic function that creates a command with the given information
    /// but handling the case of a solo since most of the code is shared between
    /// all types.
    let createSoloCommand
        name
        description
        progressText
        (action: ActionFun<ConcertEventResult>)
        ongoingConcert
        =
        createCommand
            name
            description
            action
            (fun result points ->
                showProgressBarAsync progressText 2<second>

                match result with
                | LowPerformance reasons ->
                    Concert.soloResultLowPerformance reasons points
                | AveragePerformance reasons ->
                    Concert.soloResultAveragePerformance reasons points
                | GoodPerformance _
                | GreatPerformance -> Concert.soloResultGreatPerformance points
                | _ -> Concert.tooManySolos points
                |> showMessage)
            ongoingConcert
