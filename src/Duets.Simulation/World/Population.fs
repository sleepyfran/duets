module Duets.Simulation.World.Population

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns the population range of places that follow the usual daily routine,
/// like streets or metro platforms.
let private dailyRoutineRange dayMoment =
    match dayMoment with
    | EarlyMorning
    | Midnight -> 0, 3
    | Morning
    | Night -> 1, 6
    | Midday
    | Afternoon
    | Evening -> 2, 9

/// Returns the population range of places that get busier at night, like bars.
let private nightlifeRange dayMoment =
    match dayMoment with
    | Evening
    | Night
    | Midnight -> 2, 10
    | _ -> 0, 4

/// Returns a range of population values that a room can have, meaning that
/// the room's population will be a random number between the two values.
let private roomPopulationRange (place: Place) (room: Room) dayMoment =
    match place.PlaceType, room.RoomType with
    (* Private places will only be populated by their specific NPCs. *)
    | Home, _
    | MerchandiseWorkshop, _
    | Studio _, _ -> 0, 0
    | RadioStudio _, RoomType.Lobby
    | RehearsalSpace _, RoomType.Lobby -> 0, 3
    | _, RoomType.Backstage
    | _, RoomType.Bedroom
    | _, RoomType.Kitchen
    | _, RoomType.LivingRoom
    | _, RoomType.MasteringRoom
    | _, RoomType.RecordingRoom
    | _, RoomType.RehearsalRoom
    | _, RoomType.Stage
    | _, RoomType.Workshop -> 0, 0
    | _, RoomType.Bar -> nightlifeRange dayMoment
    | _, RoomType.BoardingGate -> 2, 10
    | _, RoomType.Cafe -> 0, 6
    | _, RoomType.CasinoFloor -> 2, 10
    | _, RoomType.ChangingRoom -> 0, 3
    | _, RoomType.Gym -> 1, 6
    | _, RoomType.Lobby -> 0, 4
    | _, RoomType.Platform
    | _, RoomType.Street -> dailyRoutineRange dayMoment
    | _, RoomType.ReadingRoom -> 0, 5
    | _, RoomType.Restaurant _ -> 1, 8
    | _, RoomType.ScreeningRoom -> 0, 15
    | _, RoomType.SecurityControl -> 1, 5
    | _, RoomType.ShowRoom -> 0, 3

/// Returns a random known NPC in the given city, if any.
let private allKnownNpcs cityId state =
    Queries.Relationship.fromCity cityId state
    |> List.map (fun relationship ->
        Queries.Characters.find state relationship.Character)

let private generateForRoomWithSpecificNpcs place (room: Room) state =
    let bandMembers =
        Queries.Bands.currentBandMembersWithoutPlayableCharacter state
        |> List.map (_.CharacterId >> Queries.Characters.find state)

    match place.PlaceType, room.RoomType with
    | RehearsalSpace _, RoomType.RehearsalRoom -> bandMembers
    | Studio studio, _ -> studio.Producer :: bandMembers
    | _ -> []

/// Generates an effect that puts a random number of people in the given room,
/// keeping in mind the room's population range, which depends on the type of
/// the room and place and the time of the day.
let generateForRoom cityId (place: Place) roomId state =
    let room = Queries.World.roomById cityId place.Id roomId

    let dayMoment =
        Queries.Calendar.today state |> Calendar.Query.dayMomentOf

    let numberOfPeople =
        roomPopulationRange place room dayMoment ||> RandomGen.genBetween

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

    let roomSpecificNpcs = generateForRoomWithSpecificNpcs place room state

    randomNpcs @ roomSpecificNpcs |> WorldPeopleInCurrentRoomChanged
