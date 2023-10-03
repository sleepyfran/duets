namespace Duets.Simulation.Queries

open Aether
open Duets.Common
open Duets.Data
open Duets.Entities

module Items =
    let private defaultItems (coords: RoomCoordinates) =
        let cityId, placeId, roomId = coords
        let place = (cityId, placeId) ||> World.placeInCityById
        let room = (cityId, placeId, roomId) |||> World.roomById

        match place.PlaceType, room.RoomType with
        | PlaceType.Hotel _, RoomType.Bedroom ->
            (* Otherwise hotels wouldn't have anywhere to sleep on. *)
            [ fst Items.Furniture.Bed.ikeaBed ]
        | PlaceType.Gym, RoomType.Gym ->
            (* Otherwise, there'd be nothing to do on the gym. *)
            Items.Gym.all |> List.map fst
        | PlaceType.ConcertSpace _, RoomType.Backstage ->
            (* Basic stuff for the band to consume. *)
            let localBeer = Items.Drink.Beer.byLocation |> Map.find cityId

            localBeer @ Items.Food.Snack.all |> List.map fst
        | _ -> []

    /// Returns all the items currently available in the given coordinates.
    let allIn state coords =
        let defaultLocationItems = defaultItems coords

        let placedItems =
            Optic.get Lenses.State.worldItems_ state
            |> Map.tryFind coords
            |> Option.defaultValue []

        defaultLocationItems @ placedItems

    /// Determines whether the given item is located in the given world location,
    /// the character's inventory or none of them.
    let itemLocation state location item =
        let locationItems = allIn state location
        let inventory = Inventory.get state

        if locationItems |> List.contains item then
            ItemLocation.World
        else if inventory |> List.contains item then
            ItemLocation.Inventory
        else
            ItemLocation.Nowhere
