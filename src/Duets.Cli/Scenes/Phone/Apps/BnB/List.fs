module Duets.Cli.Scenes.Phone.Apps.BnB.List

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

/// Lists all of the current bookings that the character holds.
let listAll rentApp =
    let tableColumns =
        [ Styles.header "Place name"
          Styles.header "Location"
          Styles.header "Contract expiration date" ]

    let tableRows =
        Queries.Rentals.all (State.get ())
        |> List.map (fun rental ->
            let cityId, _ = rental.Coords
            let place = rental.Coords ||> Queries.World.placeInCityById
            let expirationDate = Rental.dueDate rental

            [ place.Name
              Styles.place
                  $"""{Generic.cityName cityId} {Styles.faded $"({place.Zone.Name})"}"""
              Generic.dateWithDay expirationDate ])

    showTable tableColumns tableRows

    rentApp ()
