module Cli.Scenes.InteractiveSpaces.ConcertSpace.Components

open Cli.Components
open Cli.Text
open FSharp.Data.UnitSystems.SI.UnitNames

/// Shows a progress bar with the default speech progress text.
let showSpeechProgress () =
    showProgressBarSync
        [ ConcertText ConcertSpeechProgress
          |> I18n.translate ]
        2<second>
