module Duets.Entities.Item

module Chip =
    /// Creates a chip to access a place in a city.
    let createFor cityId placeId =
        { Brand = "Chip"
          Type = Chip(cityId, placeId) |> Key }
