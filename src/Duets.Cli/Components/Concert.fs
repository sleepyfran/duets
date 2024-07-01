[<AutoOpen>]
module Duets.Cli.Components.Concert

open Duets.Cli.Text
open Duets.Entities
open FSharp.Data.UnitSystems.SI.UnitNames

/// Shows a progress bar with the default speech progress text.
let showSpeechProgress () =
    showProgressBarSync [ Concert.speechProgress ] 2<second>

/// Shows a progress bar with the default song progress text.
let private showPlaySongProgress song =
    showProgressBarSync
        [ Concert.playSongProgressPlaying song ]
        (song.Length.Minutes / 1<minute / second>)

/// Shows the sequence of messages for playing a song.
let showPlaySong finishedSong result points energy =
    let song = Song.fromFinished finishedSong
    Concert.playSongBeforeResult song result energy |> showMessage

    showPlaySongProgress song

    Concert.playSongAfterResult result points energy |> showMessage
