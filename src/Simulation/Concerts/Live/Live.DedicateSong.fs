[<AutoOpen>]
module Simulation.Concerts.Live.DedicateSong

open Entities

type DedicateSongResult =
    | Dedicated of PlaySongResult
    | TooManyDedications

/// Allows for two dedications which play the given song with the given energy
/// and applying a plus of 10 points on top of the actual result of playing
/// the song.
let dedicateSong ongoingConcert song energy =
    let event = CommonEvent DedicateSong

    let dedicationsGiven =
        Concert.Ongoing.timesDoneEvent ongoingConcert event

    match dedicationsGiven with
    | times when times < 2 ->
        playSong ongoingConcert song energy
        |> Response.addEvent event
        |> Response.addPoints 10
        |> Response.mapResult Dedicated
    | _ -> Response.empty' ongoingConcert TooManyDedications
