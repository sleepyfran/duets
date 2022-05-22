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
            CommandGiveSpeechDescription
            giveSpeech
            (fun result points ->
                match result with
                | LowPerformance -> ConcertSpeechGivenLowSkill points
                | AveragePerformance -> ConcertSpeechGivenMediumSkill points
                | GoodPerformance
                | GreatPerformance -> ConcertSpeechGivenHighSkill points
                | _ -> ConcertTooManySpeeches
                |> ConcertText
                |> I18n.translate
                |> showMessage)
            ongoingConcert
