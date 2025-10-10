namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data
open Duets.Data.Items
open Duets.Entities
open Duets.Simulation

module Shop =
    /// Gather all available interactions inside a shop.
    let internal interactions items =
        [ ShopInteraction.Order items |> Interaction.Shop
          ShopInteraction.SeeMenu items |> Interaction.Shop ]

module Bar =
    /// Gather all available interactions inside a bar.
    let internal interactions cityId roomType =
        match roomType with
        | RoomType.Bar -> Queries.Shop.cityDrinks cityId |> Shop.interactions
        | _ -> []

module Bookstore =
    /// Gather all available interactions inside a bookstore.
    let internal interactions roomType =
        match roomType with
        | RoomType.ReadingRoom ->
            let items =
                Items.Book.all |> List.sortBy (fun (item, _) -> item.Brand)

            [ ShopInteraction.Buy items |> Interaction.Shop ]
        | _ -> []

module Cafe =
    /// Gather all available interactions inside a cafe.
    let internal interactions roomType =
        match roomType with
        | RoomType.Cafe ->
            Food.Breakfast.all @ Drink.Coffee.all |> Shop.interactions
        | _ -> []

module CarDealer =
    /// Gather all available interactions inside a car dealer.
    let internal interactions carDealer roomType =
        match roomType with
        | RoomType.ShowRoom when carDealer.PriceRange = CarPriceRange.Budget ->
            [ (carDealer, Vehicles.Car.budget)
              |> ShopInteraction.BuyCar
              |> Interaction.Shop ]
        | RoomType.ShowRoom when carDealer.PriceRange = CarPriceRange.MidRange ->
            [ (carDealer, Vehicles.Car.midRange)
              |> ShopInteraction.BuyCar
              |> Interaction.Shop ]
        | RoomType.ShowRoom when carDealer.PriceRange = CarPriceRange.Premium ->
            [ (carDealer, Vehicles.Car.premium)
              |> ShopInteraction.BuyCar
              |> Interaction.Shop ]
        | _ -> []

module Restaurant =
    /// Gather all available interactions inside a restaurant.
    let internal interactions cityId room =
        let items = Queries.Shop.menuOfRoom cityId room

        match items with
        | [] -> []
        | _ -> items |> Shop.interactions
