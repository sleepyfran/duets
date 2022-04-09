[<AutoOpen>]
module Simulation.Concerts.Live.PlaySong

open Common
open Entities

let private multiplierFromEnergy energy =
    match energy with
    | Energetic -> 15.0
    | PerformEnergy.Normal -> 8.0
    | Limited -> 2.0

let private calculatePointIncrease (FinishedSong song, _) energy =
    multiplierFromEnergy energy
    |> (*) (float song.Practice / 100.0)
    |> Math.ceilToNearest

type PlaySongResult =
    | RepeatedSong
    | LowPracticePerformance
    | NormalPracticePerformance
    | HighPracticePerformance

/// Plays the given song in the concert with the specified energy. The result
/// depends on whether the song was already played or not and the energy.
let playSong ongoingConcert (FinishedSong song, quality) energy =
    // TODO: Apply health/energy effects once we support those.
    let event = PlaySong(song, energy) |> CommonEvent

    let alreadyPlayedSong =
        Concert.Ongoing.hasPlayedSong ongoingConcert song

    if alreadyPlayedSong then
        Response.forEvent' ongoingConcert event -50 RepeatedSong
    else
        let pointIncrease =
            calculatePointIncrease (FinishedSong song, quality) energy

        let result =
            match song.Practice with
            | p when p < 40<practice> -> LowPracticePerformance
            | p when p < 80<practice> -> NormalPracticePerformance
            | _ -> HighPracticePerformance

        Response.forEvent' ongoingConcert event pointIncrease result
