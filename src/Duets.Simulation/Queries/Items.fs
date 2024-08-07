namespace Duets.Simulation.Queries

open Aether
open Duets.Data
open Duets.Entities

module Items =
    let private defaultItems (coords: RoomCoordinates) =
        let cityId, placeId, roomId = coords
        let place = (cityId, placeId) ||> World.placeInCityById
        let room = (cityId, placeId, roomId) |||> World.roomById

        (*
        NOTE: Do not add here any items that contain stateful properties like
        a shelf since they will be overriden by these default items and the
        state will be lost.
        *)
        match place.PlaceType, room.RoomType with
        | PlaceType.Bar, RoomType.Bar ->
            Items.Electronics.Dartboard.dartboard
            :: [ Items.Furniture.BilliardTable.sonomaTable ]
            |> List.map fst
        | PlaceType.Home, RoomType.Bedroom
        | PlaceType.Hotel _, RoomType.Bedroom ->
            (* Otherwise, nowhere to sleep on. *)
            [ fst Items.Furniture.Bed.ikeaBed ]
        | PlaceType.Gym, RoomType.Gym ->
            (* Otherwise, there's nothing to do on the gym. *)
            Items.Gym.all |> List.map fst
        | PlaceType.ConcertSpace _, RoomType.Backstage ->
            (* Basic stuff for the band to consume. *)
            let localBeer = Items.Drink.Beer.byLocation |> Map.find cityId

            localBeer @ Items.Food.Snack.all |> List.map fst
        | _ -> []

    /// Returns all the items currently available in the given coordinates,
    /// including those that should not be visible to the character.
    let allWithHiddenIn state coords =
        let defaultLocationItems = defaultItems coords

        Optic.get Lenses.State.worldItems_ state
        |> Map.tryFind coords
        |> Option.defaultValue []
        |> (@) defaultLocationItems

    /// Returns all the items currently available in the given coordinates.
    let allIn state coords =
        (* 
        Filter deliverable items since those should not be visible to the
        character.
        *)
        allWithHiddenIn state coords
        |> List.filter (fun item ->
            item.Properties
            |> List.exists (function
                | Deliverable _ -> true
                | _ -> false)
            |> not)

    /// Determines whether the given item is located in the given world location,
    /// the character's inventory or none of them.
    let itemLocation state location item =
        let locationItems = allIn state location
        let inventory = Inventory.character state

        if locationItems |> List.contains item then
            ItemLocation.World
        else if inventory |> List.contains item then
            ItemLocation.Inventory
        else
            ItemLocation.Nowhere
