module Duets.Entities.Item

open Microsoft.FSharp.Reflection

module Property =
    /// Attempts to retrieve the main property of the given item, if any.
    let tryMain item = item.Properties |> List.tryHead

    /// Attempts to pick a property from the given item.
    let tryPick fn item = item.Properties |> List.tryPick fn

    /// Checks if the given item has a property that satisfies the given function.
    let has fn item = item.Properties |> List.exists fn

    /// Removes the current given property from the item and re-adds the updated
    /// one, returning the updated item. The matching of the property is done
    /// against the name of the case via reflection, so it'd match even if
    /// the values inside the property are different.
    let update property item =
        let updatedPropertyInfo, _ =
            FSharpValue.GetUnionFields(property, property.GetType())

        item.Properties
        |> List.filter (fun currentProperty ->
            let currentPropertyInfo, _ =
                FSharpValue.GetUnionFields(
                    currentProperty,
                    currentProperty.GetType()
                )

            currentPropertyInfo.Name <> updatedPropertyInfo.Name)
        |> (@) [ property ]

/// Updates the given item by removing the current property that matches
/// the given one, and re-adds the updated property returning the updated item.
let updateProperty property item =
    let updatedProperties = Property.update property item

    { item with
        Properties = updatedProperties }

module Beer =
    /// Creates a beer item.
    let create brand amount alcoholContent =
        { Brand = brand
          Name = "beer"
          Properties =
            [ Drinkable(
                  { Amount = amount
                    DrinkType = Beer(alcoholContent) }
              ) ] }

module Chip =
    /// Creates a chip to access a place in a city.
    let createFor cityId placeId =
        { Brand = "DuetsCorp"
          Name = "Chip"
          Properties = [ Chip(cityId, placeId) |> Key ] }

module Coffee =
    /// Creates a coffee item.
    let create name amount caffeineAmount =
        { Brand = "DuetsBeans"
          Name = name
          Properties =
            [ Drinkable(
                  { Amount = amount
                    DrinkType = Coffee(caffeineAmount) }
              ) ] }

module Food =
    /// Creates a food item.
    let create name amount foodType =
        { Brand = "DuetsFoods"
          Name = name
          Properties =
            [ Edible({ Amount = amount; FoodType = foodType })
              PlaceableInStorage(Fridge) ] }

module Soda =
    /// Creates a soda item.
    let create brand amount =
        { Brand = brand
          Name = brand
          Properties = [ Drinkable({ Amount = amount; DrinkType = Soda }) ] }
