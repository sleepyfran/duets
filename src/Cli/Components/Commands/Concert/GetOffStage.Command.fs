namespace Cli.Components.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Entities
open Simulation.Concerts.Live.Encore

[<RequireQualifiedAccess>]
module GetOffStageCommand =
    /// Command which moves the person from the stage into the backstage. This
    /// might end the concert if people is not really interested in staying for
    /// the encore.
    let rec create ongoingConcert =
        Concert.createCommand
            "get off stage"
            CommandGetOffStageDescription
            getOffStage
            (fun canPerformEncore _ ->
                lineBreak ()

                if canPerformEncore then
                    ConcertText ConcertGetOffStageEncorePossible
                    |> I18n.translate
                    |> showMessage

                    lineBreak ()
                else
                    ConcertText ConcertGetOffStageNoEncorePossible
                    |> I18n.translate
                    |> showMessage

                    lineBreak ())
            ongoingConcert
