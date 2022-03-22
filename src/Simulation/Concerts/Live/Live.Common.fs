module Simulation.Concerts.Live.Common

open Aether
open Common
open Entities

/// Adds the given amount of points to the ongoing concert, keeping the value
/// between 0 and 100.
let addPoints ongoingConcert points =
    Optic.map
        Lenses.Concerts.Ongoing.points_
        (fun currentPoints ->
            currentPoints + (points * 1<quality>)
            |> Math.clamp 0<quality> 100<quality>)
        ongoingConcert
