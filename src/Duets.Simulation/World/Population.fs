module Duets.Simulation.World.Population

open Duets.Common
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
        (min 20 concertSpace.Capacity) (* TODO: Base this on whether there's any concert or not and the assistance to the concert. *)
    | Gym -> 1, 5
    | Home -> 0, 0 (* Homes are private *)
    | Hotel _ -> 1, 5
    | Hospital -> 1, 2
    | RehearsalSpace _ -> 1, 10
    | Restaurant -> 1, 10 (* TODO: Base this on the type of restaurant. *)
    | Studio _ ->
        (* Studios will only be populated by the producer and the band. *)
        0, 0

/// Returns a random known NPC in the given city, if any.
let private allKnownNpcs cityId state =
    Queries.Relationship.fromCity cityId state
    |> List.map (fun relationship ->
        Queries.Characters.find state relationship.Character)

/// Generates an effect that puts a random number of people in the given place,
/// keeping in mind the place's population range, which depends on the place type
/// and whether it is private or not (like a home or a studio).
let generateForPlace cityId place state =
    let numberOfPeople = placePopulationRange place ||> RandomGen.genBetween
    let knownNpcs = allKnownNpcs cityId state

    [ 1..numberOfPeople ]
    |> List.fold
        (fun (npcsInRoom, knownNpcs) _ ->
            let shouldPullFromKnownNpcs =
                knownNpcs |> List.isNotEmpty
                && RandomGen.chance
                    Config.Population.chanceOfKnownPeopleAtPlace

            if shouldPullFromKnownNpcs then
                (*
                Pull a new character from the known NPCs and remove them from
                the list of known NPCs so that they don't appear again.
                *)
                let randomIndex = RandomGen.sampleIndex knownNpcs
                let randomNpc = knownNpcs |> List.item randomIndex
                let knownNpcs = knownNpcs |> List.removeAt randomIndex

                (randomNpc :: npcsInRoom, knownNpcs)
            else
                (* Create a new NPC and keep the known list as is. *)
                let randomNpc = Character.Npc.generateRandom state
                (randomNpc :: npcsInRoom, knownNpcs))
        ([], knownNpcs)
    |> fst
    |> WorldPeopleInCurrentRoomChanged
