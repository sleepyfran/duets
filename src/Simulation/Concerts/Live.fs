module Simulation.Concerts.Live

open Aether
open Common
open Entities

let private multiplierFromEnergy energy =
    match energy with
    | Energetic -> 15.0
    | PerformEnergy.Normal -> 8.0
    | Limited -> 2.0

let private playSong' ongoingConcert (FinishedSong song, _) energy =
    multiplierFromEnergy energy
    |> (*) (float song.Practice / 100.0)
    |> Math.roundToNearest
    |> (*) 1<quality>
    |> fun points ->
        Optic.set Lenses.Concerts.Ongoing.points_ points ongoingConcert

let private removePoints ongoingConcert points =
    Optic.map
        Lenses.Concerts.Ongoing.points_
        (fun concertPoints -> concertPoints - points |> max 0<quality>)
        ongoingConcert

/// Returns a modified ongoing concert with a PlaySong event added and the
/// amount of points gathered through this event.
let playSong ongoingConcert (FinishedSong song, quality) energy =
    let updatedOngoingConcert =
        if Concert.Ongoing.hasPlayedSong ongoingConcert song then
            removePoints ongoingConcert 50<quality>
        else
            playSong' ongoingConcert (FinishedSong song, quality) energy

    updatedOngoingConcert
    |> Optic.map
        Lenses.Concerts.Ongoing.events_
        (List.append [
            PlaySong(song, energy) |> CommonEvent
         ])
