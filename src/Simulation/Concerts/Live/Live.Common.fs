[<AutoOpen>]
module Simulation.Concerts.Live.Common

open Aether
open Common
open Entities

type OngoingConcertEventResponse<'r> =
    { OngoingConcert: OngoingConcert
      Points: int
      Result: 'r }

let private addPoints ongoingConcert points =
    Optic.map
        Lenses.Concerts.Ongoing.points_
        (fun currentPoints ->
            currentPoints + (points * 1<quality>)
            |> Math.clamp 0<quality> 100<quality>)
        ongoingConcert

let private addEvent event =
    Optic.map Lenses.Concerts.Ongoing.events_ (List.append [ event ])

/// Adds the given points to the given ongoing concert making sure that the
/// value does not go above 100 or below 0, adds the given event as well and
/// then creates a response with all the given parameters and the updated state.
let internal response ongoingConcert event points result =
    addPoints ongoingConcert points
    |> addEvent event
    |> fun updatedOngoingConcert ->
        { OngoingConcert = updatedOngoingConcert
          Points = points
          Result = result }
