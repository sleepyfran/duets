module UI.Text.World.Directions

open Entities
open Simulation

let directionName direction =
    match direction with
    | North -> "north"
    | NorthEast -> "north east"
    | East -> "east"
    | SouthEast -> "south east"
    | South -> "south"
    | SouthWest -> "south west"
    | West -> "west"
    | NorthWest -> "north west"

let destinationName state (coords: NodeCoordinates) =
    let nodeContent =
        Queries.World.Common.coordinates state coords

    match nodeContent.Content with
    | ResolvedPlaceCoordinates room ->
        match room.Room with
        | RoomType.Backstage -> "Backstage"
        | RoomType.Bar _ -> "Bar"
        | RoomType.Bedroom -> "Bedroom"
        | RoomType.Kitchen -> "Kitchen"
        | RoomType.LivingRoom -> "Living Room"
        | RoomType.Lobby -> "Lobby"
        | RoomType.MasteringRoom -> "Mastering Room"
        | RoomType.RecordingRoom -> "Recording Room"
        | RoomType.RehearsalRoom -> "Rehearsal Room"
        | RoomType.Stage -> "Stage"
    | ResolvedOutsideCoordinates outsideNode -> outsideNode.Node.Name
