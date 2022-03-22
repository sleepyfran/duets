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

let private calculatePointIncrease (FinishedSong song, _) energy =
    multiplierFromEnergy energy
    |> (*) (float song.Practice / 100.0)
    |> Math.ceilToNearest

type PlaySongResult =
    | RepeatedSong
    | LowPracticePerformance
    | NormalPracticePerformance
    | HighPracticePerformance

type PlaySongResponse =
    { OngoingConcert: OngoingConcert
      Points: int
      Result: PlaySongResult }

/// Returns a modified ongoing concert with a PlaySong event added and the
/// amount of points gathered through this event.
let playSong ongoingConcert (FinishedSong song, quality) energy =
    // TODO: Apply health/energy effects once we support those.
    let alreadyPlayedSong = Concert.Ongoing.hasPlayedSong ongoingConcert song

    let (ongoingConcertWithPoints, points, result) =
        if alreadyPlayedSong then
            (addPoints ongoingConcert -50, -50, RepeatedSong)
        else
            let pointIncrease =
                calculatePointIncrease (FinishedSong song, quality) energy

            let result =
                match song.Practice with
                | p when p < 40<practice> -> LowPracticePerformance
                | p when p < 80<practice> -> NormalPracticePerformance
                | _ -> HighPracticePerformance

            (addPoints ongoingConcert pointIncrease, pointIncrease, result)

    let ongoingConcertWithEvent =
        ongoingConcertWithPoints
        |> Optic.map
            Lenses.Concerts.Ongoing.events_
            (List.append [ PlaySong(song, energy) |> CommonEvent ])


    { OngoingConcert = ongoingConcertWithEvent
      Points = points
      Result = result }
