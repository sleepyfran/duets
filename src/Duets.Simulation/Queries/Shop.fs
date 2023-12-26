namespace Duets.Simulation.Queries

open Duets.Common
open Duets.Data.Items
open Duets.Data.World
open Duets.Entities

module Shop =
    type RestaurantError = | NoRestaurantInRooms

    /// Returns all the drinks that are typical for the given city.
    let cityDrinks cityId =
        let beer =
            Drink.Beer.byLocation
            |> Map.tryFind cityId
            |> Option.defaultValue []

        beer @ Drink.SoftDrinks.all

    /// Returns the menu of the given room inside the given city.
    let menuOfRoom cityId roomType =
        match roomType with
        | RoomType.Restaurant cuisineType ->
            match cuisineType with
            | American -> Food.USA.all
            | Czech -> Food.Czech.all
            | French -> Food.French.all
            | Italian -> Food.Italian.all
            | Japanese -> Food.Japanese.all
            | Mexican -> Food.Mexican.all
            | Turkish -> Food.Turkish.all
            | Vietnamese -> Food.Vietnamese.all
            @ cityDrinks cityId
        | _ -> []

    /// Returns the menu of the given place by searching for the closest
    /// restaurant in the rooms. If no restaurant is found, returns an error.
    let menuOfPlace cityId (place: Place) =
        place.Rooms.Nodes
        |> Map.tryFind Ids.Common.restaurant
        |> Option.map (fun room -> menuOfRoom cityId room.RoomType)
        |> Result.ofOption' NoRestaurantInRooms
