module rec Duets.Cli.Scenes.Phone.Apps.BnB.List

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation

/// Lists all of the current bookings that the character holds.
let listAll rentApp =
    let tableColumns =
        [ Styles.header "Place name"
          Styles.header "Location"
          Styles.header "Start date"
          Styles.header "End date"
          Styles.header "Price" ]

    let tableRows =
        Queries.Rentals.all (State.get ())
        |> List.map (fun rental ->
            let cityId, _ = rental.Coords
            let place = rental.Coords ||> Queries.World.placeInCityById
            let expirationDate = Rental.dueDate rental

            let price =
                match rental.RentalType with
                | Seasonal _ -> $"{Styles.money rental.Amount} / season"
                | OneTime _ -> Styles.money rental.Amount

            [ placeName place
              Styles.place
                  $"""{Generic.cityName cityId} {Styles.faded $"({place.Zone.Name})"}"""
              startDate rental
              Generic.dateWithDay expirationDate
              price ])

    showTable tableColumns tableRows

    rentApp ()

let private placeName (place: Place) =
    $"""{place.Name} {Styles.faded
                          $"({place.PlaceType |> World.Place.Type.toIndex |> World.placeTypeName})"}"""

let private startDate rental =
    match rental.RentalType with
    | Seasonal _ -> Styles.faded "-"
    | OneTime(fromDate, _) -> Generic.dateWithDay fromDate
