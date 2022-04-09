[<AutoOpen>]
module Simulation.Concerts.Live.Common

open Aether
open Common
open Entities

type OngoingConcertEventResponse<'r> =
    { Effects: Effect list
      OngoingConcert: OngoingConcert
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

module internal Response =
    /// Returns an empty response that contains only the given ongoing concert
    /// with no modifications.
    let empty ongoingConcert =
        { Effects = []
          OngoingConcert = ongoingConcert
          Points = 0
          Result = () }

    /// Returns an empty response that contains only the given ongoing concert
    /// with no modifications and a result.
    let empty' ongoingConcert result =
        { Effects = []
          OngoingConcert = ongoingConcert
          Points = 0
          Result = result }

    /// Adds the given points to the given ongoing concert making sure that the
    /// value does not go above 100 or below 0, adds the given event as well and
    /// then creates a response with all the given parameters and the updated state.
    let forEvent ongoingConcert event points =
        addPoints ongoingConcert points
        |> addEvent event
        |> fun updatedOngoingConcert ->
            { Effects = []
              OngoingConcert = updatedOngoingConcert
              Points = points
              Result = () }

    /// Adds the given points to the given ongoing concert making sure that the
    /// value does not go above 100 or below 0, adds the given event as well and
    /// then creates a response with all the given parameters and the updated state.
    let forEvent' ongoingConcert event points result =
        addPoints ongoingConcert points
        |> addEvent event
        |> fun updatedOngoingConcert ->
            { Effects = []
              OngoingConcert = updatedOngoingConcert
              Points = points
              Result = result }

    /// Adds the given set of effects to the response.
    let addEffects effects response = { response with Effects = effects }
