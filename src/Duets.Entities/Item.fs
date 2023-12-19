module Duets.Entities.Item

module Property =
    /// Attempts to pick a property from the given item.
    let tryPick fn item = item.Properties |> List.tryPick fn

    /// Checks if the given item has a property that satisfies the given function.
    let has fn item = item.Properties |> List.exists fn

module Beer =
    /// Creates a beer item.
    let create brand amount alcoholContent =
        { Brand = brand
          Properties =
            [ Drinkable(
                  { Amount = amount
                    DrinkType = Beer(alcoholContent) }
              ) ] }

module Chip =
    /// Creates a chip to access a place in a city.
    let createFor cityId placeId =
        { Brand = "Chip"
          Properties = [ Chip(cityId, placeId) |> Key ] }

module Coffee =
    /// Creates a coffee item.
    let create brand amount caffeineAmount =
        { Brand = brand
          Properties =
            [ Drinkable(
                  { Amount = amount
                    DrinkType = Coffee(caffeineAmount) }
              ) ] }

module Food =
    /// Creates a food item.
    let create name amount foodType =
        { Brand = name
          Properties =
            [ Edible({ Amount = amount; FoodType = foodType })
              PlaceableInStorage(Fridge) ] }

module Soda =
    /// Creates a soda item.
    let create brand amount =
        { Brand = brand
          Properties = [ Drinkable({ Amount = amount; DrinkType = Soda }) ] }
