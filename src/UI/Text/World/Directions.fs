module UI.Text.World.Directions

open Entities

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
