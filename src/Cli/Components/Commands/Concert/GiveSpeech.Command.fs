namespace Cli.Components.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GiveSpeechCommand =
    /// Command which simulates giving a speech during a concert.
    let rec create ongoingConcert =
        Concert.createCommand
            "give speech"
            Command.giveSpeechDescription
            giveSpeech
            (fun result points ->
                Concert.showSpeechProgress ()

                match result with
                | LowPerformance _ -> Concert.speechGivenLowSkill points
                | AveragePerformance _ -> Concert.speechGivenMediumSkill points
                | GoodPerformance _
                | GreatPerformance _ -> Concert.speechGivenHighSkill points
                | _ -> Concert.tooManySpeeches
                |> showMessage)
            ongoingConcert
