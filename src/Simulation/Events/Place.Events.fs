module Simulation.Events.Place

open Entities
open Simulation

let private runRoomEvents (coords: ResolvedRoomCoordinates) state =
    let placeId, _ = coords.Coordinates

    match coords.Room with
    | RoomType.Stage -> Concerts.Scheduler.startScheduledConcerts state placeId
    | _ -> []

/// Runs all the effects that are associated with the current place. This currently
/// just starts a scheduled concert when the character steps in the concert's
/// place, but it can be extended to do much more.
let internal run (_, coords) state =
    let resolvedCoords = Queries.World.Common.coordinates state coords

    match resolvedCoords.Content with
    | ResolvedPlaceCoordinates roomCoordinates ->
        runRoomEvents roomCoordinates state
    | _ -> []
