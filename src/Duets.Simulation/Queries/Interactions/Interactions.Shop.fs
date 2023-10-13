namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data.Items
open Duets.Entities

module Shop =
    /// Gather all available interactions inside a shop.
    let internal interactions items =
        [ ShopInteraction.Order items |> Interaction.Shop
          ShopInteraction.SeeMenu items |> Interaction.Shop ]

module Bar =
    let internal cityDrinks cityId =
        let beer =
            Drink.Beer.byLocation
            |> Map.tryFind cityId
            |> Option.defaultValue []

        beer @ Drink.SoftDrinks.all

    /// Gather all available interactions inside a bar.
    let internal interactions cityId roomType =
        match roomType with
        | RoomType.Bar -> cityDrinks cityId |> Shop.interactions
        | _ -> []

module Cafe =
    /// Gather all available interactions inside a cafe.
    let internal interactions roomType =
        match roomType with
        | RoomType.Cafe ->
            Food.Breakfast.all @ Drink.Coffee.all |> Shop.interactions
        | _ -> []

module Restaurant =
    /// Gather all available interactions inside a restaurant.
    let internal interactions cityId roomType =
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
            @ Bar.cityDrinks cityId
            |> Shop.interactions
        | _ -> []
