module Cli.Components.Common

open Cli.Text
open Entities

/// Transforms an `EntranceError` into the correct text to show.
let showEntranceError error =
    match error with
    | EntranceError.CannotEnterBackstageOutsideConcert ->
        WorldText WorldConcertSpaceKickedOutOfBackstage
    | EntranceError.CannotEnterStageOutsideConcert ->
        WorldText WorldConcertSpaceKickedOutOfStage
    |> I18n.translate
    |> showMessage
