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
    | Bookstore -> 1, 5
    | Cafe -> 1, 5
    | CarDealer _ -> 1, 2
    | Casino -> 1, 10
    | ConcertSpace concertSpace ->
        1,
        (min 20 concertSpace.Capacity) (* TODO: Base this on whether there's any concert or not and the assistance to the concert. *)
    | Gym -> 1, 5
    | Home -> 0, 0 (* Homes are private *)
    | Hotel _ -> 1, 5
    | Hospital -> 1, 2
    | MetroStation -> 1, 20
    | MerchandiseWorkshop -> 0, 0
    | RadioStudio _ ->
        (* Radio studios will only be populated by the band. *)
        0, 0
    | RehearsalSpace _ ->
        (* Rehearsal spaces will only be populated by the band. *)
        0, 0
    | Restaurant -> 1, 10 (* TODO: Base this on the type of restaurant. *)
    | Studio _ ->
        (* Studios will only be populated by the producer and the band. *)
        0, 0
    | Street -> 1, 20 (* TODO: Base this on the current time of day. *)

/// Returns a random known NPC in the given city, if any.
let private allKnownNpcs cityId state =
    Queries.Relationship.fromCity cityId state
    |> List.map (fun relationship ->
        Queries.Characters.find state relationship.Character)

let private generateForPlaceWithSpecificNpcs place state =
    let bandMembers =
        Queries.Bands.currentBandMembersWithoutPlayableCharacter state
        |> List.map (_.CharacterId >> Queries.Characters.find state)

    match place.PlaceType with
    | RehearsalSpace _ -> bandMembers
    | Studio studio -> studio.Producer :: bandMembers
    | _ -> []

/// Generates an effect that puts a random number of people in the given place,
/// keeping in mind the place's population range, which depends on the place type
/// and whether it is private or not (like a home or a studio).
let generateForPlace cityId place state =
    let numberOfPeople = placePopulationRange place ||> RandomGen.genBetween
    let knownNpcs = allKnownNpcs cityId state

    let randomNpcs =
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

    let placeSpecificNpcs = generateForPlaceWithSpecificNpcs place state

    randomNpcs @ placeSpecificNpcs |> WorldPeopleInCurrentRoomChanged
