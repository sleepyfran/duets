module Duets.Simulation.Character.Npc

open Duets.Common
open Duets.Entities
open Duets.Data
open Duets.Simulation

let private randomAdultBirthday state =
    let age = RandomGen.genBetween 18 65
    let dayVariation = RandomGen.genBetween -30 0 |> (*) 1<days>
    let currentDate = Queries.Calendar.today state

    currentDate
    |> Calendar.Ops.addYears -(age * 1<years>)
    |> Calendar.Ops.addDays dayVariation

/// Generates a random NPC with a name and a gender from the database and a
/// random birthday between 18 and 65 years ago.
let generateRandom state =
    let name, gender = Npcs.random ()
    let birthday = randomAdultBirthday state

    Character.from name gender birthday

type private NpcRoomRoalDistribution =
    | Fixed of int
    | Weighted of float

let private roomRoleDistribution placeType roomType =
    match placeType, roomType with
    | Airport, RoomType.Lobby -> [ NpcRoomGoal.Working(Security), Fixed(1) ]
    | Airport, RoomType.SecurityControl ->
        [ NpcRoomGoal.Working(Security), Fixed(5) ]
    | Airport, RoomType.BoardingGate ->
        [ NpcRoomGoal.Working(Security), Fixed(1) ]
    | Bar, RoomType.Bar ->
        [ NpcRoomGoal.Working(PlayableWork Bartender), Fixed(1)
          NpcRoomGoal.Customer, Weighted 0.9 ]
    | Bookstore, RoomType.ReadingRoom -> [ NpcRoomGoal.Customer, Weighted 1 ]
    | CarDealer _, RoomType.ShowRoom ->
        [ NpcRoomGoal.Working(Security), Fixed 1
          NpcRoomGoal.Working(Seller), Fixed 1
          NpcRoomGoal.Customer, Weighted 0.9 ]
    | Cafe, RoomType.Cafe ->
        [ NpcRoomGoal.Working(PlayableWork Barista), Fixed 1
          NpcRoomGoal.Customer, Weighted 0.9 ]
    | Casino, RoomType.Lobby ->
        [ NpcRoomGoal.Working(Security), Weighted 0.2
          NpcRoomGoal.Customer, Weighted 0.8 ]
    | Casino, RoomType.CasinoFloor ->
        [ NpcRoomGoal.Working(Security), Weighted 0.2
          NpcRoomGoal.Working(Croupier), Weighted 0.1
          NpcRoomGoal.Customer, Weighted 0.7 ]
    | Cinema, RoomType.Lobby ->
        [ NpcRoomGoal.Working(Seller), Fixed 1
          NpcRoomGoal.Customer, Weighted 0.9 ]
    | Cinema, RoomType.ScreeningRoom -> [ NpcRoomGoal.Customer, Weighted 1 ]
    | ConcertSpace _, RoomType.Lobby ->
        [ NpcRoomGoal.Working(Security), Weighted 0.1
          NpcRoomGoal.Audience, Weighted 0.9 ]
    | ConcertSpace _, RoomType.Bar ->
        [ NpcRoomGoal.Working(PlayableWork Bartender), Fixed 1
          NpcRoomGoal.Audience, Weighted 0.9 ]
    | Gym, RoomType.Lobby
    | Gym, RoomType.ChangingRoom
    | Gym, RoomType.Gym -> [ NpcRoomGoal.Customer, Weighted 1 ]
    | Hotel _, RoomType.Lobby ->
        [ NpcRoomGoal.Working(Recepcionist), Fixed 1
          NpcRoomGoal.Tourist, Weighted 0.9 ]
    | Hospital, RoomType.Lobby ->
        [ NpcRoomGoal.Working(Nurse), Weighted 0.1
          NpcRoomGoal.Customer, Weighted 0.9 ]
    | MerchandiseWorkshop, RoomType.Workshop ->
        [ NpcRoomGoal.Working(Seller), Fixed 1
          NpcRoomGoal.Customer, Weighted 0.9 ]
    | MetroStation, RoomType.Platform ->
        [ NpcRoomGoal.Tourist, Weighted 0.2
          NpcRoomGoal.Commuter, Weighted 0.8 ]
    | RadioStudio _, RoomType.Lobby ->
        [ NpcRoomGoal.Working(Recepcionist), Fixed 1 ]
    | RehearsalSpace _, RoomType.Lobby ->
        [ (NpcRoomGoal.Musician, Weighted(0.1)) ]
    | RehearsalSpace _, RoomType.Bar ->
        [ NpcRoomGoal.Working(PlayableWork Bartender), Fixed(1)
          NpcRoomGoal.Musician, Weighted 0.2 ]
    | Restaurant, RoomType.Restaurant _ ->
        [ NpcRoomGoal.Working(Waiter), Weighted 0.1
          NpcRoomGoal.Customer, Weighted 0.9 ]
    | Street, RoomType.Street ->
        [ NpcRoomGoal.Tourist, Weighted 0.6
          NpcRoomGoal.Commuter, Weighted 0.4 ]
    | Studio _, RoomType.MasteringRoom
    | Studio _, RoomType.RecordingRoom ->
        [ NpcRoomGoal.Working(PlayableWork MusicProducer), Fixed(1) ]
    | _ -> []

let assignRoomRoles placeType roomType characters =
    let distribution = roomRoleDistribution placeType roomType
    let totalNpcs = List.length characters

    let assignedDistribution =
        distribution
        |> List.collect (fun (goal, weight) ->
            match weight with
            | Fixed i -> [ for _ in 1..i -> goal ]
            | Weighted w ->
                [ for _ in 1 .. Math.roundToNearest (w * float totalNpcs) ->
                      goal ])

    let finalDistribution =
        if assignedDistribution.Length = totalNpcs then
            assignedDistribution
        else
            // Attempt to fill the rest of the distribution with a weighted
            // goal instead of a fixed one, but resort to head if not possible.
            let padFriendlyGoal =
                distribution
                |> List.tryFind (function
                    | _, Weighted _ -> true
                    | _ -> false)
                |> Option.defaultValue (List.head distribution)
                |> fst

            assignedDistribution
            @ [ for _ in 1 .. (totalNpcs - (assignedDistribution.Length - 1)) ->
                    padFriendlyGoal ]

    characters
    |> List.indexed
    |> List.map (fun (i, character) ->
        let goal = finalDistribution |> List.item i
        Npc.fromCharacterWithGoal goal character)
