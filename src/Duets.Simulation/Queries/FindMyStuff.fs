namespace Duets.Simulation.Queries

open Duets.Entities
open Duets.Simulation.Queries

module FindMyStuff =
    /// Represents a located item in the world with its coordinates and city/street info.
    type LocatedItem =
        { Item: Item
          Coordinates: RoomCoordinates
          CityName: CityId
          StreetName: string }

    /// Finds all cars owned by the player in the world.
    let findCarsInWorld (state: State) : LocatedItem list =
        state.WorldItems
        |> Map.toList
        |> List.collect (fun (coords, items) ->
            items
            |> List.filter (fun item ->
                item.Properties
                |> List.exists (function
                    | Rideable(Car _) -> true
                    | _ -> false))
            |> List.map (fun item ->
                let cityId, placeId, _ = coords
                let place = World.placeInCityById cityId placeId

                { Item = item
                  Coordinates = coords
                  CityName = cityId
                  StreetName = place.Name }))
