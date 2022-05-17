module Cli.Components.Common

open Cli.Text
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames

/// Transforms an `EntranceError` into the correct text to show.
let showEntranceError error =
    match error with
    | EntranceError.CannotEnterBackstageOutsideConcert ->
        WorldText WorldConcertSpaceKickedOutOfBackstage
    | EntranceError.CannotEnterStageOutsideConcert ->
        WorldText WorldConcertSpaceKickedOutOfStage
    |> I18n.translate
    |> showMessage

/// Shows a progress bar with the default speech progress text.
let showSpeechProgress () =
    showProgressBarSync
        [ ConcertText ConcertSpeechProgress
          |> I18n.translate ]
        2<second>
