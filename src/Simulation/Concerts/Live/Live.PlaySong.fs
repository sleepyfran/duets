[<AutoOpen>]
module Simulation.Concerts.Live.PlaySong


open Aether
open Common
open Entities

let private multiplierFromEnergy energy =
    match energy with
    | Energetic -> 15.0
    | PerformEnergy.Normal -> 8.0
    | Limited -> 2.0

let private calculatePointIncrease
    ongoingConcert
    (FinishedSong song, _)
    energy
    =
    multiplierFromEnergy energy
    |> (*) (float song.Practice / 100.0)
    |> Math.ceilToNearest
    |> addPoints ongoingConcert

/// Returns a modified ongoing concert with a PlaySong event added and the
/// amount of points gathered through this event.
let playSong ongoingConcert (FinishedSong song, quality) energy =
    // TODO: Apply health/energy effects once we support those.
    let updatedOngoingConcert =
        if Concert.Ongoing.hasPlayedSong ongoingConcert song then
            removePoints ongoingConcert 50
        else
            calculatePointIncrease
                ongoingConcert
                (FinishedSong song, quality)
                energy

    updatedOngoingConcert
    |> Optic.map
        Lenses.Concerts.Ongoing.events_
        (List.append [
            PlaySong(song, energy) |> CommonEvent
         ])
