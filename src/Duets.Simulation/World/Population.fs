module Duets.Simulation.World.Population

open Duets.Entities
open Duets.Simulation

/// Returns a range of population values that a place can have, meaning
/// that the place's population will be a random number between the two
/// values.
let private placePopulationRange (place: Place) =
    match place.PlaceType with
    | Airport -> 0, 10
    | Bar -> 1, 10
    | Cafe -> 1, 5
    | Casino -> 1, 10
    | ConcertSpace concertSpace ->
        1,
        (max 20 concertSpace.Capacity) (* TODO: Base this on whether there's any concert or not and the assistance to the concert. *)
    | Gym -> 1, 5
    | Home -> 0, 0 (* Homes are private *)
    | Hotel _ -> 1, 5
    | Hospital -> 1, 2
    | RehearsalSpace _ -> 1, 10
    | Restaurant -> 1, 10 (* TODO: Base this on the type of restaurant. *)
    | Studio _ ->
        (* Studios will only be populated by the producer and the band. *)
        0, 0

/// Generates an effect that puts a random number of people in the given place,
/// keeping in mind the place's population range, which depends on the place type
/// and whether it is private or not (like a home or a studio).
let generateForPlace place state =
    let numberOfPeople = placePopulationRange place ||> RandomGen.genBetween

    [ 1..numberOfPeople ]
    |> List.map (fun _ -> Character.Npc.generateRandom state)
    |> WorldPeopleInCurrentRoomChanged
